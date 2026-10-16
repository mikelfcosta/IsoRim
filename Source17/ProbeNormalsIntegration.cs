using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
        internal static readonly string[] Cases = { "ModelStyleCases.DiffuseAndHighlightModes", "ModelStyleCases.ProtectedExceptions" };

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
                case "ModelStyleCases.DiffuseAndHighlightModes": return MovingNormalCases.Run(id, request.outputDirectory, request.fixtureDirectory);
                case "ModelStyleCases.ProtectedExceptions":
                    string path = Path.Combine(request.outputDirectory, id + ".json");
                    File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings = new { captures = StaticNormalCases.ProtectedPixels(request.fixtureDirectory, request.outputDirectory) } }));
                    return path;
                default: throw new InvalidOperationException("Unknown model style case: " + id);
            }
        }
    }
}
