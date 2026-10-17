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
            string[] expected = Prerequisites.Concat(Integrated.Select(id => "IntegratedNormalCases." + id)).Concat(ModelStyleCases.Cases).OrderBy(id => id).ToArray();
            Require(request.caseIds.Length == expected.Length && request.caseIds.OrderBy(id => id).SequenceEqual(expected), "The integrated request omits or duplicates a required case");
            var proofs = Directory.GetFiles(request.outputDirectory, "NormalProof-*.json").Select(path => JsonSerializer.Deserialize<JsonElement>(File.ReadAllText(path))).ToArray();
            var rows = new List<object>();
            foreach (string id in Prerequisites)
            {
                JsonElement proof = proofs.Single(row => row.GetProperty("caseId").GetString() == id);
                Require(proof.GetProperty("passed").GetBoolean() && proof.GetProperty("head").GetString() == request.head
                    && proof.GetProperty("nonce").GetString() == request.nonce && proof.GetProperty("assemblyHash").GetString() == request.assemblyHash
                    && proof.GetProperty("processId").GetInt32() == Environment.ProcessId && proof.GetProperty("runtime").GetString() == "10.0.10-waylithclr.dev"
                    && proof.GetProperty("gameVersion").GetString() == "1.7.4980.60", "An integrated prerequisite has stale provenance: " + id);
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

        private sealed class ComparisonSample
        {
            internal string Name;
            internal List<MeshDrawCommand> Commands = new List<MeshDrawCommand>();
            internal int[] Bounds;
        }

        private static readonly string[] ComparisonPresets = { "Classic", "MatteWrapped", "MatteOrenNayar", "MatteNoOutlines" };

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

        private static int[] ComparisonBounds(List<MeshDrawCommand> commands, Camera camera, int size)
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

        private static bool IsComparisonBackground(byte[] raw, int offset) => raw[offset] == 255 && raw[offset + 1] == 0 && raw[offset + 2] == 255;

        private static int ComparisonPixels(byte[] raw, ComparisonSample sample, List<ComparisonSample> samples, int size)
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

        private const BindingFlags Hidden = BindingFlags.Static | BindingFlags.NonPublic;
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
        internal static readonly string[] Cases = { "SceneStyleCases.WallsRocksAndRoofs" };

        private static void Check(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
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

            try
            {
                syncModel.Invoke(null, new object[] { 2, 0.22f, 0.04f, 0.65f });
                Material wallSolid = R3DMaterials.Lit(0, 1);
                Material wallTex = R3DMaterials.LitTexture("IsoRim/Walls/BricksSide", Color.white, 0, 1);
                Material mark = IsoGlow.MarkFor(wallSolid);
                Material roofMat = R3DMaterials.Roof;
                Check(wallSolid.GetFloat("_SceneClay") == 0f && wallSolid.GetFloat("_ClayMode") == 0f, "SyncModelStyle mutated curve 1 wall solid");
                Check(wallTex.GetFloat("_SceneClay") == 0f && wallTex.GetFloat("_ClayMode") == 0f, "SyncModelStyle mutated curve 1 wall texture");
                Check(mark.GetFloat("_SceneClay") == 0f && mark.GetFloat("_NormalSpace") == 0f, "SyncModelStyle mutated light mark");
                Check(roofMat.GetFloat("_SceneClay") == 0f && roofMat.GetFloat("_ClayMode") == 0f, "SyncModelStyle mutated roof");
            }
            finally
            {
                syncModel.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            }

            try
            {
                syncScene.Invoke(null, new object[] { 1, 0.22f, 0.04f, 0.65f });
                Material wallSolid = R3DMaterials.Lit(0, 1);
                Material wallTex = R3DMaterials.LitTexture("IsoRim/Walls/BricksSide", Color.white, 0, 1);
                Material newWallTex = R3DMaterials.LitTexture("IsoRim/Walls/PlanksTop", Color.white, 0, 1);
                Material roofMat = R3DMaterials.Roof;
                Check(wallSolid.GetFloat("_SceneClay") == 1f && wallSolid.GetFloat("_ClayMode") == 1f
                    && Math.Abs(wallSolid.GetFloat("_ClayWrap") - 0.22f) < 1e-5f
                    && Math.Abs(wallSolid.GetFloat("_ClayHighlight") - 0.04f) < 1e-5f
                    && Math.Abs(wallSolid.GetFloat("_ClayRoughness") - 0.65f) < 1e-5f
                    && wallSolid.GetFloat("_LightMode") == 2f,
                    "SyncSceneStyle did not update cached wall solid");
                Check(wallTex.GetFloat("_SceneClay") == 1f && wallTex.GetFloat("_ClayMode") == 1f
                    && wallTex.GetFloat("_LightMode") == 2f,
                    "SyncSceneStyle did not update cached wall texture");
                Check(newWallTex.GetFloat("_SceneClay") == 1f && newWallTex.GetFloat("_ClayMode") == 1f
                    && newWallTex.GetFloat("_LightMode") == 2f,
                    "Newly created wall texture missed scene clay sync");
                Check(roofMat.GetFloat("_SceneClay") == 1f && roofMat.GetFloat("_ClayMode") == 1f
                    && roofMat.GetFloat("_LightMode") == 3f && roofMat.GetFloat("_GlowMode") == 2f,
                    "SyncSceneStyle did not update R3DMaterials.Roof or lost SkyOnly");

                MethodInfo texMatMethod = typeof(SectionLayer_IsoRimRoofs).GetMethod("TextureMaterial", BindingFlags.NonPublic | BindingFlags.Static);
                Material texRoof = (Material)texMatMethod.Invoke(null, new object[] { 0, 0 });
                Check(texRoof.GetFloat("_SceneClay") == 1f && texRoof.GetFloat("_ClayMode") == 1f
                    && texRoof.GetFloat("_LightMode") == 3f && texRoof.GetFloat("_GlowMode") == 2f,
                    "SectionLayer_IsoRimRoofs.TextureMaterial missed scene clay sync or lost SkyOnly");
                Check(Math.Abs(texRoof.color.r - SectionLayer_IsoRimRoofs.Styles[0].r * 1.12f) < 0.01f,
                    "SectionLayer_IsoRimRoofs.TextureMaterial did not scale tint to 1.12x in clay mode");

                PropertyInfo shipMatProp = typeof(ShipRoofs).GetProperty("Mat", BindingFlags.NonPublic | BindingFlags.Static);
                Material shipMat = (Material)shipMatProp.GetValue(null);
                Check(shipMat.GetFloat("_SceneClay") == 1f && shipMat.GetFloat("_ClayMode") == 1f
                    && shipMat.GetFloat("_LightMode") == 3f && shipMat.GetFloat("_GlowMode") == 2f
                    && shipMat.GetFloat("_NormalSpace") == 1f,
                    "ShipRoofs.Mat missed scene clay sync, BakedTrueSpace, or lost SkyOnly");
            }
            finally
            {
                syncScene.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            }

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
                Material wallSolid = R3DMaterials.Lit(0, 1);
                Material roofMat = R3DMaterials.Roof;
                Check(wallSolid.GetFloat("_SceneClay") == 1f && wallSolid.GetFloat("_ClayMode") == 2f
                    && Math.Abs(wallSolid.GetFloat("_ClayWrap") - 0.31f) < 1e-5f
                    && Math.Abs(wallSolid.GetFloat("_ClayHighlight") - 0.08f) < 1e-5f
                    && Math.Abs(wallSolid.GetFloat("_ClayRoughness") - 0.9f) < 1e-5f,
                    "View3D.Tick() did not sync scene clay to wall");
                Check(roofMat.GetFloat("_SceneClay") == 1f && roofMat.GetFloat("_ClayMode") == 2f,
                    "View3D.Tick() did not sync scene clay to roof");
            }
            finally
            {
                IsoRimMod.Settings = prevSettings;
                syncScene.Invoke(null, new object[] { 0, 0f, 0f, 0f });
                syncModel.Invoke(null, new object[] { 0, 0f, 0f, 0f });
            }

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
            MethodInfo shipPut = typeof(ShipRoofs).GetMethod("Put", BindingFlags.NonPublic | BindingFlags.Static);
            var section = new Section(new IntVec3(0, 0, 0), Find.CurrentMap);
            var roofLayer = new SectionLayer_IsoRimRoofs(section);
            Material shipMatRef = (Material)typeof(ShipRoofs).GetProperty("Mat", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
            LayerSubMesh sm = MeshWriter.Sub(roofLayer, shipMatRef);
            shipPut.Invoke(null, new object[] { sm, roofPiece, Vector2.zero, 0f, 1f, -1f, 0f, 0f });
            Check(sm.normals.Count == sm.verts.Count, "ShipRoofs.Put normals count differs from vertices");
            Check(sm.normals.Any(n => n.sqrMagnitude > 0.5f), "ShipRoofs.Put omitted non-zero normals");
            Check(sm.normals.Any(n => Math.Abs(n.sqrMagnitude - 1f) < 0.01f), "Stamped normals not normalized");

            IsoRimSettings settingsCopy = IsoRimMod.Settings;
            float width100 = 0f, width75 = 0f;
            Thing wallThing = null;
            try
            {
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f };
                Check(Math.Abs(WallBoxes.LineScale - 0.75f) < 1e-5f, "LineScale differs for outlineWidth=0.75");
                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f };
                Check(Math.Abs(WallBoxes.LineScale - 1.00f) < 1e-5f, "LineScale differs for outlineWidth=1.00");
                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f };
                Check(WallBoxes.LineScale == 0f, "LineScale not 0 when outlines=false");

                wallThing = ThingMaker.MakeThing(ThingDefOf.Wall, ThingDefOf.Steel);
                GenSpawn.Spawn(wallThing, Find.CurrentMap.Center + new IntVec3(5, 0, 5), Find.CurrentMap);

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 1.00f, ruggedRock = false };
                var layer100 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(layer100, wallThing, 1f);
                LayerSubMesh solid100 = layer100.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(solid100 != null && solid100.verts.Count > 0, "Outline quads missing at outlineWidth=1.00");

                IsoRimMod.Settings = new IsoRimSettings { outlines = true, outlineWidth = 0.75f, ruggedRock = false };
                var layer75 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(layer75, wallThing, 1f);
                LayerSubMesh solid75 = layer75.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                Check(solid75 != null && solid75.verts.Count == solid100.verts.Count, "Solid outline vertex count differs between 0.75 and 1.00");

                Color32 lineColor = R3DMaterials.Shaded(wallThing.DrawColor, 0.32f);
                int idx100 = solid100.colors.FindIndex(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                int idx75 = solid75.colors.FindIndex(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                Check(idx100 >= 0 && idx75 >= 0, "Outline rim vertices not found in solid submesh");

                width100 = (solid100.verts[idx100 + 1] - solid100.verts[idx100]).magnitude;
                width75 = (solid75.verts[idx75 + 1] - solid75.verts[idx75]).magnitude;
                Check(Math.Abs(width75 / width100 - 0.75f) < 0.02f, "Outline quad width did not scale to 0.75");

                IsoRimMod.Settings = new IsoRimSettings { outlines = false, outlineWidth = 0.75f, ruggedRock = false };
                var layer0 = new SectionLayer_IsoRimWalls(section);
                WallBoxes.PrintBlock(layer0, wallThing, 1f);
                LayerSubMesh solid0 = layer0.subMeshes.FirstOrDefault(s => s.material == R3DMaterials.Solid);
                bool hasOutlines0 = solid0 != null && solid0.colors.Any(c => c.r == lineColor.r && c.g == lineColor.g && c.b == lineColor.b);
                Check(!hasOutlines0, "Outline quads emitted when outlines=false");
            }
            finally
            {
                IsoRimMod.Settings = settingsCopy;
                if (wallThing != null && wallThing.Spawned) wallThing.Destroy();
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

            var captures = new List<object>();
            var wallNormQuant = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, true, 0, 0, -1,
                "-wall-norm-quant", sceneClay: 0f, lightMode: 2);
            var wallNormClay = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, true, 0, 0, -1,
                "-wall-norm-clay", sceneClay: 1f, lightMode: 2, style: 1);
            captures.Add(wallNormQuant);
            captures.Add(wallNormClay);

            var wallLitClassic = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, -1,
                "-wall-lit-classic", sceneClay: 0f, lightMode: 2, style: 0);
            var wallLitMode1 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, -1,
                "-wall-lit-mode1", sceneClay: 1f, lightMode: 2, style: 1, highlight: 0.04f);
            var wallLitMode2 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, -1,
                "-wall-lit-mode2", sceneClay: 1f, lightMode: 2, style: 2, highlight: 0.04f);
            captures.Add(wallLitClassic);
            captures.Add(wallLitMode1);
            captures.Add(wallLitMode2);

            var roofLitClassic = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, -1,
                "-roof-lit-classic", sceneClay: 0f, lightMode: 3, style: 0);
            var roofLitMode1 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, -1,
                "-roof-lit-mode1", sceneClay: 1f, lightMode: 3, style: 1, highlight: 0.04f);
            var roofLitMode2 = StaticNormalCases.Capture(request.outputDirectory, pair, face, false, false, 0, 0, -1,
                "-roof-lit-mode2", sceneClay: 1f, lightMode: 3, style: 2, highlight: 0.04f);
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
                wallTopCaps = new { classic = capClassic.color.r, clay = capClay.color.r, lightMode = 2, glowMode = 1 },
                shipRoofs = new { normalStreamStamped = true, normalsCount = sm.normals.Count },
                outlineScaling = new { lineScale75 = 0.75, width100, width75, ratio = width75 / width100, zeroOutlineVerified = true },
                captures
            };
        }

        internal static string Run(string id, ProbeNormals.Request request)
        {
            switch (id)
            {
                case "SceneStyleCases.WallsRocksAndRoofs":
                    string path = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = WallsRocksAndRoofs(request) }));
                    return path;
                default: throw new InvalidOperationException("Unknown scene style case: " + id);
            }
        }
    }
}
