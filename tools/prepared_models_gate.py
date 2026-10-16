#!/usr/bin/env python3
"""Build and prove prepared models against the approved installed RimWorld."""

import argparse
import ast
from contextlib import contextmanager
from datetime import datetime, timezone
import hashlib
import json
import os
from pathlib import Path
import re
import shutil
import subprocess
import struct
import zlib
import sys
import time
import uuid
import xml.etree.ElementTree as ET

ROOT = Path(__file__).resolve().parents[1]
WORK = ROOT / "artifacts/prepared-models"
EVIDENCE = ROOT / ".testruns/ISORIM-003"
SDK = Path("C:/Users/mikel/AppData/Local/Microsoft/dotnet/dotnet.exe")
GAME = Path("C:/Program Files (x86)/Steam/steamapps/common/RimWorld")
HARMONY = Path("C:/Program Files (x86)/Steam/steamapps/workshop/content/294100/2009463077/1.7/Assemblies/0Harmony.dll")
FINGERPRINTS = {
    "RimWorld.dll": "485913A05EED5301E812898E82C860E69CCA6E8FA1BD91CDEA70B7E67D92A1D6",
    "Assembly-CSharp.dll": "EFF7D97949F919FB2B151EBFB906E9BE749B63410B7DBDA78EEEBB7972B245C3",
    "UnityStubs.dll": "CD95A2D1A2E0E52745F1B4562FC0CBF66B526CD942015A75DA63D2CF8770D535",
    "Waylith.dll": "15633912DA7C890E8798AAC86121DDBCF28B4138786D926921DAFA7CA908929F",
    "Waylith.UnityCompat.dll": "E4362E5F45EC39533C721B4B4E04C996CF59DE8FCDA2547EB648C3D68FBB5710",
}
HARMONY_HASH = "C8EF7856B2BE3CF24B345C9B5C2F197F68FCF92649635064FCE7B3CCC8F77CB0"


class Refusal(RuntimeError):
    pass


def digest(path):
    with Path(path).open("rb") as stream:
        return hashlib.file_digest(stream, "sha256").hexdigest().upper()


def verify_game(game):
    try:
        if (game / "Version.txt").read_text().strip() != "1.7.4980 rev60":
            raise Refusal("The game is not 1.7.4980 rev60")
        for name, expected in FINGERPRINTS.items():
            if digest(game / name) != expected:
                raise Refusal(f"The game fingerprint changed: {name}")
        config = json.loads((game / "RimWorld.runtimeconfig.json").read_text())
        frameworks = config["runtimeOptions"]["includedFrameworks"]
        if {"name": "Microsoft.NETCore.App", "version": "10.0.10-waylithclr.dev"} not in frameworks:
            raise Refusal("The bundled CLR changed")
    except (OSError, KeyError, ValueError) as error:
        raise Refusal(f"Cannot verify the installed game: {error}") from error
    return {name: digest(game / name) for name in FINGERPRINTS}


def verify_sdk(sdk, acceptance=True):
    if not sdk.is_absolute() or (acceptance and sdk.resolve() != SDK.resolve()):
        raise Refusal("Use the explicit approved SDK path")
    result = subprocess.run([str(sdk), "--version"], capture_output=True, text=True, check=False)
    if result.returncode or result.stdout.strip() != "10.0.402":
        raise Refusal("The SDK is not 10.0.402")


def verify_harmony(harmony):
    if not harmony.is_file() or digest(harmony) != HARMONY_HASH:
        raise Refusal("Harmony does not match 3.0.0-rc.3")


def verify_assets(assets, target, installed=False):
    libraries = assets.get("libraries", {})
    required = {"Lib.Harmony/3.0.0-rc.3"} if target == "1.7" else {
        "Krafs.Rimworld.Ref/" + {"1.5": "1.5.4409", "1.6": "1.6.4871"}[target],
        "Lib.Harmony/2.3.3", "Microsoft.NETFramework.ReferenceAssemblies/1.0.3",
    }
    if target == "1.7" and not installed:
        required.add("Krafs.Rimworld.Ref/1.7.4979-alpha")
    if not required <= libraries.keys():
        raise Refusal("The restored package graph differs from the approved references")
    expected = {entry.split("/")[0]: entry for entry in required}
    for entry in libraries:
        name = entry.split("/")[0]
        if name in expected and entry != expected[name]:
            raise Refusal(f"Unexpected package version: {entry}")
        if installed and name == "Krafs.Rimworld.Ref":
            raise Refusal("Package references cannot prove the installed acceptance build")


def case_manifest():
    return json.loads((ROOT / "tools/prepared_models/cases.json").read_text())


def verify_registry(cases, manifest):
    registered = set()
    for path in (ROOT / "Source17").glob("ProbeNormals*.cs"):
        registered.update(re.findall(r'case "([A-Za-z]+Cases\.[A-Za-z]+)":', path.read_text()))
    declared = set(manifest["cases"])
    if declared != registered:
        raise Refusal("The manifest and available controller registrations differ: "
                      + str(sorted(declared ^ registered)))
    required_groups = {
        "NormalDataCases": "data",
        "AssetIntakeCases": "intake",
        "StaticNormalCases": "static",
        "MovingNormalCases": "moving",
        "IntegratedNormalCases": "integrated",
    }
    for case in registered:
        group = required_groups.get(case.split(".")[0])
        if group is None or case not in manifest["groups"].get(group, []):
            raise Refusal(f"The required selectable group is missing case: {case}")
    for members in manifest["groups"].values():
        if not set(members) <= declared:
            raise Refusal("A case group contains an absent registration")
    if not set(cases) <= registered:
        raise Refusal("Selected runtime cases are absent from the controller: " + str(sorted(set(cases) - registered)))


