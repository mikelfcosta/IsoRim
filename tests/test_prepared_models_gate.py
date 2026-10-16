"""Host controls for the prepared-model acceptance runner."""

from copy import deepcopy
import json
from pathlib import Path
import subprocess
import sys
import tempfile
import unittest
from unittest import mock
import uuid

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / "tools"))


class GateHostTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        import prepared_models_gate as gate

        cls.evidence = gate.EVIDENCE / uuid.uuid4().hex
        gate.prepare_evidence(ROOT, cls.evidence)

    def test_output_isolation(self):
        import prepared_models_gate as gate

        args = gate.build_properties("1.7", ROOT / "artifacts/prepared-models/build", gate.GAME, gate.HARMONY)
        for name in ("OutputPath", "BaseIntermediateOutputPath", "IntermediateOutputPath", "MSBuildProjectExtensionsPath"):
            value = next(arg.split("=", 1)[1] for arg in args if arg.startswith(f"-p:{name}="))
            self.assertTrue(Path(value).is_absolute())
            self.assertTrue(Path(value).is_relative_to(ROOT / "artifacts/prepared-models/build"))
        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            directory = Path(tmp)
            protected = directory / "protected"
            protected.write_text("original")
            with self.assertRaises(gate.Refusal):
                gate.run_child([sys.executable, "-c", "from pathlib import Path; Path('protected').write_text('changed')"],
                               directory, "mutation", {protected: gate.digest(protected)}, cwd=directory)
            gate.run_child([sys.executable, "-c", "print('captured')"], directory, "success", {})
            self.assertEqual((directory / "success.stdout.log").read_text().strip(), "captured")
            with self.assertRaises(gate.Refusal):
                gate.run_child([sys.executable, "-c", "raise SystemExit(7)"], directory, "failed-child", {})
            with self.assertRaises(gate.Refusal):
                gate.reset_work_directory(directory)
            calls = []

            def formatter(argv, **kwargs):
                calls.append((argv, kwargs["env"]))
                return subprocess.CompletedProcess(argv, 0)

            with mock.patch("subprocess.run", side_effect=formatter):
                from argparse import Namespace
                gate.format_sources(Namespace(dotnet=gate.SDK, game_dir=gate.GAME, harmony=gate.HARMONY), directory, {})
            self.assertEqual(len(calls), 2)
            for argv, environment in calls:
                self.assertIn("--verify-no-changes", argv)
                self.assertIn("--no-restore", argv)
                for name in ("OutputPath", "BaseIntermediateOutputPath", "IntermediateOutputPath", "MSBuildProjectExtensionsPath"):
                    self.assertTrue(Path(environment[name]).is_relative_to(gate.WORK / "build"))

    def test_ignore_before_capture(self):
        import prepared_models_gate as gate

        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            root = Path(tmp)
            subprocess.run(["git", "init", "--quiet", str(root)], check=True)
            evidence = root / ".testruns/ISORIM-003/nonce"
            with self.assertRaises(gate.Refusal):
                gate.prepare_evidence(root, evidence)
            self.assertFalse(evidence.exists())
            (root / ".gitignore").write_text("/.testruns/ISORIM-003/\n")
            gate.prepare_evidence(root, evidence)
            self.assertTrue(evidence.is_dir())
            for path in (root / "artifacts/report.json", root.parent / "escape"):
                with self.assertRaises(gate.Refusal):
                    gate.prepare_evidence(root, path)
            tracked = root / ".testruns/ISORIM-003/tracked"
            tracked.write_text("tracked")
            subprocess.run(["git", "-C", str(root), "add", "-f", str(tracked)], check=True)
            with self.assertRaises(gate.Refusal):
                gate.prepare_evidence(root, evidence)

    def test_fail_closed_selection_and_receipts(self):
        import prepared_models_gate as gate

        manifest = gate.case_manifest()
        data = {
            "NormalDataCases.AuthoredIndices",
            "NormalDataCases.InvalidTriangleFallback",
            "NormalDataCases.GeometricChannelsPreserved",
            "NormalDataCases.DerivedModels",
        }
        selected = gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S01", manifest)
        self.assertEqual(set(selected), data)
        self.assertEqual(set(manifest["cases"]), data)  # Later stories have no available cases at this S01 head.
        for missing in sorted(data):
            shortened = deepcopy(manifest)
            del shortened["cases"][missing]
            shortened["groups"]["data"].remove(missing)
            with self.subTest(removed_required_case=missing):
                with self.assertRaises(gate.Refusal):
                    gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S01", shortened)
                with self.assertRaises(gate.Refusal):
                    gate.select_cases(b"M\0tools/prepared_models/cases.json\0", "ISORIM-003.S01", shortened)
            for relocate in (False, True):
                ungrouped = deepcopy(manifest)
                ungrouped["groups"]["data"].remove(missing)
                if relocate:
                    ungrouped["groups"]["static"].append(missing)
                with self.subTest(required_group_case=missing, relocated=relocate):
                    with self.assertRaises(gate.Refusal):
                        gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S01", ungrouped)
        gate.verify_registry(selected, manifest)
        with self.assertRaises(gate.Refusal):
            gate.verify_registry(["NormalDataCases.Missing"], manifest)
        for stream in (b"", b"M\0Source17/Models.cs", b"Q\0Source17/Models.cs\0", b"R100\0Source17/Models.cs\0"):
            with self.assertRaises(gate.Refusal):
                gate.select_cases(stream, "ISORIM-003.S01", manifest)
        for story in ("S01", "ISORIM-003.S00", "ISORIM-003.S99"):
            with self.assertRaises(gate.Refusal):
                gate.select_cases(b"M\0Source17/Models.cs\0", story, manifest)
        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            directory = Path(tmp)
            log = directory / "host-controls.stderr.log"
            log.write_text("Ran 4 tests in 0.1s\n\nOK\n")
            self.assertEqual(gate.verify_host_controls(manifest, directory)["passed"], 4)
            for summary in ("Ran 0 tests in 0.0s\n\nOK\n", "Ran 4 tests in 0.1s\n\nOK (skipped=4)\n"):
                log.write_text(summary)
                with self.assertRaises(gate.Refusal):
                    gate.verify_host_controls(manifest, directory)
        for changes in (b"M\0README.txt\0", b"D\0unknown.cs\0",
                        b"R100\0Source17/Models.cs\0unknown.cs\0"):
            with self.assertRaises(gate.Refusal):
                gate.select_cases(changes, "ISORIM-003.S01", manifest)
        self.assertEqual(set(gate.select_cases(b"D\0Source17/Models.cs\0", "ISORIM-003.S01", manifest)), data)
        self.assertEqual(set(gate.select_cases(b"R100\0Source17/Models.cs\0Source17/Models.cs\0", "ISORIM-003.S01", manifest)), data)
        manifest["groups"]["static"] = ["StaticNormalCases.Example"]
        manifest["cases"]["StaticNormalCases.Example"] = {"story": 1}
        with self.assertRaises(gate.Refusal):
            gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S01", manifest)
        manifest = gate.case_manifest()
        del manifest["cases"][next(iter(data))]
        with self.assertRaises(gate.Refusal):
            gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S01", manifest)
        request = {"nonce": "fresh", "head": "head", "assemblyHash": "product"}
        proof = dict(request, caseId="case", passed=True, modules={"module": "hash"},
                     runtime="10.0.10-waylithclr.dev", gameVersion="1.7.4980.60", processId=42, evidencePaths=[])
        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            path = Path(tmp) / "proof.json"
            proof["engineLogRoot"] = tmp
            path.write_text(json.dumps(proof))
            gate.verify_proofs([path], ["case"], request, {"module": "hash"}, 42, 0)
            for change in ({"nonce": "stale"}, {"passed": False}, {"caseId": "unknown"},
                           {"modules": {}}, {"processId": 43}, {"skipped": True}, {"engineLogRoot": str(ROOT)}):
                path.write_text(json.dumps(dict(proof, **change)))
                with self.assertRaises(gate.Refusal):
                    gate.verify_proofs([path], ["case"], request, {"module": "hash"}, 42, 0)
            for paths in ([], [path, path]):
                with self.assertRaises(gate.Refusal):
                    gate.verify_proofs(paths, ["case"], request, {"module": "hash"}, 42, 0)
            path.write_text(json.dumps(proof))
            with self.assertRaises(gate.Refusal):
                gate.verify_proofs([path], ["case"], request, {"module": "hash"}, 42, path.stat().st_mtime + 1)
            for key in ("head", "assemblyHash", "runtime"):
                path.write_text(json.dumps(dict(proof, **{key: "changed"})))
                with self.assertRaises(gate.Refusal):
                    gate.verify_proofs([path], ["case"], request, {"module": "hash"}, 42, 0)
            path.write_text("{}")
            with self.assertRaises(gate.Refusal):
                gate.verify_proofs([path], ["case"], request, {}, 42, 0)

    def test_reference_identity(self):
        import prepared_models_gate as gate

        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            game = Path(tmp)
            (game / "Version.txt").write_text("1.7.4979 rev1")
            with self.assertRaises(gate.Refusal):
                gate.verify_game(game)
            (game / "Version.txt").write_text("1.7.4980 rev60")
            for name in gate.FINGERPRINTS:
                (game / name).write_bytes(b"changed game fingerprint")
            (game / "RimWorld.runtimeconfig.json").write_text(json.dumps({"runtimeOptions": {"includedFrameworks": [
                {"name": "Microsoft.NETCore.App", "version": "10.0.10-waylithclr.dev"}]}}))
            with self.assertRaises(gate.Refusal):
                gate.verify_game(game)
            assets = {"libraries": {"Krafs.Rimworld.Ref/1.7.4979-alpha": {}, "Lib.Harmony/3.0.0-rc.3": {}}}
            with self.assertRaises(gate.Refusal):
                gate.verify_assets(assets, "1.7", installed=True)
            gate.verify_assets(assets, "1.7", installed=False)
            gate.verify_assets({"libraries": {"Lib.Harmony/3.0.0-rc.3": {}}}, "1.7", installed=True)
            with self.assertRaises(gate.Refusal):
                gate.verify_harmony(game / "RimWorld.dll")
            for target, version in (("1.5", "1.5.4409"), ("1.6", "1.6.4871")):
                libraries = {f"Krafs.Rimworld.Ref/{version}": {}, "Lib.Harmony/2.3.3": {},
                             "Microsoft.NETFramework.ReferenceAssemblies/1.0.3": {}}
                gate.verify_assets({"libraries": libraries}, target)
                libraries["Lib.Harmony/2.4.0"] = {}
                with self.assertRaises(gate.Refusal):
                    gate.verify_assets({"libraries": libraries}, target)
        with self.assertRaises(gate.Refusal):
            gate.verify_sdk(Path("dotnet"))
        with self.assertRaises(gate.Refusal):
            gate.verify_sdk(Path(sys.executable))
        with mock.patch("subprocess.run", return_value=subprocess.CompletedProcess([], 0, "10.0.402\n", "")):
            gate.verify_sdk(gate.SDK)
        with mock.patch("subprocess.run", return_value=subprocess.CompletedProcess([], 0, "6.0.100\n", "")):
            with self.assertRaises(gate.Refusal):
                gate.verify_sdk(gate.SDK)


if __name__ == "__main__":
    unittest.main()
