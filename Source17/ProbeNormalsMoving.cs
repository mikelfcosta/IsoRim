using System;
using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Material = UnityEngine.Material;
using Mesh = UnityEngine.Mesh;
using UnityEngine;
using Verse;
using MeshDrawCommand = Waylith.Rendering.MeshDrawCommand;
using MeshDrawSubmission = Waylith.Rendering.MeshDrawSubmission;

namespace IsoRim
{
    internal static class MovingNormalCases
    {
        private static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        private static float[] V(Vector3 v) => new[] { v.x, v.y, v.z };
        private static readonly Vector3 N = new Vector3(.94f, .1f, .326190128606f);

        internal static string Run(string id, string evidence, string fixtures)
        {
            var hook = new Harmony("isorim.normalproof.s04.drawdiagnostic");
            var drawMethod = typeof(Waylith.Rendering.MeshBatchRenderer).GetMethod("DrawBatchInstanced", BindingFlags.Instance | BindingFlags.NonPublic);
            var recordMethod = typeof(Waylith.Rendering.MeshBatchRenderer).GetMethod("RecordBatch");
            hook.Patch(recordMethod, prefix: new HarmonyMethod(typeof(MovingNormalCases).GetMethod("RecordMovingFrame", BindingFlags.Static | BindingFlags.NonPublic)));
            hook.Patch(drawMethod, prefix: new HarmonyMethod(typeof(MovingNormalCases).GetMethod("DrawDiagnostic", BindingFlags.Static | BindingFlags.NonPublic)));
            object observations;
            string kind;
            bool textured = IsoRimMod.Settings.texturedModels;
            IsoRimMod.Settings.texturedModels = true;
            try
            {
                switch (id)
                {
                    case "MovingNormalCases.DynamicTrueSpace": observations = DynamicTrueSpace(fixtures); kind = "dynamic"; break;
                    case "MovingNormalCases.InstanceMatrices": observations = new { seam = "DynamicModels.DrawLocal -> PartBatches.Flush -> binding 4" }; kind = "instances"; break;
                    case "MovingNormalCases.MergedPartTransforms": observations = MergedPartTransforms(fixtures); kind = "merged"; break;
                    case "MovingNormalCases.TexturesMarksAndFallback": observations = TexturesMarksAndFallback(fixtures); kind = "fallback"; break;
                    case "MovingNormalCases.CacheAndBatchReuse": observations = CacheAndBatchReuse(fixtures); kind = "cache"; break;
                    case "MovingNormalCases.GpuAgreement": observations = new { seam = "same transformed singles and heterogeneous instances, plus both local merged parts" }; kind = "agreement"; break;
                    case "MovingNormalCases.ProductionLighting": observations = new { seam = "ordinary model lighting, diagnostics off, all 24 tuples" }; kind = "lighting"; break;
                    default: throw new InvalidOperationException("Unknown moving case: " + id);
                }
                List<object> captures = Pixels(id, evidence, fixtures, kind);
                int singleInstanceMaximumDifference = 0;
                if (kind == "agreement")
                {
                    var rows = captures.Select(row => JsonSerializer.SerializeToElement(row)).Where(row => row.GetProperty("transportFault").GetInt32() == 0 && row.GetProperty("lightingFault").GetInt32() == 0).ToArray();
                    foreach (string pose in new[] { "standing", "scaled", "laid", "mirror" })
                        foreach (string surface in new[] { "solid", "texture" })
                        {
                            int[][] Single(string path) => rows.Single(row => row.GetProperty("tupleId").GetString() == path + "-" + surface + "-" + pose).GetProperty("patches")[0].Deserialize<int[][]>();
                            int difference = Single("single").Zip(Single("instanced"), (a, b) => Enumerable.Range(0, 3).Max(c => Math.Abs(a[c] - b[c]))).Max();
                            singleInstanceMaximumDifference = Math.Max(singleInstanceMaximumDifference, difference);
                            Require(difference <= 2, "Single and instanced patches disagree");
                        }
                }
                string path = Path.Combine(evidence, id + ".json");
                File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = new { observations, captures, singleInstanceMaximumDifference } }, new JsonSerializerOptions { WriteIndented = true }));
                return path;
            }
            finally { IsoRimMod.Settings.texturedModels = textured; hook.Unpatch(drawMethod, HarmonyPatchType.Prefix, hook.Id); hook.Unpatch(recordMethod, HarmonyPatchType.Prefix, hook.Id); captureRenderer = null; }
        }

        private static Waylith.Rendering.MeshBatchRenderer captureRenderer;
        private static int nativeBatches;
        private static void DrawDiagnostic(Waylith.Rendering.MeshBatchRenderer __instance, ref string diagnosticPrefix)
        {
            if (!ReferenceEquals(__instance, captureRenderer)) return;
            diagnosticPrefix = "IsoRim normal proof"; nativeBatches++;
        }

        // CameraTextureBaker.RecordBatch only dispatches single draws. Use the same native
        // RecordPass as the scene renderer, inside the baker's real render pass and fence.
        // The baker retains ownership of the FinalDrawItems, so this wrapper does not dispose them.
        private static bool RecordMovingFrame(Waylith.Rendering.MeshBatchRenderer __instance, in Waylith.Rendering.FrameContext frameContext,
            [HarmonyArgument(1)] IReadOnlyList<Waylith.Rendering.FinalDrawItem> draws, [HarmonyArgument(2)] Waylith.Rendering.CameraView view, [HarmonyArgument(3)] System.Numerics.Vector3? sunDirection)
        {
            if (!ReferenceEquals(__instance, captureRenderer)) return true;
            var sorted = draws.OrderBy(item => item.RenderQueue).ToList();
            int frameNumber = (int)typeof(Waylith.Rendering.MeshBatchRenderer).GetField("currentFrameNumber", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance);
            var frame = new Waylith.Rendering.MeshDrawFrame(frameNumber, view, sorted, new List<Waylith.Rendering.FinalDrawItem>(), sunDirection ?? System.Numerics.Vector3.UnitY);
            foreach (byte pass in sorted.Select(item => item.PassId).Distinct())
                __instance.RecordPass(in frameContext, frame, pass, view, "IsoRim normal proof", int.MinValue, int.MaxValue, 1, false);
            return false;
        }

        private static Model Fixture(string fixtures, bool textured = false, bool authored = true)
        {
            string path = Path.Combine(fixtures, "moving-" + textured + "-" + authored + ".obj");
            File.WriteAllText(Path.ChangeExtension(path, ".mtl"), "newmtl sample\nKd "
                + (textured ? "0.55555556 0.55555556 0.55555556\nmap_Kd Textures/Diagnostics/NormalProofNeutral.png\n" : "0.6 0.6 0.6\n"));
            File.WriteAllText(path, "mtllib " + Path.GetFileName(Path.ChangeExtension(path, ".mtl"))
                + "\nv 0 0 0\nv 2 0 0\nv 0 1 0\nvt 0 0\nvt 1 0\nvt 0 1\nvn .94 .1 -.326190128606\nusemtl sample\n"
                + (authored ? "f 1/1/1 2/2/1 3/3/1\n" : "f 1/1 2/2 3/3\n"));
            Model model = ObjLoader.Load(path);
            Require(model != null && model.positions.Length == 3, "Moving fixture failed to load");
            return model;
        }

        private static List<DynamicModels.Part> Parts(Model model) => (List<DynamicModels.Part>)typeof(DynamicModels)
            .GetMethod("PartsFor", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[] { model, Color.white, "Metal" });

        private static DynamicModels.Part Face(List<DynamicModels.Part> parts, bool textured = false) => parts.Single(part =>
            !part.outline && part.material.GetFloat("_LightMode") == 1 && (part.material.mainTexture != null) == textured);

        private static object Core(object wrapper, string property) => wrapper.GetType().GetProperty(property,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).GetValue(wrapper);

        private static float[] MatrixRows(Matrix4x4 m) => new[] { m.m00,m.m01,m.m02,m.m03, m.m10,m.m11,m.m12,m.m13,
            m.m20,m.m21,m.m22,m.m23, m.m30,m.m31,m.m32,m.m33 };
        private static float[] MatrixRows(System.Numerics.Matrix4x4 m) => new[] { m.M11,m.M12,m.M13,m.M14, m.M21,m.M22,m.M23,m.M24,
            m.M31,m.M32,m.M33,m.M34, m.M41,m.M42,m.M43,m.M44 };

        private static Matrix4x4 Placed(Spec spec, Vector3 position) => Matrix4x4.Translate(new Vector3(position.x, position.y / View3D.Squash, position.z))
            * Matrix4x4.Scale(new Vector3(1, 1 / View3D.Squash, 1)) * Matrix4x4.Rotate(Quaternion.AngleAxis(spec.Turn, Vector3.up)) * spec.Local;

        private static void Freeze(Material material, Spec spec, bool diagnostic, int transport, int lighting)
        {
            material.SetFloat("_LightMode", 1); material.SetFloat("_GlowMode", 0); material.SetFloat("_MarkPass", 0);
            material.SetFloat("_NormalView", diagnostic ? 1 : 0); material.SetFloat("_NormalTransportFault", transport); material.SetFloat("_NormalLightingFault", lighting);
            material.SetVector("_IsoView", new Vector4(spec.G.x, spec.G.y, spec.G.z, View3D.Squash));
            material.SetVector("_IsoSun", new Vector4(spec.L.x, spec.L.y, spec.L.z, 0)); material.SetVector("_IsoSky", Vector4.one);
            material.SetVector("_IsoCellMap", Vector4.zero);
            Material mark = IsoGlow.MarkFor(material);
            Require(mark != null && mark.GetFloat("_NormalSpace") == 0, "Moving mark must retain geometric ownership");
            mark.SetVector("_IsoView", material.GetVector("_IsoView")); mark.SetVector("_IsoSky", Vector4.one);
            mark.SetVector("_IsoCellMap", Vector4.zero); mark.SetVector("_IsoGlowMap", Vector4.zero);
            mark.SetTexture("_IsoGlowTex", Texture2D.whiteTexture);
        }

        private static object Capture(string caseId, string evidence, string path, Spec spec, Model model, List<DynamicModels.Part> merged,
            bool diagnostic, int transport, int lighting, int expected, string variant = "")
        {
            const int width = 1024, height = 256;
            bool instanced = path.StartsWith("instanced", StringComparison.Ordinal), textured = path.EndsWith("texture", StringComparison.Ordinal);
            bool ownCell = variant == "mark";
            string tuple = path + "-" + spec.Pose + (spec.Part >= 0 ? "-part" + spec.Part : "") + (variant == "reset" || variant == "" ? "" : "-" + variant);
            string name = "moving-" + caseId.Split('.')[1] + "-" + tuple + "-" + (diagnostic ? "direction" : "lighting") + "-t" + transport + "-l" + lighting + (variant == "reset" ? "reset" : "");
            var parts = merged ?? Parts(model);
            var face = Face(parts, textured);
            Require(face.material.GetFloat("_NormalSpace") == 2, "Moving framebuffer material is not model-local");
            int corner = spec.Part < 0 ? 0 : spec.Part * 3;
            Vector3[] triangle = face.mesh.vertices.Skip(corner).Take(3).Select(v => spec.True.MultiplyPoint3x4(v)).ToArray();
            Require(triangle.Length == 3, "Moving framebuffer triangle is absent");
            Vector3 centroid = triangle.Aggregate(Vector3.zero, (sum, v) => sum + v) / 3;
            var owner = new GameObject("IsoRim moving normal proof");
            Camera camera = owner.AddComponent<Camera>();
            Vector3 origin = new Vector3(0, 10, 0);
            camera.enabled = false; camera.orthographic = true; camera.aspect = 4; camera.cullingMask = -1;
            camera.transform.rotation = Quaternion.LookRotation(-spec.G, Mathf.Abs(spec.G.y) > .9f ? Vector3.forward : Vector3.up);
            camera.transform.position = origin + spec.G * 10;
            camera.worldToCameraMatrix = camera.worldToCameraMatrix * Matrix4x4.Scale(new Vector3(1, View3D.Squash, 1));
            camera.orthographicSize = triangle.Max(v => (v - centroid).magnitude) * 1.25f;
            var target = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            Vector3 axis = camera.transform.right;
            Vector3 targetPosition = origin - axis * camera.orthographicSize * 3 - centroid;
            var others = Specs().Where(s => s.Part == spec.Part).ToArray();
            Spec other = others[(Array.FindIndex(others, s => s.Pose == spec.Pose) + 1) % others.Length];
            int targetIndex = instanced ? Array.FindIndex(others, s => s.Pose == spec.Pose) % 2 : 0;
            Spec[] requests = instanced ? (targetIndex == 0 ? new[] { spec, other } : new[] { other, spec }) : new[] { spec };
            Vector3[] positions = requests.Select((s, i) => i == targetIndex ? targetPosition : origin + axis * 1000).ToArray();
            Matrix4x4[] expectedMatrices = requests.Select((s, i) => Placed(s, positions[i])).ToArray();
            Texture2D glow = null, image = null;
            Material[] references = new Material[3];
            Mesh referenceMesh = new Mesh { vertices = triangle.Select(v => v - centroid).ToArray(), triangles = new[] { 0, 1, 2 }, colors32 = Enumerable.Repeat(new Color32(255, 255, 255, 255), 3).ToArray() };
            referenceMesh.RecalculateBounds();
            double[] shades = ownCell ? new[] { spec.Shades[0] * 64 / 255, spec.Shades[0] * 192 / 255, spec.Shades[0] } : spec.Shades;
            Vector3[] directions = { spec.N, spec.G, spec.Q };
            foreach (var part in parts.Where(p => !p.outline)) Freeze(part.material, spec, diagnostic, transport, lighting);
            if (ownCell)
            {
                glow = new Texture2D(2, 1, TextureFormat.RGBA32, false, true) { filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp };
                glow.SetPixels32(new[] { new Color32(64, 64, 64, 0), new Color32(192, 192, 192, 0) }); glow.Apply();
                foreach (var part in parts.Where(p => !p.outline))
                {
                    Material mark = IsoGlow.MarkFor(part.material); mark.SetVector("_IsoSky", Vector4.zero); mark.SetTexture("_IsoGlowTex", glow);
                    Vector3 center = origin - axis * camera.orthographicSize * 3;
                    mark.SetVector("_IsoGlowMap", new Vector4(center.x + .5f - .2f, 0, 2, 0));
                }
            }
            for (int i = 0; i < 3; i++)
            {
                references[i] = new Material(R3DMaterials.Solid);
                references[i].SetFloat("_LightMode", 0); references[i].SetFloat("_GlowMode", 0); references[i].SetVector("_IsoCellMap", Vector4.zero);
                references[i].color = diagnostic ? new Color(directions[i].x * .5f + .5f, directions[i].y * .5f + .5f, directions[i].z * .5f + .5f)
                    : new Color((float)(.6 * shades[i]), (float)(.6 * shades[i]), (float)(.6 * shades[i]));
            }
            var backend = UnityStubs.Rendering.CameraTextureRenderer.Current;
            Require(backend != null && backend.GetType().FullName == "RimWorldWaylith.Rendering.CameraTextureBaker", "Approved GPU backend unavailable");
            FieldInfo serial = backend.GetType().GetField("_frameNumber", BindingFlags.Instance | BindingFlags.NonPublic);
            int before = (int)serial.GetValue(backend);
            captureRenderer = (Waylith.Rendering.MeshBatchRenderer)backend.GetType().GetField("_renderer", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(backend);
            int nativeBefore = nativeBatches;
            RenderTexture previous = RenderTexture.active;
            bool batches = PartBatches.Enabled, outlines = IsoRimMod.Settings.outlines;
            try
            {
                PartBatches.Flush(); PartBatches.Enabled = instanced; IsoRimMod.Settings.outlines = false;
                var commands = new List<MeshDrawCommand>();
                using (MeshDrawSubmission.BeginCapture(commands))
                {
                    PartBatches.Begin();
                    for (int i = 0; i < requests.Length; i++)
                        if (merged != null) DynamicModels.DrawParts(parts, positions[i], requests[i].Turn, requests[i].Local);
                        else DynamicModels.DrawLocal(model, positions[i], requests[i].Turn, requests[i].Local, Color.white, "Metal");
                    PartBatches.Flush();
                }
                object coreMesh = Core(face.mesh, "EngineMesh"), coreMaterial = Core(face.material, "engineMaterial");
                var actual = commands.Single(c => ReferenceEquals(c.Mesh, coreMesh) && ReferenceEquals(c.Material, coreMaterial));
                var markCommand = commands.Single(c => ReferenceEquals(c.Mesh, coreMesh) && ReferenceEquals(c.Material, Core(IsoGlow.MarkFor(face.material), "engineMaterial")));
                float[][] actualMatrices;
                byte[] storageBytes = Array.Empty<byte>();
                if (instanced)
                {
                    Require(actual.Instancing != null && actual.Instancing.InstanceCount == 2, "PartBatches did not submit the heterogeneous instance pair");
                    Require(markCommand.Instancing != null && markCommand.Instancing.InstanceCount == 2, "Moving marks did not share the instanced matrices");
                    var binding = actual.Instancing.StorageBindings.Single(b => b.PropertyId == Shader.PropertyToID("_InstanceTransforms"));
                    Require(binding.Dynamic != null && binding.Dynamic.Stride == 64, "Existing binding-4 matrix layout changed");
                    storageBytes = binding.Dynamic.GetBytes(binding.Generation).ToArray();
                    Require(storageBytes.Length == 128, "Instance matrix data count differs");
                    float[] values = MemoryMarshal.Cast<byte, float>(storageBytes).ToArray();
                    actualMatrices = new[] { values.Take(16).ToArray(), values.Skip(16).Take(16).ToArray() };
                    var markBinding = markCommand.Instancing.StorageBindings.Single(b => b.PropertyId == Shader.PropertyToID("_InstanceTransforms"));
                    Require(markBinding.Dynamic.GetBytes(markBinding.Generation).SequenceEqual(storageBytes), "Mark and face matrix buffers differ");
                }
                else
                {
                    Require(actual.Instancing == null && markCommand.Instancing == null, "Single proof did not use an actual single submission");
                    actualMatrices = new[] { MatrixRows(actual.Matrix) };
                    Require(MatrixRows(actual.Matrix).SequenceEqual(MatrixRows(markCommand.Matrix)), "Mark and face push matrices differ");
                }
                float[][] matrices = expectedMatrices.Select(MatrixRows).ToArray();
                Require(actualMatrices.Length == matrices.Length && actualMatrices.Zip(matrices, (a, b) => a.Zip(b, (x, y) => Mathf.Abs(x - y)).Max()).Max() < .002f, "Submitted matrices differ from the requested placement");
                object[] submissions = commands.Select(c => (object)new
                {
                    meshIdentity = RuntimeHelpers.GetHashCode(c.Mesh),
                    materialIdentity = RuntimeHelpers.GetHashCode(c.Material),
                    instanced = c.Instancing != null,
                    count = c.Instancing?.InstanceCount ?? 1,
                    pushMatrix = MatrixRows(c.Matrix)
                }).ToArray();
                using (MeshDrawSubmission.BeginCapture(commands))
                    for (int i = 0; i < 3; i++)
                        Graphics.DrawMesh(referenceMesh, Matrix4x4.Scale(new Vector3(1, 1 / View3D.Squash, 1)) * Matrix4x4.Translate(origin + axis * ((i - .5f) * camera.orthographicSize * 2)), references[i], 0);
                backend.Render(target, commands, UnityStubs.Rendering.CameraViewFactory.GetView(camera), new Rect(0, 0, 1, 1), Color.magenta);
                int completed = (int)serial.GetValue(backend);
                Require(completed == before + 1, "No unique completed GPU frame");
                Require(backend.TryReadPixels(target, out byte[] raw) && raw.Length == width * height * 4, "Actual Vulkan readback failed");
                RenderTexture.active = target; image = new Texture2D(width, height, TextureFormat.RGBA32, false);
                image.ReadPixels(new Rect(0, 0, width, height), 0, 0); image.Apply(); byte[] png = image.EncodeToPNG();
                File.WriteAllBytes(Path.Combine(evidence, name + ".rgba"), raw); File.WriteAllBytes(Path.Combine(evidence, name + ".png"), png);
                int[][][] patches = Enumerable.Range(0, 4).Select(slot => Enumerable.Range(-1, 3).SelectMany(dy => Enumerable.Range(-1, 3).Select(dx =>
                    raw.Skip(((height / 2 + dy) * width + slot * 256 + 128 + dx) * 4).Take(4).Select(v => (int)v).ToArray())).ToArray()).ToArray();
                int[] differences = Enumerable.Range(1, 3).Select(i => patches[0].Zip(patches[i], (a, b) => Enumerable.Range(0, 3).Max(c => Math.Abs(a[c] - b[c]))).Max()).ToArray();
                bool interior = patches.All(p => p.All(v => v[3] == 255 && !(v[0] == 255 && v[1] == 0 && v[2] == 255)));
                bool separated = diagnostic || Enumerable.Range(1, 3).All(i => Enumerable.Range(i + 1, 3 - i).All(j => Enumerable.Range(0, 3).All(c => Math.Abs(patches[i][0][c] - patches[j][0][c]) >= 12)));
                var row = new
                {
                    name,
                    tupleId = tuple,
                    pathKind = path,
                    pose = spec.Pose,
                    partIndex = spec.Part,
                    instanceIndex = targetIndex,
                    width,
                    height,
                    backend = backend.GetType().FullName,
                    recordingPath = "MeshBatchRenderer.RecordPass",
                    nativeInstancedBatches = nativeBatches - nativeBefore,
                    gpuFrameBefore = before,
                    gpuFrameCompleted = completed,
                    unityFrame = Time.frameCount,
                    shader = face.material.shader.name,
                    textured,
                    diagnostic,
                    transportFault = transport,
                    lightingFault = lighting,
                    observedDiagnostic = face.material.GetFloat("_NormalView"),
                    observedTransportFault = face.material.GetFloat("_NormalTransportFault"),
                    observedLightingFault = face.material.GetFloat("_NormalLightingFault"),
                    normalSpace = face.material.GetFloat("_NormalSpace"),
                    lightMode = face.material.GetFloat("_LightMode"),
                    textureName = face.material.mainTexture?.name,
                    textureWidth = face.material.mainTexture?.width ?? 0,
                    textureHeight = face.material.mainTexture?.height ?? 0,
                    expectedN = V(spec.N),
                    expectedG = V(spec.G),
                    expectedBinN = V(spec.Q),
                    sun = V(spec.L),
                    expectedShades = shades,
                    squash = View3D.Squash,
                    baseRgb = .6,
                    actualMatrices,
                    expectedMatrices = matrices,
                    matrixBufferHex = Convert.ToHexString(storageBytes),
                    submissions,
                    meshId = face.mesh.GetInstanceID(),
                    materialId = face.material.GetInstanceID(),
                    markSpace = IsoGlow.MarkFor(face.material).GetFloat("_NormalSpace"),
                    markSubmissions = commands.Take(submissions.Length).Count(c => ReferenceEquals(c.Material, Core(IsoGlow.MarkFor(face.material), "engineMaterial"))),
                    partLocal = spec.Part >= 0 ? MatrixRows(PartLocal(spec.Part)) : null,
                    requestedLocal = MatrixRows(spec.Local),
                    requestedYaw = spec.Turn,
                    positions = positions.Select(V).ToArray(),
                    cameraView = camera.worldToCameraMatrix.ToString(),
                    cameraProjection = camera.projectionMatrix.ToString(),
                    ownCell,
                    patches,
                    differences,
                    expectedIndex = expected,
                    authoredComparisonPassed = differences[0] <= 2,
                    rawPath = name + ".rgba",
                    rawHash = Convert.ToHexString(SHA256.HashData(raw)),
                    pngPath = name + ".png",
                    pngHash = Convert.ToHexString(SHA256.HashData(png))
                };
                File.WriteAllText(Path.Combine(evidence, name + ".json"), JsonSerializer.Serialize(row, new JsonSerializerOptions { WriteIndented = true }));
                Require(nativeBatches - nativeBefore == (instanced ? submissions.Length : 0), "Native instance dispatch count differs from the captured submissions");
                Require(interior, "Moving patch is empty, clipped or nonopaque: " + name);
                Require(separated, "Moving lighting references fail RGB separation: " + name);
                Require(differences[expected] <= 2, "Moving framebuffer differs: " + name + " " + string.Join(",", differences));
                if (expected != 0) Require(differences[0] > 2, "Moving fault/fallback silently passes authored comparison");
                return row;
            }
            finally
            {
                PartBatches.Enabled = batches; IsoRimMod.Settings.outlines = outlines; RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(target); if (image != null) UnityEngine.Object.Destroy(image); if (glow != null) UnityEngine.Object.Destroy(glow);
                foreach (Material reference in references) UnityEngine.Object.Destroy(reference);
                foreach (var part in parts.Where(p => !p.outline)) { part.material.SetFloat("_NormalView", 0); part.material.SetFloat("_NormalTransportFault", 0); part.material.SetFloat("_NormalLightingFault", 0); }
                UnityEngine.Object.Destroy(referenceMesh); UnityEngine.Object.Destroy(owner);
            }
        }

        private static object DynamicTrueSpace(string fixtures)
        {
            Model model = Fixture(fixtures);
            DynamicModels.Part part = Face(Parts(model));
            Require(part.mesh.normals.Length == 3, "DynamicModels omitted its complete model-local normal stream");
            Require(part.mesh.normals.All(n => (n - N).sqrMagnitude < 0.000001f), "DynamicModels changed the authored model-local stream");
            Require(part.material.GetFloat("_NormalSpace") == 2, "DynamicModels selected a non-model-local material");
            return new { meshId = part.mesh.GetInstanceID(), materialId = part.material.GetInstanceID(), normals = part.mesh.normals.Select(V).ToArray() };
        }

        private static Matrix4x4 PartLocal(int index) => index == 0
            ? Matrix4x4.Rotate(Quaternion.AngleAxis(15, Vector3.forward)) * Matrix4x4.Scale(new Vector3(1.2f, .8f, 1.1f))
            : Matrix4x4.Translate(new Vector3(50, 0, 0)) * Matrix4x4.Rotate(Quaternion.AngleAxis(10, Vector3.up)) * Matrix4x4.Scale(new Vector3(1.1f, .7f, .9f));

        private static object MergedPartTransforms(string fixtures)
        {
            Model model = Fixture(fixtures);
            var parts = DynamicModels.BuildMerged(new List<Model> { model, model }, new List<Matrix4x4> { PartLocal(0), PartLocal(1) }, new List<Color> { Color.white, Color.white }, 0);
            DynamicModels.Part part = Face(parts);
            Vector3[] expected = { new Vector3(.855265873899f, .381978997758f, .350160434966f), new Vector3(.963097542056f, .152112250115f, .222047264902f) };
            Require(part.mesh.normals.Length == 6, "BuildMerged omitted its complete authored normal stream");
            for (int i = 0; i < 6; i++) Require((part.mesh.normals[i] - expected[i / 3]).sqrMagnitude < .000001f, "BuildMerged did not apply the part-local inverse transpose");
            for (int i = 0; i < 6; i++) Require((part.mesh.vertices[i] - PartLocal(i / 3).MultiplyPoint3x4(model.positions[i % 3])).sqrMagnitude < .000001f, "BuildMerged changed a local vertex placement");
            Require(part.material.GetFloat("_NormalSpace") == 2, "Merged stream has the wrong normal-space semantics");
            return new { meshId = part.mesh.GetInstanceID(), normals = part.mesh.normals.Select(V).ToArray(), partLocals = new[] { PartLocal(0).ToString(), PartLocal(1).ToString() } };
        }

        private static object CacheAndBatchReuse(string fixtures)
        {
            Model model = Fixture(fixtures);
            var first = Parts(model);
            Require(ReferenceEquals(first, Parts(model)), "Repeated parts lookup rebuilt the mesh");
            model.shadingNormals = Enumerable.Repeat(new Vector3(0, .6f, .8f), 3).ToArray();
            var changed = Parts(model);
            Require(!ReferenceEquals(first, changed), "PartsFor aliases a replaced authored-data identity");
            Require(Face(changed).mesh.normals.All(n => (n - new Vector3(0, .6f, .8f)).sqrMagnitude < .000001f), "Cache returned the wrong authored stream");
            return new { originalMesh = Face(first).mesh.GetInstanceID(), changedMesh = Face(changed).mesh.GetInstanceID(), repeatedListReused = true };
        }

        private sealed class Spec
        {
            internal string Pose;
            internal int Part;
            internal Vector3 Scale, N, G, Q, L;
            internal float Turn, Lay;
            internal double[] Shades;
            internal Matrix4x4 Local => Matrix4x4.Scale(Scale) * Matrix4x4.Rotate(Quaternion.AngleAxis(Lay, Vector3.right));
            internal Matrix4x4 True => Matrix4x4.Rotate(Quaternion.AngleAxis(Turn, Vector3.up)) * Local;
        }

        // Independent double-precision cofactor calculations, fixed before the GPU run.
        // Each merged reference includes its local part transform, then the final figure transform.
        private static Spec[] Specs() => new[] {
            new Spec { Pose = "standing", Part = -1, Scale = new Vector3(1f, 1f, 1f), Turn = 0f, Lay = 0f,
                N = new Vector3(0.94f, 0.1f, 0.326190128606f), G = new Vector3(0f, 0f, 1f), Q = new Vector3(1f, 0f, 6.12323399574e-17f),
                L = new Vector3(0f, 0.8f, 0.6f), Shades = new[] { 0.6723212982272506d, 0.8749999999999999d, 0.5d } },
            new Spec { Pose = "standing", Part = 0, Scale = new Vector3(1f, 1f, 1f), Turn = 0f, Lay = 0f,
                N = new Vector3(0.855265873899f, 0.381978997758f, 0.350160434966f), G = new Vector3(0f, 0f, 1f), Q = new Vector3(0.819152044289f, 0.573576436351f, 5.01585964527e-17f),
                L = new Vector3(0.352671151375f, 0.8f, -0.485410196625f), Shades = new[] { 0.773274595687395d, 0.5d, 0.9673452773073856d } },
            new Spec { Pose = "standing", Part = 1, Scale = new Vector3(1f, 1f, 1f), Turn = 0f, Lay = 0f,
                N = new Vector3(0.963097542056f, 0.152112250115f, 0.222047264902f), G = new Vector3(0.173648177667f, 0f, 0.984807753012f), Q = new Vector3(1f, 0f, 6.12323399574e-17f),
                L = new Vector3(0f, 0.8f, 0.6f), Shades = new[] { 0.6593238493959865d, 0.8693029073795779d, 0.5d } },
            new Spec { Pose = "scaled", Part = -1, Scale = new Vector3(1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 0f,
                N = new Vector3(0.913344804013f, 0.140757555142f, -0.382084518992f), G = new Vector3(0.707106781187f, 0f, 0.707106781187f), Q = new Vector3(0.707106781187f, 0f, -0.707106781187f),
                L = new Vector3(0.3f, 0.8f, 0.519615242271f), Shades = new[] { 0.6175453408834178d, 0.8622221848584005d, 0.5d } },
            new Spec { Pose = "scaled", Part = 0, Scale = new Vector3(1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 0f,
                N = new Vector3(0.81676326516f, 0.502178909963f, -0.284102289807f), G = new Vector3(0.707106781187f, 0f, 0.707106781187f), Q = new Vector3(0.819152044289f, 0.573576436351f, 5.01585964527e-17f),
                L = new Vector3(0.124009603256f, 0.7f, -0.703293408401f), Shades = new[] { 0.9661564236949959d, 0.5d, 0.8593473039028318d } },
            new Spec { Pose = "scaled", Part = 1, Scale = new Vector3(1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 0f,
                N = new Vector3(0.847830892952f, 0.214966069333f, -0.484739482601f), G = new Vector3(0.803710647766f, 0f, 0.595020331305f), Q = new Vector3(0.707106781187f, 0f, -0.707106781187f),
                L = new Vector3(0.3f, 0.8f, 0.519615242271f), Shades = new[] { 0.6090275622887797d, 0.8439342674607264d, 0.5d } },
            new Spec { Pose = "laid", Part = -1, Scale = new Vector3(1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 90f,
                N = new Vector3(0.7071657557f, -0.445294756764f, -0.549207769031f), G = new Vector3(3.54254750278e-17f, -1f, 3.54254750278e-17f), Q = new Vector3(0.707106781187f, 0f, -0.707106781187f),
                L = new Vector3(0.53927642778f, 0.8f, -0.263022688073f), Shades = new[] { 0.6059850755573993d, 0.5d, 0.8545694658497438d } },
            new Spec { Pose = "laid", Part = 0, Scale = new Vector3(1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 90f,
                N = new Vector3(0.846617553873f, -0.463442389279f, -0.26164836956f), G = new Vector3(3.54254750278e-17f, -1f, 3.54254750278e-17f), Q = new Vector3(1f, 0f, 6.12323399574e-17f),
                L = new Vector3(0.652401950198f, 0.7f, 0.290468062579f), Shades = new[] { 0.6085176969568145d, 0.5d, 0.9660013929982185d } },
            new Spec { Pose = "laid", Part = 1, Scale = new Vector3(1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 90f,
                N = new Vector3(0.783909739482f, -0.31112277345f, -0.537297068842f), G = new Vector3(0.0856822653088f, -0.992631401288f, -0.0856822653088f), Q = new Vector3(0.707106781187f, 0f, -0.707106781187f),
                L = new Vector3(0.594160841245f, 0.8f, 0.083503860576f), Shades = new[] { 0.6075024199912714d, 0.5d, 0.7256806336820181d } },
            new Spec { Pose = "mirror", Part = -1, Scale = new Vector3(-1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 0f,
                N = new Vector3(-0.382084518992f, 0.140757555142f, 0.913344804013f), G = new Vector3(0.707106781187f, -0f, 0.707106781187f), Q = new Vector3(-0.707106781187f, 0f, 0.707106781187f),
                L = new Vector3(0.352671151375f, 0.8f, 0.485410196625f), Shades = new[] { 0.763251711112553d, 0.8703831277231766d, 0.5586629243900865d } },
            new Spec { Pose = "mirror", Part = 0, Scale = new Vector3(-1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 0f,
                N = new Vector3(-0.284102289807f, 0.502178909963f, 0.81676326516f), G = new Vector3(0.707106781187f, -0f, 0.707106781187f), Q = new Vector3(0f, 0.573576436351f, 0.819152044289f),
                L = new Vector3(-0.692929748372f, 0.7f, -0.172766790272f), Shades = new[] { 0.7909132838770151d, 0.5d, 0.6857008828635542d } },
            new Spec { Pose = "mirror", Part = 1, Scale = new Vector3(-1.3f, 0.9f, 1.1f), Turn = 45f, Lay = 0f,
                N = new Vector3(-0.484739482601f, 0.214966069333f, 0.847830892952f), G = new Vector3(0.595020331305f, -0f, 0.803710647766f), Q = new Vector3(-0.707106781187f, 0f, 0.707106781187f),
                L = new Vector3(0.335515742082f, 0.8f, 0.497422543533f), Shades = new[] { 0.7694158296892381d, 0.8746390516804079d, 0.5715533732662041d } },
        };

        private static List<object> Pixels(string id, string evidence, string fixtures, string kind)
        {
            var rows = new List<object>();
            Model solid = Fixture(fixtures), texture = Fixture(fixtures, true);
            List<DynamicModels.Part> merged = null;
            if (kind == "merged" || kind == "agreement" || kind == "lighting")
                merged = DynamicModels.BuildMerged(new List<Model> { solid, solid }, new List<Matrix4x4> { PartLocal(0), PartLocal(1) }, new List<Color> { Color.white, Color.white }, 0);
            if (kind == "fallback")
            {
                Spec spec = Specs().First(s => s.Part < 0 && s.Pose == "mirror");
                foreach (string path in new[] { "single-texture", "instanced-texture" })
                {
                    rows.Add(Capture(id, evidence, path, spec, Fixture(fixtures, true, false), null, true, 0, 0, 1, "missing"));
                    rows.Add(Capture(id, evidence, path, spec, InvalidFixture(fixtures), null, true, 0, 0, 1, "invalid"));
                    rows.Add(Capture(id, evidence, path, Specs()[0], texture, null, false, 0, 0, 0, "mark"));
                }
                return rows;
            }
            foreach (Spec spec in Specs())
                foreach (string path in spec.Part < 0 ? new[] { "single-solid", "single-texture", "instanced-solid", "instanced-texture" } : new[] { "merged-solid" })
                {
                    if (kind == "dynamic" && path != "single-solid" || kind == "instances" && spec.Part >= 0
                        || kind == "merged" && spec.Part < 0 || kind == "cache" && (path != "instanced-solid" || spec.Pose != "standing")) continue;
                    Model model = path.EndsWith("texture", StringComparison.Ordinal) ? texture : solid;
                    var parts = spec.Part >= 0 ? merged : null;
                    if (kind == "lighting")
                    {
                        rows.Add(Capture(id, evidence, path, spec, model, parts, false, 0, 0, 0));
                        rows.Add(Capture(id, evidence, path, spec, model, parts, false, 0, 1, 1));
                        rows.Add(Capture(id, evidence, path, spec, model, parts, false, 0, 2, 2));
                        rows.Add(Capture(id, evidence, path, spec, model, parts, false, 0, 0, 0, "reset"));
                    }
                    else
                    {
                        rows.Add(Capture(id, evidence, path, spec, model, parts, true, 0, 0, 0));
                        if (kind == "agreement")
                        {
                            rows.Add(Capture(id, evidence, path, spec, model, parts, true, 1, 0, 1));
                            rows.Add(Capture(id, evidence, path, spec, model, parts, true, 0, 1, 0));
                            rows.Add(Capture(id, evidence, path, spec, model, parts, true, 0, 2, 0));
                        }
                    }
                }
            return rows;
        }

        private static Model InvalidFixture(string fixtures)
        {
            Model model = Fixture(fixtures, true);
            model.positions = model.positions.Concat(model.positions.Select(p => p + Vector3.right * 50)).ToArray();
            model.normals = model.normals.Concat(model.normals).ToArray(); model.colors = model.colors.Concat(model.colors).ToArray();
            model.tinted = model.tinted.Concat(model.tinted).ToArray(); model.uvs = model.uvs.Concat(model.uvs).ToArray();
            model.material = model.material.Concat(model.material).ToArray(); model.hull = model.hull.Concat(model.hull).ToArray();
            model.authoredNormals = Enumerable.Repeat(true, 6).ToArray(); model.shadingNormals = Enumerable.Repeat(N, 6).ToArray();
            model.shadingNormals[1] = new Vector3(float.NaN, 0, 0);
            return model;
        }

        private static object TexturesMarksAndFallback(string fixtures)
        {
            var valid = Parts(Fixture(fixtures, true)); var missing = Parts(Fixture(fixtures, true, false)); var invalid = Parts(InvalidFixture(fixtures));
            foreach (bool textured in new[] { false, true })
            {
                var a = Face(valid, textured); var b = Face(missing, textured); var c = Face(invalid, textured);
                Require(a.mesh.vertices.SequenceEqual(b.mesh.vertices), "Moving texture lift or hull uses shading normals");
                Require(b.mesh.normals.Length == 3 && b.mesh.normals.All(n => n == Vector3.zero), "Missing moving normals selected the stock up vector");
                Require(c.mesh.normals.Length == 6 && c.mesh.normals.Take(3).All(n => n == Vector3.zero) && c.mesh.normals.Skip(3).All(n => (n - N).sqrMagnitude < .000001f), "Invalid moving triangle retained partial normals or discarded an independent triangle");
                Require(IsoGlow.MarkFor(a.material).GetFloat("_NormalSpace") == 0, "Moving mark inherited model-local normal semantics");
            }
            Model glow = Fixture(fixtures, true); glow.materials[glow.material[0]].glow = true;
            Require(Parts(glow).Where(p => !p.outline).All(p => p.material.GetFloat("_LightMode") == 0 && p.material.GetFloat("_NormalSpace") == 0), "Unlit moving geometry changed normal semantics");
            return new { liftAndHullGeometric = true, missingCorners = 3, invalidCorners = 3, independentValidCorners = 3, markSpace = 0, unlitPreserved = true };
        }
    }
}