def verify_host_controls(manifest, evidence):
    source = ast.parse((ROOT / "tests/test_prepared_models_gate.py").read_text())
    actual = {node.name for node in ast.walk(source) if isinstance(node, ast.FunctionDef) and node.name.startswith("test_")}
    expected = set(manifest["hostCases"])
    if not expected or actual != expected:
        raise Refusal("The host control registrations differ from the implemented tests")
    output = (evidence / "host-controls.stderr.log").read_text()
    summary = re.search(r"^Ran (\d+) tests? in ", output, re.MULTILINE)
    if not summary or int(summary[1]) != len(expected) or not re.search(r"^OK$", output, re.MULTILINE) or "skipped=" in output:
        raise Refusal("The host controls did not run every required test without skips")
    return {"run": len(expected), "passed": len(expected), "failed": 0, "skipped": 0, "caseIds": sorted(expected)}


def changed_paths(records):
    tokens = records.decode("utf-8", errors="strict").split("\0")
    if tokens.pop() != "":
        raise Refusal("The changed-path stream is not NUL terminated")
    paths = []
    while tokens:
        status = tokens.pop(0)
        if not status or status[0] not in "AMDTCR":
            raise Refusal("Unsupported changed-path status")
        count = 2 if status[0] in "RC" else 1
        if len(tokens) < count:
            raise Refusal("Truncated changed-path stream")
        for _ in range(count):
            path = tokens.pop(0)
            if not path:
                raise Refusal("Empty changed path")
            paths.append(path)
    return sorted(set(paths))


def select_cases(records, story, manifest):
    try:
        ordinal = int(story.removeprefix("ISORIM-003.S"))
    except ValueError as error:
        raise Refusal("Unknown story") from error
    if story != f"ISORIM-003.S{ordinal:02d}" or ordinal not in range(1, 6):
        raise Refusal("Unknown story")
    verify_registry((), manifest)
    selected = set()
    paths = changed_paths(records)
    if not paths:
        raise Refusal("The commissioned change is empty")
    for path in paths:
        groups = manifest["paths"].get(path)
        if not groups:
            raise Refusal(f"Unmapped changed path: {path}")
        if groups == ["all"]:
            groups = manifest["groups"].keys()
        for group in groups:
            if group not in manifest["groups"]:
                raise Refusal(f"Unknown case group: {group}")
            for case in manifest["groups"][group]:
                registration = manifest["cases"].get(case)
                if registration is None:
                    raise Refusal(f"Missing case registration: {case}")
                if registration["story"] not in range(1, ordinal + 1):
                    raise Refusal(f"The registry contains a case beyond the requested story: {case}")
                selected.add(case)
    if not selected:
        raise Refusal("No registered runtime cases cover the change")
    return sorted(selected)


def verify_proofs(paths, cases, request, modules, pid, started):
    found = {}
    for path in paths:
        try:
            proof = json.loads(path.read_text())
            case = proof["caseId"]
            if case in found or case not in cases:
                raise Refusal("Duplicate or unknown proof case")
            if path.stat().st_mtime < started:
                raise Refusal("The proof predates this run")
            if any(proof.get(key) != request[key] for key in ("nonce", "head", "assemblyHash")):
                raise Refusal("The proof identity is stale")
            if proof.get("modules") != modules or proof.get("processId") != pid:
                raise Refusal("The observed process or modules differ")
            if proof.get("runtime") != "10.0.10-waylithclr.dev":
                raise Refusal("The proof did not use the approved bundled CLR")
            if proof.get("gameVersion") != "1.7.4980.60":
                raise Refusal("The loaded game version differs from the approved identity")
            if Path(proof.get("engineLogRoot", "")).resolve() != path.parent.resolve():
                raise Refusal("The engine log root is outside this run's evidence")
            if proof.get("passed") is not True or proof.get("skipped", False):
                raise Refusal(f"Runtime case failed: {case}: {proof.get('details')}")
            for evidence in proof["evidencePaths"]:
                evidence_path = Path(evidence)
                if not evidence_path.resolve().is_relative_to(path.parent.resolve()) or not evidence_path.is_file():
                    raise Refusal("The proof names missing or escaping evidence")
            found[case] = proof
        except (OSError, ValueError, KeyError, TypeError) as error:
            raise Refusal(f"Invalid proof: {path}: {error}") from error
    if set(found) != set(cases):
        raise Refusal("The requested proof set is incomplete")
    return found


def build_properties(target, audit, game=None, harmony=None):
    audit = audit.resolve() / target
    properties = [f"-p:{name}={audit / suffix}/" for name, suffix in (
        ("OutputPath", "bin"), ("BaseIntermediateOutputPath", "obj"),
        ("IntermediateOutputPath", "obj"), ("MSBuildProjectExtensionsPath", "obj"))]
    if target == "1.7" and game is not None:
        properties += ["-p:UseInstalledRimWorld=true", f"-p:RimWorldReferenceDirectory={game.resolve()}",
                       f"-p:HarmonyReferencePath={harmony.resolve()}"]
    elif target != "1.7":
        properties += [f"-p:RimWorldVersion={target}"]
    return properties


def run_child(argv, evidence, label, protected, cwd=ROOT, timeout=600, env=None, expected_exit=0):
    verify_snapshot(protected)
    started = time.time()
    result = None
    try:
        with (evidence / f"{label}.stdout.log").open("wb") as stdout, (evidence / f"{label}.stderr.log").open("wb") as stderr:
            result = subprocess.run([str(arg) for arg in argv], cwd=cwd, stdout=stdout, stderr=stderr,
                                    timeout=timeout, env=env, check=False)
    finally:
        verify_snapshot(protected)
        with (evidence / "children.jsonl").open("a", encoding="utf-8") as stream:
            stream.write(json.dumps({"argv": [str(arg) for arg in argv], "exit": result.returncode if result else None,
                                     "started": started, "ended": time.time(), "protectedInputs": len(protected)}) + "\n")
    if expected_exit is not None and result.returncode != expected_exit:
        raise Refusal(f"{label} exited {result.returncode}. See {evidence}")
    return result


