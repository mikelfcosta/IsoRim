using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using System.Security.Cryptography;
using System.Text.Json;
using RimWorld;
using UnityEngine;
using Verse;
using MeshDrawCommand = Waylith.Rendering.MeshDrawCommand;
using MeshDrawSubmission = Waylith.Rendering.MeshDrawSubmission;

namespace IsoRim
{
    internal static class IntegratedNormalCases
    {
        internal static string Run(string id, ProbeNormals.Request request)
        {
            switch (id)
            {
                case "IntegratedNormalCases.AllPaths": return MovingNormalCases.Run(id, request.outputDirectory, request.fixtureDirectory);
                case "IntegratedNormalCases.ProductionLightingAllPaths": return Write(id, request, ProductionLighting(request));
                case "IntegratedNormalCases.RegressionControls": return Write(id, request, Regression(request));
                case "IntegratedNormalCases.ProvenanceAndCompleteness": return Write(id, request, Provenance(request));
                case "IntegratedNormalCases.FrozenInputsAndArtifact": return Write(id, request, Frozen(request));
                case "IntegratedNormalCases.HandoffContract": return Write(id, request, Handoff(request));
                default: throw new InvalidOperationException("Unknown integrated case: " + id);
            }
        }

        private static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        private static string Hash(string path) => Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path)));
        private static readonly JsonSerializerOptions Json = new JsonSerializerOptions { WriteIndented = true };
        private static string Write(string id, ProbeNormals.Request request, object readings)
        {
            string path = Path.Combine(request.outputDirectory, id + ".json");
            File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings }, Json));
            return path;
        }

        private static JsonElement Report(ProbeNormals.Request request, string id)
        {
            string path = Path.Combine(request.outputDirectory, id + ".json");
            Require(File.Exists(path) && File.GetLastWriteTimeUtc(path) >= request.createdUtc.UtcDateTime, "Missing or stale integrated dependency: " + id);
            JsonElement report = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(path));
            Require(report.GetProperty("caseId").GetString() == id && report.GetProperty("passed").GetBoolean(), "Failed integrated dependency: " + id);
            return report.GetProperty("readings");
        }

        private static object Frame(ProbeNormals.Request request, JsonElement row, string name, bool diagnostic, int lighting, int expected)
        {
            Require(row.GetProperty("name").GetString() == name && row.GetProperty("diagnostic").GetBoolean() == diagnostic
                && row.GetProperty("observedDiagnostic").GetInt32() == (diagnostic ? 1 : 0)
                && row.GetProperty("transportFault").GetInt32() == 0 && row.GetProperty("observedTransportFault").GetInt32() == 0
                && row.GetProperty("lightingFault").GetInt32() == lighting && row.GetProperty("observedLightingFault").GetInt32() == lighting
                && row.GetProperty("lightMode").GetInt32() == 1 && row.GetProperty("expectedIndex").GetInt32() == expected, "Integrated production state differs: " + name);
            foreach (string kind in new[] { "raw", "png" })
            {
                string file = Path.Combine(request.outputDirectory, row.GetProperty(kind + "Path").GetString());
                Require(Path.GetDirectoryName(Path.GetFullPath(file)) == Path.GetFullPath(request.outputDirectory)
                    && File.GetLastWriteTimeUtc(file) >= request.createdUtc.UtcDateTime && Hash(file) == row.GetProperty(kind + "Hash").GetString(), "Stale integrated framebuffer: " + name);
            }
            byte[] raw = File.ReadAllBytes(Path.Combine(request.outputDirectory, row.GetProperty("rawPath").GetString()));
            Require(raw.Length == 1024 * 256 * 4, "Integrated framebuffer dimensions differ");
            int[] difference = new int[3];
            for (int slot = 1; slot < 4; slot++)
                for (int dy = -1; dy <= 1; dy++) for (int dx = -1; dx <= 1; dx++) for (int c = 0; c < 3; c++)
                {
                    int a = raw[((128 + dy) * 1024 + 128 + dx) * 4 + c];
                    int b = raw[((128 + dy) * 1024 + 128 + slot * 256 + dx) * 4 + c];
                    difference[slot - 1] = Math.Max(difference[slot - 1], Math.Abs(a - b));
                }
            Require(difference[expected] <= 2 && (expected == 0 || difference[0] > 2), "Integrated production fault was accepted: " + name);
            if (!diagnostic)
                for (int a = 1; a < 4; a++) for (int b = a + 1; b < 4; b++) for (int c = 0; c < 3; c++)
                    Require(Math.Abs(raw[(128 * 1024 + 128 + a * 256) * 4 + c] - raw[(128 * 1024 + 128 + b * 256) * 4 + c]) >= 12, "Integrated references do not separate");
            return new { name, diagnostic, lighting, expected, difference, rawHash = row.GetProperty("rawHash").GetString(), pngHash = row.GetProperty("pngHash").GetString(), gpuFrame = row.GetProperty("gpuFrameCompleted").GetInt32() };
        }

        private static object ProductionLighting(ProbeNormals.Request request)
        {
            JsonElement solid = Report(request, "StaticNormalCases.ProductionLighting");
            JsonElement staticTransport = Report(request, "StaticNormalCases.OracleControls");
            JsonElement moving = Report(request, "MovingNormalCases.ProductionLighting").GetProperty("captures");
            JsonElement movingTransport = Report(request, "MovingNormalCases.GpuAgreement").GetProperty("captures");
            var tuples = new List<object>();
            foreach (string pose in new[] { "standing", "scaled", "laid", "mirror" })
                foreach (string path in new[] { "static-solid", "static-texture", "single-solid", "single-texture", "instanced-solid", "instanced-texture", "merged-solid-part0", "merged-solid-part1" })
                {
                    bool isStatic = path.StartsWith("static-", StringComparison.Ordinal);
                    string tuple = isStatic ? pose + "-" + path.Substring(7) : path.StartsWith("merged", StringComparison.Ordinal) ? "merged-solid-" + pose + "-" + path.Substring(13) : path + "-" + pose;
                    string litPrefix = isStatic ? "lighting-" + tuple : "moving-ProductionLighting-" + tuple + "-lighting";
                    string transportPrefix = isStatic ? "direction-" + tuple : "moving-GpuAgreement-" + tuple + "-direction";
                    var lit = (isStatic ? solid : moving).EnumerateArray().ToDictionary(row => row.GetProperty("name").GetString());
                    var direction = (isStatic ? staticTransport : movingTransport).EnumerateArray().ToDictionary(row => row.GetProperty("name").GetString());
                    var frames = new List<object>();
                    foreach (int fault in new[] { 0, 1, 2 })
                    {
                        string name = litPrefix + "-t0-l" + fault;
                        Require(lit.ContainsKey(name), "An integrated lighting tuple is absent: " + name);
                        frames.Add(Frame(request, lit[name], name, false, fault, fault));
                        if (fault > 0)
                        {
                            name = transportPrefix + "-t0-l" + fault;
                            Require(direction.ContainsKey(name), "An integrated fault transport tuple is absent: " + name);
                            frames.Add(Frame(request, direction[name], name, true, fault, 0));
                        }
                    }
                    string reset = litPrefix + "-t0-l0reset";
                    Require(lit.ContainsKey(reset), "The integrated lighting reset is absent: " + reset);
                    frames.Add(Frame(request, lit[reset], reset, false, 0, 0));
                    tuples.Add(new { tupleId = path + "-" + pose, frames });
                }
            Require(tuples.Count == 32, "The integrated ordinary-lighting tuple set is incomplete");
            string[] dependencies = { "StaticNormalCases.ProductionLighting", "StaticNormalCases.OracleControls", "MovingNormalCases.ProductionLighting", "MovingNormalCases.GpuAgreement" };
            return new { tuples, dependencyHashes = dependencies.ToDictionary(id => id, id => Hash(Path.Combine(request.outputDirectory, id + ".json"))) };
        }

        private static readonly string[] Prerequisites = {
            "NormalDataCases.AuthoredIndices", "NormalDataCases.InvalidTriangleFallback", "NormalDataCases.GeometricChannelsPreserved", "NormalDataCases.DerivedModels",
            "AssetIntakeCases.InternalPreference", "AssetIntakeCases.DefinitionRoot", "AssetIntakeCases.AdjacentMaterials", "AssetIntakeCases.FolderDiscoveryFreeze", "AssetIntakeCases.PreparedValidation",
            "StaticNormalCases.TrueSpaceTransform", "StaticNormalCases.MixedStreams", "StaticNormalCases.MaterialAndShaderContract", "StaticNormalCases.GpuAuthoredDirection", "StaticNormalCases.ProductionLighting", "StaticNormalCases.GeometricResponsibilities", "StaticNormalCases.OracleControls",
            "MovingNormalCases.DynamicTrueSpace", "MovingNormalCases.InstanceMatrices", "MovingNormalCases.MergedPartTransforms", "MovingNormalCases.TexturesMarksAndFallback", "MovingNormalCases.CacheAndBatchReuse", "MovingNormalCases.GpuAgreement", "MovingNormalCases.ProductionLighting" };
        private static readonly string[] Integrated = { "AllPaths", "ProductionLightingAllPaths", "RegressionControls", "ProvenanceAndCompleteness", "FrozenInputsAndArtifact", "HandoffContract" };

        private static object Provenance(ProbeNormals.Request request)
        {
            string[] expected = Prerequisites.Concat(Integrated.Select(id => "IntegratedNormalCases." + id)).Concat(ModelStyleCases.Cases).Concat(SceneStyleCases.Cases).OrderBy(id => id).ToArray();
            Require(request.caseIds.Length == expected.Length && request.caseIds.OrderBy(id => id).SequenceEqual(expected), "The integrated request omits or duplicates a required case");
            var proofs = Directory.GetFiles(request.outputDirectory, "NormalProof-*.json").Select(path => JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(path))).ToArray();
            var rows = new List<object>();
            foreach (string id in Prerequisites)
            {
                JsonElement proof = proofs.Single(row => row.GetProperty("caseId").GetString() == id);
                Require(proof.GetProperty("passed").GetBoolean() && proof.GetProperty("head").GetString() == request.head
                    && proof.GetProperty("nonce").GetString() == request.nonce && proof.GetProperty("assemblyHash").GetString() == request.assemblyHash
                    && proof.GetProperty("processId").GetInt32() == Environment.ProcessId && proof.GetProperty("runtime").GetString() == "10.0.13-waylithclr.dev"
                    && proof.GetProperty("gameVersion").GetString() == "1.7.4981.136", "An integrated prerequisite has stale provenance: " + id);
                rows.Add(new { caseId = id, assemblyHash = request.assemblyHash, processId = Environment.ProcessId });
            }
            string compiler = Path.Combine(request.outputDirectory, "compiler-references-1.7.json");
            Require(Hash(compiler) == request.compilerManifestHash, "The integrated compiler reference manifest changed");
            var refs = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(compiler));
            foreach (string name in new[] { "RimWorld.dll", "Assembly-CSharp.dll", "UnityStubs.dll", "Waylith.dll", "Waylith.UnityCompat.dll", "0Harmony.dll" })
                Require(refs.Any(item => Path.GetFileName(item.Key) == name && item.Value == Hash(name == "0Harmony.dll" ? typeof(HarmonyLib.Harmony).Assembly.Location : Path.Combine(AppContext.BaseDirectory, name))), "Compiler and loaded reference differ: " + name);
            return new { head = request.head, request.nonce, processId = Environment.ProcessId, requiredCaseIds = expected, prerequisites = rows, request.compilerManifestHash };
        }

        private static object Frozen(ProbeNormals.Request request)
        {
            var counts = new Dictionary<string, int>();
            var hashes = new Dictionary<string, string>();
            foreach (string file in new[] { "protected-inputs.json", "runtime-inputs.json" })
            {
                string path = Path.Combine(request.outputDirectory, file);
                var inputs = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(path));
                Require(inputs.Count > 0, "An integrated freeze manifest is empty");
                foreach (var item in inputs) Require(File.Exists(item.Key) && Hash(item.Key) == item.Value, "An integrated frozen input changed: " + item.Key);
                counts[file] = inputs.Count; hashes[file] = Hash(path);
            }
            string loaded = typeof(ProbeNormals).Assembly.Location;
            Require(Hash(loaded) == request.packagedHash && request.packagedHash == request.assemblyHash, "The distribution, build and actual loaded module differ");
            var artifact = JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(Path.Combine(request.outputDirectory, "artifact-identity.json")));
            Require(artifact.GetProperty("head").GetString() == request.head && artifact.GetProperty("sha256").GetString() == request.packagedHash, "The committed artifact identity differs");
            return new { counts, manifestHashes = hashes, packagedHash = request.packagedHash, builtHash = request.assemblyHash, loadedHash = Hash(loaded), loadedPath = loaded };
        }

        private static object Handoff(ProbeNormals.Request request)
        {
            string root = Path.Combine(AppContext.BaseDirectory, "Mods", "IsoRim", "1.7", "Models");
            string contract = Path.Combine(root, "Diagnostics", "normal-contract.json");
            Require(Hash(contract) == request.contractHash, "The staged preparation contract differs");
            var fields = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(contract));
            string[] required = { "profile", "units", "axes", "ground", "pivot", "normalSpace", "triangleValidity", "winding", "transform", "channels", "geometric", "paths", "materials", "numeric", "fallback", "exports" };
            Require(fields.Count == required.Length && required.All(fields.ContainsKey), "The handoff omits a required convention");
            Require(fields["normalSpace"] == "OBJ model-local before import; true-space shading after import"
                && fields["channels"] == "Normal=shading; UV0=texture; UV1=cutaway; binding4=instance matrices"
                && fields["fallback"] == "absent vn is legal; explicit invalid vn is a prepared error"
                && fields["exports"] == "generated asymmetric fixture; no production export required", "The handoff changed the approved convention");
            string note = File.ReadAllText(Path.Combine(root, "README.md"));
            Require(required.All(key => note.Contains("| " + key + " | " + fields[key] + " |"))
                && note.Contains("R01–R29") && note.Contains("230 subcases") && note.Contains("23 earlier staged controls"), "The preparation note omits the settled profile or recovery ledger");
            return new { fields, contractHash = Hash(contract), noteHash = Hash(Path.Combine(root, "README.md")), requiredRecoveryClasses = 29, requiredRecoverySubcases = 230, requiredEarlierControls = 23 };
        }

        private static object Regression(ProbeNormals.Request request)
        {
            // CPU equality checks cover all geometric writers. GPU scenes also require visible
            // real selection brackets and identical authored/legacy coverage under camera changes.
            Report(request, "StaticNormalCases.GeometricResponsibilities");
            Report(request, "MovingNormalCases.TexturesMarksAndFallback");
            var owner = ThingMaker.MakeThing(ThingDefOf.Steel);
            owner.Position = new IntVec3(0, 0, 0);
            bool isometric = IsoRimMod.Settings.isometric;
            float oldPitch = IsoRimMod.Settings.pitch, oldSquash = IsoRimMod.Settings.squash;
            var scenes = new List<object>();
            try
            {
                foreach (float pitch in new[] { 30f, 60f, 85f })
                    foreach (float squash in new[] { .002f, .004f })
                    {
                        IsoRimMod.Settings.isometric = false; IsoRimMod.Settings.pitch = pitch; IsoRimMod.Settings.squash = squash;
                        var a = StaticNormalCases.RegressionMeshes(request.fixtureDirectory, true, owner);
                        var b = StaticNormalCases.RegressionMeshes(request.fixtureDirectory, false, owner);
                        Require(a.Count == 4 && a.Count == b.Count, "The scene lacks a geometric control");
                        for (int i = 0; i < a.Count; i++)
                        {
                            Require(a[i].Length > 0 && a[i].Length == b[i].Length, "A scene geometric writer is absent");
                            for (int j = 0; j < a[i].Length; j++)
                                Require(a[i][j].mesh.vertices.SequenceEqual(b[i][j].mesh.vertices)
                                    && a[i][j].mesh.triangles.SequenceEqual(b[i][j].mesh.triangles)
                                    && a[i][j].mesh.uv.SequenceEqual(b[i][j].mesh.uv)
                                    && a[i][j].mesh.uv2.SequenceEqual(b[i][j].mesh.uv2)
                                    && a[i][j].mesh.colors32.SequenceEqual(b[i][j].mesh.colors32), "Authored normals changed scene geometry, lift, damage or channels");
                        }
                        foreach (string environment in new[] { "day", "night", "indoor" })
                            foreach (int repeats in new[] { 1, 2 })
                                scenes.Add(Scene(request, a, b, owner, pitch, squash, environment, repeats));
                        StaticNormalCases.ClearIntegrationMeshes();
                    }
                var environmentChecks = new List<object>();
                foreach (int pitch in new[] { 30, 60, 85 }) foreach (int squash in new[] { 2000, 4000 }) foreach (int repeats in new[] { 1, 2 })
                {
                    byte[] Read(string environment) => File.ReadAllBytes(Path.Combine(request.outputDirectory,
                        "scene-p" + pitch + "-s" + squash + "-" + environment + "-r" + repeats + "-authored.rgba"));
                    byte[] day = Read("day"), night = Read("night"), indoor = Read("indoor");
                    int matched = 0;
                    // The known shade .6723212982272506 at base .6 gives 103/255.
                    // GlowLight gives .25049 at night and .50196 indoors, hence 26 and 52.
                    for (int i = 0; i < day.Length; i += 4)
                        if (Enumerable.Range(0, 3).All(c => day[i + c] == 103 && Math.Abs(night[i + c] - 26) <= 2 && Math.Abs(indoor[i + c] - 52) <= 2)) matched++;
                    Require(matched >= 10, "The actual day/night/indoor lighting response differs or is empty");
                    environmentChecks.Add(new { pitch, squash, repeats, day = 103, night = 26, indoor = 52, matchingPixels = matched });
                }
                return new { geometricControls = new[] { "textured-outline", "damage", "ghost", "unlit" }, scenes, environmentChecks };
            }
            finally
            {
                IsoRimMod.Settings.isometric = isometric; IsoRimMod.Settings.pitch = oldPitch; IsoRimMod.Settings.squash = oldSquash;
                StaticNormalCases.ClearIntegrationMeshes();
            }
        }

        private static object Scene(ProbeNormals.Request request, List<LayerSubMesh[]> authored, List<LayerSubMesh[]> legacy, Thing owner,
            float pitch, float squash, string environment, int repeats)
        {
            string name = "scene-p" + (int)pitch + "-s" + (int)(squash * 1000000) + "-" + environment + "-r" + repeats;
            var gameObject = new GameObject("IsoRim integrated scene");
            Camera camera = gameObject.AddComponent<Camera>();
            camera.enabled = false; camera.orthographic = true; camera.aspect = 1; camera.cullingMask = -1; camera.orthographicSize = 8;
            float yaw = pitch == 30 ? -20 : pitch == 60 ? 0 : 20;
            camera.transform.rotation = Quaternion.Euler(pitch, yaw + 180, 0);
            camera.transform.position = new Vector3(1, 2, 1) - camera.transform.forward * 20;
            camera.worldToCameraMatrix = camera.worldToCameraMatrix * Matrix4x4.Scale(new Vector3(1, squash, 1));
            var target = RenderTexture.GetTemporary(512, 512, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            var glow = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
            Color32 glowPixel = environment == "day" ? new Color32(0, 0, 0, 0) : environment == "night" ? new Color32(32, 32, 32, 0) : new Color32(128, 128, 128, 255);
            float sky = environment == "night" ? .125f : 1;
            glow.SetPixels32(new[] { glowPixel }); glow.Apply();
            var frames = new List<object>(); var images = new List<byte[]>();
            var materials = new List<Material>();
            RenderTexture previous = RenderTexture.active;
            try
            {
                for (int mode = 0; mode < 3; mode++)
                {
                    var commands = new List<MeshDrawCommand>();
                    using (MeshDrawSubmission.BeginCapture(commands))
                    {
                        if (mode < 2)
                        {
                            var groups = mode == 0 ? authored : legacy;
                            for (int i = 0; i < groups.Count; i++) foreach (LayerSubMesh sub in groups[i])
                            {
                                Material mat = new Material(sub.material); materials.Add(mat);
                                mat.SetFloat("_ClayMode", 0); mat.SetFloat("_ClayWrap", 0); mat.SetFloat("_ClayHighlight", 0); mat.SetFloat("_ClayRoughness", 0);
                                mat.SetFloat("_NormalView", 0); mat.SetFloat("_NormalLightingFault", 0); mat.SetFloat("_NormalTransportFault", 0);
                                mat.SetVector("_IsoSun", new Vector4(0, .8f, .6f, 0));
                                mat.SetVector("_IsoView", new Vector4(0, .5f, 1, squash)); mat.SetVector("_IsoCellMap", Vector4.zero);
                                if (sub.material.GetFloat("_GlowMode") == 1) IsoGlow.Use(mat, IsoGlow.OwnCell);
                                mat.SetVector("_IsoSky", new Vector4(sky, sky, sky, 1)); mat.SetTexture("_IsoGlowTex", glow); mat.SetVector("_IsoGlowMap", Vector4.zero);
                                Material mark = IsoGlow.MarkFor(mat);
                                if (mark != null)
                                {
                                    Require(mark.GetFloat("_NormalSpace") == 0, "Scene glow ownership inherited shading normals");
                                    mark.SetVector("_IsoSky", new Vector4(sky, sky, sky, 1)); mark.SetVector("_IsoView", new Vector4(0, .5f, 1, squash));
                                    mark.SetTexture("_IsoGlowTex", glow); mark.SetVector("_IsoGlowMap", Vector4.zero); mark.SetVector("_IsoCellMap", Vector4.zero);
                                }
                                for (int r = 0; r < repeats; r++)
                                    IsoGlow.DrawLit(sub.mesh, Matrix4x4.Translate(new Vector3((i - 1.5f) * 3, 0, r * 2)), mat, 0);
                            }
                        }
                    }
                    int geometryCommands = commands.Count;
                    using (MeshDrawSubmission.BeginCapture(commands)) Interaction.Bracket(owner);
                    Require(commands.Count == geometryCommands + 8, "The actual selection-bracket path omitted a stroke");
                    byte[] raw = MovingNormalCases.RenderProofScene(target, camera, commands, out int before, out int completed);
                    Require(raw.Length == 512 * 512 * 4, "Scene framebuffer dimensions differ");
                    images.Add(raw);
                    RenderTexture.active = target;
                    var image = new Texture2D(512, 512, TextureFormat.RGBA32, false);
                    image.ReadPixels(new Rect(0, 0, 512, 512), 0, 0); image.Apply();
                    string stem = name + "-" + new[] { "authored", "legacy", "overlay" }[mode];
                    string rawPath = Path.Combine(request.outputDirectory, stem + ".rgba"), pngPath = Path.Combine(request.outputDirectory, stem + ".png");
                    File.WriteAllBytes(rawPath, raw); File.WriteAllBytes(pngPath, image.EncodeToPNG()); UnityEngine.Object.Destroy(image);
                    frames.Add(new
                    {
                        name = stem,
                        width = 512,
                        height = 512,
                        gpuFrameBefore = before,
                        gpuFrameCompleted = completed,
                        unityFrame = Time.frameCount,
                        geometryCommands,
                        overlayCommands = 8,
                        rawPath = stem + ".rgba",
                        pngPath = stem + ".png",
                        rawHash = Hash(rawPath),
                        pngHash = Hash(pngPath)
                    });
                }
                bool Background(byte[] bytes, int offset) => bytes[offset] == 255 && bytes[offset + 1] == 0 && bytes[offset + 2] == 255;
                int coverage = 0, overlayPixels = 0, appearanceDifference = 0;
                for (int i = 0; i < images[0].Length; i += 4)
                {
                    Require(Background(images[0], i) == Background(images[1], i), "Authored normals moved the scene silhouette");
                    if (!Background(images[0], i)) coverage++;
                    if (!Background(images[2], i))
                    {
                        overlayPixels++;
                        for (int c = 0; c < 3; c++) Require(Math.Abs(images[0][i + c] - images[1][i + c]) <= 2, "The gameplay overlay differs with authored normals");
                    }
                    else for (int c = 0; c < 3; c++) appearanceDifference = Math.Max(appearanceDifference, Math.Abs(images[0][i + c] - images[1][i + c]));
                }
                Require(coverage > 100 && overlayPixels > 8 && appearanceDifference > 2, "The scene or gameplay overlay is empty or the authored path was bypassed");
                return new
                {
                    name,
                    pitch,
                    yaw,
                    squash,
                    environment,
                    repeats,
                    sky,
                    glow = new[] { (int)glowPixel.r, glowPixel.g, glowPixel.b, glowPixel.a },
                    coverage,
                    overlayPixels,
                    appearanceDifference,
                    cameraView = camera.worldToCameraMatrix.ToString(),
                    cameraProjection = camera.projectionMatrix.ToString(),
                    frames
                };
            }
            finally
            {
                RenderTexture.active = previous; RenderTexture.ReleaseTemporary(target);
                foreach (Material mat in materials) UnityEngine.Object.Destroy(mat);
                UnityEngine.Object.Destroy(glow); UnityEngine.Object.Destroy(gameObject);
            }
        }
    }

    internal static class ModelStyleCases
    {
        internal static readonly string[] Cases = { "ModelStyleCases.DiffuseAndHighlightModes", "ModelStyleCases.ProtectedExceptions", "ModelStyleCases.ControlledOutlines", "ModelStyleCases.GameplayComparisonAndPerf" };

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        private static object ControlledOutlines(ProbeNormals.Request request)
        {
            string[] fields = { "modelStyle", "clayWrap", "clayHighlight", "clayRoughness", "outlineWidth", "squash" };
            double[] defaults = { 1, .22, .04, .65, .75, .003 };
            FieldInfo[] members = fields.Select(name => typeof(IsoRimSettings).GetField(name)).ToArray();
            Check(members.All(field => field != null), "Missing approved model-style setting");
            var settings = new IsoRimSettings();
            void Values(IsoRimSettings value, double[] expected, string label)
            {
                for (int i = 0; i < members.Length; i++)
                    Check(Math.Abs(Convert.ToDouble(members[i].GetValue(value)) - expected[i]) < .000001, label + " differs: " + fields[i]);
            }
            Values(settings, defaults, "Constructor default");
            string file = Path.Combine(request.fixtureDirectory, "style-settings.xml");
            File.WriteAllText(file, "<settings />");
            Scribe.loader.InitLoading(file); settings.ExposeData(); Scribe.loader.FinalizeLoading();
            Values(settings, defaults, "Missing-field fallback");
            double[] changed = { 2, .31, .08, .9, 1.25, .0034 };
            for (int i = 0; i < members.Length; i++) members[i].SetValue(settings, Convert.ChangeType(changed[i], members[i].FieldType));
            Scribe.saver.InitSaving(file, "settings"); settings.ExposeData(); Scribe.saver.FinalizeSaving();
            var loaded = new IsoRimSettings();
            Scribe.loader.InitLoading(file); loaded.ExposeData(); Scribe.loader.FinalizeLoading();
            Values(loaded, changed, "Settings round trip");
            IsoRimSettings previous = IsoRimMod.Settings;
            try
            {
                IsoRimMod.Settings = loaded;
                View3D.Tick();
                Material material = R3DMaterials.Lit(0);
                Check(material.GetFloat("_ClayMode") == 2 && Math.Abs(material.GetFloat("_ClayWrap") - .31f) < 1e-6
                    && Math.Abs(material.GetFloat("_ClayHighlight") - .08f) < 1e-6 && Math.Abs(material.GetFloat("_ClayRoughness") - .9f) < 1e-6,
                    "View3D frame did not synchronize model style");
                loaded.outlines = false; loaded.Reset(); Values(loaded, defaults, "Reset");
                Check(loaded.outlines, "Reset did not restore outlines");
                return new { settingsDefaults = defaults, persisted = changed, frameSync = true, outlines = OutlineProof(request), uiProxy = SettingsUiProxy(request) };
            }
            finally { IsoRimMod.Settings = previous; R3DMaterials.SyncModelStyle(0, 0, 0, 0); }
        }

        internal sealed class ComparisonSample
        {
            internal string Name;
            internal List<MeshDrawCommand> Commands = new List<MeshDrawCommand>();
            internal int[] Bounds;
        }

        internal static readonly string[] ComparisonPresets = { "Classic", "MatteWrapped", "MatteOrenNayar", "MatteNoOutlines" };

        private static object GameplayComparisonAndPerf(ProbeNormals.Request request)
        {
            IsoRimSettings previous = IsoRimMod.Settings;
            bool cull = Cull.Enabled, batches = PartBatches.Enabled;
            var owner = new GameObject("IsoRim gameplay style comparison");
            Camera camera = owner.AddComponent<Camera>();
            camera.enabled = false; camera.orthographic = true; camera.orthographicSize = 11; camera.aspect = 1; camera.cullingMask = 0;
            camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = Color.magenta;
            const int size = 512;
            RenderTexture target = RenderTexture.GetTemporary(size, size, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            RenderTexture active = RenderTexture.active; camera.targetTexture = target;
            var backend = UnityStubs.Rendering.CameraTextureRenderer.Current;
            Check(backend?.GetType().FullName == "RimWorldWaylith.Rendering.CameraTextureBaker", "Comparison GPU backend unavailable");
            FieldInfo serial = backend.GetType().GetField("_frameNumber", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo immediate = typeof(Graphics).GetField("ImmediateCapture", Hidden);
            Check(immediate != null, "Camera immediate submission seam unavailable");
            var frames = new List<object>(); var timings = new List<object>();
            var images = new Dictionary<string, byte[]>();
            var layers = new List<SectionLayer>();
            var restore = new List<Action>();
            Building_Door door = null;
            Pawn colonist = null;
            Vector3 center = new Vector3(Find.CurrentMap.Center.x + .5f, 0, Find.CurrentMap.Center.z + .5f);
            Camera.CameraCallback submit = null;
            try
            {
                IsoRimMod.Settings = new IsoRimSettings { squash = .003f, texturedModels = true, workAnimations = false };
                Cull.Enabled = false; PartBatches.Flush(); PartBatches.Enabled = false;
                colonist = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer);
                if (colonist.apparel.WornApparel.Count == 0)
                    colonist.apparel.Wear((Apparel)ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Apparel_BasicShirt"), ThingDefOf.Cloth));
                Check(colonist.apparel.WornApparel.Count > 0, "Comparison colonist is not dressed");
                Pawn muffalo = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.GetNamed("Muffalo"));
                door = (Building_Door)ThingMaker.MakeThing(ThingDefOf.Door, ThingDefOf.Steel);
                GenSpawn.Spawn(door, Find.CurrentMap.Center + new IntVec3(2, 0, 4), Find.CurrentMap);
                var definitions = new[] { "Armchair", "Table2x2c", "ElectricStove", "StandingLamp", "Campfire", "Plant_TreeOak", "SmoothFixture", "Colonist", "Muffalo", "Turret_MiniTurret", "Door", "GrowthVat" };
                // Keep the exact same Thing IDs and picked variants across every preset.
                var things = definitions.Select(name =>
                {
                    if (name == "SmoothFixture" || name == "Colonist" || name == "Muffalo" || name == "Door") return null;
                    ThingDef def = DefDatabase<ThingDef>.GetNamed(name);
                    Thing thing = ThingMaker.MakeThing(def, def.MadeFromStuff ? (name == "Armchair" ? ThingDefOf.Cloth : ThingDefOf.WoodLog) : null);
                    if (name == "Armchair")
                    {
                        thing.TryGetComp<CompColorable>()?.SetColor(new Color(.35f, .65f, .8f));
                        Check(thing.Stuff != null && thing.DrawColor != Color.white, "Comparison Armchair lacks stuff tint");
                    }
                    return thing;
                }).ToArray();
                Model smooth = (Model)typeof(StaticNormalCases).GetMethod("Fixture", Hidden).Invoke(null, new object[] { request.fixtureDirectory, false, true });
                for (int preset = 0; preset < ComparisonPresets.Length; preset++)
                {
                    IsoRimSettings st = IsoRimMod.Settings;
                    st.modelStyle = preset == 0 ? 0 : preset == 2 ? 2 : 1;
                    st.outlines = preset != 3; st.outlineWidth = preset == 0 ? 1f : .75f;
                    st.clayWrap = .22f; st.clayHighlight = .04f; st.clayRoughness = .65f;
                    DynamicModels.Forget(); People.ForgetAll();
                    R3DMaterials.SyncModelStyle(st.modelStyle, st.clayWrap, st.clayHighlight, st.clayRoughness);
                    var samples = new List<ComparisonSample>();
                    for (int i = 0; i < definitions.Length; i++)
                    {
                        string name = definitions[i];
                        Vector3 at = center + new Vector3((i % 4 - 1.5f) * 4, 0, (i / 4 - 1) * 4);
                        var sample = new ComparisonSample { Name = name }; samples.Add(sample);
                        void Bake(Thing thing, Model model, ModelEntry entry = null)
                        {
                            var layer = new SectionLayer_IsoRimBodies(new Section(Find.CurrentMap.Center, Find.CurrentMap)); layers.Add(layer);
                            Vector3 scale = entry != null ? entry.Fit(thing.def) * entry.def.scale : Vector3.one;
                            Check((bool)typeof(ModelBaker).GetMethod("Write", Hidden).Invoke(null, new object[] { layer, thing, model, at,
                                Quaternion.identity, Quaternion.identity, Vector3.zero, scale, entry?.def.offset ?? Vector3.zero, 0f,
                                thing.DrawColor, false, entry?.def.outline ?? 1f }), "Comparison static bake refused " + name);
                            CutTags.Finish(layer); NormalStreams.Finish(layer);
                            typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(layer, new object[] { MeshParts.All });
                            foreach (LayerSubMesh sub in layer.subMeshes.Where(sub => sub.mesh.vertexCount > 0)) Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                        }
                        using (MeshDrawSubmission.BeginCapture(sample.Commands))
                        {
                            if (i < 6 || name == "GrowthVat" || name == "Turret_MiniTurret")
                            {
                                Thing thing = things[i]; ThingDef def = thing.def;
                                thing.Position = new IntVec3((int)at.x, 0, (int)at.z);
                                Check(ModelLibrary.TryGet(def, out ModelEntry entry) && entry.HasModel, "Missing comparison model " + name);
                                Bake(thing, entry.Pick(thing), entry);
                                if (name == "Turret_MiniTurret")
                                {
                                    Check(entry.top != null, "Comparison turret lacks gun");
                                    DynamicModels.Draw(entry.top, at + Vector3.up * entry.def.topHeight, 25, entry.Fit(def) * entry.def.scale, thing.DrawColor, R3DTextures.ModelSwatch(thing));
                                }
                                if (name == "GrowthVat")
                                {
                                    Check(thing is Building_GrowthVat && entry.glass != null, "Comparison lacks Growth Vat chamber");
                                    typeof(Glass).GetMethod("Draw", Hidden).Invoke(null, new object[] { thing, entry });
                                }
                            }
                            else if (name == "SmoothFixture") Bake(ThingMaker.MakeThing(ThingDefOf.Steel), smooth);
                            else if (name == "Colonist") Check(People.Draw(colonist, at, out _, out _, out _), "Comparison People draw refused dressed colonist");
                            else if (name == "Muffalo") Check(Animals.Draw(muffalo, at), "Comparison Muffalo draw refused body/legs");
                            else if (name == "Door")
                            {
                                Check(Doors.Draw(door, .18f), "Comparison Door draw refused leaves");
                            }
                        }
                        Check(sample.Commands.Count > 0, "Comparison omitted submissions for " + name);
                        if (name == "Muffalo") Check(sample.Commands.Select(command => command.Matrix).Distinct().Count() >= 5, "Comparison Muffalo omitted legs");
                        if (name == "GrowthVat") Check(sample.Commands.Any(command => command.Material.DebugName == "IsoRim_Glass"), "Comparison omitted Glass.Mat");
                    }
                    var commands = samples.SelectMany(sample => sample.Commands).ToList();
                    // Camera.Render owns the approved baker's capture, render pass and fence. Replay
                    // the actual production submissions into its immediate list, without recooking meshes.
                    submit = rendering => { if (rendering == camera) ((List<MeshDrawCommand>)immediate.GetValue(null)).AddRange(commands); };
                    Camera.onPreRender += submit;
                    var materials = commands.Select(command => command.Material).Distinct().ToArray();
                    foreach (var mat in materials)
                    {
                        var view = mat.GetVector(Shader.PropertyToID("_IsoView")); var sun = mat.GetVector(Shader.PropertyToID("_IsoSun"));
                        var sky = mat.GetVector(Shader.PropertyToID("_IsoSky")); var cell = mat.GetVector(Shader.PropertyToID("_IsoCellMap"));
                        float glow = mat.GetFloat("_GlowMode");
                        restore.Add(() => { mat.SetVector("_IsoView", view); mat.SetVector("_IsoSun", sun); mat.SetVector("_IsoSky", sky); mat.SetVector("_IsoCellMap", cell); mat.SetFloat("_GlowMode", glow); });
                        mat.SetVector("_IsoSun", new System.Numerics.Vector4(.3f, .8f, .52f, 0));
                        mat.SetVector("_IsoSky", new System.Numerics.Vector4(1, 1, 1, 1)); mat.SetVector("_IsoCellMap", System.Numerics.Vector4.Zero); mat.SetFloat("_GlowMode", 0);
                    }
                    for (int angle = 0; angle < 2; angle++)
                    {
                        float pitch = angle == 0 ? 35.3f : 55f, yaw = angle == 0 ? 45f : 0f;
                        camera.ResetWorldToCameraMatrix();
                        camera.transform.rotation = Quaternion.Euler(pitch, yaw + 180, 0); camera.transform.position = center + Vector3.up - camera.transform.forward * 30;
                        camera.worldToCameraMatrix = camera.worldToCameraMatrix * Matrix4x4.Scale(new Vector3(1, View3D.Squash, 1));
                        Vector3 direction = -camera.transform.forward;
                        foreach (var mat in materials) mat.SetVector("_IsoView", new System.Numerics.Vector4(direction.x, direction.y, direction.z, View3D.Squash));
                        foreach (ComparisonSample sample in samples)
                        {
                            try { sample.Bounds = ComparisonBounds(sample.Commands, camera, size); }
                            catch (Exception error) { throw new InvalidOperationException(sample.Name + ": " + error.Message, error); }
                        }
                        int before = (int)serial.GetValue(backend); camera.Render(); int completed = (int)serial.GetValue(backend);
                        Check(completed == before + 1, "Comparison has no completed GPU render");
                        Check(backend.TryReadPixels(target, out byte[] raw), "Comparison GPU readback unavailable");
                        Check(raw.Length == size * size * 4, "Comparison framebuffer dimensions differ");
                        var visible = samples.Select(sample => new
                        {
                            name = sample.Name,
                            commands = sample.Commands.Count,
                            bounds = sample.Bounds,
                            pixels = ComparisonPixels(raw, sample, samples, size)
                        }).ToArray();
                        Check(visible.All(sample => sample.pixels >= 5), "Comparison sample is empty, occluded or omitted: " + string.Join(",", visible.Where(sample => sample.pixels < 5).Select(sample => sample.name)));
                        string stem = "gameplay-" + ComparisonPresets[preset] + "-" + (angle == 0 ? "isometric" : "straight");
                        File.WriteAllBytes(Path.Combine(request.outputDirectory, stem + ".rgba"), raw);
                        RenderTexture.active = target; var image = new Texture2D(size, size, TextureFormat.RGBA32, false);
                        image.ReadPixels(new Rect(0, 0, size, size), 0, 0); image.Apply(); byte[] png = image.EncodeToPNG(); UnityEngine.Object.Destroy(image);
                        File.WriteAllBytes(Path.Combine(request.outputDirectory, stem + ".png"), png); images.Add(stem, raw);
                        frames.Add(new
                        {
                            name = stem,
                            preset = ComparisonPresets[preset],
                            pitch,
                            yaw,
                            orthographicSize = 11,
                            width = size,
                            height = size,
                            samples = visible,
                            commands = commands.Count,
                            gpuFrameBefore = before,
                            gpuFrameCompleted = completed,
                            rawPath = stem + ".rgba",
                            rawHash = Convert.ToHexString(SHA256.HashData(raw)),
                            pngPath = stem + ".png",
                            pngHash = Convert.ToHexString(SHA256.HashData(png))
                        });
                        if (angle == 0 && preset < 3)
                        {
                            for (int warm = 0; warm < 3; warm++) camera.Render();
                            int start = (int)serial.GetValue(backend); var ms = new List<double>();
                            for (int warm = 0; warm < 60; warm++)
                            {
                                long ticks = System.Diagnostics.Stopwatch.GetTimestamp(); camera.Render();
                                ms.Add((System.Diagnostics.Stopwatch.GetTimestamp() - ticks) * 1000.0 / System.Diagnostics.Stopwatch.Frequency);
                            }
                            int finish = (int)serial.GetValue(backend); double[] sorted = ms.OrderBy(value => value).ToArray();
                            Check(finish == start + 60 && ms.All(value => value > 0), "Comparison lacks 60 completed warm render timings");
                            timings.Add(new
                            {
                                preset = ComparisonPresets[preset],
                                commands = commands.Count,
                                gpuFrameBefore = start,
                                gpuFrameCompleted = finish,
                                samplesMs = ms,
                                medianMs = (sorted[29] + sorted[30]) / 2,
                                meanMs = ms.Average(),
                                p95Ms = sorted[56]
                            });
                        }
                    }
                    Camera.onPreRender -= submit; submit = null;
                    foreach (Action reset in restore) reset(); restore.Clear();
                    foreach (SectionLayer layer in layers) foreach (LayerSubMesh sub in layer.subMeshes) UnityEngine.Object.Destroy(sub.mesh); layers.Clear();
                }
                foreach (string angle in new[] { "isometric", "straight" })
                    for (int a = 0; a < 4; a++) for (int b = a + 1; b < 4; b++)
                    {
                        byte[] first = images["gameplay-" + ComparisonPresets[a] + "-" + angle], second = images["gameplay-" + ComparisonPresets[b] + "-" + angle];
                        if (a == 1 && b == 2)
                            Check(Enumerable.Range(0, size * size).All(pixel => IsComparisonBackground(first, pixel * 4) == IsComparisonBackground(second, pixel * 4)),
                                "Wrapped/Oren-Nayar changed comparison geometry instead of shading");
                        int changed = Enumerable.Range(0, size * size).Count(pixel => Enumerable.Range(0, 3).Any(c => Math.Abs(first[pixel * 4 + c] - second[pixel * 4 + c]) > 2));
                        Check(changed >= 20, "Comparison presets are not visibly distinct: " + ComparisonPresets[a] + "/" + ComparisonPresets[b]);
                    }
                return new { frames, timings, dressedApparel = colonist.apparel.WornApparel.Select(item => item.def.defName).ToArray(), backend = backend.GetType().FullName, timingMethod = "Stopwatch.GetTimestamp around Camera.Render; completed native GPU fence" };
            }
            finally
            {
                if (submit != null) Camera.onPreRender -= submit;
                foreach (Action reset in restore) reset();
                foreach (SectionLayer layer in layers) foreach (LayerSubMesh sub in layer.subMeshes) UnityEngine.Object.Destroy(sub.mesh);
                if (door?.Spawned == true) door.DeSpawn();
                DynamicModels.Forget(); People.ForgetAll(); Cull.Enabled = cull; PartBatches.Enabled = batches;
                IsoRimMod.Settings = previous; R3DMaterials.SyncModelStyle(0, 0f, 0f, 0f);
                RenderTexture.active = active; RenderTexture.ReleaseTemporary(target); UnityEngine.Object.Destroy(owner);
            }
        }

        internal static int[] ComparisonBounds(List<MeshDrawCommand> commands, Camera camera, int size)
        {
            var points = new List<Vector3>(); Matrix4x4 projection = camera.projectionMatrix * camera.worldToCameraMatrix;
            foreach (MeshDrawCommand command in commands)
            {
                var bounds = command.Mesh.Bounds;
                for (int corner = 0; corner < 8; corner++)
                {
                    var p = System.Numerics.Vector3.Transform(new System.Numerics.Vector3((corner & 1) == 0 ? bounds.Min.X : bounds.Max.X,
                        (corner & 2) == 0 ? bounds.Min.Y : bounds.Max.Y, (corner & 4) == 0 ? bounds.Min.Z : bounds.Max.Z), System.Numerics.Matrix4x4.Transpose(command.Matrix));
                    Vector4 clip = projection * new Vector4(p.X, p.Y, p.Z, 1);
                    points.Add(new Vector3((clip.x / clip.w + 1) * size / 2, (clip.y / clip.w + 1) * size / 2, clip.z / clip.w));
                }
            }
            Check(points.All(point => point.x > 2 && point.x < size - 3 && point.y > 2 && point.y < size - 3 && point.z > -1 && point.z < 1), "Comparison clips a submitted sample: " + string.Join(";", points.Where(point => point.x <= 2 || point.x >= size - 3 || point.y <= 2 || point.y >= size - 3 || point.z <= -1 || point.z >= 1).Take(3).Select(point => point.ToString())));
            return new[] { (int)Math.Floor(points.Min(p => p.x)), (int)Math.Floor(points.Min(p => p.y)), (int)Math.Ceiling(points.Max(p => p.x)), (int)Math.Ceiling(points.Max(p => p.y)) };
        }

        internal static bool IsComparisonBackground(byte[] raw, int offset) => raw[offset] == 255 && raw[offset + 1] == 0 && raw[offset + 2] == 255;

        internal static int ComparisonPixels(byte[] raw, ComparisonSample sample, List<ComparisonSample> samples, int size)
        {
            int count = 0; int[] b = sample.Bounds;
            for (int y = b[1]; y <= b[3]; y++) for (int x = b[0]; x <= b[2]; x++)
            {
                if (samples.Any(other => other != sample && x >= other.Bounds[0] && x <= other.Bounds[2] && y >= other.Bounds[1] && y <= other.Bounds[3])) continue;
                int offset = (y * size + x) * 4;
                if (!IsComparisonBackground(raw, offset)) count++;
            }
            return count;
        }

        internal const BindingFlags Hidden = BindingFlags.Static | BindingFlags.NonPublic;
        private sealed class OutlineGeometry
        {
            internal List<DynamicModels.Part> Parts;
            internal List<LayerSubMesh> Static;
            internal People.Look Look;
            internal bool OwnParts;
            internal Vector3[] Base, Shell, Face, Texture;
        }

        private static OutlineGeometry Geometry(Model model, string kind)
        {
            var result = new OutlineGeometry();
            if (kind == "static")
            {
                var layer = new SectionLayer_IsoRimBodies(new Section(new IntVec3(0, 0, 0), Find.CurrentMap));
                Check((bool)typeof(ModelBaker).GetMethod("Write", Hidden).Invoke(null, new object[] { layer, ThingMaker.MakeThing(ThingDefOf.Steel), model,
                    Vector3.zero, Quaternion.identity, Quaternion.identity, new Vector3(0, 3, 0), Vector3.one, Vector3.zero, 0f, Color.white, false, 1f }), "Static outline fixture refused");
                CutTags.Finish(layer); NormalStreams.Finish(layer);
                typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(layer, new object[] { MeshParts.All });
                result.Static = layer.subMeshes.Where(sub => sub.mesh.vertexCount > 0).ToList();
                Vector3[] Vertices(Func<LayerSubMesh, bool> predicate) => result.Static.Where(predicate).SelectMany(sub => sub.mesh.vertices)
                    .Select(v => new Vector3(v.x, v.y * View3D.Squash, v.z)).ToArray();
                result.Shell = Vertices(sub => sub.material == R3DMaterials.Outline);
                result.Face = Vertices(sub => sub.material.GetFloat("_LightMode") == 1 && sub.material.mainTexture == null);
                result.Texture = Vertices(sub => sub.material.mainTexture != null);
                result.Base = model.positions.Select(v => v + new Vector3(0, 3, 0)).ToArray();
            }
            else
            {
                if (kind == "dynamic") result.Parts = (List<DynamicModels.Part>)typeof(DynamicModels).GetMethod("PartsFor", Hidden).Invoke(null, new object[] { model, Color.white, "Metal" });
                else if (kind == "merged")
                {
                    result.OwnParts = true;
                    result.Parts = DynamicModels.BuildMerged(new List<Model> { model }, new List<Matrix4x4> { Matrix4x4.Scale(new Vector3(1.2f, .8f, 1.1f)) }, new List<Color> { Color.white }, 0);
                }
                else
                {
                    result.Look = new People.Look { lower = model };
                    result.Parts = (List<DynamicModels.Part>)typeof(People).GetMethod("Merged", Hidden).Invoke(null, new object[] { result.Look, false, false, 0 });
                }
                result.Shell = result.Parts.Where(part => part.outline).SelectMany(part => part.mesh.vertices).ToArray();
                result.Face = result.Parts.Where(part => !part.outline && part.material.mainTexture == null).SelectMany(part => part.mesh.vertices).ToArray();
                result.Texture = result.Parts.Where(part => part.material.mainTexture != null).SelectMany(part => part.mesh.vertices).ToArray();
                result.Base = kind == "dynamic" ? model.positions : result.Face;
            }
            return result;
        }

        private static object OutlineProof(ProbeNormals.Request request)
        {
            string file = Path.Combine(request.fixtureDirectory, "outline-cube.obj");
            File.WriteAllText(Path.ChangeExtension(file, ".mtl"), "newmtl sample\nKd 0.6 0.6 0.6\nmap_Kd Textures/Diagnostics/NormalProofNeutral.png\n");
            File.WriteAllText(file, "mtllib outline-cube.mtl\nusemtl sample\nv -1 0 -1\nv 1 0 -1\nv 1 2 -1\nv -1 2 -1\nv -1 0 1\nv 1 0 1\nv 1 2 1\nv -1 2 1\nvt 0 0\n"
                + "f 1/1 4/1 3/1 2/1\nf 5/1 6/1 7/1 8/1\nf 1/1 2/1 6/1 5/1\nf 4/1 8/1 7/1 3/1\nf 1/1 5/1 8/1 4/1\nf 2/1 3/1 7/1 6/1\n");
            Model model = ObjLoader.Load(file);
            Check(model != null && model.positions.Length == 36 && model.hull.Any(v => v.sqrMagnitude > .0001f), "Outline cube has no real hull");
            var rows = new List<object>();
            outlineFrames.Clear();
            var geometries = new List<OutlineGeometry>();
            IsoRimSettings st = IsoRimMod.Settings;
            st.texturedModels = true;
            try
            {
                foreach (string kind in new[] { "static", "dynamic", "merged", "people" })
                {
                    var samples = new List<OutlineGeometry>();
                    var pixels = new List<int>();
                    foreach (float width in new[] { 1f, .75f, 0f, .001f, -1f, 1f })
                    {
                        st.outlineWidth = width; st.outlines = samples.Count != 5;
                        DynamicModels.Forget();
                        OutlineGeometry geometry = Geometry(model, kind); samples.Add(geometry); geometries.Add(geometry);
                        bool visible = st.outlines && width > .001f;
                        Check(geometry.Shell.Length == (visible ? 36 : 0), "Outline suppression differs: " + kind + " width " + width);
                        if (samples.Count > 1)
                            Check(geometry.Face.SequenceEqual(samples[0].Face) && geometry.Texture.SequenceEqual(samples[0].Texture), "Outline width moved plain or textured faces: " + kind);
                        if (samples.Count == 2)
                        {
                            for (int i = 0; i < 36; i++)
                                Check((geometry.Shell[i] - geometry.Base[i] - (samples[0].Shell[i] - samples[0].Base[i]) * .75f).sqrMagnitude < .000001f,
                                    "Outline vertex ratio differs: " + kind);
                            Check((samples[0].Shell[0] - samples[0].Base[0]).sqrMagnitude > .0001f, "Outline ratio witness has zero signal");
                        }
                        if (samples.Count <= 3) pixels.Add(OutlinePixels(request, kind + "-" + samples.Count, geometry));
                    }
                    Check(pixels[0] > pixels[1] && pixels[1] > pixels[2] && pixels[2] > 1000, "Outline framebuffer silhouette did not shrink: " + kind + " " + string.Join(",", pixels));
                    if (kind != "static")
                    {
                        st.outlines = false; st.outlineWidth = 1;
                        Check(OutlinePixels(request, kind + "-cached-disabled", samples[0]) == pixels[2], "Cached outline commands survived the outline toggle: " + kind);
                        st.outlines = true; st.outlineWidth = 0;
                        Check(OutlinePixels(request, kind + "-cached-zero", samples[0]) == pixels[2], "Cached outline commands survived zero width: " + kind);
                    }
                    st.outlines = true; st.outlineWidth = 1f;
                    model.noOutline = Enumerable.Repeat(true, 36).ToArray(); DynamicModels.Forget();
                    OutlineGeometry masked = Geometry(model, kind); geometries.Add(masked);
                    Check(masked.Shell.Length == 0 && masked.Face.SequenceEqual(samples[0].Face) && masked.Texture.SequenceEqual(samples[0].Texture), "noOutline mask changed geometry or gained a shell: " + kind);
                    model.noOutline = null;
                    rows.Add(new { path = kind, fullWidthVertices = samples[0].Shell.Length, reducedWidthVertices = samples[1].Shell.Length, pixels });
                }
                st.outlines = true; st.outlineWidth = 1f; DynamicModels.Forget();
                OutlineGeometry dynamicBefore = Geometry(model, "dynamic"), personBefore = Geometry(model, "people");
                geometries.Add(dynamicBefore); geometries.Add(personBefore);
                var looks = (Dictionary<int, People.Look>)typeof(People).GetField("looks", Hidden).GetValue(null);
                looks.Add(int.MinValue, personBefore.Look);
                var signature = typeof(View3D).GetMethod("StateSignature", Hidden);
                st.outlineWidth = 0;
                int zero = (int)signature.Invoke(null, null);
                st.outlineWidth = .25f;
                Check(zero != (int)signature.Invoke(null, null), "Zero outline width aliases the minimum enabled width");
                st.outlines = false;
                int disabled = (int)signature.Invoke(null, null);
                st.outlineWidth = 1;
                Check(disabled == (int)signature.Invoke(null, null), "Disabled outlines depend on width");
                st.outlines = true;
                int before = (int)signature.Invoke(null, null);
                st.outlineWidth = .75f;
                int after = (int)signature.Invoke(null, null);
                Check(before != after, "Outline width left StateSignature unchanged");
                typeof(View3D).GetField("appliedSig", Hidden).SetValue(null, before);
                typeof(View3D).GetField("pendingSig", Hidden).SetValue(null, after);
                typeof(View3D).GetField("pendingSince", Hidden).SetValue(null, -1000f);
                typeof(View3D).GetMethod("RegenerateIfStateChanged", Hidden).Invoke(null, null);
                OutlineGeometry dynamicAfter = Geometry(model, "dynamic"); geometries.Add(dynamicAfter);
                var personAfter = (List<DynamicModels.Part>)typeof(People).GetMethod("Merged", Hidden).Invoke(null, new object[] { personBefore.Look, false, false, 0 });
                Check(!ReferenceEquals(dynamicBefore.Parts, dynamicAfter.Parts) && !ReferenceEquals(personBefore.Parts, personAfter), "Outline state retained a cached dynamic or People mesh");
                Check(personBefore.Look.meshes.Count > 0 && !looks.ContainsKey(int.MinValue), "People look invalidation did not clear the lookup");
                Vector3[] personShell = personAfter.Where(part => part.outline).SelectMany(part => part.mesh.vertices).ToArray();
                for (int i = 0; i < 36; i++)
                {
                    Check((dynamicAfter.Shell[i] - dynamicAfter.Base[i] - (dynamicBefore.Shell[i] - dynamicBefore.Base[i]) * .75f).sqrMagnitude < .000001f, "Dynamic cache rebuilt wrong outline width");
                    Check((personShell[i] - model.positions[i] - (personBefore.Shell[i] - model.positions[i]) * .75f).sqrMagnitude < .000001f, "People cache rebuilt wrong outline width");
                }
                Check(ModelBaker.OutlineShade == .25f && View3D.OutlinePush == .20f, "Outline shade or push changed");
                return new { paths = rows, signatureBefore = before, signatureAfter = after, dynamicAndPeopleRebuilt = true, frames = outlineFrames.ToArray() };
            }
            finally
            {
                foreach (OutlineGeometry geometry in geometries)
                {
                    if (geometry.Static != null) foreach (LayerSubMesh sub in geometry.Static) UnityEngine.Object.Destroy(sub.mesh);
                    else if (geometry.Look != null) geometry.Look.Forget();
                    else if (geometry.OwnParts) DynamicModels.Destroy(geometry.Parts);
                }
                DynamicModels.Forget();
            }
        }

        private static readonly List<object> outlineFrames = new List<object>();

        private static readonly HashSet<string> uiLabels = new HashSet<string>();
        private static readonly Dictionary<string, float> uiSliders = new Dictionary<string, float> { { "Soft light wrap", .31f }, { "Matte highlight", .08f }, { "Roughness", .9f }, { "Outline width", 1.25f } };
        private static string uiSlider;
        private static bool UiLabel(object[] __args)
        {
            foreach (object value in __args)
                if (value is string text)
                {
                    uiLabels.Add(text);
                    if (uiSliders.ContainsKey(text)) uiSlider = text;
                }
            return false;
        }
        private static bool UiButton(object[] __args, ref bool __result)
        {
            string label = __args.OfType<string>().FirstOrDefault() ?? "";
            uiLabels.Add(label); __result = label.StartsWith("Model shading:", StringComparison.Ordinal); return false;
        }
        private static bool UiSlider(ref float __result)
        {
            Check(uiSlider != null && uiSliders.ContainsKey(uiSlider), "UI slider has no model-style label");
            __result = uiSliders[uiSlider]; uiSlider = null; return false;
        }

        private static object SettingsUiProxy(ProbeNormals.Request request)
        {
            var hook = new Harmony("isorim.style.settings-ui-proof");
            var label = typeof(Widgets).GetMethods().Where(method => method.Name == "Label" && method.GetParameters().Length >= 2 && method.GetParameters()[0].ParameterType == typeof(Rect) && method.GetParameters()[1].ParameterType == typeof(string)).OrderBy(method => method.GetParameters().Length).First();
            var button = typeof(Widgets).GetMethods().Where(method => method.Name == "ButtonText" && method.GetParameters().Length >= 2 && method.GetParameters()[0].ParameterType == typeof(Rect) && method.GetParameters()[1].ParameterType == typeof(string)).OrderBy(method => method.GetParameters().Length).First();
            var slider = typeof(Listing_Standard).GetMethods().Where(method => method.Name == "Slider" && method.ReturnType == typeof(float)).OrderBy(method => method.GetParameters().Length).First();
            hook.Patch(label, prefix: new HarmonyMethod(typeof(ModelStyleCases).GetMethod("UiLabel", Hidden)));
            hook.Patch(button, prefix: new HarmonyMethod(typeof(ModelStyleCases).GetMethod("UiButton", Hidden)));
            hook.Patch(slider, prefix: new HarmonyMethod(typeof(ModelStyleCases).GetMethod("UiSlider", Hidden)));
            string[] names = { "Classic faceted", "Matte clay (wrapped)", "Matte clay (Oren-Nayar)" };
            var readings = new List<object>();
            try
            {
                for (int mode = 0; mode < 3; mode++)
                {
                    uiLabels.Clear(); uiSlider = null;
                    IsoRimMod.Settings.modelStyle = mode;
                    var listing = new Listing_Standard(); listing.Begin(new Rect(0, 0, 700, 1600));
                    typeof(SettingsWindow).GetMethod("ObjectsTab", Hidden).Invoke(null, new object[] { null, listing, IsoRimMod.Settings });
                    listing.End();
                    Check(uiLabels.Contains("Model style") && uiLabels.Contains("Model shading: " + names[mode]), "Actual settings tab lacks the shading selector");
                    foreach (string name in new[] { "Soft light wrap", "Matte highlight", "Roughness", "Outline width" })
                        Check(uiLabels.Contains(name) == (name == "Outline width" || name == "Roughness" && mode == 2 || (name == "Soft light wrap" || name == "Matte highlight") && mode > 0), "Model-style slider visibility differs: " + name);
                    FloatMenu menu = Find.WindowStack.Windows.OfType<FloatMenu>().LastOrDefault();
                    Check(menu != null, "Shading selector did not open the real FloatMenu");
                    var options = (List<FloatMenuOption>)typeof(FloatMenu).GetField("options", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(menu);
                    Check(options.Select(option => option.Label).SequenceEqual(names), "Shading selector choices differ");
                    options[(mode + 1) % 3].action(); menu.Close();
                    Check(IsoRimMod.Settings.modelStyle == (mode + 1) % 3, "Shading menu callback did not update settings");
                    View3D.Tick();
                    Check(R3DMaterials.LitModel.GetFloat("_ClayMode") == (mode + 1) % 3, "UI selection did not reach live materials");
                    readings.Add(new { mode, labels = uiLabels.OrderBy(value => value).ToArray() });
                }
                IsoRimSettings st = IsoRimMod.Settings;
                Check(st.clayWrap == .31f && st.clayHighlight == .08f && st.clayRoughness == .9f && st.outlineWidth == 1.25f, "Actual tab did not accept slider input");
                string file = Path.Combine(request.fixtureDirectory, "ui-proxy-settings.xml");
                Scribe.saver.InitSaving(file, "settings"); st.ExposeData(); Scribe.saver.FinalizeSaving();
                var reopened = new IsoRimSettings(); Scribe.loader.InitLoading(file); reopened.ExposeData(); Scribe.loader.FinalizeLoading();
                Check(reopened.modelStyle == 0 && reopened.clayWrap == .31f && reopened.clayHighlight == .08f && reopened.clayRoughness == .9f && reopened.outlineWidth == 1.25f,
                    "UI selections did not survive settings reload");
                return new { automatedProxyOnly = true, manualWitnessPerformed = false, modes = readings, settingsReloaded = true };
            }
            finally { hook.Unpatch(label, HarmonyPatchType.Prefix, hook.Id); hook.Unpatch(button, HarmonyPatchType.Prefix, hook.Id); hook.Unpatch(slider, HarmonyPatchType.Prefix, hook.Id); }
        }

        private static int OutlinePixels(ProbeNormals.Request request, string name, OutlineGeometry geometry)
        {
            const int size = 512;
            var owner = new GameObject("IsoRim controlled outline framebuffer");
            Camera camera = owner.AddComponent<Camera>();
            Vector3 direction = new Vector3(3, 2, 4).normalized;
            Vector3 center = geometry.Face.Aggregate(Vector3.zero, (sum, v) => sum + v) / geometry.Face.Length;
            camera.enabled = false; camera.orthographic = true; camera.aspect = 1; camera.cullingMask = 0; camera.orthographicSize = 2.5f;
            camera.transform.rotation = Quaternion.LookRotation(-direction, Vector3.up);
            camera.transform.position = center + direction * 10;
            camera.worldToCameraMatrix = camera.worldToCameraMatrix * Matrix4x4.Scale(new Vector3(1, View3D.Squash, 1));
            RenderTexture target = RenderTexture.GetTemporary(size, size, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            RenderTexture previous = RenderTexture.active;
            var materials = geometry.Static != null ? geometry.Static.Select(sub => sub.material).Distinct().ToArray() : geometry.Parts.Select(part => part.material).Distinct().ToArray();
            var states = materials.Select(mat => (mat, light: mat.GetFloat("_LightMode"), glow: mat.GetFloat("_GlowMode"), view: mat.GetVector("_IsoView"), cell: mat.GetVector("_IsoCellMap"))).ToArray();
            bool cull = Cull.Enabled, batches = PartBatches.Enabled;
            Texture2D image = null;
            try
            {
                foreach (Material mat in materials)
                {
                    mat.SetFloat("_LightMode", 0); mat.SetFloat("_GlowMode", 0);
                    mat.SetVector("_IsoView", new Vector4(direction.x, direction.y, direction.z, View3D.Squash)); mat.SetVector("_IsoCellMap", Vector4.zero);
                }
                Cull.Enabled = false; PartBatches.Flush(); PartBatches.Enabled = false;
                var commands = new List<MeshDrawCommand>();
                using (MeshDrawSubmission.BeginCapture(commands))
                {
                    if (geometry.Static != null)
                        foreach (LayerSubMesh sub in geometry.Static) Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                    else DynamicModels.DrawParts(geometry.Parts, Vector3.zero, 0, Matrix4x4.identity);
                }
                Check(commands.Count > 0, "Outline framebuffer submitted no production geometry");
                byte[] raw = MovingNormalCases.RenderProofScene(target, camera, commands, out int before, out int completed);
                Check(raw.Length == size * size * 4, "Controlled outline framebuffer dimensions differ");
                int pixels = 0;
                for (int i = 0; i < raw.Length; i += 4) if (!(raw[i] == 255 && raw[i + 1] == 0 && raw[i + 2] == 255)) pixels++;
                string stem = "controlled-outline-" + name;
                File.WriteAllBytes(Path.Combine(request.outputDirectory, stem + ".rgba"), raw);
                RenderTexture.active = target; image = new Texture2D(size, size, TextureFormat.RGBA32, false);
                image.ReadPixels(new Rect(0, 0, size, size), 0, 0); image.Apply(); byte[] png = image.EncodeToPNG();
                File.WriteAllBytes(Path.Combine(request.outputDirectory, stem + ".png"), png);
                outlineFrames.Add(new
                {
                    name,
                    pixels,
                    commands = commands.Count,
                    gpuFrameBefore = before,
                    gpuFrameCompleted = completed,
                    rawPath = stem + ".rgba",
                    rawHash = Convert.ToHexString(SHA256.HashData(raw)),
                    pngPath = stem + ".png",
                    pngHash = Convert.ToHexString(SHA256.HashData(png))
                });
                return pixels;
            }
            finally
            {
                foreach (var state in states)
                {
                    state.mat.SetFloat("_LightMode", state.light); state.mat.SetFloat("_GlowMode", state.glow);
                    state.mat.SetVector("_IsoView", state.view); state.mat.SetVector("_IsoCellMap", state.cell);
                }
                Cull.Enabled = cull; PartBatches.Enabled = batches; RenderTexture.active = previous;
                RenderTexture.ReleaseTemporary(target); if (image != null) UnityEngine.Object.Destroy(image); UnityEngine.Object.Destroy(owner);
            }
        }

        // Fixed double-precision angular Oren-Nayar references: incidence/reflection angles,
        // sin(max(theta)) * tan(min(theta)), and the projected azimuth angle. No production helpers.
        internal static double Reference(int sample, bool authored, int mode)
        {
            double[,,] rgb = { { { .543180084565, .444063253520 }, { .406548999604, .466124226562 } },
                { { .3132, .3132 }, { .504641407082, .412556924411 } },
                { { .550853781860, .450336692263 }, { .445017746988, .500762121535 } } };
            return rgb[sample, authored ? 1 : 0, mode - 1];
        }

        internal static double HighlightReference(int sample, bool authored, int mode)
        {
            // Authored Mode 2 uses L = V = N: diffuse .490514950166, highlighted .503309813865.
            // The 3.262690243/255 lift rejects an omitted highlight at the unchanged 2/255 tolerance.
            double[,,] rgb = { { { .552116131114, .450699256114 }, { .406951311423, .503309813865 } },
                { { .3132, .3132 }, { .511319890787, .503309813865 } },
                { { .560395576570, .457417538045 }, { .445642119697, .503309813865 } } };
            return rgb[sample, authored ? 1 : 0, mode - 1];
        }

        internal static string Run(string id, ProbeNormals.Request request)
        {
            switch (id)
            {
                case "ModelStyleCases.GameplayComparisonAndPerf":
                    string comparisonPath = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(comparisonPath, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = GameplayComparisonAndPerf(request) }));
                    return comparisonPath;
                case "ModelStyleCases.ControlledOutlines":
                    string controlledPath = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(controlledPath, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = ControlledOutlines(request) }));
                    return controlledPath;
                case "ModelStyleCases.DiffuseAndHighlightModes": return MovingNormalCases.Run(id, request.outputDirectory, request.fixtureDirectory);
                case "ModelStyleCases.ProtectedExceptions":
                    string path = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = new { captures = StaticNormalCases.ProtectedPixels(request.fixtureDirectory, request.outputDirectory) } }));
                    return path;
                default: throw new InvalidOperationException("Unknown model style case: " + id);
            }
        }
    }
    internal static class SceneStyleCases
    {
        internal static readonly string[] Cases = { "SceneStyleCases.WallsRocksAndRoofs", "SceneStyleCases.TerrainCardsSlabsAndWater", "SceneStyleCases.ColonySceneComparisonAndPerf" };

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
        private static void SunPrefix(ref System.Numerics.Vector3? sunDirection)
        {
            sunDirection = new System.Numerics.Vector3(0f, 0.8f, 0.6f);
        }



        private static object WallsRocksAndRoofs(ProbeNormals.Request request)
        {
            string root = Path.Combine(AppContext.BaseDirectory, "Mods", "IsoRim", "1.7", "Shaders");
            string[] files = { "IsoRim.vert.glsl", "IsoRimTex.vert.glsl", "IsoRimSolid.frag.glsl" };
            string[] sources = files.Select(file => File.ReadAllText(Path.Combine(root, file))).ToArray();
            string Layout(string source) => System.Text.RegularExpressions.Regex.Replace(source.Substring(source.IndexOf("uniform MaterialUniforms", StringComparison.Ordinal),
                source.IndexOf("} uMaterial;", StringComparison.Ordinal) - source.IndexOf("uniform MaterialUniforms", StringComparison.Ordinal)), @"//[^\r\n]*|\s+", "");
            Check(sources.All(source => Layout(source) == Layout(sources[0])), "Shader material layouts diverged");
            string[] requiredFields = { "_ClayMode", "_ClayWrap", "_ClayHighlight", "_ClayRoughness", "_SceneClay" };
            foreach (string field in requiredFields)
                Check(sources.All(source => source.Contains("float " + field + "; // @default = 0.0")), "Missing zero-default style field: " + field);

            var syncScene = typeof(R3DMaterials).GetMethod("SyncSceneStyle", new[] { typeof(int), typeof(float), typeof(float), typeof(float) });
            Check(syncScene != null, "Missing explicit scene-style API");
            var syncModel = typeof(R3DMaterials).GetMethod("SyncModelStyle", new[] { typeof(int), typeof(float), typeof(float), typeof(float) });
            Check(syncModel != null, "Missing explicit model-style API");

            syncScene.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            syncModel.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            Material wallSolid = null;
            Material wallTex = null;
            Material roofMat = null;
            try
            {
                syncModel.Invoke(null, new object[] { 2, 0.22f, 0.04f, 0.65f });
                wallSolid = R3DMaterials.Lit(0, 1);
                wallTex = R3DMaterials.LitTexture("IsoRim/Walls/BricksSide", Color.white, 0, 1);
                Material mark = IsoGlow.MarkFor(wallSolid);
                roofMat = R3DMaterials.Roof;
                Check(wallSolid.GetFloat("_SceneClay") == 0f && wallSolid.GetFloat("_ClayMode") == 0f, "SyncModelStyle mutated curve 1 wall solid");
                Check(wallTex.GetFloat("_SceneClay") == 0f && wallTex.GetFloat("_ClayMode") == 0f, "SyncModelStyle mutated curve 1 wall texture");
                Check(mark.GetFloat("_SceneClay") == 0f && mark.GetFloat("_NormalSpace") == 0f, "SyncModelStyle mutated light mark");
                Check(roofMat.GetFloat("_SceneClay") == 0f && roofMat.GetFloat("_ClayMode") == 0f, "SyncModelStyle mutated roof");
            }
            finally
            {
                syncModel.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            }

            roofMat = R3DMaterials.Roof;
            MethodInfo texMatMethod = typeof(SectionLayer_IsoRimRoofs).GetMethod("TextureMaterial", BindingFlags.NonPublic | BindingFlags.Static);
            Material texRoof = (Material)texMatMethod.Invoke(null, new object[] { 0, 0 });
            PropertyInfo shipMatProp = typeof(ShipRoofs).GetProperty("Mat", BindingFlags.NonPublic | BindingFlags.Static);
            Material shipMat = (Material)shipMatProp.GetValue(null);

            // Assert style transitions across 0 -> 1 -> 2 -> 0 on cached roof/ship-roof materials
            syncScene.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            Check(roofMat.GetFloat("_SceneClay") == 0f && roofMat.GetFloat("_ClayMode") == 0f && roofMat.GetFloat("_LightMode") == 3f && roofMat.GetFloat("_GlowMode") == 2f,
                "Cached roof not reset in mode 0");
            Check(texRoof.GetFloat("_SceneClay") == 0f && texRoof.GetFloat("_ClayMode") == 0f && texRoof.GetFloat("_LightMode") == 3f && texRoof.GetFloat("_GlowMode") == 2f
                && Math.Abs(texRoof.color.r - SectionLayer_IsoRimRoofs.Styles[0].r * 1.25f) < 0.01f,
                "Cached textured roof tint not 1.25x in mode 0");
            Check(shipMat.GetFloat("_SceneClay") == 0f && shipMat.GetFloat("_ClayMode") == 0f && shipMat.GetFloat("_LightMode") == 3f && shipMat.GetFloat("_GlowMode") == 2f
                && shipMat.GetFloat("_NormalSpace") == 1f,
                "Cached ship roof not reset in mode 0");

            syncScene.Invoke(null, new object[] { 1, 0.22f, 0.04f, 0.65f });
            wallSolid = R3DMaterials.Lit(0, 1);
            wallTex = R3DMaterials.LitTexture("IsoRim/Walls/BricksSide", Color.white, 0, 1);
            Material newWallTex = R3DMaterials.LitTexture("IsoRim/Walls/PlanksTop", Color.white, 0, 1);
            Check(wallSolid.GetFloat("_SceneClay") == 1f && wallSolid.GetFloat("_ClayMode") == 1f
                && Math.Abs(wallSolid.GetFloat("_ClayWrap") - 0.22f) < 1e-5f
                && Math.Abs(wallSolid.GetFloat("_ClayHighlight") - 0.04f) < 1e-5f
                && Math.Abs(wallSolid.GetFloat("_ClayRoughness") - 0.65f) < 1e-5f
                && wallSolid.GetFloat("_LightMode") == 2f,
                "SyncSceneStyle did not update cached wall solid");
            Check(wallTex.GetFloat("_SceneClay") == 1f && wallTex.GetFloat("_ClayMode") == 1f && wallTex.GetFloat("_LightMode") == 2f,
                "SyncSceneStyle did not update cached wall texture");
            Check(newWallTex.GetFloat("_SceneClay") == 1f && newWallTex.GetFloat("_ClayMode") == 1f && newWallTex.GetFloat("_LightMode") == 2f,
                "Newly created wall texture missed scene clay sync");
            Check(roofMat.GetFloat("_SceneClay") == 1f && roofMat.GetFloat("_ClayMode") == 1f && Math.Abs(roofMat.GetFloat("_ClayWrap") - 0.22f) < 1e-5f,
                "Cached roof transition to mode 1 failed");
            Check(texRoof.GetFloat("_SceneClay") == 1f && Math.Abs(texRoof.color.r - SectionLayer_IsoRimRoofs.Styles[0].r * 1.12f) < 0.01f,
                "Cached textured roof transition to mode 1 failed");
            Check(shipMat.GetFloat("_SceneClay") == 1f && shipMat.GetFloat("_ClayMode") == 1f && Math.Abs(shipMat.GetFloat("_ClayWrap") - 0.22f) < 1e-5f,
                "Cached ship roof transition to mode 1 failed");

            syncScene.Invoke(null, new object[] { 2, 0.35f, 0.08f, 0.90f });
            Check(roofMat.GetFloat("_SceneClay") == 1f && roofMat.GetFloat("_ClayMode") == 2f && Math.Abs(roofMat.GetFloat("_ClayRoughness") - 0.90f) < 1e-5f,
                "Cached roof transition to mode 2 failed");
            Check(texRoof.GetFloat("_SceneClay") == 1f && texRoof.GetFloat("_ClayMode") == 2f,
                "Cached textured roof transition to mode 2 failed");
            Check(shipMat.GetFloat("_SceneClay") == 1f && shipMat.GetFloat("_ClayMode") == 2f,
                "Cached ship roof transition to mode 2 failed");

            syncScene.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            Check(roofMat.GetFloat("_SceneClay") == 0f && roofMat.GetFloat("_ClayMode") == 0f,
                "Cached roof return to mode 0 failed");
            Check(texRoof.GetFloat("_SceneClay") == 0f && Math.Abs(texRoof.color.r - SectionLayer_IsoRimRoofs.Styles[0].r * 1.25f) < 0.01f,
                "Cached textured roof return to mode 0 failed");
            Check(shipMat.GetFloat("_SceneClay") == 0f && shipMat.GetFloat("_ClayMode") == 0f,
                "Cached ship roof return to mode 0 failed");

            IsoRimSettings prevSettings = IsoRimMod.Settings;
            try
            {
                var testSettings = new IsoRimSettings
                {
                    modelStyle = 2,
                    clayWrap = 0.31f,
                    clayHighlight = 0.08f,
                    clayRoughness = 0.9f
                };
                IsoRimMod.Settings = testSettings;
                View3D.Tick();
                Material wallSolidTick = R3DMaterials.Lit(0, 1);
                Material roofMatTick = R3DMaterials.Roof;
                Material texRoofTick = (Material)texMatMethod.Invoke(null, new object[] { 0, 0 });
                Material shipMatTick = (Material)shipMatProp.GetValue(null);

                Check(wallSolidTick.GetFloat("_SceneClay") == 1f && wallSolidTick.GetFloat("_ClayMode") == 2f
                    && Math.Abs(wallSolidTick.GetFloat("_ClayWrap") - 0.31f) < 1e-5f
                    && Math.Abs(wallSolidTick.GetFloat("_ClayHighlight") - 0.08f) < 1e-5f
                    && Math.Abs(wallSolidTick.GetFloat("_ClayRoughness") - 0.9f) < 1e-5f,
                    "View3D.Tick() did not sync scene clay to wall");
                Check(roofMatTick.GetFloat("_SceneClay") == 1f && roofMatTick.GetFloat("_ClayMode") == 2f
                    && Math.Abs(roofMatTick.GetFloat("_ClayWrap") - 0.31f) < 1e-5f
                    && Math.Abs(roofMatTick.GetFloat("_ClayHighlight") - 0.08f) < 1e-5f
                    && Math.Abs(roofMatTick.GetFloat("_ClayRoughness") - 0.9f) < 1e-5f,
                    "View3D.Tick() did not sync scene clay to roof");
                Check(texRoofTick.GetFloat("_SceneClay") == 1f && texRoofTick.GetFloat("_ClayMode") == 2f
                    && Math.Abs(texRoofTick.GetFloat("_ClayWrap") - 0.31f) < 1e-5f
                    && Math.Abs(texRoofTick.GetFloat("_ClayHighlight") - 0.08f) < 1e-5f
                    && Math.Abs(texRoofTick.GetFloat("_ClayRoughness") - 0.9f) < 1e-5f,
                    "View3D.Tick() did not sync scene clay to textured roof");
                Check(shipMatTick.GetFloat("_SceneClay") == 1f && shipMatTick.GetFloat("_ClayMode") == 2f
                    && Math.Abs(shipMatTick.GetFloat("_ClayWrap") - 0.31f) < 1e-5f
                    && Math.Abs(shipMatTick.GetFloat("_ClayHighlight") - 0.08f) < 1e-5f
                    && Math.Abs(shipMatTick.GetFloat("_ClayRoughness") - 0.9f) < 1e-5f,
                    "View3D.Tick() did not sync scene clay to ship roof");
            }
            finally
            {
                IsoRimMod.Settings = prevSettings;
                syncScene.Invoke(null, new object[] { 0, 0f, 0f, 0f });
                syncModel.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            }

            MethodInfo stateSigMethod = typeof(View3D).GetMethod("StateSignature", BindingFlags.NonPublic | BindingFlags.Static);
            Check(stateSigMethod != null, "View3D.StateSignature method missing");
            IsoRimMod.Settings.modelStyle = 0;
            int sigClassic = (int)stateSigMethod.Invoke(null, null);
            IsoRimMod.Settings.modelStyle = 1;
            int sigClay = (int)stateSigMethod.Invoke(null, null);
            Check(sigClassic != sigClay, "View3D.StateSignature omitted modelStyle bit");

            var section = new Section(new IntVec3(0, 0, 0), Find.CurrentMap);
            Thing wallThing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.Steel);
            GenSpawn.Spawn(wallThing, Find.CurrentMap.Center + new IntVec3(5, 0, 5), Find.CurrentMap);

            IsoRimMod.Settings.modelStyle = 0;
            var wallLayerClassic = new SectionLayer_IsoRimWalls(section);
            WallBoxes.PrintBlock(wallLayerClassic, wallThing, 1f);
            LayerSubMesh capClassicMesh = wallLayerClassic.subMeshes.First(s => s.material.mainTexture != null && s.material.mainTexture.name.Contains("Top"));
            float builtClassicShade = capClassicMesh.material.color.r;
            Check(Math.Abs(builtClassicShade - wallThing.DrawColor.r * 1.12f) < 0.01f, "Built wall top cap did not take 1.12f classic shade: observed " + builtClassicShade);

            IsoRimMod.Settings.modelStyle = 1;
            View3D.Tick();
            var wallLayerClay = new SectionLayer_IsoRimWalls(section);
            WallBoxes.PrintBlock(wallLayerClay, wallThing, 1f);
            LayerSubMesh capClayMesh = wallLayerClay.subMeshes.First(s => s.material.mainTexture != null && s.material.mainTexture.name.Contains("Top"));
            float builtClayShade = capClayMesh.material.color.r;
            Check(Math.Abs(builtClayShade - wallThing.DrawColor.r * 1.04f) < 0.01f, "Built wall top cap did not take 1.04f clay shade after style toggle: observed " + builtClayShade);
            Check(Math.Abs(builtClassicShade - builtClayShade) > 0.03f, "Built wall top cap did not rebuild upon modelStyle toggle: classic=" + builtClassicShade + " clay=" + builtClayShade);

            MethodInfo wallMatFor = typeof(WallBoxes).GetMethod("MaterialFor", BindingFlags.NonPublic | BindingFlags.Static);
            Check(wallMatFor != null, "WallBoxes.MaterialFor method missing");
            IsoRimMod.Settings.modelStyle = 0;
            Material capClassic = (Material)wallMatFor.Invoke(null, new object[] { WallKind.Bricks, true, 0, false, Color.white });
            Check(capClassic.GetFloat("_LightMode") == 2f && capClassic.GetFloat("_GlowMode") == 1f,
                "Wall top cap did not route to LightMode=2 GlowMode=1");
            Check(Math.Abs(capClassic.color.r - 1.12f) < 0.01f, "Classic wall top cap shade is not 1.12f");

            IsoRimMod.Settings.modelStyle = 1;
            Material capClay = (Material)wallMatFor.Invoke(null, new object[] { WallKind.Bricks, true, 0, false, Color.white });
            Check(capClay.GetFloat("_LightMode") == 2f && capClay.GetFloat("_GlowMode") == 1f,
                "Clay wall top cap did not route to LightMode=2 GlowMode=1");
            Check(Math.Abs(capClay.color.r - 1.04f) < 0.01f, "Clay wall top cap shade is not 1.04f");
            Check(capClay != capClassic || Math.Abs(capClay.color.r - capClassic.color.r) > 0.05f,
                "WallBoxes.materials cache did not invalidate on modelStyle toggle");
            IsoRimMod.Settings.modelStyle = 0;

            typeof(ShipRoofs).GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
            Model roofPiece = (Model)typeof(ShipRoofs).GetField("square", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            Check(roofPiece != null && roofPiece.authoredNormals != null && roofPiece.shadingNormals != null, "ShipRoofs model roof_square missing authored normals");
            Check(roofPiece.shadingNormals.Any(n => Math.Abs(n.x) > 0.5f || Math.Abs(n.z) > 0.5f), "Roof model has no horizontal normals");
            float m00 = 0f, m01 = 1f, m10 = -1f, m11 = 0f;
            MethodInfo shipPut = typeof(ShipRoofs).GetMethod("Put", BindingFlags.NonPublic | BindingFlags.Static);
            var roofLayer = new SectionLayer_IsoRimRoofs(section);
            Material shipMatRef = (Material)typeof(ShipRoofs).GetProperty("Mat", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            LayerSubMesh sm = MeshWriter.Sub(roofLayer, shipMatRef);
            shipPut.Invoke(null, new object[] { sm, roofPiece, Vector2.zero, m00, m01, m10, m11, 0f });
            Check(sm.normals.Count == sm.verts.Count && sm.normals.Count == roofPiece.positions.Length, "ShipRoofs.Put normals count differs from vertices");

            StaticNormalCases.Upload(roofLayer);
            LayerSubMesh shipSub = roofLayer.subMeshes.First(s => s.material.name.Contains("ShipRoof"));
            Vector3[] uploadedNormals = shipSub.mesh.normals;
            Check(uploadedNormals.Length == roofPiece.positions.Length, "Uploaded normals count differs from model positions");
            Vector3[] triNormals = {
                new Vector3(0, -1, 0), new Vector3(0, 1, 0), new Vector3(-1, 0, 0),
                new Vector3(0, 0, -1), new Vector3(1, 0, 0), new Vector3(0, 0, 1)
            };
            bool hasNonVertical = false;
            for (int k = 0; k < roofPiece.positions.Length; k++)
            {
                int triIndex = k / 3;
                Vector3 expectedK = triNormals[triIndex % 6];
                if (Math.Abs(expectedK.y) < 0.5f) hasNonVertical = true;
                Check((uploadedNormals[k] - expectedK).sqrMagnitude < 0.0001f,
                    $"Uploaded ship-roof normal at vertex {k} differs from expected rotated direction: observed {uploadedNormals[k]} expected {expectedK}");
            }
            Check(hasNonVertical, "Ship-roof model normals are constant-up");

            IsoRimSettings settingsCopy = IsoRimMod.Settings;
            Thing rockThing = null;
            Thing adjacentWall = null;
            float width100 = 0f, width75 = 0f;
            try
            {
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                Check(Math.Abs(WallBoxes.LineScale - 0.75f) < 1e-5f, "LineScale differs for outlineWidth=0.75");
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                Check(Math.Abs(WallBoxes.LineScale - 1.00f) < 1e-5f, "LineScale differs for outlineWidth=1.00");
                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                Check(WallBoxes.LineScale == 0f, "LineScale not 0 when outlines=false");

                // Builder (a): WallBoxes.PrintBlock (cap rim and foot lines)
                Color32 lineColor = R3DMaterials.Shaded(wallThing.DrawColor, 0.32f);

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f, ruggedRock = false };
                var layer100 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(layer100, wallThing, 1f);
                LayerSubMesh solid100 = layer100.subMeshes.First(s => s.material == R3DMaterials.Solid);
                int rimIdx100 = solid100.colors.FindIndex(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                Check(rimIdx100 >= 0, "Wall rim line vertices missing at outlineWidth=1.00");
                width100 = (solid100.verts[rimIdx100 + 1] - solid100.verts[rimIdx100]).magnitude;
                float footHeight100 = (solid100.verts[rimIdx100 + 5] - solid100.verts[rimIdx100 + 4]).magnitude;

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f, ruggedRock = false };
                var layer75 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(layer75, wallThing, 1f);
                LayerSubMesh solid75 = layer75.subMeshes.First(s => s.material == R3DMaterials.Solid);
                int rimIdx75 = solid75.colors.FindIndex(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                Check(rimIdx75 >= 0, "Wall rim line vertices missing at outlineWidth=0.75");
                width75 = (solid75.verts[rimIdx75 + 1] - solid75.verts[rimIdx75]).magnitude;
                float footHeight75 = (solid75.verts[rimIdx75 + 5] - solid75.verts[rimIdx75 + 4]).magnitude;
                Check(Math.Abs(width75 / width100 - 0.75f) < 0.02f, "Wall rim line did not scale to 0.75");
                Check(Math.Abs(footHeight75 / footHeight100 - 0.75f) < 0.02f, "Wall foot line did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f, ruggedRock = false };
                var layer0 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(layer0, wallThing, 1f);
                LayerSubMesh solid0 = layer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(solid0 == null || !solid0.colors.Any(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b),
                    "Wall outline quads emitted when outlines=false");

                // Builder (b): Rugged rock PrintRock (rim, foot lines, and conditional cutaway tags)
                rockThing = ThingMaker.MakeThing(ThingDefOf.MineableSteel);
                adjacentWall = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.Steel);
                GenSpawn.Spawn(rockThing, Find.CurrentMap.Center + new IntVec3(7, 0, 7), Find.CurrentMap);
                GenSpawn.Spawn(adjacentWall, Find.CurrentMap.Center + new IntVec3(7, 0, 8), Find.CurrentMap);
                CutTags.Enabled = true;

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f, ruggedRock = true };
                var rockLayer100 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(rockLayer100, rockThing, 1f);
                CutTags.Finish(rockLayer100);
                LayerSubMesh rockSolid100 = rockLayer100.subMeshes.First(s => s.material == R3DMaterials.Solid);
                Color32 rockLineCol = R3DMaterials.Shaded(rockThing.DrawColor, 0.32f);
                int rockIdx100 = rockSolid100.colors.FindIndex(c => c.r == rockLineCol.r && c.g == rockLineCol.g && c.b == rockLineCol.b);
                Check(rockIdx100 >= 0, "Rock rim line vertices missing at outlineWidth=1.00");
                float rockRim100 = (rockSolid100.verts[rockIdx100 + 1] - rockSolid100.verts[rockIdx100]).magnitude;
                float rockFoot100 = (rockSolid100.verts[rockIdx100 + 5] - rockSolid100.verts[rockIdx100 + 4]).magnitude;

                // Cutaway tag check on rock face (witness for Defect #2):
                int expectedCond = ShaderCut.Cond(IntVec3.North);
                Check(expectedCond != 0, "ShaderCut.Cond(IntVec3.North) is 0");
                Check(rockSolid100.uvsChannelTwo != null && rockSolid100.uvsChannelTwo.Count > 0, "Rugged rock missing cutaway UV channel");
                Check(rockSolid100.uvsChannelTwo.Any(uv => ((int)uv.z & expectedCond) == expectedCond), "PrintRock backing face or outline omitted conditional cutaway tags (defect #2)");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f, ruggedRock = true };
                var rockLayer75 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(rockLayer75, rockThing, 1f);
                CutTags.Finish(rockLayer75);
                LayerSubMesh rockSolid75 = rockLayer75.subMeshes.First(s => s.material == R3DMaterials.Solid);
                int rockIdx75 = rockSolid75.colors.FindIndex(c => c.r == rockLineCol.r && c.g == rockLineCol.g && c.b == rockLineCol.b);
                Check(rockIdx75 >= 0, "Rock rim line vertices missing at outlineWidth=0.75");
                float rockRim75 = (rockSolid75.verts[rockIdx75 + 1] - rockSolid75.verts[rockIdx75]).magnitude;
                float rockFoot75 = (rockSolid75.verts[rockIdx75 + 5] - rockSolid75.verts[rockIdx75 + 4]).magnitude;
                Check(Math.Abs(rockRim75 / rockRim100 - 0.75f) < 0.02f, "Rock rim line did not scale to 0.75");
                Check(Math.Abs(rockFoot75 / rockFoot100 - 0.75f) < 0.02f, "Rock foot line did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f, ruggedRock = true };
                var rockLayer0 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(rockLayer0, rockThing, 1f);
                CutTags.Finish(rockLayer0);
                LayerSubMesh rockSolid0 = rockLayer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(rockSolid0 == null || !rockSolid0.colors.Any(c => c.r == rockLineCol.r && c.g == rockLineCol.g && c.b == rockLineCol.b),
                    "Rock outline quads emitted when outlines=false");

                CutTags.Enabled = false;

                // Builder (c): Plain wall sides PrintPlainSides
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                var plainLayer100 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintPlainSides(plainLayer100, wallThing, 1f);
                LayerSubMesh plainSolid100 = plainLayer100.subMeshes.First(s => s.material == R3DMaterials.Solid);
                int plainIdx100 = plainSolid100.colors.FindIndex(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                Check(plainIdx100 >= 0, "Plain sides foot line missing at outlineWidth=1.00");
                float plainFoot100 = (plainSolid100.verts[plainIdx100 + 1] - plainSolid100.verts[plainIdx100]).magnitude;

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                var plainLayer75 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintPlainSides(plainLayer75, wallThing, 1f);
                LayerSubMesh plainSolid75 = plainLayer75.subMeshes.First(s => s.material == R3DMaterials.Solid);
                int plainIdx75 = plainSolid75.colors.FindIndex(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                Check(plainIdx75 >= 0, "Plain sides foot line missing at outlineWidth=0.75");
                float plainFoot75 = (plainSolid75.verts[plainIdx75 + 1] - plainSolid75.verts[plainIdx75]).magnitude;
                Check(Math.Abs(plainFoot75 / plainFoot100 - 0.75f) < 0.02f, "Plain sides foot line did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                var plainLayer0 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintPlainSides(plainLayer0, wallThing, 1f);
                LayerSubMesh plainSolid0 = plainLayer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(plainSolid0 == null || !plainSolid0.colors.Any(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b),
                    "Plain sides emitted outlines when outlines=false");
                // Builder (d): Hull prism PrintPrism and RimSquare
                Vector2[] prismPoly = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1) };
                WallBoxes.PrismEdge[] prismEdges = {
                    new WallBoxes.PrismEdge { open = true, joinStart = Vector2.right, joinEnd = Vector2.up, lineStart = true, lineEnd = true },
                    new WallBoxes.PrismEdge { open = true, joinStart = Vector2.up, joinEnd = Vector2.left, lineStart = true, lineEnd = true },
                    new WallBoxes.PrismEdge { open = false }
                };

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                var prismLayer100 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintPrism(prismLayer100, prismPoly, prismEdges, 0f, 1f, WallKind.Metal, Color.white);
                LayerSubMesh prismSolid100 = prismLayer100.subMeshes.First(s => s.material == R3DMaterials.Solid);
                Color32 prismLineColor = R3DMaterials.Shaded(Color.white, 0.32f);
                int prismIdx100 = prismSolid100.colors.FindIndex(c => c.r == prismLineColor.r && c.g == prismLineColor.g && c.b == prismLineColor.b);
                Check(prismIdx100 >= 0, "Prism rim line vertices missing at outlineWidth=1.00");
                float prismRim100 = (prismSolid100.verts[prismIdx100 + 1] - prismSolid100.verts[prismIdx100]).magnitude;

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                var prismLayer75 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintPrism(prismLayer75, prismPoly, prismEdges, 0f, 1f, WallKind.Metal, Color.white);
                LayerSubMesh prismSolid75 = prismLayer75.subMeshes.First(s => s.material == R3DMaterials.Solid);
                int prismIdx75 = prismSolid75.colors.FindIndex(c => c.r == prismLineColor.r && c.g == prismLineColor.g && c.b == prismLineColor.b);
                Check(prismIdx75 >= 0, "Prism rim line vertices missing at outlineWidth=0.75");
                float prismRim75 = (prismSolid75.verts[prismIdx75 + 1] - prismSolid75.verts[prismIdx75]).magnitude;
                Check(Math.Abs(prismRim75 / prismRim100 - 0.75f) < 0.02f, "Prism rim quad did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                var prismLayer0 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintPrism(prismLayer0, prismPoly, prismEdges, 0f, 1f, WallKind.Metal, Color.white);
                LayerSubMesh prismSolid0 = prismLayer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(prismSolid0 == null || !prismSolid0.colors.Any(c => c.r == prismLineColor.r && c.g == prismLineColor.g && c.b == prismLineColor.b),
                    "Prism emitted outlines when outlines=false");

                // Builder (e): RimSquare
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                var rsLayer100 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.RimSquare(rsLayer100, new Vector2(0, 0), new Vector2(1, 1), 0f, 1f, Color.white);
                LayerSubMesh rsSolid100 = rsLayer100.subMeshes.First(s => s.material == R3DMaterials.Solid);
                float rsWidth100 = (rsSolid100.verts[1] - rsSolid100.verts[0]).magnitude;
                Check(Math.Abs(rsWidth100 - 0.05f) < 0.001f, "RimSquare width at 1.00 differs from 0.05");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                var rsLayer75 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.RimSquare(rsLayer75, new Vector2(0, 0), new Vector2(1, 1), 0f, 1f, Color.white);
                LayerSubMesh rsSolid75 = rsLayer75.subMeshes.First(s => s.material == R3DMaterials.Solid);
                float rsWidth75 = (rsSolid75.verts[1] - rsSolid75.verts[0]).magnitude;
                Check(Math.Abs(rsWidth75 / rsWidth100 - 0.75f) < 0.01f, "RimSquare did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                var rsLayer0 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.RimSquare(rsLayer0, new Vector2(0, 0), new Vector2(1, 1), 0f, 1f, Color.white);
                LayerSubMesh rsSolid0 = rsLayer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(rsSolid0 == null || rsSolid0.verts.Count == 0, "RimSquare emitted quads when outlines=false");

                // Builder (f): EndLine UV encoded width
                MethodInfo endLineMethod = typeof(WallBoxes).GetMethod("EndLine", BindingFlags.NonPublic | BindingFlags.Static);
                Check(endLineMethod != null, "WallBoxes.EndLine method missing");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                var elLayer100 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh elSm100 = MeshWriter.Sub(elLayer100, R3DMaterials.Solid);
                endLineMethod.Invoke(null, new object[] { elSm100, 0, Vector3.zero, Vector3.right, Vector3.right, 1f, lineColor, true });
                Vector2 elUv100 = new Vector2(elSm100.uvs[0].x, elSm100.uvs[0].y);
                float elWidth100 = elUv100.magnitude;
                Check(Math.Abs(elWidth100 - 0.05f) < 0.001f, "EndLine UV width at 1.00 differs from 0.05");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                var elLayer75 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh elSm75 = MeshWriter.Sub(elLayer75, R3DMaterials.Solid);
                endLineMethod.Invoke(null, new object[] { elSm75, 0, Vector3.zero, Vector3.right, Vector3.right, 1f, lineColor, true });
                Vector2 elUv75 = new Vector2(elSm75.uvs[0].x, elSm75.uvs[0].y);
                float elWidth75 = elUv75.magnitude;
                Check(Math.Abs(elWidth75 / elWidth100 - 0.75f) < 0.01f, "EndLine UV width did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                var elLayer0 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh elSm0 = MeshWriter.Sub(elLayer0, R3DMaterials.Solid);
                endLineMethod.Invoke(null, new object[] { elSm0, 0, Vector3.zero, Vector3.right, Vector3.right, 1f, lineColor, true });
                Check(elSm0.verts.Count == 0, "EndLine emitted quad when outlines=false");

                // Builder (g): CornerLine UV encoded width
                MethodInfo cornerLineMethod = typeof(WallBoxes).GetMethod("CornerLine", BindingFlags.NonPublic | BindingFlags.Static);
                Check(cornerLineMethod != null, "WallBoxes.CornerLine method missing");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                var clLayer100 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh clSm100 = MeshWriter.Sub(clLayer100, R3DMaterials.Solid);
                cornerLineMethod.Invoke(null, new object[] { clSm100, Vector2.zero, Vector2.right, Vector2.up, Vector2.up, true, 0f, 1f, lineColor });
                Vector2 clUv100 = new Vector2(clSm100.uvs[2].x, clSm100.uvs[2].y);
                float clWidth100 = clUv100.magnitude;
                Check(Math.Abs(clWidth100 - 0.05f) < 0.001f, "CornerLine UV width at 1.00 differs from 0.05");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                var clLayer75 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh clSm75 = MeshWriter.Sub(clLayer75, R3DMaterials.Solid);
                cornerLineMethod.Invoke(null, new object[] { clSm75, Vector2.zero, Vector2.right, Vector2.up, Vector2.up, true, 0f, 1f, lineColor });
                Vector2 clUv75 = new Vector2(clSm75.uvs[2].x, clSm75.uvs[2].y);
                float clWidth75 = clUv75.magnitude;
                Check(Math.Abs(clWidth75 / clWidth100 - 0.75f) < 0.01f, "CornerLine UV width did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                var clLayer0 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh clSm0 = MeshWriter.Sub(clLayer0, R3DMaterials.Solid);
                cornerLineMethod.Invoke(null, new object[] { clSm0, Vector2.zero, Vector2.right, Vector2.up, Vector2.up, true, 0f, 1f, lineColor });
                Check(clSm0.verts.Count == 0, "CornerLine emitted quad when outlines=false");

                // Builder (e): SectionLayer_IsoRimRoofs.Build roof eave lines and roof opening tags
                IntVec3 roofCell = new IntVec3(5, 0, 5);
                Find.CurrentMap.roofGrid.SetRoof(roofCell, RoofDefOf.RoofConstructed);
                MethodInfo roofBuildMethod = typeof(SectionLayer_IsoRimRoofs).GetMethod("Build", BindingFlags.Instance | BindingFlags.NonPublic);

                try
                {
                    CutTags.Enabled = true;

                    IsoRimMod.Settings = new IsoRimSettings { showRoofs = true, outlines = true, outlineWidth = 1.00f };
                    var bldRoof100 = new SectionLayer_IsoRimRoofs(section);
                    roofBuildMethod.Invoke(bldRoof100, null);
                    CutTags.Finish(bldRoof100);
                    LayerSubMesh bldSm100 = bldRoof100.subMeshes.First(s => s.material == R3DMaterials.Roof);
                    Color32 eaveCol = R3DMaterials.Shaded(SectionLayer_IsoRimRoofs.Styles[0], 0.28f);
                    int eaveIdx100 = bldSm100.colors.FindIndex(c => c.r == eaveCol.r && c.g == eaveCol.g && c.b == eaveCol.b);
                    Check(eaveIdx100 >= 0, "Roof eave line quad missing at outlineWidth=1.00");
                    float eaveWidth100 = (bldSm100.verts[eaveIdx100 + 1] - bldSm100.verts[eaveIdx100]).magnitude;

                    // Roof opening tag check:
                    Check(bldSm100.uvsChannelTwo != null && bldSm100.uvsChannelTwo.Any(uv => ((int)uv.z & ShaderCut.Roof) != 0), "Roof opening cutaway tag missing");

                    IsoRimMod.Settings = new IsoRimSettings { showRoofs = true, outlines = true, outlineWidth = 0.75f };
                    var bldRoof75 = new SectionLayer_IsoRimRoofs(section);
                    roofBuildMethod.Invoke(bldRoof75, null);
                    CutTags.Finish(bldRoof75);
                    LayerSubMesh bldSm75 = bldRoof75.subMeshes.First(s => s.material == R3DMaterials.Roof);
                    int eaveIdx75 = bldSm75.colors.FindIndex(c => c.r == eaveCol.r && c.g == eaveCol.g && c.b == eaveCol.b);
                    Check(eaveIdx75 >= 0, "Roof eave line quad missing at outlineWidth=0.75");
                    float eaveWidth75 = (bldSm75.verts[eaveIdx75 + 1] - bldSm75.verts[eaveIdx75]).magnitude;
                    Check(Math.Abs(eaveWidth75 / eaveWidth100 - 0.75f) < 0.02f, "Roof eave line did not scale to 0.75");

                    IsoRimMod.Settings = new IsoRimSettings { showRoofs = true, outlines = false, outlineWidth = 0.75f };
                    var bldRoof0 = new SectionLayer_IsoRimRoofs(section);
                    roofBuildMethod.Invoke(bldRoof0, null);
                    CutTags.Finish(bldRoof0);
                    LayerSubMesh bldSm0 = bldRoof0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Roof);
                    Check(bldSm0 == null || !bldSm0.colors.Any(c => c.r == eaveCol.r && c.g == eaveCol.g && c.b == eaveCol.b),
                        "Roof eave line emitted when outlines=false");

                    CutTags.Enabled = false;
                }
                finally
                {
                    Find.CurrentMap.roofGrid.SetRoof(roofCell, null);
                }
            }
            finally
            {
                IsoRimMod.Settings = settingsCopy;
                if (wallThing != null && wallThing.Spawned) wallThing.Destroy();
                if (rockThing != null && rockThing.Spawned) rockThing.Destroy();
                if (adjacentWall != null && adjacentWall.Spawned) adjacentWall.Destroy();
            }

            var pair = new StaticNormalCases.Pair
            {
                Name = "wall-roof-style",
                Scale = new Vector3(1.3f, .9f, 1.1f),
                Turn = 20,
                N = new Vector3(.989247403381f, .140757555142f, .039710018574f),
                G = new Vector3(.342020143326f, 0, .939692620786f),
                Q = Vector3.forward,
                L = new Vector3(0, .8f, .6f),
                Shades = new[] { 1d, 0.75d, 0.5d }
            };
            SectionLayer bakeLayer = StaticNormalCases.Bake(StaticNormalCases.Fixture(request.fixtureDirectory, false, false),
                Quaternion.AngleAxis(20, Vector3.up), Quaternion.identity, pair.Scale);
            StaticNormalCases.Upload(bakeLayer);
            LayerSubMesh face = StaticNormalCases.Face(bakeLayer, false);

            SectionLayer bakeLayerTex = StaticNormalCases.Bake(StaticNormalCases.Fixture(request.fixtureDirectory, true, false),
                Quaternion.AngleAxis(20, Vector3.up), Quaternion.identity, pair.Scale);
            StaticNormalCases.Upload(bakeLayerTex);
            LayerSubMesh faceTex = StaticNormalCases.Face(bakeLayerTex, true);

            var captures = new List<object>();
            var wallNormClay = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, true, 0, 0, 1,
                "-wall-norm-clay", sceneClay: 1f, lightMode: 2, style: 1);
            captures.Add(wallNormClay);
            // Analytical derivation of reference RGB values for the fixed fixture:
            // Fixture inputs: albedo = 0.6, pair.G = (0.34202014, 0, 0.93969262), L = (0, 0.8, 0.6), V = pair.G, squash = 0.003
            // 1. Wall Classic (mode 2, clayActive false):
            //    BinNormal(G) quantizes az=20 deg to (0, 0, 1). sunFlat = (0, 1). dot = 1.0.
            //    WallShade = mix(0.52, 0.80, 1.0) = 0.80. color = 0.6 * 0.80 = 0.48 -> 122/255.
            // 2. Wall Mode 1 (mode 2, clayActive true, style 1, highlight 0.04):
            //    Continuous n. WallShade = mix(0.52, 0.80, (0.93969 + 1) * 0.5) = 0.791557.
            //    clayShade: wrapped = (dot(n,L) + 0.22) / 1.22 = 0.642472; t = wrapped / top = 0.768447;
            //    t = mix(t, smoothstep(0,1,t), 0.35) = 0.802577; floorShade = mix(0.54, 0.58, 0.44) = 0.5576;
            //    clayShade = mix(0.5576, 1.0, 0.802577) = 0.912660.
            //    diffuse = mix(WallShade, clayShade, 0.65) = 0.870274. color = 0.6 * 0.870274 = 0.522164.
            //    highlight: H = normalize(L + V), ndoth = 0.88439; lobe = 0.60026; litGate = 0.620709;
            //    hlTerm = lobe * litGate * (0.65 * 0.04) * (0.522164 * 1.5) = 0.00758;
            //    hlColor = mix(0.522164, 1.0, 0.35) * hlTerm = 0.00523. Total = 0.527394 -> 134/255.
            // 3. Wall Mode 2 (mode 2, clayActive true, style 2, highlight 0.04):
            //    Oren-Nayar onFactor with roughness = 0.65: A = 0.71873, B = 0.37059, onFactor = 0.71873.
            //    clayShade_on = clamp(clayShade * mix(1.0, 0.71873, 0.65), 0.5576, 1.05) = 0.745778.
            //    diffuse = mix(WallShade, clayShade_on, 0.65) = 0.761801. color = 0.6 * 0.761801 = 0.45708.
            //    plus highlight = 0.004535. Total = 0.461616 -> 118/255.
            // 4. Roof Classic (mode 3, clayActive false):
            //    Quantized normal (0, 0, 1). ModelShade = 0.875. roofShade = min(1.1, 0.875 * 1.05) = 0.91875.
            //    Expected color = 0.6 * 0.91875 = 0.55125 -> 141/255.
            // 5. Roof Mode 1 (mode 3, clayActive true, style 1, highlight 0.04):
            //    clayShade with floor = 0.522: clayShade = 0.905632. diffuse = min(1.06, clayShade * 1.02) = 0.923744.
            //    color = 0.6 * 0.923744 = 0.554247. plus highlight = 0.009003. Total = 0.563250 -> 144/255.
            // 6. Roof Mode 2 (mode 3, clayActive true, style 2, highlight 0.04):
            //    clayShade_on = 0.740061. diffuse = min(1.06, 0.740061 * 1.02) = 0.754862.
            //    color = 0.6 * 0.754862 = 0.452917. plus highlight = 0.006857. Total = 0.459774 -> 117/255.
            // 7. Wall Textured Clay (mode 2, style 1):
            //    Center pixel texture albedo is white (1.0). Evaluates identical Mode 1 diffuse + highlight = 134/255.
            // 8. Wall Cutaway Clay (mode 2, style 1, cutaway true):
            //    Vertex shader lowers geometry to height 2.0. Center pixel remains inside lowered face with shade = 134/255.
            const double RefWallClassic = 122.0 / 255.0;
            const double RefWallMode1 = 134.0 / 255.0;
            const double RefWallMode2 = 118.0 / 255.0;
            const double RefRoofClassic = 141.0 / 255.0;
            const double RefRoofMode1 = 144.0 / 255.0;
            const double RefRoofMode2 = 117.0 / 255.0;
            const double RefWallTextured = 134.0 / 255.0;
            const double RefWallCutaway = 134.0 / 255.0;

            var wallLitClassic = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, 0,
                "-wall-lit-classic", referenceRgb: RefWallClassic, sceneClay: 0f, lightMode: 2, style: 0);
            var wallLitMode1 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, 0,
                "-wall-lit-mode1", referenceRgb: RefWallMode1, sceneClay: 1f, lightMode: 2, style: 1, highlight: 0.04f);
            var wallLitMode2 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, 0,
                "-wall-lit-mode2", referenceRgb: RefWallMode2, sceneClay: 1f, lightMode: 2, style: 2, highlight: 0.04f);
            captures.Add(wallLitClassic);
            captures.Add(wallLitMode1);
            captures.Add(wallLitMode2);

            // Textured wall clay capture compared against independently derived reference
            var wallTexturedClay = StaticNormalCases.Capture(request.outputDirectory, pair, faceTex, true, false, 0, 0, 0,
                "-wall-tex-clay", referenceRgb: RefWallTextured, sceneClay: 1f, lightMode: 2, style: 1, highlight: 0.04f);
            captures.Add(wallTexturedClay);

            // Cutaway wall clay capture compared against independently lowered geometry
            SectionLayer bakeLayerCut = StaticNormalCases.Bake(StaticNormalCases.Fixture(request.fixtureDirectory, false, false),
                Quaternion.AngleAxis(20, Vector3.up), Quaternion.identity, pair.Scale);
            LayerSubMesh cutSub = StaticNormalCases.Face(bakeLayerCut, false);
            cutSub.uvsChannelTwo.Clear();
            cutSub.uvsChannelTwo.AddRange(Enumerable.Repeat(CutTags.Tag(new IntVec3(4, 0, 5), ShaderCut.Lower | ShaderCut.TexSide), 3));
            StaticNormalCases.Upload(bakeLayerCut);
            var wallCutawayClay = StaticNormalCases.Capture(request.outputDirectory, pair, cutSub, false, false, 0, 0, 0,
                "-wall-cutaway-clay", cutaway: true, referenceRgb: RefWallCutaway, sceneClay: 1f, lightMode: 2, style: 1, highlight: 0.04f);
            captures.Add(wallCutawayClay);

            // Compare actual and reference coverage masks (discriminates cut height 0.50 vs 0.47)
            string rawCutPath = Path.Combine(request.outputDirectory, "lighting-wall-roof-style-solid-t0-l0-wall-cutaway-clay.rgba");
            byte[] rawCutBytes = File.ReadAllBytes(rawCutPath);
            int cutMaskDiffs = 0, fgActual = 0, fgReference = 0;
            for (int y = 0; y < 256; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    int p0 = (y * 1024 + x) * 4;
                    int p1 = (y * 1024 + x + 256) * 4;
                    bool actualFg = !(rawCutBytes[p0] == 255 && rawCutBytes[p0 + 1] == 0 && rawCutBytes[p0 + 2] == 255);
                    bool refFg = !(rawCutBytes[p1] == 255 && rawCutBytes[p1 + 1] == 0 && rawCutBytes[p1 + 2] == 255);
                    if (actualFg) fgActual++;
                    if (refFg) fgReference++;
                    if (actualFg != refFg) cutMaskDiffs++;
                }
            }
            Check(fgActual > 1000 && fgReference > 1000, "Cutaway foreground coverage empty");
            Check(cutMaskDiffs <= 4, "Cutaway coverage mask differs from independently lowered reference: mismatched pixels = " + cutMaskDiffs);

            // Roof SkyOnly captures (glowMode = 2)
            var roofLitClassic = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, 0,
                "-roof-lit-classic", referenceRgb: RefRoofClassic, sceneClay: 0f, lightMode: 3, style: 0, glowMode: 2);
            var roofLitMode1 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, 0,
                "-roof-lit-mode1", referenceRgb: RefRoofMode1, sceneClay: 1f, lightMode: 3, style: 1, highlight: 0.04f, glowMode: 2);
            var roofLitMode2 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, 0,
                "-roof-lit-mode2", referenceRgb: RefRoofMode2, sceneClay: 1f, lightMode: 3, style: 2, highlight: 0.04f, glowMode: 2);
            captures.Add(roofLitClassic);
            captures.Add(roofLitMode1);
            captures.Add(roofLitMode2);
            int PatchR(object row)
            {
                using var doc = JsonDocument.Parse(JsonSerializer.Serialize(row));
                return doc.RootElement.GetProperty("patches")[0][0][0].GetInt32();
            }

            int wClassicR = PatchR(wallLitClassic);
            int wMode1R = PatchR(wallLitMode1);
            int wMode2R = PatchR(wallLitMode2);
            Check(Math.Abs(wClassicR - 122) <= 2, "Wall Classic shading differs from reference 122: observed " + wClassicR);
            Check(Math.Abs(wMode1R - 134) <= 2, "Wall Mode 1 wrapped clay shading differs from reference 134: observed " + wMode1R);
            Check(Math.Abs(wMode2R - 118) <= 2, "Wall Mode 2 Oren-Nayar clay shading differs from reference 118: observed " + wMode2R);
            Check(wMode1R != wClassicR && wMode2R != wMode1R, "Wall clay modes do not separate");

            int rClassicR = PatchR(roofLitClassic);
            int rMode1R = PatchR(roofLitMode1);
            int rMode2R = PatchR(roofLitMode2);
            Check(Math.Abs(rClassicR - 141) <= 2, "Roof Classic shading differs from reference 141: observed " + rClassicR);
            Check(Math.Abs(rMode1R - 144) <= 2, "Roof Mode 1 wrapped clay shading differs from reference 144: observed " + rMode1R);
            Check(Math.Abs(rMode2R - 117) <= 2, "Roof Mode 2 Oren-Nayar clay shading differs from reference 117: observed " + rMode2R);
            Check(rMode1R != rClassicR && rMode2R != rMode1R, "Roof clay modes do not separate");

            StaticNormalCases.ClearIntegrationMeshes();

            return new
            {
                layoutVerified = true,
                styleFields = requiredFields,
                syncModelIsolated = true,
                syncSceneVerified = true,
                view3DSyncVerified = true,
                builtWallRegressionVerified = true,
                wallTopCaps = new { classic = capClassic.color.r, clay = capClay.color.r, lightMode = 2, glowMode = 1 },
                shipRoofs = new { normalStreamStamped = true, normalsCount = sm.normals.Count },
                outlineScaling = new { lineScale75 = 0.75, width100, width75, ratio = width75 / width100, zeroOutlineVerified = true },
                captures
            };
        }

        private static object TerrainCardsSlabsAndWater(ProbeNormals.Request request)
        {
            IsoRimSettings prevSettings = IsoRimMod.Settings;
            bool prevView = View3D.Active;
            var section = new Section(new IntVec3(0, 0, 0), Find.CurrentMap);
            try
            {
                // -------------------------------------------------------------
                // 1. Terrain and Built Floors (Ground.Sync & Bounded Texture2D)
                // -------------------------------------------------------------
                TerrainDef soilDef = DefDatabase<TerrainDef>.GetNamed("Soil");
                TerrainDef soilRichDef = DefDatabase<TerrainDef>.GetNamed("SoilRich");
                Check(soilDef != null && soilRichDef != null, "Soil/SoilRich TerrainDefs missing");
                View3D.Active = true;
                Ground.Restore();
                Texture origSoilTex = soilDef.DrawMatSingle.mainTexture;
                Color origSoilCol = soilDef.DrawMatSingle.color;
                Texture origRichTex = soilRichDef.DrawMatSingle.mainTexture;
                Color origRichCol = soilRichDef.DrawMatSingle.color;
                // Sync with modelStyle = 0 (Classic)
                IsoRimMod.Settings = new IsoRimSettings { groundStyle = true, grassSoil = false, modelStyle = 0 };
                Ground.Sync();
                Texture classicSoilTex = soilDef.DrawMatSingle.mainTexture;
                Texture classicRichTex = soilRichDef.DrawMatSingle.mainTexture;
                Check(classicSoilTex != null && classicRichTex != null, "Ground.Sync failed to assign textures at modelStyle=0");

                // Sync with modelStyle = 1 (Clay mode)
                IsoRimMod.Settings = new IsoRimSettings { groundStyle = true, grassSoil = false, modelStyle = 1 };
                Ground.Sync();
                Texture claySoilTex = soilDef.DrawMatSingle.mainTexture;
                Texture clayRichTex = soilRichDef.DrawMatSingle.mainTexture;
                Check(claySoilTex != null && clayRichTex != null, "Ground.Sync failed to assign clay textures at modelStyle=1");
                Check(claySoilTex != classicSoilTex, "Ground.Sync did not update texture when modelStyle changed to 1");

                // Verify softened blend formula on clay texture: x' = Round(mu + 0.75 * (x - mu))
                if (claySoilTex is Texture2D clayTex && classicSoilTex is Texture2D classicTex)
                {
                    Color32[] origPx = classicTex.GetPixels32(0);
                    Color32[] clayPx = clayTex.GetPixels32(0);
                    Check(origPx.Length == clayPx.Length, "Clay texture pixel length differs from classic texture");

                    long sumR = 0, sumG = 0, sumB = 0;
                    for (int i = 0; i < origPx.Length; i++) { sumR += origPx[i].r; sumG += origPx[i].g; sumB += origPx[i].b; }
                    float muR = (float)sumR / origPx.Length;
                    float muG = (float)sumG / origPx.Length;
                    float muB = (float)sumB / origPx.Length;

                    for (int i = 0; i < Math.Min(100, origPx.Length); i++)
                    {
                        byte expectedR = (byte)Mathf.Clamp(Mathf.RoundToInt(muR + 0.75f * ((float)origPx[i].r - muR)), 0, 255);
                        byte expectedG = (byte)Mathf.Clamp(Mathf.RoundToInt(muG + 0.75f * ((float)origPx[i].g - muG)), 0, 255);
                        byte expectedB = (byte)Mathf.Clamp(Mathf.RoundToInt(muB + 0.75f * ((float)origPx[i].b - muB)), 0, 255);
                        Check(Math.Abs(clayPx[i].r - expectedR) <= 1 && Math.Abs(clayPx[i].g - expectedG) <= 1 && Math.Abs(clayPx[i].b - expectedB) <= 1,
                            "Generated clay texture deviates from x' = mu + 0.75*(x - mu) by more than 1/255");
                    }

                    if (clayRichTex is Texture2D clayRich)
                    {
                        Color32[] richPx = clayRich.GetPixels32(0);
                        long sumRichR = 0;
                        for (int i = 0; i < richPx.Length; i++) sumRichR += richPx[i].r;
                        float muRichR = (float)sumRichR / richPx.Length;
                        Check(Math.Abs(muR - muRichR) > 5f, "Soil and SoilRich lost fertility mean color distinction");
                    }
                }

                // Verify Ground.Restore():
                Ground.Restore();
                var origField = typeof(Ground).GetField("originals", BindingFlags.NonPublic | BindingFlags.Static);
                var origDict = (System.Collections.IDictionary)origField.GetValue(null);
                Check(origDict.Contains(soilDef.DrawMatSingle), "Ground.originals missing Soil material entry");
                object savedVal = origDict[soilDef.DrawMatSingle];
                FieldInfo savedTexField = savedVal.GetType().GetField("texture");
                FieldInfo savedColField = savedVal.GetType().GetField("color");
                Texture savedTex = (Texture)savedTexField.GetValue(savedVal);
                Color savedCol = (Color)savedColField.GetValue(savedVal);
                Check(soilDef.DrawMatSingle.mainTexture == savedTex && soilDef.DrawMatSingle.color == savedCol,
                    "Ground.Restore() failed to restore exact Saved.texture and Saved.color on Soil");
                Check(soilDef.DrawMatSingle.mainTexture == origSoilTex && soilDef.DrawMatSingle.color == origSoilCol,
                    "Ground.Restore() failed to restore original Soil texture and color");
                Check(soilRichDef.DrawMatSingle.mainTexture == origRichTex && soilRichDef.DrawMatSingle.color == origRichCol,
                    "Ground.Restore() failed to restore original SoilRich texture and color");

                // -------------------------------------------------------------
                // -------------------------------------------------------------
                // 2. Upright Plant Cards (UprightCards.Move & Shading Contract)
                // -------------------------------------------------------------
                var cardLayer = new SectionLayer_IsoRimWalls(section);
                Thing plantThing = ThingMaker.MakeThing(ThingDefOf.Plant_TreeOak);
                Material plantMat = plantThing.Graphic.MatSingleFor(plantThing);
                Check(plantMat != null, "Plant material missing");
                Material cardMat = UprightCards.For(plantMat);
                Check(cardMat != null, "UprightCards.For returned null");

                LayerSubMesh tempSm = MeshWriter.Sub(cardLayer, R3DMaterials.Solid);
                Vector3 pivot = new Vector3(10f, 0f, 10f);
                float sizeX = 0.8f, sizeY = 1.2f;
                Vector3 p0 = new Vector3(pivot.x - sizeX * 0.5f, 0f, pivot.z);
                Vector3 p1 = new Vector3(pivot.x - sizeX * 0.5f, 0f, pivot.z + sizeY);
                Vector3 p2 = new Vector3(pivot.x + sizeX * 0.5f, 0f, pivot.z + sizeY);
                Vector3 p3 = new Vector3(pivot.x + sizeX * 0.5f, 0f, pivot.z);
                MeshWriter.TexturedQuad(tempSm, p0, p1, p2, p3, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), Color.white);

                bool moved = UprightCards.Move(cardLayer, tempSm, plantMat, pivot, 0f);
                Check(moved, "UprightCards.Move failed on flat plant card");
                LayerSubMesh cardSub = cardLayer.subMeshes.First(s => s.material == cardMat);
                Check(cardSub.verts.Count == 4 && cardSub.uvsChannelTwo.Count == 4, "Card submesh vertex or UV2 count differs from 4");

                // Check hinge root vs crown tip offsets in uvsChannelTwo:
                Check(Math.Abs(cardSub.uvsChannelTwo[0].z) < 1e-4f, "Card vertex 0 offset d.z is not at hinge root (0)");
                Check(Math.Abs(cardSub.uvsChannelTwo[1].z - sizeY) < 1e-4f, "Card vertex 1 offset d.z is not at crown tip");
                Check(Math.Abs(cardSub.uvsChannelTwo[0].y) < 1e-4f && Math.Abs(cardSub.uvsChannelTwo[1].y) < 1e-4f, "Flat quad inUv1 d.y is non-zero");

                // Finalize mesh on cardLayer for GPU rendering:
                CutTags.Finish(cardLayer);
                NormalStreams.Finish(cardLayer);
                typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(cardLayer, new object[] { MeshParts.All });
                cardSub.mesh.SetUVs(1, cardSub.uvsChannelTwo);
                cardSub.mesh.triangles = new[] { 0, 1, 2, 0, 2, 3, 0, 2, 1, 0, 3, 2 };
                cardSub.mesh.bounds = new Bounds(pivot, new Vector3(10f, 10f, 10f));

                // Render cardSub directly on the GPU through IsoRimCardPlant:
                var cardCamOwner = new GameObject("IsoRim plant card proof");
                Camera cardCam = cardCamOwner.AddComponent<Camera>();
                cardCam.enabled = false; cardCam.orthographic = true; cardCam.aspect = 1f; cardCam.cullingMask = -1;
                cardCam.clearFlags = CameraClearFlags.SolidColor; cardCam.backgroundColor = Color.magenta;
                RenderTexture cardTarget = RenderTexture.GetTemporary(128, 128, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                cardCam.targetTexture = cardTarget;
                cardCam.transform.position = new Vector3(pivot.x, 0.6f, pivot.z - 5f);
                cardCam.transform.LookAt(new Vector3(pivot.x, 0.6f, pivot.z));
                cardCam.orthographicSize = 1.0f;
                UprightCards.SetCamera(0f, 0f);
                Vector4 prevIsoView = Shader.GetGlobalVector("_IsoView");
                Shader.SetGlobalVector("_IsoView", new Vector4(0f, 0f, 1f, 1f));

                int origCardQueue = cardMat.renderQueue;
                cardMat.renderQueue = 2000;
                Texture origCardTex = cardMat.mainTexture;
                Texture2D cardTex = new Texture2D(32, 32, TextureFormat.RGBA32, false);
                Color32 plantGreen = new Color32(100, 160, 60, 255);
                Color32[] cardPx = new Color32[32 * 32];
                for (int i = 0; i < cardPx.Length; i++) cardPx[i] = plantGreen;
                for (int y = 16; y < 32; y++)
                    for (int x = 0; x < 14; x++)
                        cardPx[y * 32 + x] = new Color32(100, 160, 60, 0); // u in [0, 0.44], v in [0.5, 1.0] alpha 0 < 0.35 -> cutout!
                cardTex.SetPixels32(cardPx);
                cardTex.Apply();
                cardMat.mainTexture = cardTex;
                Color Pixel(byte[] raw, int px, int py)
                {
                    int idx = (py * 128 + px) * 4;
                    return new Color(raw[idx] / 255f, raw[idx + 1] / 255f, raw[idx + 2] / 255f, raw[idx + 3] / 255f);
                }
                float Lum(Color c) => c.r * 0.2126f + c.g * 0.7152f + c.b * 0.0722f;
                byte[] RenderCard(Vector4 isoClayCard, Vector4 isoSun, Color vertexColor, float sway)
                {
                    cardMat.SetVector("_IsoCard", new Vector4(-1f, 0f, 0f, 1f));
                    cardMat.SetVector("_IsoView", new Vector4(0f, 0f, 1f, 1f));
                    cardMat.SetVector("_IsoClayCard", isoClayCard);
                    cardMat.SetVector("_IsoSun", isoSun);
                    cardMat.SetFloat("_SwayHead", sway);
                    Color32 c32 = (Color32)vertexColor;
                    cardSub.mesh.colors32 = new[] { c32, c32, c32, c32 };
                    cardSub.mesh.normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                    cardSub.mesh.SetUVs(1, cardSub.uvsChannelTwo);
                    cardSub.mesh.triangles = new[] { 0, 1, 2, 0, 2, 3, 0, 2, 1, 0, 3, 2 };
                    cardSub.mesh.bounds = new Bounds(new Vector3(pivot.x, 0.6f, pivot.z), new Vector3(5f, 5f, 5f));
                    Camera.CameraCallback draw = active =>
                    {
                        if (active != cardCam) return;
                        cardMat.SetPass(0);
                        Graphics.DrawMeshNow(cardSub.mesh, Matrix4x4.identity);
                    };
                    Camera.onPreRender += draw;
                    cardCam.Render();
                    Camera.onPreRender -= draw;
                    var backend = UnityStubs.Rendering.CameraTextureRenderer.Current;
                    byte[] raw = null;
                    Check(backend != null && backend.TryReadPixels(cardTarget, out raw) && raw != null, "GPU backend TryReadPixels failed for plant card");
                    int nonMag = 0;
                    for (int i = 0; i < raw.Length; i += 4)
                        if (!(raw[i] == 255 && raw[i + 1] == 0 && raw[i + 2] == 255)) nonMag++;
                    Check(nonMag > 0, "DrawMeshNow produced 0 non-magenta pixels for plant card");
                    return raw;
                }

                try
                {
                    byte[] clayFrame = RenderCard(new Vector4(1f, 0.22f, 0f, 0f), new Vector4(0f, 0.8f, 0.6f, 0f), Color.white, 0f);

                    // Grounding: root (py=98) vs crown tip (py=28) in Clay mode
                    float tipLum = Lum(Pixel(clayFrame, 64, 28));
                    float rootLum = Lum(Pixel(clayFrame, 64, 98));
                    Check(tipLum > rootLum, $"Crown tip rendered pixel ({tipLum:F3}) is not brighter than hinge root ({rootLum:F3})");
                    float gRatio = tipLum / rootLum;
                    Check(gRatio >= 1.05f && gRatio <= 1.25f, "Grounding ratio differs from 1.02 / 0.86 = 1.186: observed " + gRatio);

                    // Two sun directions: high sun vs low opposing sun (sampled at center 64, 64)
                    byte[] highSunFrame = RenderCard(new Vector4(1f, 0.22f, 0f, 0f), new Vector4(0f, 0.9f, 0.435f, 0f), Color.white, 0f);
                    byte[] lowSunFrame = RenderCard(new Vector4(1f, 0.22f, 0f, 0f), new Vector4(0f, 0.2f, -0.98f, 0f), Color.white, 0f);
                    float highSunLum = Lum(Pixel(highSunFrame, 64, 64));
                    float lowSunLum = Lum(Pixel(lowSunFrame, 64, 64));
                    Check(highSunLum > lowSunLum + 0.03f, "Varying _IsoSun direction failed to change rendered plant card brightness");

                    // Two wrap values under oblique sun (sampled at center 64, 64)
                    Vector4 obliqueSun = new Vector4(0.5f, 0.5f, -0.7f, 0f);
                    byte[] wrap1Frame = RenderCard(new Vector4(1f, 0.10f, 0f, 0f), obliqueSun, Color.white, 0f);
                    byte[] wrap2Frame = RenderCard(new Vector4(1f, 0.50f, 0f, 0f), obliqueSun, Color.white, 0f);
                    float wrap1Lum = Lum(Pixel(wrap1Frame, 64, 64));
                    float wrap2Lum = Lum(Pixel(wrap2Frame, 64, 64));
                    Check(Math.Abs(wrap1Lum - wrap2Lum) > 0.01f, "Varying clayWrap failed to change rendered plant card brightness");

                    // Classic comparison with non-white card (sampled at center 64, 64):
                    Color nonWhite = new Color(0.7f, 0.6f, 0.8f);
                    byte[] classicWhiteFrame = RenderCard(new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0.8f, 0.6f, 0f), Color.white, 0f);
                    byte[] classicNonWhiteFrame = RenderCard(new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0.8f, 0.6f, 0f), nonWhite, 0f);
                    float classicWhiteLum = Lum(Pixel(classicWhiteFrame, 64, 64));
                    float classicNonWhiteLum = Lum(Pixel(classicNonWhiteFrame, 64, 64));
                    Check(Math.Abs(classicNonWhiteLum - classicWhiteLum) < 0.01f, "Classic plant card output changed for non-white card");
                    byte[] clayNonWhiteFrame = RenderCard(new Vector4(1f, 0.22f, 0f, 0f), new Vector4(0f, 0.8f, 0.6f, 0f), nonWhite, 0f);
                    float clayWhiteLum = Lum(Pixel(clayFrame, 64, 64));
                    float clayNonWhiteLum = Lum(Pixel(clayNonWhiteFrame, 64, 64));
                    Check(clayNonWhiteLum < clayWhiteLum - 0.03f, "Clay plant card failed to multiply vertex color");

                    // Cutout observation at interior row py=48 (y_rel=0.29 from top in Vulkan readback, v=0.70):
                    // px=51 is u=0.25 (inside transparent patch) -> reveals background magenta:
                    Color cutoutPx = Pixel(clayFrame, 51, 48);
                    Color opaquePx = Pixel(clayFrame, 77, 48);
                    Check(opaquePx.g > cutoutPx.g + 0.20f, $"Cutout patch failed to reveal background terrain: opaque green {opaquePx.g:F3} vs cutout {cutoutPx.g:F3}");

                    // Wind sway observation at right edge px=90 (world X=10.4):
                    byte[] swayedFrame = RenderCard(new Vector4(1f, 0.22f, 0f, 0f), new Vector4(0f, 0.8f, 0.6f, 0f), Color.white, 25f);
                    Color stillPx = Pixel(clayFrame, 90, 64);
                    Color swayedPx = Pixel(swayedFrame, 90, 64);
                    Check(stillPx != swayedPx, "Wind sway failed to shift plant card position");
                }
                finally
                {
                    Shader.SetGlobalVector("_IsoView", prevIsoView);
                    cardMat.renderQueue = origCardQueue;
                    cardMat.mainTexture = origCardTex;
                    UnityEngine.Object.DestroyImmediate(cardTex);
                    RenderTexture.ReleaseTemporary(cardTarget);
                    UnityEngine.Object.DestroyImmediate(cardCamOwner);
                }

                // -------------------------------------------------------------
                // 3. Sprite Slabs (SpriteSolids & Extruder)
                // -------------------------------------------------------------
                Thing weaponThing = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Gun_Revolver"));
                GenSpawn.Spawn(weaponThing, Find.CurrentMap.Center + new IntVec3(3, 0, 3), Find.CurrentMap);

                // Build SpriteSolids and verify dual sides / claySides:
                MethodInfo forMethod = typeof(SpriteSolids).GetMethod("For", BindingFlags.NonPublic | BindingFlags.Static);
                object solidObj = forMethod.Invoke(null, new object[] { weaponThing.Graphic.MatSingleFor(weaponThing) });
                FieldInfo sidesField = solidObj.GetType().GetField("sides");
                FieldInfo claySidesField = solidObj.GetType().GetField("claySides");
                Mesh classicSidesMesh = (Mesh)sidesField.GetValue(solidObj);
                Mesh claySidesMesh = (Mesh)claySidesField.GetValue(solidObj);
                Check(classicSidesMesh != null && claySidesMesh != null, "SpriteSolids missing sides or claySides mesh");

                Color32[] classicColors = classicSidesMesh.colors32;
                Color32[] clayColors = claySidesMesh.colors32;
                Check(classicColors.Length == clayColors.Length && classicColors.Length > 0, "SpriteSolids sides vertex count differs");

                // Extract mask and find lit upward-facing edge run for contour comparison:
                Extruder.Mask mask = Extruder.MaskOf(weaponThing.Graphic.MatSingleFor(weaponThing).mainTexture, Vector2.one, Vector2.zero);
                Check(mask != null, "MaskOf returned null for weapon texture");
                int edgeVertCount = mask.runs.Count * 4;
                float cMin = classicColors.Take(edgeVertCount).Min(c => (float)c.r / 255f);
                float cMax = classicColors.Take(edgeVertCount).Max(c => (float)c.r / 255f);
                Check(cMax - cMin > 0.20f, "Classic edge vertices do not contain directional contrast");
                int upEdgeIdx = -1;
                for (int i = 0; i < mask.runs.Count; i++)
                {
                    if (mask.runs[i].dir == 1 && classicColors[i * 4].r > 20)
                    {
                        upEdgeIdx = i * 4;
                        break;
                    }
                }
                Check(upEdgeIdx >= 0, "No lit upward-facing edge run found in weapon solid");
                // Observe production submissions across modelStyle 0, 1, 0:
                var commandsClassic = new List<MeshDrawCommand>();
                using (MeshDrawSubmission.BeginCapture(commandsClassic))
                {
                    IsoRimMod.Settings.modelStyle = 0;
                    SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 0f);
                    SpriteSolids.DrawUpright(weaponThing, new Vector3(10f, 1f, 10f), 0f, false, 1f);
                    SpriteSolids.DrawLying(weaponThing, new Vector3(10f, 0f, 10f), 0.1f, 1f);
                }
                var classicSideCmds = commandsClassic.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_Solid").ToList();
                Check(classicSideCmds.Count == 3, "Classic slab did not submit 3 side draws with unlit Solid");
                Check(classicSideCmds.All(c => ReferenceEquals(c.Mesh, classicSidesMesh.EngineMesh)), "Classic side draw did not submit classicSidesMesh EngineMesh");
                Check(!classicSideCmds.Any(c => ReferenceEquals(c.Mesh, claySidesMesh.EngineMesh)), "Classic side draw incorrectly submitted claySidesMesh EngineMesh");

                var commandsClay = new List<MeshDrawCommand>();
                using (MeshDrawSubmission.BeginCapture(commandsClay))
                {
                    IsoRimMod.Settings.modelStyle = 1;
                    SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 0f);
                    SpriteSolids.DrawUpright(weaponThing, new Vector3(10f, 1f, 10f), 0f, false, 1f);
                    SpriteSolids.DrawLying(weaponThing, new Vector3(10f, 0f, 10f), 0.1f, 1f);
                }
                var claySideCmds = commandsClay.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_LitModel").ToList();
                Check(claySideCmds.Count == 3, "Clay slab did not submit 3 side draws with LitModel");
                Check(claySideCmds.All(c => ReferenceEquals(c.Mesh, claySidesMesh.EngineMesh)), "Clay side draw did not submit claySidesMesh EngineMesh");
                Check(!claySideCmds.Any(c => ReferenceEquals(c.Mesh, classicSidesMesh.EngineMesh)), "Clay side draw incorrectly submitted classicSidesMesh EngineMesh");

                // Verify 0.92 edge and 0.86 fill on the submitted clay sides mesh:
                Color32[] submittedClayColors = claySidesMesh.colors32;
                Color32[] submittedClassicColors = classicSidesMesh.colors32;
                float observedEdgeRatio = (float)submittedClayColors[upEdgeIdx].r / submittedClassicColors[upEdgeIdx].r;
                Check(Math.Abs(observedEdgeRatio - 0.92f) < 0.03f, "Clay contour edge shading on submitted mesh does not pin 0.92f: observed " + observedEdgeRatio);
                int fillIdx = edgeVertCount;
                float observedFillRatio = (float)submittedClayColors[fillIdx].r / submittedClassicColors[fillIdx].r;
                Check(Math.Abs(observedFillRatio - (0.86f / 0.70f)) < 0.03f, "Clay interior fill shading on submitted mesh does not pin 0.86f (vs classic 0.70f): observed " + observedFillRatio);

                var commandsRestore = new List<MeshDrawCommand>();
                using (MeshDrawSubmission.BeginCapture(commandsRestore))
                {
                    IsoRimMod.Settings.modelStyle = 0;
                    SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 0f);
                    SpriteSolids.DrawUpright(weaponThing, new Vector3(10f, 1f, 10f), 0f, false, 1f);
                    SpriteSolids.DrawLying(weaponThing, new Vector3(10f, 0f, 10f), 0.1f, 1f);
                }
                var restoreSideCmds = commandsRestore.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_Solid").ToList();
                Check(restoreSideCmds.Count == 3, "Restored slab did not submit 3 side draws with unlit Solid");
                Check(restoreSideCmds.All(c => ReferenceEquals(c.Mesh, classicSidesMesh.EngineMesh)), "Restored side draw did not submit classicSidesMesh EngineMesh");
                Check(!restoreSideCmds.Any(c => ReferenceEquals(c.Mesh, claySidesMesh.EngineMesh)), "Restored side draw incorrectly submitted claySidesMesh EngineMesh");

                // Compare two held rotations under fixed _IsoSun in clay mode:
                var slabProofOwner = new GameObject("IsoRim slab rotation proof");
                Camera slabCamera = slabProofOwner.AddComponent<Camera>();
                slabCamera.enabled = false; slabCamera.orthographic = true; slabCamera.aspect = 1f; slabCamera.cullingMask = -1;
                slabCamera.clearFlags = CameraClearFlags.SolidColor; slabCamera.backgroundColor = Color.magenta;
                RenderTexture slabTarget = RenderTexture.GetTemporary(128, 128, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                slabCamera.targetTexture = slabTarget;
                Vector3 slabCenter = new Vector3(10f, 1f / View3D.Squash, 10f);
                Vector3 slabCamDir = new Vector3(1f, 1f, -1.5f).normalized;
                slabCamera.transform.position = slabCenter - slabCamDir * 5f;
                slabCamera.transform.LookAt(slabCenter);
                slabCamera.orthographicSize = 1.0f;
                Vector4 prevLitSun = R3DMaterials.LitModel.GetVector("_IsoSun");
                Vector4 prevLitView = R3DMaterials.LitModel.GetVector("_IsoView");
                R3DMaterials.LitModel.SetVector("_IsoSun", new Vector4(0f, 0.8f, 0.6f, 0f));
                R3DMaterials.LitModel.SetVector("_IsoView", new Vector4(slabCamDir.x, slabCamDir.y, slabCamDir.z, View3D.Squash));
                Shader.SetGlobalVector("_IsoSun", new Vector4(0f, 0.8f, 0.6f, 0f));
                Shader.SetGlobalVector("_IsoView", new Vector4(slabCamDir.x, slabCamDir.y, slabCamDir.z, View3D.Squash));

                float AverageLum(byte[] raw, out int sidePixels)
                {
                    sidePixels = 0;
                    float lumSum = 0f;
                    for (int i = 0; i < raw.Length; i += 4)
                    {
                        if (!(raw[i] == 255 && raw[i + 1] == 0 && raw[i + 2] == 255))
                        {
                            sidePixels++;
                            lumSum += raw[i] * 0.2126f + raw[i + 1] * 0.7152f + raw[i + 2] * 0.0722f;
                        }
                    }
                    return sidePixels > 0 ? (lumSum / sidePixels / 255f) : 0f;
                }

                // Clone production side geometry with uniform vertex colours to isolate lighting from contour/fill colour differences:
                Mesh uniformSideMesh = UnityEngine.Object.Instantiate(claySidesMesh);
                Color32[] uniformColors = new Color32[uniformSideMesh.vertexCount];
                for (int i = 0; i < uniformColors.Length; i++) uniformColors[i] = new Color32(200, 200, 200, 255);
                uniformSideMesh.colors32 = uniformColors;

                var sunHook = new Harmony("isorim.slab.sun");
                var recordMethod = typeof(Waylith.Rendering.MeshBatchRenderer).GetMethod("RecordBatch");
                sunHook.Patch(recordMethod, prefix: new HarmonyMethod(typeof(SceneStyleCases).GetMethod(nameof(SunPrefix), BindingFlags.Static | BindingFlags.NonPublic)));
                try
                {
                    // Render uniform clone with production LitModel material across two orientations:
                    claySidesField.SetValue(solidObj, uniformSideMesh);
                    IsoRimMod.Settings.modelStyle = 1;
                    R3DMaterials.SyncModelStyle(1, 0.22f, 0.04f, 0.65f);
                    var aim0Commands = new List<MeshDrawCommand>();
                    using (MeshDrawSubmission.BeginCapture(aim0Commands))
                    {
                        IsoRimMod.Settings.modelStyle = 1;
                        SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 0f);
                    }
                    var aim0SideOnly = aim0Commands.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_LitModel").ToList();
                    Check(aim0SideOnly.Count > 0, "No side draw submitted at aim 0");
                    byte[] rawAim0 = MovingNormalCases.RenderProofScene(slabTarget, slabCamera, aim0SideOnly, out _, out _);

                    var aim90Commands = new List<MeshDrawCommand>();
                    using (MeshDrawSubmission.BeginCapture(aim90Commands))
                    {
                        IsoRimMod.Settings.modelStyle = 1;
                        SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 90f);
                    }
                    var aim90SideOnly = aim90Commands.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_LitModel").ToList();
                    Check(aim90SideOnly.Count > 0, "No side draw submitted at aim 90");
                    byte[] rawAim90 = MovingNormalCases.RenderProofScene(slabTarget, slabCamera, aim90SideOnly, out _, out _);

                    float avgLum0 = AverageLum(rawAim0, out int sidePixels0);
                    float avgLum90 = AverageLum(rawAim90, out int sidePixels90);
                    Check(sidePixels0 >= 20 && sidePixels90 >= 20, $"Held slab side pixel coverage below floor (20): aim0={sidePixels0}, aim90={sidePixels90}");
                    float delta = Math.Abs(avgLum0 - avgLum90);
                    Check(delta > 0.03f, $"Held slab side shading delta ({delta:F3}) on uniform mesh under fixed sun is not > 0.03: aim0={avgLum0:F3}, aim90={avgLum90:F3}");

                    // Meaningful control: same uniform geometry with lighting neutralised (unlit Solid, modelStyle 0) must NOT show delta:
                    sidesField.SetValue(solidObj, uniformSideMesh);
                    IsoRimMod.Settings.modelStyle = 0;
                    R3DMaterials.SyncModelStyle(0, 0.22f, 0.04f, 0.65f);

                    var aim0UnlitCommands = new List<MeshDrawCommand>();
                    using (MeshDrawSubmission.BeginCapture(aim0UnlitCommands))
                    {
                        SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 0f);
                    }
                    var aim0UnlitSideOnly = aim0UnlitCommands.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_Solid").ToList();
                    Check(aim0UnlitSideOnly.Count > 0, "No side draw submitted for unlit control at aim 0");
                    byte[] rawAim0Unlit = MovingNormalCases.RenderProofScene(slabTarget, slabCamera, aim0UnlitSideOnly, out _, out _);

                    var aim90UnlitCommands = new List<MeshDrawCommand>();
                    using (MeshDrawSubmission.BeginCapture(aim90UnlitCommands))
                    {
                        SpriteSolids.DrawHeld(weaponThing, new Vector3(10f, 1f, 10f), 90f);
                    }
                    var aim90UnlitSideOnly = aim90UnlitCommands.Where(c => c.Material != null && c.Material.DebugName == "IsoRim_Solid").ToList();
                    Check(aim90UnlitSideOnly.Count > 0, "No side draw submitted for unlit control at aim 90");
                    byte[] rawAim90Unlit = MovingNormalCases.RenderProofScene(slabTarget, slabCamera, aim90UnlitSideOnly, out _, out _);

                    float avgUnlit0 = AverageLum(rawAim0Unlit, out int unlitPixels0);
                    float avgUnlit90 = AverageLum(rawAim90Unlit, out int unlitPixels90);
                    Check(unlitPixels0 >= 20 && unlitPixels90 >= 20, $"Unlit control side coverage below floor (20): aim0={unlitPixels0}, aim90={unlitPixels90}");
                    float unlitDelta = Math.Abs(avgUnlit0 - avgUnlit90);
                    Check(unlitDelta < 0.01f, $"Neutralised lighting control showed spurious delta ({unlitDelta:F3}): aim0={avgUnlit0:F3}, aim90={avgUnlit90:F3}");
                }
                finally
                {
                    claySidesField.SetValue(solidObj, claySidesMesh);
                    sidesField.SetValue(solidObj, classicSidesMesh);
                    IsoRimMod.Settings.modelStyle = 1;
                    R3DMaterials.SyncModelStyle(1, 0.22f, 0.04f, 0.65f);
                    UnityEngine.Object.DestroyImmediate(uniformSideMesh);
                    RenderTexture.ReleaseTemporary(slabTarget);
                    UnityEngine.Object.DestroyImmediate(slabProofOwner);
                    sunHook.Unpatch(recordMethod, HarmonyPatchType.Prefix, sunHook.Id);
                    R3DMaterials.LitModel.SetVector("_IsoSun", prevLitSun);
                    R3DMaterials.LitModel.SetVector("_IsoView", prevLitView);
                }

                // Extruder.Extrude outline scaling and BottomShade:
                Vector3 ev0 = new Vector3(10f, 0f, 10f);
                Vector3 ev1 = new Vector3(10f, 0f, 11f);
                Vector3 ev2 = new Vector3(11f, 0f, 11f);
                Vector3 ev3 = new Vector3(11f, 0f, 10f);
                Material extMat = weaponThing.Graphic.MatSingleFor(weaponThing);

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f, modelStyle = 1 };
                var extLayer100 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh extSm100 = MeshWriter.Sub(extLayer100, extMat);
                MeshWriter.TexturedQuad(extSm100, ev0, ev1, ev2, ev3, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), Color.white);
                bool ext100 = Extruder.Extrude(extLayer100, extSm100, 4, extMat, 0f, 1f);
                Check(ext100, "Extruder.Extrude failed at outlineWidth=1.00");
                LayerSubMesh extSolid100 = extLayer100.subMeshes.First(s => s.material == R3DMaterials.Solid);
                float squash = View3D.Squash;
                float expectedLine100 = 0.03f * 1.00f / squash;
                float expectedLine75 = 0.03f * 0.75f / squash;
                bool hasLine100 = extSolid100.verts.Any(v => Math.Abs(v.y * squash - 0.03f * 1.00f) < 1e-3f);
                Check(hasLine100, "Extruder ground line quad missing at outlineWidth=1.00");

                // Extruder bottom shade check in clay mode (0.86):
                float bottomShadeObs100 = 0f;
                foreach (var sub in extLayer100.subMeshes)
                {
                    if (sub == extSm100 || sub.material == R3DMaterials.Solid) continue;
                    for (int i = 0; i < sub.colors.Count; i += 4)
                    {
                        int topVal = Math.Max(sub.colors[i + 1].r, Math.Max(sub.colors[i + 1].g, sub.colors[i + 1].b));
                        int botVal = Math.Max(sub.colors[i].r, Math.Max(sub.colors[i].g, sub.colors[i].b));
                        if (topVal > 20 && botVal > 0)
                        {
                            bottomShadeObs100 = (float)botVal / topVal;
                            break;
                        }
                    }
                    if (bottomShadeObs100 > 0f) break;
                }
                Check(bottomShadeObs100 > 0f && Math.Abs(bottomShadeObs100 - 0.86f) < 0.03f, "Extruder bottom shade is not 0.86 in clay mode: observed " + bottomShadeObs100);
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f, modelStyle = 1 };
                var extLayer75 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh extSm75 = MeshWriter.Sub(extLayer75, extMat);
                MeshWriter.TexturedQuad(extSm75, ev0, ev1, ev2, ev3, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), Color.white);
                Extruder.Extrude(extLayer75, extSm75, 4, extMat, 0f, 1f);
                LayerSubMesh extSolid75 = extLayer75.subMeshes.First(s => s.material == R3DMaterials.Solid);
                bool hasLine75 = extSolid75.verts.Any(v => Math.Abs(v.y * squash - 0.03f * 0.75f) < 1e-3f);
                Check(hasLine75, "Extruder ground line quad missing at outlineWidth=0.75");
                float observedH100 = extSolid100.verts.Where(v => v.y > 0.001f).Min(v => v.y) * squash;
                float observedH75 = extSolid75.verts.Where(v => v.y > 0.001f).Min(v => v.y) * squash;
                Check(Math.Abs(observedH75 / observedH100 - 0.75f) < 0.02f, "Extruder ground line height did not scale to 0.75");

                // Extruder bottom shade check in classic mode (0.78):
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f, modelStyle = 0 };
                var extLayerClassic = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh extSmClassic = MeshWriter.Sub(extLayerClassic, extMat);
                MeshWriter.TexturedQuad(extSmClassic, ev0, ev1, ev2, ev3, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), Color.white);
                Extruder.Extrude(extLayerClassic, extSmClassic, 4, extMat, 0f, 1f);
                float bottomShadeObsClassic = 0f;
                foreach (var sub in extLayerClassic.subMeshes)
                {
                    if (sub == extSmClassic || sub.material == R3DMaterials.Solid) continue;
                    for (int i = 0; i < sub.colors.Count; i += 4)
                    {
                        int topVal = Math.Max(sub.colors[i + 1].r, Math.Max(sub.colors[i + 1].g, sub.colors[i + 1].b));
                        int botVal = Math.Max(sub.colors[i].r, Math.Max(sub.colors[i].g, sub.colors[i].b));
                        if (topVal > 20 && botVal > 0)
                        {
                            bottomShadeObsClassic = (float)botVal / topVal;
                            break;
                        }
                    }
                    if (bottomShadeObsClassic > 0f) break;
                }
                Check(bottomShadeObsClassic > 0f && Math.Abs(bottomShadeObsClassic - 0.78f) < 0.03f, "Extruder bottom shade is not 0.78 in classic mode: observed " + bottomShadeObsClassic);
                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f, modelStyle = 1 };
                var extLayer0 = new SectionLayer_IsoRimWalls(section);
                LayerSubMesh extSm0 = MeshWriter.Sub(extLayer0, extMat);
                MeshWriter.TexturedQuad(extSm0, ev0, ev1, ev2, ev3, new Vector2(0f, 0f), new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(1f, 0f), Color.white);
                Extruder.Extrude(extLayer0, extSm0, 4, extMat, 0f, 1f);
                LayerSubMesh extSolid0 = extLayer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                bool hasLine0 = extSolid0 != null && extSolid0.verts.Any(v => Math.Abs(v.y * squash - 0.03f * 1.00f) < 1e-3f || Math.Abs(v.y * squash - 0.03f * 0.75f) < 1e-3f);
                Check(!hasLine0, "Extruder emitted ground line quads when outlines=false");
                Check(extSolid0 == null || extSolid0.verts.Count < extSolid100.verts.Count, "Extruder vertex count did not decrease when outlines=false");

                // -------------------------------------------------------------
                // 4. Stylised Water (SectionLayer_IsoRimWater & WaterOverlay)
                // -------------------------------------------------------------
                Material waterMat = R3DMaterials.WaterOverlay;
                Check(waterMat != null && waterMat.GetFloat("_LightMode") == 4f, "R3DMaterials.WaterOverlay missing LightMode 4");

                // Shore-to-deep fixture with concave inner corner at shoreCenter:
                Section waterSection = Find.CurrentMap.mapDrawer.SectionAt(Find.CurrentMap.Center + new IntVec3(15, 0, 15));
                IntVec3 shoreCenter = waterSection.botLeft + new IntVec3(8, 0, 8);
                var waterCells = new List<IntVec3>();
                for (int dz = -5; dz <= 5; dz++)
                {
                    for (int dx = -5; dx <= 5; dx++)
                    {
                        IntVec3 c = shoreCenter + new IntVec3(dx, 0, dz);
                        if (c.InBounds(Find.CurrentMap))
                        {
                            if (dx == -5 && dz == -5) continue; // concave inner corner on land
                            bool shallow = dx == -5 || dz == -5 || dx == 5 || dz == 5;
                            TerrainDef wDef = (dx == 1 && dz == 1) ? DefDatabase<TerrainDef>.GetNamed("WaterMovingShallow") : (shallow ? TerrainDefOf.WaterShallow : TerrainDefOf.WaterDeep);
                            Find.CurrentMap.terrainGrid.SetTerrain(c, wDef);
                            waterCells.Add(c);
                        }
                    }
                }
                FieldInfo ripplesField = typeof(SectionLayer_IsoRimWater).GetField("ripples", BindingFlags.NonPublic | BindingFlags.Static);
                FieldInfo flowField = typeof(SectionLayer_IsoRimWater).GetField("flow", BindingFlags.NonPublic | BindingFlags.Static);

                try
                {
                    // Water in Clay mode (modelStyle = 1):
                    IsoRimMod.Settings = new IsoRimSettings { waterStyle = true, effects3D = true, modelStyle = 1 };
                    var waterLayerClay = new SectionLayer_IsoRimWater(waterSection);
                    waterLayerClay.Regenerate();
                    LayerSubMesh waterSmClay = waterLayerClay.subMeshes.First(s => s.material == R3DMaterials.WaterOverlay);
                    Check(waterSmClay.colors.Count > 0, "Water submesh has no vertices in clay mode");

                    // Verify shallow (210), intermediate (strictly between 210 and 245), deep (245):
                    Check(waterSmClay.colors.Any(c => c.a == 210), "Water submesh missing shallow shoreline alpha 210 in clay mode");
                    Check(waterSmClay.colors.Any(c => c.a > 210 && c.a < 245), "Water submesh missing intermediate feathered alpha strictly between 210 and 245 in clay mode");
                    Check(waterSmClay.colors.Any(c => c.a == 245), "Water submesh missing deep water alpha 245 in clay mode");
                    // Check known ordinary shallow-shore vertex (d <= 1, non-corner CornerColour vertex) independent of RoundInnerCorners tip:
                    int ordinaryShoreIdx = waterSmClay.verts.FindIndex(v => Math.Abs(v.x - shoreCenter.x) < 1e-3f && Math.Abs(v.z - (shoreCenter.z + 6)) < 1e-3f);
                    Check(ordinaryShoreIdx >= 0, "Ordinary shallow shoreline vertex (d <= 1) missing in clay water mesh");
                    Check(waterSmClay.colors[ordinaryShoreIdx].a == 210, $"Ordinary shallow shoreline vertex does not have alpha 210: observed {waterSmClay.colors[ordinaryShoreIdx].a}");

                    // Check RoundInnerCorners tip alpha 210 at the concave land corner:
                    IntVec3 landCorner = shoreCenter + new IntVec3(-5, 0, -5);
                    int kx = landCorner.x + 1;
                    int kz = landCorner.z + 1;
                    float reach = 0.34f + 0.12f * Hashing.Unit(kx, kz, 7);
                    int tipIdx = waterSmClay.verts.FindIndex(v => Math.Abs(v.x - (kx - reach)) < 1e-3f && Math.Abs(v.z - kz) < 1e-3f);
                    Check(tipIdx >= 0, "RoundInnerCorners tip vertex missing in clay water mesh");
                    Check(waterSmClay.colors[tipIdx].a == 210, "RoundInnerCorners tip vertex does not have alpha 210 in clay mode: observed " + waterSmClay.colors[tipIdx].a);
                    Check(ordinaryShoreIdx != tipIdx, "Ordinary shoreline vertex coincided with RoundInnerCorners tip");

                    // Observe wave opacity and scrolling in clay mode:
                    waterLayerClay.DrawLayer();
                    Material rClay = (Material)ripplesField.GetValue(null);
                    Material fClay = (Material)flowField.GetValue(null);
                    Check(rClay != null && Math.Abs(rClay.color.a - 0.68f) < 0.03f, "Ripples material opacity is not 0.68 in clay mode");
                    Check(fClay != null && Math.Abs(fClay.color.a - 0.68f) < 0.03f, "Flow material opacity is not 0.68 in clay mode");
                    Vector2 shift1 = rClay.mainTextureOffset;
                    Vector2 flow1 = fClay.mainTextureOffset;
                    Find.TickManager.DebugSetTicksGame(Find.TickManager.TicksGame + 120);
                    waterLayerClay.DrawLayer();
                    Vector2 shift2 = rClay.mainTextureOffset;
                    Vector2 flow2 = fClay.mainTextureOffset;
                    Check(shift1 != shift2, "Ripples texture offset did not scroll with game ticks");
                    Check(flow1 != flow2, "Flow texture offset did not scroll with game ticks");

                    // Water in Classic mode (modelStyle = 0):
                    IsoRimMod.Settings = new IsoRimSettings { waterStyle = true, effects3D = true, modelStyle = 0 };
                    var waterLayerClassic = new SectionLayer_IsoRimWater(waterSection);
                    waterLayerClassic.Regenerate();
                    LayerSubMesh waterSmClassic = waterLayerClassic.subMeshes.First(s => s.material == R3DMaterials.WaterOverlay);
                    Check(waterSmClassic.colors.All(c => c.a == 255), "Water submesh wrote non-255 alpha in classic mode");

                    // Observe wave opacity in classic mode:
                    waterLayerClassic.DrawLayer();
                    Check(Math.Abs(rClay.color.a - 1.0f) < 0.03f, "Ripples material opacity is not 1.0 in classic mode");
                    Check(Math.Abs(fClay.color.a - 1.0f) < 0.03f, "Flow material opacity is not 1.0 in classic mode");
                }
                finally
                {
                    foreach (IntVec3 c in waterCells)
                    {
                        Find.CurrentMap.terrainGrid.SetTerrain(c, TerrainDefOf.Soil);
                    }
                }

                if (weaponThing.Spawned) weaponThing.Destroy();
            }
            finally
            {
                View3D.Active = prevView;
                IsoRimMod.Settings = prevSettings;
            }

            // -------------------------------------------------------------
            // 5. GPU Framebuffer Captures for S02 Surfaces
            // -------------------------------------------------------------
            var slabPair = new StaticNormalCases.Pair
            {
                Name = "terrain-cards-water-slab",
                Scale = new Vector3(1.3f, 0.9f, 1.1f),
                Turn = 20,
                N = new Vector3(0.989247403381f, 0.140757555142f, 0.039710018574f),
                G = new Vector3(0.342020143326f, 0f, 0.939692620786f),
                Q = Vector3.forward,
                L = new Vector3(0f, 0.8f, 0.6f),
                Shades = new[] { 1d, 0.75d, 0.5d }
            };
            var waterPair = new StaticNormalCases.Pair
            {
                Name = "terrain-cards-water-sheen",
                Scale = new Vector3(1.3f, 0.9f, 1.1f),
                Turn = 20,
                N = new Vector3(0.989247403381f, 0.140757555142f, 0.039710018574f),
                G = new Vector3(0f, 0.7071068f, -0.7071068f),
                Q = Vector3.forward,
                L = new Vector3(0f, 0.7071068f, 0.7071068f),
                Shades = new[] { 1d, 0.75d, 0.5d }
            };
            SectionLayer bakeLayer = StaticNormalCases.Bake(StaticNormalCases.Fixture(request.fixtureDirectory, false, false),
                Quaternion.AngleAxis(20, Vector3.up), Quaternion.identity, slabPair.Scale);
            StaticNormalCases.Upload(bakeLayer);
            LayerSubMesh face = StaticNormalCases.Face(bakeLayer, false);

            var captures = new List<object>();

            // Water sheen capture:
            // L = (0, 0.707, 0.707), V = (0, 0.707, -0.707) -> H = (0, 1, 0), H.y = 1.0 -> spec 0.35 * 0.50 * 1.0 = 0.175, diff mix(0.94, 1.03, 0.707) = 1.0036 -> shade 1.1786 -> albedo 0.6 * 1.1786 = 0.7072 -> 180/255
            const double RefWaterSheenClay = 180.0 / 255.0;
            // Classic unlit water: shade 1.0 -> albedo 0.6 * 1.0 = 0.60 -> 153/255 (separates from 180 by 27 levels)
            const double RefWaterClassic = 153.0 / 255.0;
            // Slab in clay mode: LightMode 1 under sun -> albedo 0.6 * clayShade 0.9267 = 0.556 -> 141/255
            const double RefSlabClay = 141.0 / 255.0;
            // Slab in classic mode: production unlit LightMode 0 -> albedo 0.6 * 1.0 = 0.60 -> 153/255 (separates from 141 by 12 levels)
            const double RefSlabClassic = 153.0 / 255.0;

            var waterSheenClay = StaticNormalCases.Capture(request.outputDirectory, waterPair, face, false, false, 0, 0, 0,
                "-water-sheen-clay", referenceRgb: RefWaterSheenClay, sceneClay: 1f, lightMode: 4, style: 1, highlight: 0.50f, alignedHighlight: false);
            captures.Add(waterSheenClay);

            var waterSheenClassic = StaticNormalCases.Capture(request.outputDirectory, waterPair, face, false, false, 0, 0, 0,
                "-water-sheen-classic", referenceRgb: RefWaterClassic, sceneClay: 0f, lightMode: 4, style: 0, highlight: 0f, alignedHighlight: false);
            captures.Add(waterSheenClassic);

            var slabClay = StaticNormalCases.Capture(request.outputDirectory, slabPair, face, false, false, 0, 0, 0,
                "-slab-clay", referenceRgb: RefSlabClay, sceneClay: 1f, lightMode: 1, style: 1, highlight: 0.04f);
            captures.Add(slabClay);

            var slabClassic = StaticNormalCases.Capture(request.outputDirectory, slabPair, face, false, false, 0, 0, 0,
                "-slab-classic", referenceRgb: RefSlabClassic, sceneClay: 0f, lightMode: 0, style: 0);
            captures.Add(slabClassic);

            StaticNormalCases.ClearIntegrationMeshes();

            StaticNormalCases.ClearIntegrationMeshes();

            return new
            {
                groundSyncVerified = true,
                soilFertilityContrast = true,
                groundRestoreVerified = true,
                plantCardGrounding = new { hingeRoot = 0.86, crownTip = 1.02, ratio = 1.02 / 0.86 },
                plantCardSunAndWrap = true,
                spriteSolidsClayLitModel = true,
                spriteSolidsClassicUnlit = true,
                extruderOutlineScaling = new { line100 = 0.03f, line75 = 0.0225f, ratio = 0.75 },
                waterAlphaFeathering = new { shallowAlpha = 210, deepAlpha = 245, classicAlpha = 255 },
                waterSheenAndWaveOpacity = true,
                captures
            };
        }

        private static object ColonySceneComparisonAndPerf(ProbeNormals.Request request)
        {
            IsoRimSettings previous = IsoRimMod.Settings;
            bool prevView = View3D.Active;
            bool cull = Cull.Enabled, batches = PartBatches.Enabled;
            var owner = new GameObject("IsoRim colony style comparison");
            Camera camera = owner.AddComponent<Camera>();
            camera.enabled = false; camera.orthographic = true; camera.orthographicSize = 11; camera.aspect = 1; camera.cullingMask = 0;
            camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = Color.magenta;
            const int size = 512;
            RenderTexture target = RenderTexture.GetTemporary(size, size, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            RenderTexture active = RenderTexture.active; camera.targetTexture = target;
            var backend = UnityStubs.Rendering.CameraTextureRenderer.Current;
            Check(backend?.GetType().FullName == "RimWorldWaylith.Rendering.CameraTextureBaker", "Comparison GPU backend unavailable");
            FieldInfo serial = backend.GetType().GetField("_frameNumber", BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo immediate = typeof(Graphics).GetField("ImmediateCapture", ModelStyleCases.Hidden);
            Check(immediate != null, "Camera immediate submission seam unavailable");
            var frames = new List<object>(); var timings = new List<object>();
            var images = new Dictionary<string, byte[]>();
            var sampleMaps = new Dictionary<string, List<ModelStyleCases.ComparisonSample>>();
            var layers = new List<SectionLayer>();
            var restore = new List<Action>();
            Camera.CameraCallback submit = null;

            Vector3 center = new Vector3(Find.CurrentMap.Center.x + .5f, 0, Find.CurrentMap.Center.z + .5f);
            Pawn colonist = PawnGenerator.GeneratePawn(PawnKindDefOf.Colonist, Faction.OfPlayer);
            if (colonist.apparel.WornApparel.Count == 0)
                colonist.apparel.Wear((Apparel)ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Apparel_BasicShirt"), ThingDefOf.Cloth));
            Pawn muffalo = PawnGenerator.GeneratePawn(DefDatabase<PawnKindDef>.GetNamed("Muffalo"));
            Thing revolver = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Gun_Revolver"));
            Thing wallThing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.Steel);
            Thing rockThing = ThingMaker.MakeThing(ThingDefOf.MineableSteel);
            Vector3 atWall = center + new Vector3(-2f, 0f, -5f);
            IntVec3 wallCell = new IntVec3((int)atWall.x, 0, (int)atWall.z);
            GenSpawn.Spawn(wallThing, wallCell, Find.CurrentMap);
            GenSpawn.Spawn(rockThing, wallCell + IntVec3.North, Find.CurrentMap);
            Thing armchair = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Armchair"), ThingDefOf.Cloth);
            armchair.TryGetComp<CompColorable>()?.SetColor(new Color(.35f, .65f, .8f));
            Thing lamp = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("StandingLamp"), ThingDefOf.Steel);
            Thing vat = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("GrowthVat"));
            Thing oakTree = ThingMaker.MakeThing(ThingDefOf.Plant_TreeOak);
            TerrainDef soilDef = DefDatabase<TerrainDef>.GetNamed("Soil");
            TerrainDef richDef = DefDatabase<TerrainDef>.GetNamed("SoilRich");
            TerrainDef woodDef = DefDatabase<TerrainDef>.GetNamed("WoodPlankFloor");

            ModelEntry armEntry, lampEntry, vatEntry;
            ModelLibrary.TryGet(armchair.def, out armEntry);
            ModelLibrary.TryGet(lamp.def, out lampEntry);
            ModelLibrary.TryGet(vat.def, out vatEntry);

            try
            {
                IsoRimMod.Settings = new IsoRimSettings { squash = .003f, texturedModels = true, workAnimations = false };
                View3D.Active = true;
                Cull.Enabled = false; PartBatches.Flush(); PartBatches.Enabled = false;

                for (int preset = 0; preset < ModelStyleCases.ComparisonPresets.Length; preset++)
                {
                    IsoRimSettings st = IsoRimMod.Settings;
                    st.modelStyle = preset == 0 ? 0 : preset == 2 ? 2 : 1;
                    st.outlines = preset != 3;
                    st.outlineWidth = preset == 0 ? 1f : .75f;
                    st.clayWrap = .22f;
                    st.clayHighlight = .04f;
                    st.clayRoughness = .65f;
                    st.groundStyle = preset > 0;
                    st.floorStyle = preset > 0;
                    st.waterStyle = preset > 0;
                    Ground.Sync();
                    DynamicModels.Forget(); People.ForgetAll();
                    R3DMaterials.SyncModelStyle(st.modelStyle, st.clayWrap, st.clayHighlight, st.clayRoughness);
                    R3DMaterials.SyncSceneStyle(st.modelStyle, st.clayWrap, st.clayHighlight, st.clayRoughness);

                    var samples = new List<ModelStyleCases.ComparisonSample>();
                    Mesh cardSubMesh = null;
                    Material cardSubMat = null;

                    void Bake(Thing thing, Model model, ModelEntry entry, Vector3 atPos)
                    {
                        var layer = new SectionLayer_IsoRimBodies(Find.CurrentMap.mapDrawer.SectionAt(new IntVec3((int)atPos.x, 0, (int)atPos.z)));
                        layers.Add(layer);
                        Vector3 scale = entry != null ? entry.Fit(thing.def) * entry.def.scale : Vector3.one;
                        Check((bool)typeof(ModelBaker).GetMethod("Write", ModelStyleCases.Hidden).Invoke(null, new object[] { layer, thing, model, atPos,
                            Quaternion.identity, Quaternion.identity, Vector3.zero, scale, entry?.def.offset ?? Vector3.zero, 0f,
                            thing.DrawColor, false, entry?.def.outline ?? 1f }), "Comparison static bake refused " + thing.def.defName);
                        CutTags.Finish(layer); NormalStreams.Finish(layer);
                        typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(layer, new object[] { MeshParts.All });
                        foreach (LayerSubMesh sub in layer.subMeshes.Where(s => s.mesh.vertexCount > 0))
                        {
                            sub.mesh.bounds = new Bounds(atPos, new Vector3(2f, 2f, 2f));
                            Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                        }
                    }

                    // 1. TerrainAndFloor
                    var sTerrain = new ModelStyleCases.ComparisonSample { Name = "TerrainAndFloor" }; samples.Add(sTerrain);
                    Vector3 atTerrain = center + new Vector3(-6f, 0f, -5f);
                    using (MeshDrawSubmission.BeginCapture(sTerrain.Commands))
                    {
                        Mesh tMesh = new Mesh();
                        tMesh.vertices = new[] {
                            new Vector3(atTerrain.x - 1.2f, 0, atTerrain.z - 0.6f), new Vector3(atTerrain.x - 1.2f, 0, atTerrain.z + 0.6f),
                            new Vector3(atTerrain.x - 0.4f, 0, atTerrain.z + 0.6f), new Vector3(atTerrain.x - 0.4f, 0, atTerrain.z - 0.6f),
                            new Vector3(atTerrain.x - 0.4f, 0, atTerrain.z - 0.6f), new Vector3(atTerrain.x - 0.4f, 0, atTerrain.z + 0.6f),
                            new Vector3(atTerrain.x + 0.4f, 0, atTerrain.z + 0.6f), new Vector3(atTerrain.x + 0.4f, 0, atTerrain.z - 0.6f),
                            new Vector3(atTerrain.x + 0.4f, 0, atTerrain.z - 0.6f), new Vector3(atTerrain.x + 0.4f, 0, atTerrain.z + 0.6f),
                            new Vector3(atTerrain.x + 1.2f, 0, atTerrain.z + 0.6f), new Vector3(atTerrain.x + 1.2f, 0, atTerrain.z - 0.6f),
                        };
                        tMesh.uv = new[] {
                            Vector2.zero, Vector2.up, Vector2.one, Vector2.right,
                            Vector2.zero, Vector2.up, Vector2.one, Vector2.right,
                            Vector2.zero, Vector2.up, Vector2.one, Vector2.right,
                        };
                        tMesh.subMeshCount = 3;
                        tMesh.SetTriangles(new[] { 0, 1, 2, 0, 2, 3 }, 0);
                        tMesh.SetTriangles(new[] { 4, 5, 6, 4, 6, 7 }, 1);
                        tMesh.SetTriangles(new[] { 8, 9, 10, 8, 10, 11 }, 2);
                        tMesh.RecalculateBounds();
                        Graphics.DrawMesh(tMesh, Matrix4x4.identity, soilDef.DrawMatSingle, 0, null, 0);
                        Graphics.DrawMesh(tMesh, Matrix4x4.identity, richDef.DrawMatSingle, 0, null, 1);
                        Graphics.DrawMesh(tMesh, Matrix4x4.identity, woodDef.DrawMatSingle, 0, null, 2);
                    }

                    // 2. WallAndRock
                    var sWall = new ModelStyleCases.ComparisonSample { Name = "WallAndRock" }; samples.Add(sWall);
                    using (MeshDrawSubmission.BeginCapture(sWall.Commands))
                    {
                        var wallLayer = new SectionLayer_IsoRimWalls(Find.CurrentMap.mapDrawer.SectionAt(wallCell));
                        layers.Add(wallLayer);
                        WallBoxes.PrintBlock(wallLayer, wallThing, 1f);
                        WallBoxes.PrintBlock(wallLayer, rockThing, 1f);
                        CutTags.Finish(wallLayer); NormalStreams.Finish(wallLayer);
                        typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(wallLayer, new object[] { MeshParts.All });
                        foreach (LayerSubMesh sub in wallLayer.subMeshes.Where(s => s.mesh.vertexCount > 0))
                        {
                            sub.mesh.bounds = new Bounds(atWall, new Vector3(2f, 2f, 2f));
                            Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                        }
                    }

                    // 3. BuiltRoof
                    var sRoof = new ModelStyleCases.ComparisonSample { Name = "BuiltRoof" }; samples.Add(sRoof);
                    Vector3 atRoof = center + new Vector3(2f, 0f, -5f);
                    using (MeshDrawSubmission.BeginCapture(sRoof.Commands))
                    {
                        for (int rz = -2; rz <= 1; rz++)
                            for (int rx = -1; rx <= 2; rx++)
                                Find.CurrentMap.roofGrid.SetRoof(new IntVec3((int)atRoof.x + rx, 0, (int)atRoof.z + rz), RoofDefOf.RoofConstructed);
                        var roofLayer = new SectionLayer_IsoRimRoofs(Find.CurrentMap.mapDrawer.SectionAt(new IntVec3((int)atRoof.x, 0, (int)atRoof.z)));
                        layers.Add(roofLayer);
                        MethodInfo roofBuild = typeof(SectionLayer_IsoRimRoofs).GetMethod("Build", BindingFlags.Instance | BindingFlags.NonPublic);
                        roofBuild.Invoke(roofLayer, null);
                        CutTags.Finish(roofLayer); NormalStreams.Finish(roofLayer);
                        typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(roofLayer, new object[] { MeshParts.All });
                        foreach (LayerSubMesh sub in roofLayer.subMeshes.Where(s => s.mesh.vertexCount > 0))
                        {
                            sub.mesh.bounds = new Bounds(new Vector3(atRoof.x + 0.5f, 1f, atRoof.z), new Vector3(3.5f, 3f, 3.5f));
                            Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                        }
                    }

                    // 4. PlantCard
                    var sCard = new ModelStyleCases.ComparisonSample { Name = "PlantCard" }; samples.Add(sCard);
                    Vector3 atCard = center + new Vector3(6f, 0f, -5f);
                    using (MeshDrawSubmission.BeginCapture(sCard.Commands))
                    {
                        Material plantMat = oakTree.Graphic.MatSingleFor(oakTree);
                        Material cardMat = UprightCards.For(plantMat);
                        if (cardMat != null)
                        {
                            cardMat.renderQueue = 2000;
                            if (cardMat.mainTexture == null || !(cardMat.mainTexture is Texture2D t2d && t2d.width > 0))
                            {
                                Texture2D plantTex = new Texture2D(32, 32, TextureFormat.RGBA32, false);
                                Color32 plantGreen = new Color32(100, 160, 60, 255);
                                Color32[] plantPx = new Color32[32 * 32];
                                for (int i = 0; i < plantPx.Length; i++) plantPx[i] = plantGreen;
                                plantTex.SetPixels32(plantPx);
                                plantTex.Apply();
                                cardMat.mainTexture = plantTex;
                            }
                        }
                        var cardLayer = new SectionLayer_IsoRimWalls(Find.CurrentMap.mapDrawer.SectionAt(new IntVec3((int)atCard.x, 0, (int)atCard.z)));
                        layers.Add(cardLayer);
                        LayerSubMesh tempSm = MeshWriter.Sub(cardLayer, R3DMaterials.Solid);
                        float sx = 0.8f, sy = 1.2f;
                        Vector3 p0 = new Vector3(atCard.x - sx * 0.5f, 0f, atCard.z);
                        Vector3 p1 = new Vector3(atCard.x - sx * 0.5f, 0f, atCard.z + sy);
                        Vector3 p2 = new Vector3(atCard.x + sx * 0.5f, 0f, atCard.z + sy);
                        Vector3 p3 = new Vector3(atCard.x + sx * 0.5f, 0f, atCard.z);
                        MeshWriter.TexturedQuad(tempSm, p0, p1, p2, p3, Vector2.zero, Vector2.up, Vector2.one, Vector2.right, Color.white);
                        UprightCards.Move(cardLayer, tempSm, plantMat, atCard, 0f);
                        CutTags.Finish(cardLayer); NormalStreams.Finish(cardLayer);
                        typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(cardLayer, new object[] { MeshParts.All });
                        foreach (LayerSubMesh sub in cardLayer.subMeshes.Where(s => s.mesh.vertexCount > 0))
                        {
                            sub.mesh.SetUVs(1, sub.uvsChannelTwo);
                            sub.mesh.normals = new[] { Vector3.back, Vector3.back, Vector3.back, Vector3.back };
                            sub.mesh.colors32 = new[] { (Color32)Color.white, (Color32)Color.white, (Color32)Color.white, (Color32)Color.white };
                            sub.mesh.triangles = new[] { 0, 1, 2, 0, 2, 3, 0, 2, 1, 0, 3, 2 };
                            sub.mesh.bounds = new Bounds(new Vector3(atCard.x, 0.6f, atCard.z), new Vector3(1.5f, 2f, 1.5f));
                            Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                            cardSubMesh = sub.mesh;
                            cardSubMat = sub.material;
                        }
                    }

                    // 5. SpriteSlabs
                    var sSlab = new ModelStyleCases.ComparisonSample { Name = "SpriteSlabs" }; samples.Add(sSlab);
                    Vector3 atSlab = center + new Vector3(-6f, 0f, 0f);
                    using (MeshDrawSubmission.BeginCapture(sSlab.Commands))
                    {
                        SpriteSolids.DrawHeld(revolver, atSlab + new Vector3(0f, 1.2f, 0f), 45f);
                        var slabLayer = new SectionLayer_IsoRimWalls(Find.CurrentMap.mapDrawer.SectionAt(new IntVec3((int)atSlab.x, 0, (int)atSlab.z)));
                        layers.Add(slabLayer);

                        // 1. Extruded revolver slab (1.6 x 1.6 units):
                        LayerSubMesh extSm = MeshWriter.Sub(slabLayer, revolver.Graphic.MatSingleFor(revolver));
                        Vector3 ev0 = atSlab + new Vector3(0.1f, 0f, -0.8f);
                        Vector3 ev1 = atSlab + new Vector3(0.1f, 0f, 0.8f);
                        Vector3 ev2 = atSlab + new Vector3(1.7f, 0f, 0.8f);
                        Vector3 ev3 = atSlab + new Vector3(1.7f, 0f, -0.8f);
                        MeshWriter.TexturedQuad(extSm, ev0, ev1, ev2, ev3, Vector2.zero, Vector2.up, Vector2.one, Vector2.right, Color.white);
                        Extruder.Extrude(slabLayer, extSm, 4, revolver.Graphic.MatSingleFor(revolver), 0f, 0.6f);

                        // 2. Extruded rifle slab (1.6 x 1.6 units):
                        Thing rifle = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("Gun_BoltActionRifle"));
                        Material rifleMat = rifle.Graphic.MatSingleFor(rifle);
                        LayerSubMesh extSm2 = MeshWriter.Sub(slabLayer, rifleMat);
                        Vector3 rv0 = atSlab + new Vector3(-1.7f, 0f, -0.8f);
                        Vector3 rv1 = atSlab + new Vector3(-1.7f, 0f, 0.8f);
                        Vector3 rv2 = atSlab + new Vector3(-0.1f, 0f, 0.8f);
                        Vector3 rv3 = atSlab + new Vector3(-0.1f, 0f, -0.8f);
                        MeshWriter.TexturedQuad(extSm2, rv0, rv1, rv2, rv3, Vector2.zero, Vector2.up, Vector2.one, Vector2.right, Color.white);
                        Extruder.Extrude(slabLayer, extSm2, 4, rifleMat, 0f, 0.6f);

                        CutTags.Finish(slabLayer); NormalStreams.Finish(slabLayer);
                        typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(slabLayer, new object[] { MeshParts.All });
                        foreach (LayerSubMesh sub in slabLayer.subMeshes.Where(s => s.mesh.vertexCount > 0))
                        {
                            sub.mesh.bounds = new Bounds(atSlab, new Vector3(4f, 3f, 3f));
                            Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                        }
                    }

                    // 6. Water
                    var sWater = new ModelStyleCases.ComparisonSample { Name = "Water" }; samples.Add(sWater);
                    Vector3 atWater = center + new Vector3(-2f, 0f, 0f);
                    using (MeshDrawSubmission.BeginCapture(sWater.Commands))
                    {
                        IntVec3 wc0 = new IntVec3((int)atWater.x, 0, (int)atWater.z);
                        IntVec3 wc1 = new IntVec3((int)atWater.x + 1, 0, (int)atWater.z);
                        Find.CurrentMap.terrainGrid.SetTerrain(wc0, TerrainDefOf.WaterShallow);
                        Find.CurrentMap.terrainGrid.SetTerrain(wc1, TerrainDefOf.WaterDeep);
                        var waterLayer = new SectionLayer_IsoRimWater(Find.CurrentMap.mapDrawer.SectionAt(wc0));
                        layers.Add(waterLayer);
                        MethodInfo waterBuild = typeof(SectionLayer_IsoRimWater).GetMethod("Build", BindingFlags.Instance | BindingFlags.NonPublic);
                        waterBuild.Invoke(waterLayer, null);
                        CutTags.Finish(waterLayer);
                        NormalStreams.Finish(waterLayer);
                        typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(waterLayer, new object[] { MeshParts.All });
                        foreach (LayerSubMesh sub in waterLayer.subMeshes.Where(s => s.mesh.vertexCount > 0))
                        {
                            sub.mesh.bounds = new Bounds(atWater, new Vector3(2f, 1f, 2f));
                            Graphics.DrawMesh(sub.mesh, Matrix4x4.identity, sub.material, 0);
                        }
                        waterLayer.DrawLayer();
                    }

                    // 7. Armchair
                    var sArm = new ModelStyleCases.ComparisonSample { Name = "Armchair" }; samples.Add(sArm);
                    Vector3 atArm = center + new Vector3(2f, 0f, 0f);
                    armchair.Position = new IntVec3((int)atArm.x, 0, (int)atArm.z);
                    using (MeshDrawSubmission.BeginCapture(sArm.Commands))
                    {
                        Bake(armchair, armEntry.Pick(armchair), armEntry, atArm);
                    }

                    // 8. Colonist
                    var sCol = new ModelStyleCases.ComparisonSample { Name = "Colonist" }; samples.Add(sCol);
                    Vector3 atCol = center + new Vector3(6f, 0f, 0f);
                    using (MeshDrawSubmission.BeginCapture(sCol.Commands))
                    {
                        People.Draw(colonist, atCol, out _, out _, out _);
                        SpriteSolids.DrawHeld(revolver, atCol + new Vector3(0.2f, 0.7f, 0f), 0f);
                    }

                    // 9. Muffalo
                    var sMuf = new ModelStyleCases.ComparisonSample { Name = "Muffalo" }; samples.Add(sMuf);
                    Vector3 atMuf = center + new Vector3(-4f, 0f, 5f);
                    using (MeshDrawSubmission.BeginCapture(sMuf.Commands))
                    {
                        Animals.Draw(muffalo, atMuf);
                    }

                    // 10. StandingLamp
                    var sLamp = new ModelStyleCases.ComparisonSample { Name = "StandingLamp" }; samples.Add(sLamp);
                    Vector3 atLamp = center + new Vector3(0f, 0f, 5f);
                    lamp.Position = new IntVec3((int)atLamp.x, 0, (int)atLamp.z);
                    using (MeshDrawSubmission.BeginCapture(sLamp.Commands))
                    {
                        Bake(lamp, lampEntry.Pick(lamp), lampEntry, atLamp);
                    }

                    // 11. GrowthVat
                    var sVat = new ModelStyleCases.ComparisonSample { Name = "GrowthVat" }; samples.Add(sVat);
                    Vector3 atVat = center + new Vector3(4f, 0f, 5f);
                    vat.Position = new IntVec3((int)atVat.x, 0, (int)atVat.z);
                    using (MeshDrawSubmission.BeginCapture(sVat.Commands))
                    {
                        Bake(vat, vatEntry.Pick(vat), vatEntry, atVat);
                        typeof(Glass).GetMethod("Draw", ModelStyleCases.Hidden).Invoke(null, new object[] { vat, vatEntry });
                    }

                    var commands = samples.SelectMany(sample => sample.Commands).ToList();
                    submit = rendering => { if (rendering == camera) ((List<MeshDrawCommand>)immediate.GetValue(null)).AddRange(commands); };
                    Camera.onPreRender += submit;

                    var materials = commands.Select(command => command.Material).Distinct().ToArray();
                    foreach (var mat in materials)
                    {
                        var view = mat.GetVector(Shader.PropertyToID("_IsoView")); var sun = mat.GetVector(Shader.PropertyToID("_IsoSun"));
                        var sky = mat.GetVector(Shader.PropertyToID("_IsoSky")); var cell = mat.GetVector(Shader.PropertyToID("_IsoCellMap"));
                        float glow = mat.GetFloat("_GlowMode");
                        restore.Add(() => { mat.SetVector("_IsoView", view); mat.SetVector("_IsoSun", sun); mat.SetVector("_IsoSky", sky); mat.SetVector("_IsoCellMap", cell); mat.SetFloat("_GlowMode", glow); });
                        mat.SetVector("_IsoSun", new System.Numerics.Vector4(.3f, .8f, .52f, 0));
                        mat.SetVector("_IsoSky", new System.Numerics.Vector4(1, 1, 1, 1)); mat.SetVector("_IsoCellMap", System.Numerics.Vector4.Zero); mat.SetFloat("_GlowMode", 0);
                    }

                    for (int angle = 0; angle < 2; angle++)
                    {
                        float pitch = angle == 0 ? 35.3f : 55f, yaw = angle == 0 ? 45f : 0f;
                        UprightCards.SetCamera(pitch, yaw);
                        camera.ResetWorldToCameraMatrix();
                        camera.transform.rotation = Quaternion.Euler(pitch, yaw + 180, 0);
                        camera.transform.position = center + Vector3.up - camera.transform.forward * 30;
                        camera.worldToCameraMatrix = camera.worldToCameraMatrix * Matrix4x4.Scale(new Vector3(1, View3D.Squash, 1));
                        Vector3 direction = -camera.transform.forward;
                        foreach (var mat in materials) mat.SetVector("_IsoView", new System.Numerics.Vector4(direction.x, direction.y, direction.z, View3D.Squash));
                        foreach (var sample in samples)
                        {
                            try { sample.Bounds = ModelStyleCases.ComparisonBounds(sample.Commands, camera, size); }
                            catch (Exception error) { throw new InvalidOperationException(sample.Name + ": " + error.Message, error); }
                        }
                        int before = (int)serial.GetValue(backend);
                        camera.Render();
                        if (cardSubMesh != null && cardSubMat != null)
                        {
                            cardSubMat.SetPass(0);
                            Graphics.DrawMeshNow(cardSubMesh, Matrix4x4.identity);
                        }
                        int completed = (int)serial.GetValue(backend);
                        Check(completed == before + 1, "Comparison has no completed GPU render");
                        Check(backend.TryReadPixels(target, out byte[] raw), "Comparison GPU readback unavailable");
                        Check(raw.Length == size * size * 4, "Comparison framebuffer dimensions differ");
                        var visible = samples.Select(sample => new
                        {
                            name = sample.Name,
                            commands = sample.Commands.Count,
                            bounds = sample.Bounds,
                            pixels = ModelStyleCases.ComparisonPixels(raw, sample, samples, size)
                        }).ToArray();
                        string angleName = angle == 0 ? "isometric" : "straight";
                        Check(visible.All(sample => sample.pixels >= 5), "Colony sample is empty in " + ModelStyleCases.ComparisonPresets[preset] + "-" + angleName + ": " + string.Join(",", visible.Select(s => s.name + "=" + s.pixels)));
                        string stem = "colony-" + ModelStyleCases.ComparisonPresets[preset] + "-" + angleName;
                        File.WriteAllBytes(Path.Combine(request.outputDirectory, stem + ".rgba"), raw);
                        RenderTexture.active = target; var image = new Texture2D(size, size, TextureFormat.RGBA32, false);
                        image.ReadPixels(new Rect(0, 0, size, size), 0, 0); image.Apply(); byte[] png = image.EncodeToPNG(); UnityEngine.Object.Destroy(image);
                        File.WriteAllBytes(Path.Combine(request.outputDirectory, stem + ".png"), png); images.Add(stem, raw);
                        sampleMaps[ModelStyleCases.ComparisonPresets[preset] + "-" + angleName] = samples.Select(s => new ModelStyleCases.ComparisonSample { Name = s.Name, Bounds = (int[])s.Bounds.Clone() }).ToList();
                        frames.Add(new
                        {
                            name = stem,
                            preset = ModelStyleCases.ComparisonPresets[preset],
                            pitch,
                            yaw,
                            orthographicSize = 11,
                            width = size,
                            height = size,
                            samples = visible,
                            commands = commands.Count,
                            gpuFrameBefore = before,
                            gpuFrameCompleted = completed,
                            rawPath = stem + ".rgba",
                            rawHash = Convert.ToHexString(SHA256.HashData(raw)),
                            pngPath = stem + ".png",
                            pngHash = Convert.ToHexString(SHA256.HashData(png))
                        });

                        if (angle == 0)
                        {
                            for (int warm = 0; warm < 3; warm++) camera.Render();
                            int start = (int)serial.GetValue(backend); var ms = new List<double>();
                            for (int warm = 0; warm < 60; warm++)
                            {
                                long ticks = System.Diagnostics.Stopwatch.GetTimestamp();
                                camera.Render();
                                if (cardSubMesh != null && cardSubMat != null)
                                {
                                    cardSubMat.SetPass(0);
                                    Graphics.DrawMeshNow(cardSubMesh, Matrix4x4.identity);
                                }
                                ms.Add((System.Diagnostics.Stopwatch.GetTimestamp() - ticks) * 1000.0 / System.Diagnostics.Stopwatch.Frequency);
                            }
                            int finish = (int)serial.GetValue(backend); double[] sorted = ms.OrderBy(value => value).ToArray();
                            Check(finish == start + 60 && ms.All(value => value > 0), "Comparison lacks 60 completed warm render timings");
                            timings.Add(new
                            {
                                preset = ModelStyleCases.ComparisonPresets[preset],
                                commands = commands.Count,
                                gpuFrameBefore = start,
                                gpuFrameCompleted = finish,
                                samplesMs = ms,
                                medianMs = (sorted[29] + sorted[30]) / 2,
                                meanMs = ms.Average(),
                                p95Ms = sorted[56]
                            });
                        }
                    }
                    Camera.onPreRender -= submit; submit = null;
                    foreach (Action reset in restore) reset(); restore.Clear();
                    foreach (SectionLayer layer in layers) foreach (LayerSubMesh sub in layer.subMeshes) UnityEngine.Object.Destroy(sub.mesh); layers.Clear();
                }

                // In-engine preset pair assertions:
                foreach (string angle in new[] { "isometric", "straight" })
                {
                    byte[] classic = images["colony-Classic-" + angle];
                    byte[] wrapped = images["colony-MatteWrapped-" + angle];
                    byte[] oren = images["colony-MatteOrenNayar-" + angle];
                    byte[] noOutlines = images["colony-MatteNoOutlines-" + angle];

                    var samplesW = sampleMaps["MatteWrapped-" + angle];

                    // (a) Classic vs MatteWrapped: all 6 scene surface classes + reference models differ >= 10 pixels
                    foreach (var sample in samplesW)
                    {
                        int[] b = sample.Bounds;
                        int changed = 0;
                        for (int y = b[1]; y <= b[3]; y++)
                            for (int x = b[0]; x <= b[2]; x++)
                            {
                                int off = (y * size + x) * 4;
                                if (Math.Abs(classic[off] - wrapped[off]) > 2 ||
                                    Math.Abs(classic[off + 1] - wrapped[off + 1]) > 2 ||
                                    Math.Abs(classic[off + 2] - wrapped[off + 2]) > 2)
                                    changed++;
                            }
                        Check(changed >= 10, $"Classic vs MatteWrapped failed to differ across {sample.Name}: {changed} pixels in {angle}");
                    }

                    // (b) MatteWrapped vs MatteOrenNayar: silhouettes match, 3D rough-diffuse samples differ, 2D/cards/water not required to differ
                    Check(Enumerable.Range(0, size * size).All(pixel => ModelStyleCases.IsComparisonBackground(wrapped, pixel * 4) == ModelStyleCases.IsComparisonBackground(oren, pixel * 4)),
                        $"Wrapped/Oren-Nayar changed colony geometry silhouette instead of shading in {angle}");
                    string[] roughDiffuse = { "WallAndRock", "BuiltRoof", "Armchair", "Colonist", "Muffalo", "StandingLamp", "GrowthVat" };
                    foreach (string name in roughDiffuse)
                    {
                        var sample = samplesW.First(s => s.Name == name);
                        int[] b = sample.Bounds;
                        int changed = 0;
                        for (int y = b[1]; y <= b[3]; y++)
                            for (int x = b[0]; x <= b[2]; x++)
                            {
                                int off = (y * size + x) * 4;
                                if (Math.Abs(wrapped[off] - oren[off]) > 2 ||
                                    Math.Abs(wrapped[off + 1] - oren[off + 1]) > 2 ||
                                    Math.Abs(wrapped[off + 2] - oren[off + 2]) > 2)
                                    changed++;
                            }
                        Check(changed >= 10, $"Wrapped vs Oren-Nayar failed to differ on 3D rough-diffuse sample {name}: {changed} pixels in {angle}");
                    }

                    // (c) MatteWrapped vs MatteNoOutlines: outlined samples differ, un-outlined (TerrainAndFloor, PlantCard, Water) match
                    string[] outlined = { "WallAndRock", "BuiltRoof", "SpriteSlabs", "Armchair", "Colonist", "Muffalo", "StandingLamp", "GrowthVat" };
                    foreach (string name in outlined)
                    {
                        var sample = samplesW.First(s => s.Name == name);
                        int[] b = sample.Bounds;
                        int changed = 0;
                        for (int y = b[1]; y <= b[3]; y++)
                            for (int x = b[0]; x <= b[2]; x++)
                            {
                                int off = (y * size + x) * 4;
                                if (Math.Abs(wrapped[off] - noOutlines[off]) > 2 ||
                                    Math.Abs(wrapped[off + 1] - noOutlines[off + 1]) > 2 ||
                                    Math.Abs(wrapped[off + 2] - noOutlines[off + 2]) > 2)
                                    changed++;
                            }
                        Check(changed >= 10, $"Wrapped vs NoOutlines failed to differ on outlined sample {name}: {changed} pixels in {angle}");
                    }
                    string[] unoutlined = { "TerrainAndFloor", "PlantCard", "Water" };
                    foreach (string name in unoutlined)
                    {
                        var sample = samplesW.First(s => s.Name == name);
                        int[] b = sample.Bounds;
                        int changed = 0;
                        for (int y = b[1]; y <= b[3]; y++)
                            for (int x = b[0]; x <= b[2]; x++)
                            {
                                int off = (y * size + x) * 4;
                                if (Math.Abs(wrapped[off] - noOutlines[off]) > 2 ||
                                    Math.Abs(wrapped[off + 1] - noOutlines[off + 1]) > 2 ||
                                    Math.Abs(wrapped[off + 2] - noOutlines[off + 2]) > 2)
                                    changed++;
                            }
                        Check(changed == 0, $"Wrapped vs NoOutlines unexpectedly differed on un-outlined sample {name}: {changed} pixels in {angle}");
                    }
                }

                string[] sampleNames = { "TerrainAndFloor", "WallAndRock", "BuiltRoof", "PlantCard", "SpriteSlabs", "Water", "Armchair", "Colonist", "Muffalo", "StandingLamp", "GrowthVat" };
                return new
                {
                    frames,
                    timings,
                    samples = sampleNames,
                    backend = backend.GetType().FullName,
                    timingMethod = "Stopwatch.GetTimestamp around Camera.Render; completed native GPU fence"
                };
            }
            finally
            {
                if (submit != null) Camera.onPreRender -= submit;
                foreach (Action reset in restore) reset();
                foreach (SectionLayer layer in layers) foreach (LayerSubMesh sub in layer.subMeshes) UnityEngine.Object.Destroy(sub.mesh);
                if (wallThing?.Spawned == true) wallThing.DeSpawn();
                if (rockThing?.Spawned == true) rockThing.DeSpawn();
                Ground.Restore();
                DynamicModels.Forget(); People.ForgetAll(); Cull.Enabled = cull; PartBatches.Enabled = batches;
                IsoRimMod.Settings = previous; R3DMaterials.SyncModelStyle(0, 0f, 0f, 0f); R3DMaterials.SyncSceneStyle(0, 0f, 0f, 0f);
                RenderTexture.active = active; RenderTexture.ReleaseTemporary(target); UnityEngine.Object.Destroy(owner);
                View3D.Active = prevView;
                IntVec3 wc0 = new IntVec3((int)(center.x - 2f), 0, (int)center.z);
                IntVec3 wc1 = new IntVec3((int)(center.x - 1f), 0, (int)center.z);
                Find.CurrentMap.terrainGrid.SetTerrain(wc0, soilDef);
                Find.CurrentMap.terrainGrid.SetTerrain(wc1, soilDef);
                IntVec3 roofCell = new IntVec3((int)(center.x + 2f), 0, (int)(center.z - 5f));
                for (int rz = -2; rz <= 1; rz++)
                    for (int rx = -1; rx <= 2; rx++)
                        Find.CurrentMap.roofGrid.SetRoof(new IntVec3((int)(center.x + 2f) + rx, 0, (int)(center.z - 5f) + rz), null);
                Material cardMatRestore = UprightCards.For(oakTree.Graphic.MatSingleFor(oakTree));
                if (cardMatRestore != null) cardMatRestore.renderQueue = 2900;
            }
        }

        internal static string Run(string id, ProbeNormals.Request request)
        {
            switch (id)
            {
                case "SceneStyleCases.WallsRocksAndRoofs":
                    string path = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = WallsRocksAndRoofs(request) }));
                    return path;
                case "SceneStyleCases.TerrainCardsSlabsAndWater":
                    string path2 = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(path2, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = TerrainCardsSlabsAndWater(request) }));
                    return path2;
                case "SceneStyleCases.ColonySceneComparisonAndPerf":
                    string path3 = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(path3, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = ColonySceneComparisonAndPerf(request) }));
                    return path3;
                default: throw new InvalidOperationException("Unknown scene style case: " + id);
            }
        }
    }
}
