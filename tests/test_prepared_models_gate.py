"""Host controls for the prepared-model acceptance runner."""

from copy import deepcopy
import json
import hashlib
from pathlib import Path
import subprocess
import struct
import sys
import tempfile
import unittest
from unittest import mock
import uuid
import zlib

ROOT = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(ROOT / "tools"))

# Independent literal oracle: do not derive this set from the fixture generator.
RECOVERY_EXPECTED = """R01-unit|0|A|-
R01-original|0|A|-
R02-positive|0|A|80000000
R02-negative|0|A|BF800000
R03-leading-dot|0|A|-
R03-trailing-dot|0|A|-
R03-leading-zeros|0|A|-
R04-lower|0|A|3F800000
R04-signed|0|A|3F800000
R04-negative|0|A|3F800000
R05-empty|1|F|-
R05-sign|1|F|-
R05-dot|1|F|-
R05-exponent|1|F|-
R05-exponent-sign|1|F|-
R05-signs|1|F|-
R05-dots|1|F|-
R06-integer|1|F|-
R06-fraction|1|F|-
R06-exponent|1|F|-
R07-arabic|1|F|-
R07-fullwidth|1|F|-
R07-arabic-exp|1|F|-
R07-fullwidth-exp|1|F|-
R07-minus|1|F|-
R08-comma|1|F|-
R08-group|1|F|-
R08-currency|1|F|-
R08-parens|1|F|-
R08-trailing-sign|1|F|-
R08-hex|1|F|-
R09-nan|1|F|-
R09-nan-lower|1|F|-
R09-nan-sign|1|F|-
R09-inf|1|F|-
R09-inf-lower|1|F|-
R09-inf-negative|1|F|-
R09-overflow|1|F|-
R10-zero|1|F|-
R10-signed-zero|1|F|-
R10-underflow|1|F|-
R11-magnitude|1|F|-
R12-below|1|F|-
R12-at|1|F|-
R12-above|0|A|-
R12-next-below|1|F|-
R12-next-above|0|A|-
R12-diagonal-below|1|F|-
R12-diagonal-above|0|A|-
R13-below|0|A|3F800000
R13-tie|0|A|3F800000
R13-above|0|A|3F800001
R14-max-positive-vn|1|F|7F7FFFFF
R14-max-positive-v|0|A|7F7FFFFF
R14-max-positive-vt|0|A|7F7FFFFF
R14-max-positive-Kd|0|A|7F7FFFFF
R14-max-negative-vn|1|F|FF7FFFFF
R14-max-negative-v|0|A|FF7FFFFF
R14-max-negative-vt|0|A|FF7FFFFF
R14-max-negative-Kd|0|A|FF7FFFFF
R14-overflow-positive-vn|1|F|7F800000
R14-overflow-positive-v|1|A|7F800000
R14-overflow-positive-vt|1|A|7F800000
R14-overflow-positive-Kd|1|A|7F800000
R14-overflow-negative-vn|1|F|FF800000
R14-overflow-negative-v|1|A|FF800000
R14-overflow-negative-vt|1|A|FF800000
R14-overflow-negative-Kd|1|A|FF800000
R14-subnormal-positive-vn|1|F|00000001
R14-subnormal-positive-v|0|A|00000001
R14-subnormal-positive-vt|0|A|00000001
R14-subnormal-positive-Kd|0|A|00000001
R14-subnormal-negative-vn|1|F|80000001
R14-subnormal-negative-v|0|A|80000001
R14-subnormal-negative-vt|0|A|80000001
R14-subnormal-negative-Kd|0|A|80000001
R14-underflow-positive-vn|1|F|00000000
R14-underflow-positive-v|0|A|00000000
R14-underflow-positive-vt|0|A|00000000
R14-underflow-positive-Kd|0|A|00000000
R14-underflow-negative-vn|1|F|80000000
R14-underflow-negative-v|0|A|80000000
R14-underflow-negative-vt|0|A|80000000
R14-underflow-negative-Kd|0|A|80000000
R14-min-normal-positive-vn|1|F|00800000
R14-min-normal-positive-v|0|A|00800000
R14-min-normal-positive-vt|0|A|00800000
R14-min-normal-positive-Kd|0|A|00800000
R14-min-normal-negative-vn|1|F|80800000
R14-min-normal-negative-v|0|A|80800000
R14-min-normal-negative-vt|0|A|80800000
R14-min-normal-negative-Kd|0|A|80800000
R14-normal-mid-below|1|F|007FFFFF
R14-normal-mid-below-negative|1|F|807FFFFF
R14-normal-mid-tie|1|F|00800000
R14-normal-mid-tie-negative|1|F|80800000
R14-normal-mid-above|1|F|00800000
R14-normal-mid-above-negative|1|F|80800000
R14-sub-mid-below|1|F|00000000
R14-sub-mid-below-negative|1|F|80000000
R14-sub-mid-tie|1|F|00000000
R14-sub-mid-tie-negative|1|F|80000000
R14-sub-mid-above|1|F|00000001
R14-sub-mid-above-negative|1|F|80000001
R14-overflow-mid-below|1|F|7F7FFFFF
R14-overflow-mid-below-negative|1|F|FF7FFFFF
R14-overflow-mid-tie|1|F|7F800000
R14-overflow-mid-tie-negative|1|F|FF800000
R14-overflow-mid-above|1|F|7F800000
R14-overflow-mid-above-negative|1|F|FF800000
R14-huge-exponent-zero|1|F|00000000
R14-huge-exponent-negative-zero|1|F|80000000
R14-huge-exponent-underflow|1|F|00000000
R14-huge-exponent-overflow|1|F|7F800000
R14-long-leading-zeros|0|A|3F800000
R15-short|1|F|-
R15-later-valid|1|A|-
R15-invalid-first|1|F|-
R16-vn-numeric|1|A|-
R16-vn-junk|1|A|-
R16-v-surplus|1|A|-
R16-vt-junk|1|A|-
R16-kd-surplus|1|A|-
R16-kd-numeric|1|A|-
R16-vt-surplus|1|A|-
R16-vt-third|0|A|-
R17-spaces|0|A|-
R17-crlf|0|A|-
R17-cr|0|A|-
R17-bom|0|A|-
R17-comments|0|A|-
R18-nbsp-record|1|F|-
R18-nbsp-token|1|F|-
R18-nbsp-outer|1|A|-
R18-nul-trailing|1|A|-
R18-nul-embedded|1|F|-
R18-vt|1|F|-
R18-ff|1|F|-
R18-nel|1|F|-
R18-ls|1|F|-
R18-ps|1|F|-
R19-attached|1|F|-
R19-vn-inline|1|A|-
R19-face-inline|1|N|-
R19-mtl-inline|1|A|-
R20-bare|0|F|-
R20-uv|0|F|-
R20-mixed|0|F|-
R21-full|0|A|-
R21-no-uv|0|A|-
R21-negative|0|A|-
R21-leading|0|A|-
R22-underscore-position|1|N|-
R22-underscore-texture|1|A|-
R22-underscore-normal|1|F|-
R22-unicode-position|1|N|-
R22-unicode-texture|1|A|-
R22-unicode-normal|1|F|-
R22-exponent-position|1|N|-
R22-exponent-texture|1|A|-
R22-exponent-normal|1|F|-
R22-decimal-position|1|N|-
R22-decimal-texture|1|A|-
R22-decimal-normal|1|F|-
R22-empty-position|1|N|-
R22-empty-texture|0|A|-
R22-empty-normal|0|F|-
R22-zero-position|1|N|-
R22-zero-texture|1|A|-
R22-zero-normal|1|F|-
R22-positive-zero-position|1|N|-
R22-positive-zero-texture|1|A|-
R22-positive-zero-normal|1|F|-
R22-negative-zero-position|1|N|-
R22-negative-zero-texture|1|A|-
R22-negative-zero-normal|1|F|-
R23-min-position|1|N|-
R23-min-texture|1|A|-
R23-min-normal|1|F|-
R23-max-position|1|N|-
R23-max-texture|1|A|-
R23-max-normal|1|F|-
R23-below-min-position|1|N|-
R23-below-min-texture|1|A|-
R23-below-min-normal|1|F|-
R23-above-max-position|1|N|-
R23-above-max-texture|1|A|-
R23-above-max-normal|1|F|-
R23-forward-position|1|N|-
R23-forward-texture|1|A|-
R23-forward-normal|1|F|-
R23-too-negative-position|1|N|-
R23-too-negative-texture|1|A|-
R23-too-negative-normal|1|F|-
R23-leading-zeros-position|0|A|-
R23-long-overflow-position|1|N|-
R24-slash-surplus|1|A|-
R24-short-face|1|N|-
R25-v-underscore|1|A|-
R25-v-unicode|1|A|-
R25-v-overflow|1|A|-
R25-v-zero|0|A|-
R25-v-underflow|0|A|-
R25-v-finite|0|A|-
R25-vt-underscore|1|A|-
R25-vt-unicode|1|A|-
R25-vt-overflow|1|A|-
R25-vt-zero|0|A|-
R25-vt-underflow|0|A|-
R25-vt-finite|0|A|-
R25-rgb-underscore|1|A|-
R25-rgb-unicode|1|A|-
R25-rgb-overflow|1|A|-
R25-rgb-zero|0|A|-
R25-rgb-underflow|0|A|-
R25-rgb-finite|0|A|-
R26-underscore|1|A|-
R26-unicode|1|A|-
R26-overflow|1|A|-
R26-zero|0|A|-
R26-underflow|0|A|-
R26-finite|0|A|-
R26-short|1|A|-
R27-missing-mtl|1|A|-
R27-escaping-mtl|1|A|-
R27-undefined|1|A|-
R28-empty|1|N|-
R28-vertices|1|N|-
R29-collinear|1|N|-
R29-valid|0|A|-"""


class GateHostTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        import prepared_models_gate as gate

        cls.evidence = gate.EVIDENCE / uuid.uuid4().hex
        gate.prepare_evidence(ROOT, cls.evidence)
        print("Host evidence: " + str(cls.evidence), flush=True)

    def test_integration_completeness(self):
        import prepared_models_gate as gate

        # Host protocol control only. Actual acceptance also verifies every raw GPU source.
        paths = ("static-solid", "static-texture", "single-solid", "single-texture", "instanced-solid", "instanced-texture", "merged-solid-part0", "merged-solid-part1")
        tuples = []
        for path in paths:
            for pose in ("standing", "scaled", "laid", "mirror"):
                tuples.append({"tupleId": path + "-" + pose, "frames": [
                    {"diagnostic": diagnostic, "lighting": fault, "expected": expected}
                    for diagnostic, fault, expected in ((False, 0, 0), (False, 1, 1), (True, 1, 0), (False, 2, 2), (True, 2, 0), (False, 0, 0))]})
        self.assertEqual(gate.verify_lighting_coverage({"tuples": tuples}), 32)
        for index in range(32):
            for altered in (tuples[:index] + tuples[index+1:], tuples + [tuples[index]],
                            [dict(row, tupleId="relabeled") if i == index else row for i, row in enumerate(tuples)]):
                with self.assertRaises(gate.Refusal):
                    gate.verify_lighting_coverage({"tuples": altered})
        for changed in ([*tuples[0]["frames"][:-1]], [dict(frame, diagnostic=True) for frame in tuples[0]["frames"]],
                        [dict(frame, expected=0) for frame in tuples[0]["frames"]]):
            with self.assertRaises(gate.Refusal):
                gate.verify_lighting_coverage({"tuples": [dict(tuples[0], frames=changed), *tuples[1:]]})

    def test_artifact_identity(self):
        import prepared_models_gate as gate

        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            files = [Path(tmp) / name for name in ("committed.dll", "built.dll", "staged.dll")]
            for path in files:
                path.write_bytes(b"verified-build")
            self.assertEqual(gate.verify_artifact_files(files), hashlib.sha256(b"verified-build").hexdigest().upper())
            for path in files:
                path.write_bytes(b"stale-module")
                with self.assertRaises(gate.Refusal):
                    gate.verify_artifact_files(files)
                path.write_bytes(b"verified-build")
            files[-1].unlink()
            with self.assertRaises(gate.Refusal):
                gate.verify_artifact_files(files)

    def test_ci_is_not_acceptance(self):
        import prepared_models_gate as gate

        with mock.patch("subprocess.run", return_value=subprocess.CompletedProcess([], 0, "10.0.100\n", "")):
            gate.verify_sdk(Path("dotnet"), acceptance=False)
            with self.assertRaises(gate.Refusal):
                gate.verify_sdk(Path("dotnet"), acceptance=True)
        receipt = gate.result_receipt({"head": "committed"}, "ci", {}, {"passed": 8})
        self.assertEqual(receipt["classification"], "portable-compatibility-only")
        self.assertIs(receipt["acceptance"], False)
        self.assertIs(receipt["compatibilityOnly"], True)
        self.assertEqual(receipt["proofs"], {})
        with self.assertRaises(gate.Refusal):
            gate.result_receipt({}, "ci", {"case": {"passed": True}}, {})
        for status in (b"", b" M 1.5/Assemblies/IsoRim.dll\0 M 1.6/Assemblies/IsoRim.dll\0 M 1.7/Assemblies/IsoRim.dll\0"):
            with mock.patch("subprocess.run", return_value=subprocess.CompletedProcess([], 0, status, b"")):
                gate.verify_tree("ci")
                if status:
                    with self.assertRaises(gate.Refusal):
                        gate.verify_tree("t0")
        for status in (b" M Source17/Models.cs\0", b"?? untracked-input.obj\0", b"M  1.7/Assemblies/IsoRim.dll\0"):
            with mock.patch("subprocess.run", return_value=subprocess.CompletedProcess([], 0, status, b"")):
                with self.assertRaises(gate.Refusal):
                    gate.verify_tree("ci")

    def test_final_gate_selection(self):
        import prepared_models_gate as gate

        manifest = gate.case_manifest()
        baseline = "c43cd431232f0ba3e6a97a1911e9f4a5ef76917f"
        # A frozen integrated head can equal its epic ref. Whole selection uses base..head.
        self.assertEqual(gate.run_cases("t1", b"", "ISORIM-003.S05", manifest, baseline), sorted(manifest["cases"]))
        for base in (None, "", "HEAD", "5b5ef184"):
            with self.assertRaises(gate.Refusal):
                gate.run_cases("t1", b"", "ISORIM-003.S05", manifest, base)
        with self.assertRaises(gate.Refusal):
            gate.run_cases("t1", b"M\0unknown.cs\0", "ISORIM-003.S05", manifest, baseline)
        with self.assertRaises(gate.Refusal):
            gate.run_cases("t0", b"", "ISORIM-003.S05", manifest, None)
        help_result = subprocess.run([sys.executable, "tools/prepared_models_gate.py", "--help"],
                                     cwd=ROOT, capture_output=True, text=True, check=False)
        self.assertEqual(help_result.returncode, 0)
        self.assertIn("--base", help_result.stdout)

    def test_framebuffer_receipts(self):
        import prepared_models_gate as gate

        required = gate.static_frame_requirements(gate.case_manifest()["cases"])
        moving = gate.moving_frame_requirements(gate.case_manifest()["cases"])
        self.assertEqual({case: len(rows) for case, rows in moving.items()}, {
            "MovingNormalCases.DynamicTrueSpace": 4, "MovingNormalCases.InstanceMatrices": 16,
            "MovingNormalCases.MergedPartTransforms": 8, "MovingNormalCases.TexturesMarksAndFallback": 6,
            "MovingNormalCases.CacheAndBatchReuse": 1, "MovingNormalCases.GpuAgreement": 96,
            "MovingNormalCases.ProductionLighting": 96, "IntegratedNormalCases.AllPaths": 72})
        tuples = {path + "-" + pose for path in ("single-solid", "single-texture", "instanced-solid", "instanced-texture")
                  for pose in ("standing", "scaled", "laid", "mirror")}
        tuples |= {"merged-solid-" + pose + "-part" + str(part) for pose in ("standing", "scaled", "laid", "mirror") for part in (0, 1)}
        self.assertEqual(set(moving["MovingNormalCases.ProductionLighting"]), {
            "moving-ProductionLighting-" + item + "-lighting-" + control for item in tuples
            for control in ("t0-l0", "t0-l1", "t0-l2", "t0-l0reset")})
        self.assertEqual(set(moving["MovingNormalCases.GpuAgreement"]), {
            "moving-GpuAgreement-" + item + "-direction-" + control for item in tuples
            for control in ("t0-l0", "t1-l0", "t0-l1", "t0-l2")})
        self.assertEqual(set(moving["IntegratedNormalCases.AllPaths"]), {
            "moving-AllPaths-" + item + "-" + validity + "-direction-t0-l0"
            for item in tuples for validity in ("authored", "missing", "invalid")})
        self.assertEqual(set(required["IntegratedNormalCases.AllPaths"]), {
            "direction-integration-" + validity + "-" + pose + "-" + surface + "-t0-l0"
            for validity in ("authored", "missing", "invalid") for pose in ("standing", "scaled", "laid", "mirror") for surface in ("solid", "texture")})
        self.assertEqual({case: len(rows) for case, rows in required.items()}, {
            "StaticNormalCases.GpuAuthoredDirection": 8, "StaticNormalCases.ProductionLighting": 42,
            "StaticNormalCases.OracleControls": 24, "StaticNormalCases.GeometricResponsibilities": 2,
            "IntegratedNormalCases.AllPaths": 24})
        for case, rows in required.items():
            expected_controls = {
                "StaticNormalCases.GpuAuthoredDirection": {(True, 0, 0, 0, False)},
                "StaticNormalCases.ProductionLighting": {(False, 0, 0, 0, False), (False, 0, 1, 1, False), (False, 0, 2, 2, False), (False, 0, 0, 1, True)},
                "StaticNormalCases.OracleControls": {(True, 1, 0, 1, False), (True, 0, 1, 0, False), (True, 0, 2, 0, False)},
                "StaticNormalCases.GeometricResponsibilities": {(False, 0, 0, 0, False), (False, 0, 0, 1, False)},
                "IntegratedNormalCases.AllPaths": {(True, 0, 0, 0, False), (True, 0, 0, 1, False)},
            }[case]
            self.assertEqual(set(rows.values()), expected_controls)

        # Synthetic host protocol bytes, never an actual-game witness. The runtime gate also
        # requires fresh process/module proofs before it calls the framebuffer verifier.
        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            root = Path(tmp)
            (root / "runtime-inputs.json").write_text("{}")
            case = "StaticNormalCases.GpuAuthoredDirection"
            names = ["direction-" + placement + "-" + surface + "-t0-l0"
                     for placement in ("standing", "scaled", "laid", "mirror") for surface in ("solid", "texture")]
            raw = b"".join(bytes([level, level, level, 255]) * 256 for level in (100, 100, 150, 200)) * 256
            def chunk(kind, data):
                return struct.pack(">I", len(data)) + kind + data + struct.pack(">I", zlib.crc32(kind + data))
            png = b"\x89PNG\r\n\x1a\n" + chunk(b"IHDR", struct.pack(">IIBBBBB", 1024, 256, 8, 6, 0, 0, 0))
            png += chunk(b"IDAT", zlib.compress(b"".join(b"\x00" + raw[y * 4096:(y + 1) * 4096] for y in range(256)))) + chunk(b"IEND", b"")
            rows = []
            for index, name in enumerate(names):
                (root / (name + ".rgba")).write_bytes(raw)
                (root / (name + ".png")).write_bytes(png)
                rows.append({"name": name, "diagnostic": True, "transportFault": 0, "lightingFault": 0, "expectedIndex": 0,
                    "observedDiagnostic": 1, "observedTransportFault": 0, "observedLightingFault": 0, "lightMode": 1, "normalSpace": 1,
                    "width": 1024, "height": 256, "backend": "RimWorldWaylith.Rendering.CameraTextureBaker", "gpuFrameBefore": index,
                    "gpuFrameCompleted": index + 1, "unityFrame": 1, "textured": "-texture-" in name, "textureName": "NormalProofNeutral", "textureWidth": 1, "textureHeight": 1,
                    "rawPath": name + ".rgba", "pngPath": name + ".png", "rawHash": gate.digest(root / (name + ".rgba")), "pngHash": gate.digest(root / (name + ".png")),
                    "patches": [[[level, level, level, 255] for _ in range(9)] for level in (100, 100, 150, 200)],
                    "differences": [0, 50, 100], "authoredComparisonPassed": True})
            report = {"caseId": case, "passed": True, "readings": rows}
            path = root / (case + ".json")
            request = {"nonce": "host-synthetic", "head": "host-fixture", "assemblyHash": "host-fixture"}
            path.write_text(json.dumps(report))
            self.assertEqual(len(gate.verify_static_pixels(root, [case], request, 0, 0)), 8)
            mutations = [
                dict(report, readings=rows[1:]), dict(report, readings=rows + [rows[0]]),
                dict(report, readings=[dict(rows[0], name="relabeled"), *rows[1:]]),
                dict(report, readings=[dict(rows[0], observedDiagnostic=0), *rows[1:]]),
                dict(report, readings=[dict(rows[0], rawHash="stale"), *rows[1:]]),
                dict(report, readings=[dict(rows[0], gpuFrameCompleted=0), *rows[1:]]),
                dict(report, readings=[dict(rows[0], rawPath="../escape.rgba"), *rows[1:]]),
                dict(report, readings=[dict(rows[0], differences=[1, 50, 100]), *rows[1:]]),
            ]
            for mutation in mutations:
                path.write_text(json.dumps(mutation))
                with self.assertRaises(gate.Refusal):
                    gate.verify_static_pixels(root, [case], request, 0, 0)
            path.write_text(json.dumps(report))
            with self.assertRaises(gate.Refusal):
                gate.verify_static_pixels(root, [case], request, 0, path.stat().st_mtime + 1)

            # Independent moving submission protocol control, with the real engine's 64-byte row-major matrices.
            case = "MovingNormalCases.CacheAndBatchReuse"
            name = "moving-CacheAndBatchReuse-instanced-solid-standing-direction-t0-l0"
            for extension, content in ((".rgba", raw), (".png", png)):
                (root / (name + extension)).write_bytes(content)
            matrices = [[1,0,0,2, 0,333,0,3, 0,0,1,4, 0,0,0,1], [2,0,0,5, 0,400,0,6, 0,0,1,7, 0,0,0,1]]
            row = dict(rows[0], name=name, normalSpace=2, textured=False, pathKind="instanced-solid", partIndex=-1,
                       meshId=42, markSpace=0, markSubmissions=1, recordingPath="MeshBatchRenderer.RecordPass", nativeInstancedBatches=1, actualMatrices=matrices, expectedMatrices=matrices,
                       matrixBufferHex=struct.pack("<32f", *[v for matrix in matrices for v in matrix]).hex(),
                       submissions=[{"instanced": True, "count": 2}], rawPath=name+".rgba", pngPath=name+".png")
            report = {"caseId": case, "passed": True, "readings": {"captures": [row]}}
            path = root / (case + ".json")
            path.write_text(json.dumps(report))
            self.assertEqual(len(gate.verify_static_pixels(root, [case], request, 0, 0)), 1)
            for change in ({"nativeInstancedBatches": 0}, {"recordingPath": "RecordBatch"}, {"markSpace": 2}, {"markSubmissions": 0}, {"normalSpace": 1},
                           {"actualMatrices": matrices[:1]}, {"matrixBufferHex": ""},
                           {"submissions": [{"instanced": False, "count": 1}]}):
                path.write_text(json.dumps(dict(report, readings={"captures": [dict(row, **change)]})))
                with self.assertRaises(gate.Refusal):
                    gate.verify_static_pixels(root, [case], request, 0, 0)

    def test_prepared_asset_validation(self):
        from prepared_models.intake_fixtures import stage_intake

        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            root = Path(tmp) / "fixture"
            for relative in ("About", "Defs", "Models", "1.7/Assemblies"):
                (root / relative).mkdir(parents=True, exist_ok=True)
            (root / "About/About.xml").write_text("<ModMetaData><supportedVersions><li>1.7</li></supportedVersions></ModMetaData>")
            (root / "About/LoadFolders.xml").write_text("<loadFolders><v1.7><li>/</li><li>1.7</li></v1.7></loadFolders>")
            (root / "Defs/Models.xml").write_text("<Defs/>")
            (root / "Defs/KeyBindings.xml").write_text("<Defs/>")
            (root / "Models/isorim.mtl").write_text("newmtl sample\nKd 0.9 0.8 0.7\n")
            (root / "1.7/Assemblies/IsoRim.dll").write_bytes(b"host structural check, not a runtime assembly")
            stage_intake(root, Path(tmp) / "external")
            argv = [sys.executable, str(ROOT / "tools/validate_mod.py"), "--mod-dir", str(root), "--check-assemblies"]
            result = subprocess.run(argv, capture_output=True, text=True, check=False)
            self.assertEqual(result.returncode, 0, result.stderr)
            self.assertIn("1.7/Models/Diagnostics/internal.obj", result.stdout)
            self.assertIn("1.7/Defs/Diagnostics/WornGear.xml", result.stdout)
            mutations = [
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1_0 0 0", "scalar"),
                ("1.7/Defs/Diagnostics/WornGear.xml", "internal.obj", "missing-gear.obj", "missing-gear.obj"),
                ("1.7/Defs/Diagnostics/Models.xml", "internal.obj", "missing-model.obj", "missing-model.obj"),
                ("1.7/Models/Diagnostics/internal.obj", "sample.mtl", "missing.mtl", "missing.mtl"),
                ("1.7/Models/Diagnostics/sample.mtl", "0.2 0.3 0.4", "broken 0.3 0.4", "material colour"),
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 0 0 0", "nonzero"),
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn NaN 0 -1", "finite"),
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e39 0 0", "float32"),
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e-50 0 0", "float32"),
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e20 0 0", "float32"),
                ("1.7/Models/Diagnostics/internal.obj", "vn 0 0.6 -0.8", "vn 1e-7 0 0", "float32"),
                ("1.7/Models/Diagnostics/internal.obj", "2/2/1", "2/2/99", "normal index"),
                ("1.7/Models/Diagnostics/internal.obj", "2/2/1", "99/2/1", "position index"),
            ]
            for relative, old, new, diagnostic in mutations:
                path = root / relative
                original = path.read_text()
                self.assertIn(old, original)
                try:
                    path.write_text(original.replace(old, new))
                    result = subprocess.run(argv, capture_output=True, text=True, check=False)
                    self.assertNotEqual(result.returncode, 0, relative)
                    self.assertIn(diagnostic, result.stderr)
                finally:
                    path.write_text(original)

            path = root / "1.7/Models/Diagnostics/internal.obj"
            original = path.read_text()
            for normal, expected_exit in (("0.000002 0 0", 1), ("0.00001 0 0", 1), ("0.00002 0 0", 0),
                                          ("0.00000999999883788405 0 0", 1), ("0.00001000000065687345 0 0", 0),
                                          ("0.000006 0.000006 0", 1), ("0.000008 0.000008 0", 0)):
                with self.subTest(normal=normal):
                    try:
                        path.write_text(original.replace("vn 0 0.6 -0.8", "vn " + normal))
                        result = subprocess.run(argv, capture_output=True, text=True, check=False)
                        self.assertEqual(result.returncode, expected_exit, result.stderr)
                        if expected_exit:
                            self.assertIn("float32", result.stderr)
                        else:
                            self.assertIn("1.7/Models/Diagnostics/internal.obj", result.stdout)
                    finally:
                        path.write_text(original)
            for content in ("", "v 0 0 0\nv 2 0 0\nv 0 1 0\n"):
                try:
                    path.write_text(content)
                    result = subprocess.run(argv, capture_output=True, text=True, check=False)
                    self.assertNotEqual(result.returncode, 0, repr(content))
                    self.assertIn("no face", result.stderr)
                finally:
                    path.write_text(original)

            # Authored normals are optional; valid geometric-only exports must still pass.
            try:
                path.write_text(original.replace("vn 0 0.6 -0.8\n", "").replace("f 1/1/1 2/2/1 3/3/1", "f 1/1 2/2 3/3"))
                result = subprocess.run(argv, capture_output=True, text=True, check=False)
                self.assertEqual(result.returncode, 0, result.stderr)
                self.assertIn("1.7/Models/Diagnostics/internal.obj", result.stdout)
            finally:
                path.write_text(original)

        from prepared_models.intake_fixtures import recovery_cases, recovery_mod
        cases = recovery_cases()
        expected = {row.split("|")[0]: row.split("|")[1:] for row in RECOVERY_EXPECTED.splitlines()}

        def verify_ledger(rows):
            self.assertEqual(len(rows), len(expected))
            self.assertEqual({row["id"] for row in rows}, set(expected))
            for row in rows:
                oracle = expected[row["id"]]
                self.assertEqual([str(row["cli"]), row["loader"], row["bits"] or "-"], oracle)

        verify_ledger(cases)
        for bad in (cases[1:], cases + [cases[0]], [dict(cases[0], id="relabeled"), *cases[1:]],
                    [dict(cases[0], cli=1), *cases[1:]]):
            with self.assertRaises(AssertionError):
                verify_ledger(bad)
        (self.evidence / "omission-controls.json").write_text(json.dumps({"required": len(expected), "missingRejected": True,
            "duplicateRejected": True, "relabeledRejected": True, "wrongDispositionRejected": True}, indent=2))
        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            root = Path(tmp) / "matrix"
            recovery_mod(root, ROOT / "1.7/Assemblies/IsoRim.dll")
            path = root / "1.7/Models/Diagnostics/internal.obj"
            mtl = path.with_name("sample.mtl")
            argv = [sys.executable, str(ROOT / "tools/validate_mod.py"), "--mod-dir", str(root), "--check-assemblies"]

            def exercise(case, script=None):
                path.write_bytes(bytes.fromhex(case["objHex"]))
                if case["mtlHex"] is None:
                    mtl.unlink(missing_ok=True)
                else:
                    mtl.write_bytes(bytes.fromhex(case["mtlHex"]))
                command = argv if script is None else [sys.executable, str(script), *argv[2:]]
                result = subprocess.run(command, capture_output=True, text=True, check=False)
                return command, result

            def assert_result(case, result):
                self.assertEqual(result.returncode, int(expected[case["id"]][0]), case["id"] + result.stderr)
                if case["cli"]:
                    self.assertIn("Validation error:", result.stderr)
                else:
                    self.assertIn("1.7/Models/Diagnostics/internal.obj", result.stdout)
                if case["id"].startswith(("R23-below-min-", "R23-above-max-")):
                    self.assertIn("signed Int32", result.stderr)
                if expected[case["id"]][2] != "-":
                    bits = next(json.loads(line[len("Prepared scalar bits: "):]) for line in result.stdout.splitlines() if line.startswith("Prepared scalar bits: "))
                    self.assertTrue(any(row["token"] == case["token"] and row["bits"] == expected[case["id"]][2] for row in bits), case["id"])

            for case in cases:
                with self.subTest(recovery=case["id"]):
                    command, result = exercise(case)
                    assert_result(case, result)
            # Each disposable validator mutation must falsify the same CLI oracle.
            source = (ROOT / "tools/validate_mod.py").read_text()
            mutations = [
                ("lexical", 'if not SCALAR_TOKEN.fullmatch(token):', 'if False:', "R06-integer"),
                ("unicode", '[0-9]', r'\d', "R07-arabic"),
                ("inline", r'line = raw.strip(" \t")', r'line = raw.split("#", 1)[0].strip(" \t")', "R19-vn-inline"),
                ("int32", 'if not -2147483648 <= value <= 2147483647:', 'if False:', "R23-above-max-normal"),
                ("cutoff", 'if _float32(math.sqrt(length_squared)) <= _float32(1e-5):', 'if False:', "R12-below"),
                ("double-round", 'value = Decimal(token)', 'return _float32(float(token))\n        value = Decimal(token)', "R13-above"),
                ("kd-arity", 'if len(material) != 4:', 'if False:', "R16-kd-numeric"),
                ("vt-arity", 'if len(parts) not in allowed[kind]:', 'if False:', "R16-vt-surplus"),
            ]
            mutation_rows = []
            for name, old, new, case_id in mutations:
                self.assertTrue(old in source, name)
                mutated = source.replace(old, new)
                if name == "unicode":
                    mutated = mutated.replace(", re.ASCII", "")
                script = self.evidence / ("mutation-" + name + ".py")
                script.write_text(mutated, encoding="utf-8")
                case = next(row for row in cases if row["id"] == case_id)
                command, result = exercise(case, script)
                (self.evidence / ("mutation-" + name + ".stdout.log")).write_text(result.stdout)
                (self.evidence / ("mutation-" + name + ".stderr.log")).write_text(result.stderr)
                with self.assertRaises(AssertionError):
                    assert_result(case, result)
                mutation_rows.append({"mutation": name, "case": case_id, "command": command, "exit": result.returncode, "sourceSha256": hashlib.sha256(script.read_bytes()).hexdigest().upper(), "oracleRejected": True})
            (self.evidence / "mutation-controls.json").write_text(json.dumps(mutation_rows, indent=2))

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
            "AssetIntakeCases.InternalPreference",
            "AssetIntakeCases.DefinitionRoot",
            "AssetIntakeCases.AdjacentMaterials",
            "AssetIntakeCases.FolderDiscoveryFreeze",
            "AssetIntakeCases.PreparedValidation",
            "StaticNormalCases.TrueSpaceTransform",
            "StaticNormalCases.MixedStreams",
            "StaticNormalCases.MaterialAndShaderContract",
            "StaticNormalCases.GpuAuthoredDirection",
            "StaticNormalCases.ProductionLighting",
            "StaticNormalCases.GeometricResponsibilities",
            "StaticNormalCases.OracleControls",
            "MovingNormalCases.DynamicTrueSpace",
            "MovingNormalCases.InstanceMatrices",
            "MovingNormalCases.MergedPartTransforms",
            "MovingNormalCases.TexturesMarksAndFallback",
            "MovingNormalCases.CacheAndBatchReuse",
            "MovingNormalCases.GpuAgreement",
            "MovingNormalCases.ProductionLighting",
            "IntegratedNormalCases.AllPaths",
            "IntegratedNormalCases.ProductionLightingAllPaths",
            "IntegratedNormalCases.RegressionControls",
            "IntegratedNormalCases.ProvenanceAndCompleteness",
            "IntegratedNormalCases.FrozenInputsAndArtifact",
            "IntegratedNormalCases.HandoffContract",
        }
        selected = gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S05", manifest)
        self.assertEqual(set(selected), data)
        self.assertEqual(set(manifest["cases"]), data)  # Independent literal complete-case oracle at this S05 head.
        for missing in sorted(data):
            group = {"NormalDataCases": "data", "AssetIntakeCases": "intake", "StaticNormalCases": "static", "MovingNormalCases": "moving", "IntegratedNormalCases": "integrated"}[missing.split(".")[0]]
            shortened = deepcopy(manifest)
            del shortened["cases"][missing]
            shortened["groups"][group].remove(missing)
            with self.subTest(removed_required_case=missing):
                with self.assertRaises(gate.Refusal):
                    gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S05", shortened)
                with self.assertRaises(gate.Refusal):
                    gate.select_cases(b"M\0tools/prepared_models/cases.json\0", "ISORIM-003.S05", shortened)
            for relocate in (False, True):
                ungrouped = deepcopy(manifest)
                ungrouped["groups"][group].remove(missing)
                if relocate:
                    ungrouped["groups"]["data" if group == "integrated" else "integrated"].append(missing)
                with self.subTest(required_group_case=missing, relocated=relocate):
                    with self.assertRaises(gate.Refusal):
                        gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S05", ungrouped)
        gate.verify_registry(selected, manifest)
        with self.assertRaises(gate.Refusal):
            gate.verify_registry(["NormalDataCases.Missing"], manifest)
        for stream in (b"", b"M\0Source17/Models.cs", b"Q\0Source17/Models.cs\0", b"R100\0Source17/Models.cs\0"):
            with self.assertRaises(gate.Refusal):
                gate.select_cases(stream, "ISORIM-003.S05", manifest)
        for story in ("S01", "ISORIM-003.S00", "ISORIM-003.S99"):
            with self.assertRaises(gate.Refusal):
                gate.select_cases(b"M\0Source17/Models.cs\0", story, manifest)
        with tempfile.TemporaryDirectory(dir=self.evidence) as tmp:
            directory = Path(tmp)
            log = directory / "host-controls.stderr.log"
            log.write_text("Ran 10 tests in 0.1s\n\nOK\n")
            self.assertEqual(gate.verify_host_controls(manifest, directory)["passed"], 10)
            for summary in ("Ran 0 tests in 0.0s\n\nOK\n", "Ran 10 tests in 0.1s\n\nOK (skipped=10)\n"):
                log.write_text(summary)
                with self.assertRaises(gate.Refusal):
                    gate.verify_host_controls(manifest, directory)
        for changes in (b"M\0README.txt\0", b"D\0unknown.cs\0",
                        b"R100\0Source17/Models.cs\0unknown.cs\0"):
            with self.assertRaises(gate.Refusal):
                gate.select_cases(changes, "ISORIM-003.S05", manifest)
        self.assertEqual(set(gate.select_cases(b"D\0Source17/Models.cs\0", "ISORIM-003.S05", manifest)), data)
        self.assertEqual(set(gate.select_cases(b"R100\0Source17/Models.cs\0Source17/Models.cs\0", "ISORIM-003.S05", manifest)), data)
        manifest["groups"]["static"] = ["StaticNormalCases.Example"]
        manifest["cases"]["StaticNormalCases.Example"] = {"story": 1}
        with self.assertRaises(gate.Refusal):
            gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S05", manifest)
        manifest = gate.case_manifest()
        del manifest["cases"][next(iter(data))]
        with self.assertRaises(gate.Refusal):
            gate.select_cases(b"M\0Source17/Models.cs\0", "ISORIM-003.S05", manifest)
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