def validate_staged_intake(evidence, protected):
    staged = WORK / "staged-mod"
    rows = []

    def run(name, root, diagnostic=""):
        command = [sys.executable, "tools/validate_mod.py", "--mod-dir", str(root), "--check-assemblies"]
        inputs = [root / relative for relative in ("1.7/Defs/Diagnostics/Models.xml", "1.7/Defs/Diagnostics/WornGear.xml",
                  "1.7/Models/Diagnostics/internal.obj", "1.7/Models/Diagnostics/sample.mtl", "1.7/Models/Diagnostics/legacy.obj")]
        inputs = [path for path in inputs if path.is_file()]
        snapshot = {path: digest(path) for path in inputs}
        raw_inputs = [{"path": path.relative_to(root).as_posix(), "sha256": snapshot[path], "hex": path.read_bytes().hex()} for path in inputs]
        result = run_child(command, evidence, "validator-" + name, {**protected, **snapshot}, expected_exit=None)
        output = (evidence / f"validator-{name}.stdout.log").read_text()
        error = (evidence / f"validator-{name}.stderr.log").read_text()
        marker = next((line[len("Prepared inputs: "):] for line in output.splitlines() if line.startswith("Prepared inputs: ")), "[]")
        rows.append({"name": name, "command": command, "exit": result.returncode,
                     "expectedSuccess": not bool(diagnostic), "diagnosticMatched": not diagnostic or diagnostic in error,
                     "checkedPaths": json.loads(marker), "inputFiles": raw_inputs})

    run("passing", staged)
    if rows[0]["exit"] == 0:
        control = WORK / "validator-controls" / evidence.name
        copy_tree(staged, control)
        mutations = [
            ("missing-gear", "1.7/Defs/Diagnostics/WornGear.xml", "internal.obj", "missing-gear.obj", "missing-gear.obj"),
            ("missing-model", "1.7/Defs/Diagnostics/Models.xml", "internal.obj", "missing-model.obj", "missing-model.obj"),
            ("missing-mtl", "1.7/Models/Diagnostics/internal.obj", "sample.mtl", "missing.mtl", "missing.mtl"),
            ("malformed-mtl", "1.7/Models/Diagnostics/sample.mtl", "0.2 0.3 0.4", "broken 0.3 0.4", "material colour"),
            ("zero-normal", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0 0 0", "nonzero"),
            ("nonfinite-normal", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn NaN 0 -1", "finite"),
            ("float-overflow-normal", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e39 0 0", "float32"),
            ("float-underflow-normal", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e-50 0 0", "float32"),
            ("magnitude-overflow-normal", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e20 0 0", "float32"),
            ("small-magnitude-normal", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e-7 0 0", "float32"),
            ("normalize-below", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.000002 0 0", "float32"),
            ("normalize-at", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.00001 0 0", "float32"),
            ("normalize-above", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.00002 0 0", ""),
            ("normalize-next-below", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.00000999999883788405 0 0", "float32"),
            ("normalize-next-above", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.00001000000065687345 0 0", ""),
            ("normalize-diagonal-below", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.000006 0.000006 0", "float32"),
            ("normalize-diagonal-above", "1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0.000008 0.000008 0", ""),
            ("normal-index", "1.7/Models/Diagnostics/internal.obj", "2/2/1", "2/2/99", "normal index"),
            ("position-index", "1.7/Models/Diagnostics/internal.obj", "2/2/1", "99/2/1", "position index"),
            ("empty-obj", "1.7/Models/Diagnostics/internal.obj", None, "", "no face"),
            ("vertices-only-obj", "1.7/Models/Diagnostics/internal.obj", None, "v 0 0 0\nv 2 0 0\nv 0 1 0\n", "no face"),
            ("passing-no-vn", "1.7/Models/Diagnostics/internal.obj", None,
             "v 0 0 0\nv 2 0 0\nv 0 1 0\nf 1 2 3\n", ""),
        ]
        for name, relative, old, new, diagnostic in mutations:
            path = control / relative
            original = path.read_text()
            if old is not None and old not in original:
                raise Refusal(f"The validator fault control has no positive input: {name}")
            try:
                path.write_text(new if old is None else original.replace(old, new))
                run(name, control, diagnostic)
            finally:
                path.write_text(original)
    from prepared_models.intake_fixtures import recovery_cases, recovery_mod
    recovery = []
    root = WORK / "recovery-validator" / evidence.name
    recovery_mod(root, staged / "1.7/Assemblies/IsoRim.dll")
    for case in recovery_cases():
        obj = root / "1.7/Models/Diagnostics/internal.obj"
        mtl = obj.with_name("sample.mtl")
        obj.write_bytes(bytes.fromhex(case["objHex"]))
        if case["mtlHex"] is None:
            mtl.unlink(missing_ok=True)
        else:
            mtl.write_bytes(bytes.fromhex(case["mtlHex"]))
        actual_hash = digest(obj)
        if actual_hash != case["objHash"] or (mtl.is_file() and digest(mtl) != case["mtlHash"]):
            raise Refusal("The recovery CLI input changed before validation: " + case["id"])
        command = [sys.executable, "tools/validate_mod.py", "--mod-dir", str(root), "--check-assemblies"]
        label = "recovery-" + case["id"]
        case_inputs = {obj: actual_hash}
        if mtl.is_file():
            case_inputs[mtl] = digest(mtl)
        child = run_child(command, evidence, label, {**protected, **case_inputs}, expected_exit=None)
        output = (evidence / (label + ".stdout.log")).read_text()
        error = (evidence / (label + ".stderr.log")).read_text()
        bits = next((json.loads(line[len("Prepared scalar bits: "):]) for line in output.splitlines() if line.startswith("Prepared scalar bits: ")), [])
        bit_match = case["bits"] is None or any(row["token"] == case["token"] and row["bits"] == case["bits"] for row in bits)
        diagnostic = child.returncode == 0 or "Validation error:" in error
        if case["id"].startswith(("R23-below-min-", "R23-above-max-")):
            diagnostic = "signed Int32" in error
        recovery.append(dict(case, actualInputHash=actual_hash, actualInputHex=obj.read_bytes().hex(),
                             command=command, exit=child.returncode, diagnosticMatched=diagnostic,
                             scalarBits=bits, bitMatched=bit_match))
    report = {"nonce": evidence.name, "controls": rows, "recovery": recovery}
    (evidence / "validator-results.json").write_text(json.dumps(report, indent=2))
    return report


def verify_snapshot(protected):
    for path, expected in protected.items():
        if not path.is_file() or digest(path) != expected:
            raise Refusal(f"A protected input changed: {path}")


def git(*args, root=ROOT):
    result = subprocess.run(["git", "-C", str(root), *args], capture_output=True, check=False)
    if result.returncode:
        raise Refusal(f"Git refused {args}: {result.stderr.decode(errors='replace')}")
    return result.stdout


def prepare_evidence(root, evidence):
    allowed = root.resolve() / ".testruns/ISORIM-003"
    if not evidence.resolve().is_relative_to(allowed) or evidence.resolve() == allowed:
        raise Refusal("Evidence must be below .testruns/ISORIM-003/<nonce>")
    if evidence.parent.resolve() != allowed:
        raise Refusal("Each evidence nonce must be a direct child of the epic directory")
    for parent in [root / ".testruns", allowed, evidence]:
        if parent.is_symlink() or parent.is_junction():
            raise Refusal("Evidence directories cannot redirect writes")
    rule = git("check-ignore", "-v", "--", ".testruns/ISORIM-003/ignore-probe.json", root=root).decode()
    if "\t.testruns/ISORIM-003/ignore-probe.json" not in rule or ":/.testruns/ISORIM-003/" not in rule:
        raise Refusal("The exact epic evidence ignore rule is missing")
    if git("ls-files", "-z", "--", ".testruns/ISORIM-003", root=root):
        raise Refusal("Evidence paths are tracked")
    if evidence.exists():
        raise Refusal("The evidence nonce already exists")
    evidence.mkdir(parents=True)
    return rule.strip()


def protected_inputs(base, game=None, harmony=None):
    roots = ["Models", "Textures", "LUTs", "Defs", "Source", "1.5", "1.6", "1.7/Assemblies", "About"]
    git("diff", "--exit-code", base, "--", *roots)
    records = git("ls-tree", "-r", "-z", base, "--", *roots).split(b"\0")
    protected = {}
    for record in filter(None, records):
        metadata, name = record.split(b"\t", 1)
        path = ROOT / name.decode()
        if "/obj/" in path.as_posix():
            continue
        if not path.is_file():
            raise Refusal(f"Frozen input differs from execution base: {name.decode()}")
        protected[path] = digest(path)
    if game:
        for path in game.iterdir():
            if path.is_file():
                protected[path] = digest(path)
        for directory in (game / "Data/Core", game / "Rendering", game / "vulkan"):
            for path in directory.rglob("*"):
                if path.is_file():
                    protected[path] = digest(path)
        protected[harmony] = digest(harmony)
    return protected


def verify_restore(args, evidence, protected, installed=True):
    if Path(args.assets).resolve() != (WORK / "build" / args.target / "obj/project.assets.json").resolve():
        raise Refusal("The restored assets path is outside the isolated target directory")
    assets = json.loads(Path(args.assets).read_text())
    verify_assets(assets, args.target, installed=installed and args.target == "1.7")
    properties = build_properties(args.target, WORK / "build", args.game_dir if installed else None, args.harmony)
    project = "Source17/IsoRim.csproj" if args.target == "1.7" else "Source/IsoRim.csproj"
    label = f"references-{args.target}"
    run_child([args.dotnet, "msbuild", project, "-t:ResolveReferences", "-getItem:ReferencePath", *properties],
              evidence, label, protected)
    result = json.loads((evidence / f"{label}.stdout.log").read_text(encoding="utf-8-sig"))
    references = result["Items"]["ReferencePath"]
    resolved = {}
    for reference in references:
        path = Path(reference.get("FullPath", reference["Identity"]))
        if not path.is_file():
            raise Refusal(f"The compiler reference is missing: {path}")
        resolved[str(path)] = digest(path)
    if installed and args.target == "1.7":
        expected = {str(args.game_dir / name): value for name, value in verify_game(args.game_dir).items()}
        expected[str(args.harmony)] = HARMONY_HASH
        for path, value in expected.items():
            matches = [hash_value for name, hash_value in resolved.items() if Path(name).resolve() == Path(path).resolve()]
            if matches != [value]:
                raise Refusal(f"The evaluated compiler reference differs: {path}")
        for name in resolved:
            if "krafs.rimworld.ref" in name.lower():
                raise Refusal("The acceptance compiler resolved a portable game reference")
    (evidence / f"compiler-references-{args.target}.json").write_text(json.dumps(resolved, indent=2))
    return resolved


def compile_target(args, evidence, protected, target="1.7", installed=True):
    properties = build_properties(target, WORK / "build", args.game_dir if installed and target == "1.7" else None, args.harmony)
    project = "Source17/IsoRim.csproj" if target == "1.7" else "Source/IsoRim.csproj"
    run_child([args.dotnet, "restore", project, *properties], evidence, f"restore-{target}", protected)
    local = argparse.Namespace(**vars(args))
    local.target = target
    local.assets = WORK / "build" / target / "obj/project.assets.json"
    verify_restore(local, evidence, protected, installed=installed)
    run_child([args.dotnet, "build", project, "--no-restore", "-c", "Release", "-nologo", *properties],
              evidence, f"build-{target}", protected)


def format_sources(args, evidence, protected, target="1.7", include=None, write=False):
    directory = WORK / "build" / target
    environment = dict(os.environ)
    environment.update({
        "PreparedModelsFormatDirectory": str(directory),
        "CustomAfterMicrosoftCommonTargets": str(ROOT / "tools/prepared_models/format-isolation.targets"),
        "OutputPath": str(directory / "bin") + os.sep,
        "BaseIntermediateOutputPath": str(directory / "obj") + os.sep,
        "IntermediateOutputPath": str(directory / "obj") + os.sep,
        "MSBuildProjectExtensionsPath": str(directory / "obj") + os.sep,
        "UseInstalledRimWorld": "true" if target == "1.7" else "false",
        "RimWorldReferenceDirectory": str(args.game_dir), "HarmonyReferencePath": str(args.harmony),
        "RimWorldVersion": target,
    })
    project = "Source17/IsoRim.csproj" if target == "1.7" else "Source/IsoRim.csproj"
    for kind in ("whitespace", "style"):
        command = [args.dotnet, "format", kind, project, "--no-restore", "--verbosity", "normal"]
        if not write:
            command.append("--verify-no-changes")
        if include:
            command += ["--include", *include]
        run_child(command, evidence, f"format-{target}-{kind}-{'write' if write else 'verify'}", protected, env=environment)


def copy_tree(source, destination):
    if source.is_symlink() or source.is_junction() or destination.is_symlink() or destination.is_junction():
        raise Refusal(f"A sandbox copy path redirects writes: {source} -> {destination}")
    shutil.copytree(source, destination, dirs_exist_ok=True)


def reset_work_directory(path):
    if path.name not in {"runtime", "staged-mod"} or path.parent.resolve() != WORK.resolve():
        raise Refusal("Only the named generated sandbox directories can be replaced")
    for parent in (WORK.parent, WORK, path):
        if parent.is_symlink() or parent.is_junction():
            raise Refusal("Generated sandbox paths cannot redirect writes")
    if path.exists():
        shutil.rmtree(path)
    path.mkdir(parents=True)


def stage_runtime(args, evidence, intake=False):
    runtime = WORK / "runtime"
    staged = WORK / "staged-mod"
    # Copy only game inputs and Core. Never follow the installed Mods junction.
    reset_work_directory(runtime)
    reset_work_directory(staged)
    for path in args.game_dir.iterdir():
        if path.is_file():
            shutil.copy2(path, runtime / path.name)
    for relative in ("Data/Core", "Rendering", "vulkan"):
        copy_tree(args.game_dir / relative, runtime / relative)
    if (runtime / "Mods").exists() and {path.name for path in (runtime / "Mods").iterdir()} - {"IsoRim", "Harmony"}:
        raise Refusal("The sandbox contains an optional package")
    for relative in ("About", "Defs", "Models", "Textures", "LUTs", "1.7"):
        copy_tree(ROOT / relative, staged / relative)
    product = WORK / "build/1.7/bin/IsoRim.dll"
    shutil.copy2(product, staged / "1.7/Assemblies/IsoRim.dll")
    if intake:
        from prepared_models.intake_fixtures import stage_intake
        for version in ("1.5", "1.6"):
            copy_tree(ROOT / version, staged / version)
        stage_intake(staged, runtime / "Mods/NormalProofExternal")
        frozen_files = ["About/LoadFolders.xml", "Defs/Models.xml", "1.7/Defs/Models.xml", "1.7/Defs/WornGear.xml"]
        (evidence / "intake-frozen-inputs.json").write_text(json.dumps({
            "nonce": evidence.name, "files": {path: digest(ROOT / path) for path in frozen_files},
            "models": [node.findtext("defName") for node in ET.parse(ROOT / "1.7/Defs/Models.xml").getroot()],
            "gear": [node.findtext("defName") for node in ET.parse(ROOT / "1.7/Defs/WornGear.xml").getroot()],
        }, indent=2))
    # An opaque white texel owned by the proof, staged before the immutable runtime manifest.
    # It avoids all Core collection/sprite lookups for the texture framebuffer oracle.
    neutral = staged / "1.7/Textures/Diagnostics/NormalProofNeutral.png"
    neutral.parent.mkdir(parents=True, exist_ok=True)
    def chunk(kind, data):
        return struct.pack(">I", len(data)) + kind + data + struct.pack(">I", zlib.crc32(kind + data))
    neutral.write_bytes(b"\x89PNG\r\n\x1a\n" + chunk(b"IHDR", struct.pack(">IIBBBBB", 1, 1, 8, 6, 0, 0, 0))
                        + chunk(b"IDAT", zlib.compress(b"\x00\xff\xff\xff\xff")) + chunk(b"IEND", b""))
    copy_tree(staged, runtime / "Mods/IsoRim")
    harmony_mod = runtime / "Mods/Harmony"
    (harmony_mod / "About").mkdir(parents=True, exist_ok=True)
    (harmony_mod / "Assemblies").mkdir(exist_ok=True)
    (harmony_mod / "About/About.xml").write_text(
        "<ModMetaData><name>Harmony</name><author>pardeike</author><packageId>brrainz.harmony</packageId>"
        "<supportedVersions><li>1.7</li></supportedVersions></ModMetaData>")
    shutil.copy2(args.harmony, harmony_mod / "Assemblies/0Harmony.dll")
    save = evidence / "savedata"
    (save / "Config").mkdir(parents=True)
    (save / "Config/Prefs.xml").write_text(
        "<PrefsData><devMode>True</devMode><screenWidth>960</screenWidth><screenHeight>640</screenHeight>"
        "<fullscreen>False</fullscreen><runInBackground>True</runInBackground></PrefsData>")
    (save / "Config/ModsConfig.xml").write_text(
        "<ModsConfigData><version>1.7.4980 rev60</version><activeMods>"
        "<li>brrainz.harmony</li><li>ludeon.rimworld</li><li>connor.isorim</li>"
        + ("<li>isorim.normalproof.external</li>" if intake else "") +
        "</activeMods><knownExpansions><li>ludeon.rimworld</li></knownExpansions></ModsConfigData>")
    (save / "IsoRimProbe.txt").write_text("# Normal proof controller\n")
    return runtime, save, product


@contextmanager
def sandbox_lock():
    WORK.mkdir(parents=True, exist_ok=True)
    path = WORK / "runtime.lock"
    try:
        lock = path.open("x")
    except FileExistsError as error:
        raise Refusal("The normal test sandbox is already in use") from error
    try:
        with lock:
            lock.write(str(os.getpid()))
            lock.flush()
            yield
    finally:
        path.unlink()


def static_frame_requirements(cases):
    """Fixed case/control matrix, independent of reported rows and shader state."""
    required = {}
    for case in cases:
        rows = {}
        for placement in ("standing", "scaled", "laid", "mirror"):
            for surface in ("solid", "texture"):
                stem = placement + "-" + surface
                if case == "StaticNormalCases.GpuAuthoredDirection":
                    rows["direction-" + stem + "-t0-l0"] = (True, 0, 0, 0, False)
                elif case == "StaticNormalCases.ProductionLighting":
                    for fault in range(3):
                        rows[f"lighting-{stem}-t0-l{fault}"] = (False, 0, fault, fault, False)
                    rows["lighting-" + stem + "-t0-l0reset"] = (False, 0, 0, 0, False)
                    rows["lighting-" + stem + "-t0-l0legacy"] = (False, 0, 0, 1, True)
                elif case == "StaticNormalCases.OracleControls":
                    rows["direction-" + stem + "-t1-l0"] = (True, 1, 0, 1, False)
                    for fault in (1, 2):
                        rows[f"direction-{stem}-t0-l{fault}"] = (True, 0, fault, 0, False)
        if case == "StaticNormalCases.ProductionLighting":
            for surface in ("solid", "texture"):
                rows["lighting-legacy-oblique-" + surface + "-t0-l0legacy"] = (False, 0, 0, 1, True)
        if case == "StaticNormalCases.GeometricResponsibilities":
            rows = {"lighting-standing-solid-t0-l0own-cell": (False, 0, 0, 0, False),
                    "lighting-standing-solid-t0-l0cutaway": (False, 0, 0, 1, False)}
        if rows:
            required[case] = rows
    return required


def verify_static_pixels(evidence, cases, request, pid, started):
    """Bind every required observation to fresh raw GPU bytes and its PNG."""
    captures = []
    serials = set()
    for case, required in static_frame_requirements(cases).items():
        path = evidence / (case + ".json")
        report = json.loads(path.read_text())
        if path.stat().st_mtime < started or report.get("caseId") != case or report.get("passed") is not True:
            raise Refusal("A static pixel report is stale or invalid")
        rows = report["readings"]
        if case.endswith("GeometricResponsibilities"):
            rows = [rows["ownCell"], rows["cutaway"]]
        if len(rows) != len(required) or {row["name"] for row in rows} != set(required):
            raise Refusal("The required framebuffer matrix is incomplete, duplicated or relabelled")
        for row in rows:
            diagnostic, transport, lighting, expected, legacy = required[row["name"]]
            if (row["diagnostic"], row["transportFault"], row["lightingFault"], row["expectedIndex"]) != (diagnostic, transport, lighting, expected):
                raise Refusal("The requested framebuffer control differs from its required matrix")
            if (row["observedDiagnostic"], row["observedTransportFault"], row["observedLightingFault"], row["lightMode"], row["normalSpace"]) != (int(diagnostic), transport, lighting, 1, 0 if legacy else 1):
                raise Refusal("The observed framebuffer controls differ")
            if (row["width"], row["height"]) != (1024, 256) or row["backend"] != "RimWorldWaylith.Rendering.CameraTextureBaker":
                raise Refusal("The approved framebuffer path differs")
            serial = row["gpuFrameCompleted"]
            if serial != row["gpuFrameBefore"] + 1 or serial in serials or row["unityFrame"] <= 0:
                raise Refusal("A framebuffer has no unique completed GPU frame")
            serials.add(serial)
            if row["textured"] and (row["textureName"], row["textureWidth"], row["textureHeight"]) != ("NormalProofNeutral", 1, 1):
                raise Refusal("The texture oracle did not use its staged neutral texel")
            files = {}
            for kind in ("raw", "png"):
                relative = row[kind + "Path"]
                capture = evidence / relative
                if relative != row["name"] + (".rgba" if kind == "raw" else ".png") or capture.is_symlink() or capture.resolve().parent != evidence.resolve():
                    raise Refusal("A framebuffer path escaped its fresh evidence directory")
                if capture.stat().st_mtime < started or digest(capture) != row[kind + "Hash"]:
                    raise Refusal("A framebuffer hash is stale or differs")
                files[kind] = capture
            raw = files["raw"].read_bytes()
            png = files["png"].read_bytes()
            if len(raw) != 1024 * 256 * 4 or png[:8] != b"\x89PNG\r\n\x1a\n" or struct.unpack(">II", png[16:24]) != (1024, 256):
                raise Refusal("The framebuffer dimensions or PNG differ")
            patches = [[list(raw[((128 + dy) * 1024 + slot * 256 + 128 + dx) * 4:((128 + dy) * 1024 + slot * 256 + 128 + dx) * 4 + 4])
                        for dy in range(-1, 2) for dx in range(-1, 2)] for slot in range(4)]
            if patches != row["patches"]:
                raise Refusal("Reported patch samples differ from raw GPU bytes")
            if any(pixel[3] != 255 or pixel[:3] == [255, 0, 255] for patch in patches for pixel in patch):
                raise Refusal("An interior framebuffer patch is empty or nonopaque")
            if any(max(abs(pixel[c] - patch[0][c]) for pixel in patch for c in range(3)) > 2 for patch in patches):
                raise Refusal("An interior patch contains an edge or varying response")
            differences = [max(abs(a[c] - b[c]) for a, b in zip(patches[0], patch) for c in range(3)) for patch in patches[1:]]
            if differences != row["differences"] or differences[expected] > 2 or row["authoredComparisonPassed"] != (differences[0] <= 2):
                raise Refusal("The framebuffer comparison differs or fails")
            if expected and differences[0] <= 2:
                raise Refusal("A required framebuffer fault silently passed")
            if not diagnostic and any(abs(patches[i][0][c] - patches[j][0][c]) < 12 for i in range(1, 4) for j in range(i + 1, 4) for c in range(3)):
                raise Refusal("Ordinary lighting references fail the 12/255 separation")
            captures.append({"caseId": case, "name": row["name"], "gpuFrame": serial,
                             "rawSha256": row["rawHash"], "pngSha256": row["pngHash"],
                             "reportSha256": digest(path), "differences": differences})
    if captures:
        (evidence / "framebuffer-receipt.json").write_text(json.dumps({
            "nonce": request["nonce"], "head": request["head"], "assemblyHash": request["assemblyHash"],
            "processId": pid, "runtimeInputManifestSha256": digest(evidence / "runtime-inputs.json"),
            "captures": captures}, indent=2))
    return captures


def runtime_cases(args, evidence, protected, cases, head):
    with sandbox_lock():
        run_child(["powershell.exe", "-NoProfile", "-Command",
                   "if (Get-Process -Name RimWorld -ErrorAction SilentlyContinue) { exit 2 }"],
                  evidence, "game-exclusion", protected)
        intake = any(case.startswith("AssetIntakeCases.") for case in cases)
        runtime, save, product = stage_runtime(args, evidence, intake=intake)
        validator_results = validate_staged_intake(evidence, protected) if intake else None
        modules = {name: digest(runtime / name) for name in [*FINGERPRINTS, "System.Private.CoreLib.dll", "coreclr.dll"]}
        modules["0Harmony.dll"] = HARMONY_HASH
        request = {"nonce": evidence.name, "head": head, "assemblyHash": digest(product), "caseIds": cases,
                   "outputDirectory": str(evidence), "fixtureDirectory": str(WORK / "fixtures" / evidence.name),
                   "createdUtc": datetime.now(timezone.utc).isoformat()}
        Path(request["fixtureDirectory"]).mkdir(parents=True)
        (save / "IsoRimNormalRequest.json").write_text(json.dumps(request))
        runtime_inputs = {path: digest(path) for path in runtime.rglob("*") if path.is_file()}
        runtime_manifest = evidence / "runtime-inputs.json"
        runtime_manifest.write_text(json.dumps({str(path): value for path, value in runtime_inputs.items()}, indent=2))
        started = time.time()
        with (evidence / "runtime.stdout.log").open("wb") as stdout, (evidence / "runtime.stderr.log").open("wb") as stderr:
            environment = dict(os.environ)
            environment["DOTNET_STARTUP_HOOKS"] = str(runtime / "Mods/IsoRim/1.7/Assemblies/IsoRim.dll")
            environment["ISORIM_NORMAL_REQUEST"] = str(save / "IsoRimNormalRequest.json")
            child = subprocess.Popen([str(runtime / "RimWorld.exe"), "-quicktest", "-isorim3d", f"-savedatafolder={save}"],
                                     cwd=runtime, stdout=stdout, stderr=stderr, env=environment)
            try:
                child.wait(timeout=args.timeout)
            except subprocess.TimeoutExpired as error:
                child.terminate()  # Only the process this runner created.
                child.wait(timeout=30)
                raise Refusal("The owned runtime timed out without proof") from error
            finally:
                verify_snapshot({**protected, **runtime_inputs})
                (evidence / "runtime-process.json").write_text(json.dumps({
                    "pid": child.pid, "argv": child.args, "exit": child.returncode, "started": started,
                    "ended": time.time(), "protectedInputs": len(protected), "runtimeInputs": len(runtime_inputs),
                    "expectedModules": modules, "assemblyHash": request["assemblyHash"],
                    "runtimeInputManifestSha256": digest(runtime_manifest)}, indent=2))
        if child.returncode:
            raise Refusal(f"The runtime exited {child.returncode}")
        active = ET.parse(save / "Config/ModsConfig.xml").findall(".//activeMods/li")
        expected_active = {"brrainz.harmony", "ludeon.rimworld", "connor.isorim"}
        if intake:
            expected_active.add("isorim.normalproof.external")
        if {node.text for node in active} != expected_active or len(active) != len(expected_active):
            raise Refusal("The runtime active package manifest changed")
        proofs = verify_proofs(list(evidence.glob("NormalProof-*.json")), cases, request, modules, child.pid, started)
        verify_static_pixels(evidence, cases, request, child.pid, started)
        if validator_results and any((row["exit"] == 0) != row["expectedSuccess"] or not row["diagnosticMatched"] for row in validator_results["controls"]):
            raise Refusal("The staged validator accepted an invalid input or rejected the passing control")
        if validator_results and any(row["exit"] != row["cli"] or not row["diagnosticMatched"] or not row["bitMatched"] for row in validator_results["recovery"]):
            raise Refusal("A recovery full-CLI matrix control failed")
        if validator_results:
            loader_path = evidence / "RecoveryLoader.json"
            observed = json.loads(loader_path.read_text())
            required = {row["id"]: row for row in validator_results["recovery"]}
            if loader_path.stat().st_mtime < started or len(observed) != len(required) or {row["id"] for row in observed} != set(required):
                raise Refusal("The actual recovery observations are stale, missing, duplicated or relabelled")
            if any(not row["passed"] or row["actualLoader"] != required[row["id"]]["loader"]
                   or row["inputHash"] != required[row["id"]]["actualInputHash"] for row in observed):
                raise Refusal("The actual loader differs from the required recovery inputs or outcomes")
            (evidence / "recovery-receipt.json").write_text(json.dumps({
                "nonce": evidence.name, "head": head, "assemblyHash": request["assemblyHash"], "processId": child.pid,
                "subcaseIds": sorted(required), "runtimeInputManifestSha256": digest(runtime_manifest),
                "validatorReportSha256": digest(evidence / "validator-results.json"),
                "loaderReportSha256": digest(loader_path)}, indent=2))
        return proofs


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument("command", choices=["env", "verify-restore", "t0", "t1", "ci"])
    parser.add_argument("--story", default="ISORIM-003.S01")
    parser.add_argument("--epic-ref")
    parser.add_argument("--head", default="HEAD")
    parser.add_argument("--game-dir", type=Path, default=GAME)
    parser.add_argument("--dotnet", type=Path, default=SDK)
    parser.add_argument("--harmony", type=Path, default=HARMONY)
    parser.add_argument("--assets", type=Path)
    parser.add_argument("--target", choices=["1.5", "1.6", "1.7"], default="1.7")
    parser.add_argument("--timeout", type=int, default=420)
    args = parser.parse_args()
    try:
        verify_sdk(args.dotnet, acceptance=args.command != "ci")
        if args.command != "ci":
            verify_game(args.game_dir)
            verify_harmony(args.harmony)
        if args.command == "env":
            print(json.dumps({"sdk": str(args.dotnet), "sdkVersion": "10.0.402", "game": verify_game(args.game_dir), "harmony": digest(args.harmony)}))
            return 0
        evidence = EVIDENCE / uuid.uuid4().hex
        rule = prepare_evidence(ROOT, evidence)
        base = "c43cd431232f0ba3e6a97a1911e9f4a5ef76917f"
        protected = protected_inputs(base, args.game_dir if args.command != "ci" else None, args.harmony)
        (evidence / "protected-inputs.json").write_text(json.dumps({str(path): value for path, value in protected.items()}, indent=2))
        if args.command == "verify-restore":
            if args.assets is None:
                raise Refusal("verify-restore requires --assets")
            verify_restore(args, evidence, protected, installed=args.target == "1.7")
            print(f"Verified restored references: {evidence}")
            return 0
        if not args.epic_ref:
            raise Refusal("An explicit epic ref is required")
        epic = git("rev-parse", "--verify", args.epic_ref + "^{commit}").decode().strip()
        head = git("rev-parse", "--verify", args.head + "^{commit}").decode().strip()
        expected_epic = git("rev-parse", "--verify", "feat/ISORIM-003-prepared-models-render-correctly^{commit}").decode().strip()
        if head != git("rev-parse", "HEAD").decode().strip() or epic != expected_epic:
            raise Refusal("The execution head or epic base differs from the commission")
        git("merge-base", "--is-ancestor", epic, head)
        if git("status", "--porcelain", "--untracked-files=all"):
            raise Refusal("The gate requires a clean committed tree")
        changes = git("diff", "--name-status", "-z", "--no-renames", epic, head, "--")
        manifest = case_manifest()
        story = args.story
        if args.command in ("t1", "ci"):
            story = f"ISORIM-003.S{max(case['story'] for case in manifest['cases'].values()):02d}"
        cases = select_cases(changes, story, manifest)
        if args.command == "t1":
            cases = sorted(manifest["cases"])
        selection = {"head": head, "epic": epic, "story": story, "paths": changed_paths(changes),
                     "caseIds": cases, "hostControls": manifest["hostCases"], "ignoreProof": rule, "nonce": evidence.name}
        selection["pathCases"] = {path: select_cases(("M\0" + path + "\0").encode(), story, manifest)
                                  for path in selection["paths"]}
        (evidence / "selection.json").write_text(json.dumps(selection, indent=2))
        print(json.dumps(selection), flush=True)
        run_child([sys.executable, "-m", "unittest", "discover", "-s", "tests", "-p", "test_prepared_models_gate.py"],
                  evidence, "host-controls", protected)
        host_results = verify_host_controls(manifest, evidence)
        compile_target(args, evidence, protected, installed=args.command != "ci")
        proofs = {}
        if args.command != "ci":
            proofs = runtime_cases(args, evidence, protected, cases, head)
        if args.command in ("t1", "ci"):
            for target in ("1.5", "1.6"):
                compile_target(args, evidence, protected, target, installed=False)
        if args.command == "t1":
            for target in ("1.7", "1.6"):
                format_sources(args, evidence, protected, target)
        if git("rev-parse", "HEAD").decode().strip() != head or git("status", "--porcelain", "--untracked-files=all"):
            raise Refusal("The execution tree changed during validation")
        receipt = dict(selection, command=args.command, acceptance=args.command != "ci", proofs=proofs, hostResults=host_results,
                       compatibilityOnly=args.command == "ci")
        if (evidence / "recovery-receipt.json").is_file():
            receipt["recoveryReceiptSha256"] = digest(evidence / "recovery-receipt.json")
        if (evidence / "framebuffer-receipt.json").is_file():
            receipt["framebufferReceiptSha256"] = digest(evidence / "framebuffer-receipt.json")
        (evidence / "receipt.json").write_text(json.dumps(receipt, indent=2))
        print(f"PASS {args.command}: {len(proofs)} runtime cases. Evidence: {evidence}")
        return 0
    except (Refusal, OSError, ValueError, subprocess.SubprocessError) as error:
        print(f"REFUSED: {error}", file=sys.stderr)
        return 1


if __name__ == "__main__":
    sys.exit(main())
