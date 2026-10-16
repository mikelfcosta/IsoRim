using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using RimWorld;
using UnityEngine;
using Verse;

namespace IsoRim
{
    // Only reachable through the three opt-in guards and fresh request in ProbeNormals.
    internal static class StaticNormalCases
    {
        private const string Neutral = "Diagnostics/NormalProofNeutral";
        private static readonly JsonSerializerOptions Json = new JsonSerializerOptions { WriteIndented = true };
        private static readonly List<SectionLayer> Layers = new List<SectionLayer>();

        private static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }

        private static float[] V(Vector3 value) => new[] { value.x, value.y, value.z };
        private static void Equal(Vector3 value, Vector3 expected, string message) =>
            Require((value - expected).sqrMagnitude < 0.000001f, message + ": " + value + " != " + expected);

        internal static string Run(string id, string evidence, string fixtures)
        {
            object readings;
            bool outlines = IsoRimMod.Settings.outlines;
            bool textures = IsoRimMod.Settings.texturedModels;
            try
            {
                IsoRimMod.Settings.texturedModels = true;
                switch (id)
                {
                    case "StaticNormalCases.TrueSpaceTransform": readings = TrueSpaceTransform(fixtures); break;
                    case "StaticNormalCases.MixedStreams": readings = MixedStreams(fixtures); break;
                    case "StaticNormalCases.MaterialAndShaderContract": readings = MaterialContract(); break;
                    case "StaticNormalCases.GpuAuthoredDirection": readings = Pixels(fixtures, evidence, "direction"); break;
                    case "StaticNormalCases.ProductionLighting": readings = Pixels(fixtures, evidence, "lighting"); break;
                    case "StaticNormalCases.GeometricResponsibilities": readings = Geometric(fixtures, evidence); break;
                    case "StaticNormalCases.OracleControls": readings = Pixels(fixtures, evidence, "controls"); break;
                    default: throw new InvalidOperationException("Unknown static case: " + id);
                }
                string path = Path.Combine(evidence, id + ".json");
                File.WriteAllText(path, JsonSerializer.Serialize(new { caseId = id, passed = true, readings }, Json));
                return path;
            }
            finally
            {
                IsoRimMod.Settings.texturedModels = textures;
                IsoRimMod.Settings.outlines = outlines;
                foreach (SectionLayer layer in Layers)
                    foreach (LayerSubMesh sub in layer.subMeshes) UnityEngine.Object.Destroy(sub.mesh);
                Layers.Clear();
            }
        }

        private static Model Fixture(string fixtures, bool textured = false, bool authored = true)
        {
            // Cells, Y up, ground y=0, pivot origin, imported Z reflection. The bake lifts it three cells.
            string path = Path.Combine(fixtures, "static-" + textured + "-" + authored + ".obj");
            File.WriteAllText(Path.ChangeExtension(path, ".mtl"), "newmtl sample\nKd "
                + (textured ? "0.55555556 0.55555556 0.55555556\nmap_Kd Textures/" + Neutral + ".png\n" : "0.6 0.6 0.6\n"));
            File.WriteAllText(path, "mtllib " + Path.GetFileName(Path.ChangeExtension(path, ".mtl"))
                + "\nv 0 0 0\nv 2 0 0\nv 0 1 0\nvt 0 0\nvt 1 0\nvt 0 1\nvn 0.94 0.1 -0.326190128606\nusemtl sample\n"
                + (authored ? "f 1/1/1 2/2/1 3/3/1\n" : "f 1/1 2/2 3/3\n"));
            Model model = ObjLoader.Load(path);
            Require(model != null && model.positions.Length == 3, "The asymmetric fixture did not load");
            return model;
        }

        private static SectionLayer Bake(Model model, Quaternion turn, Quaternion lay, Vector3 scale, Thing thing = null, float outline = 0)
        {
            var section = new Section(new IntVec3(0, 0, 0), Find.CurrentMap);
            var layer = new SectionLayer_IsoRimBodies(section);
            Layers.Add(layer);
            var write = typeof(ModelBaker).GetMethod("Write", BindingFlags.Static | BindingFlags.NonPublic);
            bool result = (bool)write.Invoke(null, new object[] { layer, thing ?? ThingMaker.MakeThing(ThingDefOf.Steel), model,
                Vector3.zero, turn, lay, new Vector3(0, 3, 0), scale, Vector3.zero, 0f, Color.white, false, outline });
            Require(result, "Actual ModelBaker.Write refused the fixture");
            return layer;
        }

        private static LayerSubMesh Face(SectionLayer layer, bool textured = false) => layer.subMeshes.Single(sub =>
            sub.verts.Count == 3 && sub.material.GetFloat("_LightMode") == 1 && (sub.material.mainTexture != null) == textured);

        private static void Upload(SectionLayer layer)
        {
            CutTags.Finish(layer);
            NormalStreams.Finish(layer);
            typeof(SectionLayer).GetMethod("FinalizeMesh", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(layer, new object[] { MeshParts.All });
            foreach (LayerSubMesh sub in layer.subMeshes.Where(sub => sub.normals.Count > 0))
            {
                Require(sub.normals.Count == sub.verts.Count, "A normal stream is not aligned");
                Require(sub.mesh.normals.Length == sub.verts.Count, "LayerSubMesh dropped the normal stream on upload");
                for (int i = 0; i < sub.verts.Count; i++) Equal(sub.mesh.normals[i], sub.normals[i], "Mesh upload changed a normal");
            }
        }

        private sealed class Pair
        {
            internal string Name;
            internal Vector3 Scale, N, G, Q, L;
            internal float Turn, Lay;
            internal double[] Shades;
        }

        // Independent double-precision reference calculations of inverse-transpose(Ry * S * Rx).
        // These are literals, never obtained from the baker, shader or production BinNormal/ModelShade.
        private static Pair[] Pairs() => new[] {
            new Pair { Name = "standing", Scale = Vector3.one, N = new Vector3(.94f,.1f,.326190128606f),
                G = Vector3.forward, Q = Vector3.right, L = new Vector3(0,.8f,.6f), Shades = new[] {.6723212982272506,.875,.5} },
            new Pair { Name = "scaled", Scale = new Vector3(1.3f,.9f,1.1f), Turn = 45,
                N = new Vector3(.913344804013f,.140757555142f,-.382084518992f), G = new Vector3(.707106781187f,0,.707106781187f),
                Q = new Vector3(.707106781187f,0,-.707106781187f), L = new Vector3(.424264068712f,.8f,.424264068712f),
                Shades = new[] {.7112504338634346,.875,.5} },
            new Pair { Name = "laid", Scale = new Vector3(1.3f,.9f,1.1f), Turn = 45, Lay = 90,
                N = new Vector3(.707165755700f,-.445294756764f,-.549207769031f), G = Vector3.down,
                Q = new Vector3(.707106781187f,0,-.707106781187f), L = new Vector3(.698125347465f,.7f,-.150402789959f),
                Shades = new[] {.6889902785054138,.5,.9285714285714286} },
            new Pair { Name = "mirror", Scale = new Vector3(-1.3f,.9f,1.1f), Turn = 45,
                N = new Vector3(-.382084518992f,.140757555142f,.913344804013f), G = new Vector3(.707106781187f,0,.707106781187f),
                Q = new Vector3(-.707106781187f,0,.707106781187f), L = new Vector3(.424264068712f,.8f,.424264068712f),
                Shades = new[] {.7112504338634346,.875,.5} }
        };

        private static object TrueSpaceTransform(string fixtures)
        {
            var rows = new List<object>();
            foreach (Pair pair in Pairs())
            {
                SectionLayer layer = Bake(Fixture(fixtures), Quaternion.AngleAxis(pair.Turn, Vector3.up), Quaternion.AngleAxis(pair.Lay, Vector3.right), pair.Scale);
                LayerSubMesh sub = Face(layer);
                Require(sub.material.GetFloat("_NormalSpace") == 1, "The static baker selected the wrong normal-space material");
                Require(sub.normals.Count == 3, "Static ModelBaker did not write a complete authored stream");
                foreach (Vector3 normal in sub.normals) Equal(normal, pair.N, "Wrong true-space inverse transpose");
                Upload(layer);
                rows.Add(new
                {
                    pair.Name,
                    scale = V(pair.Scale),
                    pair.Turn,
                    pair.Lay,
                    expected = V(pair.N),
                    actual = sub.mesh.normals.Select(V).ToArray(),
                    squash = View3D.Squash
                });
            }
            return rows;
        }

        private static object MixedStreams(string fixtures)
        {
            SectionLayer layer = Bake(Fixture(fixtures), Quaternion.identity, Quaternion.identity, Vector3.one);
            LayerSubMesh sub = Face(layer);
            // Append a legacy writer, then an invalid triangle, then an independently valid triangle.
            for (int tri = 0; tri < 3; tri++)
                for (int k = 0; k < 3; k++)
                {
                    sub.tris.Add(sub.verts.Count);
                    sub.verts.Add(sub.verts[k] + Vector3.right * (tri + 1) * 3);
                    sub.colors.Add(sub.colors[k]); sub.uvs.Add(new Vector3(0, 0, 10));
                    if (tri > 0) NormalStreams.Stamp(sub, tri == 1 && k == 1 ? new Vector3(float.NaN, 0, 0) : Vector3.forward);
                }
            CutTags.Enabled = true;
            try { CutTags.Stamp(sub, CutTags.Tag(new IntVec3(4, 0, 5), ShaderCut.Leaf)); }
            finally { CutTags.Enabled = false; }
            Upload(layer);
            for (int i = 0; i < 12; i++)
                Require((sub.mesh.normals[i].sqrMagnitude > 0) == (i < 3 || i >= 9), "Mixed fallback was not whole-triangle");
            Require(sub.uvs.Skip(3).All(uv => uv.z == 10), "UV0 procedural codes were overwritten");
            Require(sub.uvsChannelTwo.Count == 12 && sub.uvsChannelTwo[11].z == ShaderCut.Leaf, "UV1 tags were overwritten");
            // Missing and zero/invalid scales must not manufacture an up vector.
            SectionLayer legacy = Bake(Fixture(fixtures, authored: false), Quaternion.identity, Quaternion.identity, Vector3.one);
            Upload(legacy);
            Require(Face(legacy).mesh.normals.All(n => n == Vector3.zero), "Legacy normals inherited the stock up default");
            return new { vertices = sub.verts.Count, normals = sub.mesh.normals.Select(V).ToArray(), legacy = Face(legacy).mesh.normals.Select(V).ToArray(), uv1 = sub.uvsChannelTwo.Select(V).ToArray() };
        }

        private static object MaterialContract()
        {
            var rows = new List<object>();
            Material priorSolid = null, priorTexture = null;
            foreach (ShadingNormalSpace space in Enum.GetValues(typeof(ShadingNormalSpace)))
            {
                Material solid = R3DMaterials.Lit(0, 0, space);
                Material texture = R3DMaterials.LitTexture(Neutral, Color.gray, 0, 0, space);
                Require(solid != null && texture != null && solid != priorSolid && texture != priorTexture, "Normal-space cache identities alias");
                Require(solid == R3DMaterials.Lit(17, 0, space) && texture == R3DMaterials.LitTexture(Neutral, Color.gray, 17, 0, space), "Equivalent cache keys miss");
                Require(solid.GetFloat("_NormalSpace") == (float)space && texture.GetFloat("_NormalSpace") == (float)space, "Material normal-space contract differs");
                Require(IsoGlow.MarkFor(solid).GetFloat("_NormalSpace") == 0 && IsoGlow.MarkFor(texture).GetFloat("_NormalSpace") == 0, "Glow marks inherited authored state");
                rows.Add(new
                {
                    state = space.ToString(),
                    solidId = solid.GetInstanceID(),
                    textureId = texture.GetInstanceID(),
                    solidShader = solid.shader.name,
                    textureShader = texture.shader.name,
                    observed = solid.GetFloat("_NormalSpace"),
                    texture = texture.mainTexture.name
                });
                priorSolid = solid; priorTexture = texture;
            }
            string root = Path.Combine(AppContext.BaseDirectory, "Mods", "IsoRim", "1.7", "Shaders");
            string[] files = { "IsoRim.vert.glsl", "IsoRimTex.vert.glsl", "IsoRimSolid.frag.glsl" };
            string[] sources = files.Select(file => File.ReadAllText(Path.Combine(root, file))).ToArray();
            string Layout(string source) => System.Text.RegularExpressions.Regex.Replace(source.Substring(source.IndexOf("uniform MaterialUniforms", StringComparison.Ordinal),
                source.IndexOf("} uMaterial;", StringComparison.Ordinal) - source.IndexOf("uniform MaterialUniforms", StringComparison.Ordinal)), @"//[^\r\n]*|\s+", "");
            Require(sources.All(source => Layout(source) == Layout(sources[0])), "Owned shader material layouts diverged");
            Require(sources.Take(2).All(source => source.Contains("@vertex Normal optional @default = vec4(0.0, 0.0, 0.0, 0.0)")
                && source.Contains("layout(location = 4) in vec4 inUv1") && source.Contains("binding = 4")), "Owned vertex/default/UV1/binding-4 contract changed");
            return new
            {
                states = rows,
                shaderSources = files.Select(file => new { file, sha256 = Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(root, file)))) }).ToArray(),
                matchingMaterialLayouts = true,
                uv1AndBinding4Preserved = true
            };
        }

        private static object Geometric(string fixtures, string evidence)
        {
            Model authored = Fixture(fixtures, true), legacy = Fixture(fixtures, true, false);
            bool outlines = IsoRimMod.Settings.outlines;
            IsoRimMod.Settings.outlines = true;
            Require(authored.normals.SequenceEqual(legacy.normals) && authored.hull.SequenceEqual(legacy.hull), "Shading redirected hull or G");
            SectionLayer a = Bake(authored, Quaternion.identity, Quaternion.identity, Vector3.one, outline: 1);
            SectionLayer b = Bake(legacy, Quaternion.identity, Quaternion.identity, Vector3.one, outline: 1);
            IsoRimMod.Settings.outlines = outlines;
            Require(a.subMeshes.Any(s => s.material == R3DMaterials.Outline && s.verts.Count == 3), "Outline witness was empty");
            Require(a.subMeshes.Count == b.subMeshes.Count, "Authored data changed geometric writer count");
            for (int i = 0; i < a.subMeshes.Count; i++)
                Require(a.subMeshes[i].verts.SequenceEqual(b.subMeshes[i].verts) && a.subMeshes[i].colors.SequenceEqual(b.subMeshes[i].colors), "Texture lift or outline used shading N");
            Thing thing = ThingMaker.MakeThing(ThingDefOf.Steel);
            SectionLayer ga, gb, da, db;
            try
            {
                ModelBaker.Ghost = new Color(.2f, .6f, .8f, .5f);
                ga = Bake(authored, Quaternion.identity, Quaternion.identity, Vector3.one, thing);
                gb = Bake(legacy, Quaternion.identity, Quaternion.identity, Vector3.one, thing);
                ModelBaker.Ghost = Color.clear;
                ModelBaker.DamageMarks = 2;
                da = Bake(authored, Quaternion.identity, Quaternion.identity, Vector3.one, thing);
                db = Bake(legacy, Quaternion.identity, Quaternion.identity, Vector3.one, thing);
            }
            finally { ModelBaker.Ghost = Color.clear; ModelBaker.DamageMarks = 0; }
            foreach (var pair in new[] { new[] { ga, gb }, new[] { da, db } })
            {
                var left = pair[0].subMeshes.Where(s => s.verts.Count > 0).ToArray();
                var right = pair[1].subMeshes.Where(s => s.verts.Count > 0).ToArray();
                Require(left.Length > 0 && left.Length == right.Length, "A geometric responsibility witness was empty");
                for (int i = 0; i < left.Length; i++) Require(left[i].verts.SequenceEqual(right[i].verts) && left[i].colors.SequenceEqual(right[i].colors), "Ghost or damage used shading N");
            }
            SectionLayer cutLayer = Bake(Fixture(fixtures), Quaternion.identity, Quaternion.identity, Vector3.one);
            LayerSubMesh cut = Face(cutLayer);
            while (cut.uvsChannelTwo.Count < 3) cut.uvsChannelTwo.Add(Vector3.zero);
            cut.uvsChannelTwo[1] = CutTags.Tag(new IntVec3(4, 0, 5), ShaderCut.Lower | ShaderCut.TexSide);
            Upload(cutLayer);
            Require(cut.mesh.normals.All(n => n == Vector3.zero), "Cutaway retained a partially authored triangle");
            cut.uvsChannelTwo[0] = cut.uvsChannelTwo[2] = cut.uvsChannelTwo[1];
            Upload(cutLayer);
            SectionLayer own = Bake(Fixture(fixtures), Quaternion.identity, Quaternion.identity, Vector3.one);
            Upload(own);
            return new
            {
                geometric = authored.normals.Select(V).ToArray(),
                hull = authored.hull.Select(V).ToArray(),
                textureLiftAndOutlineUnchanged = true,
                ghostVertices = ga.subMeshes.Sum(s => s.verts.Count),
                damageVertices = da.subMeshes.Sum(s => s.verts.Count),
                cutNormals = cut.mesh.normals.Select(V).ToArray(),
                glowMarkSpace = IsoGlow.MarkFor(cut.material).GetFloat("_NormalSpace"),
                ownCell = Capture(evidence, Pairs()[0], Face(own), false, false, 0, 0, 0, "own-cell", false, true),
                cutaway = Capture(evidence, Pairs()[0], cut, false, false, 0, 0, 1, "cutaway", false, false, true)
            };
        }

        private static object Pixels(string fixtures, string evidence, string kind)
        {
            var rows = new List<object>();
            foreach (Pair pair in Pairs())
                foreach (bool textured in new[] { false, true })
                {
                    SectionLayer layer = Bake(Fixture(fixtures, textured), Quaternion.AngleAxis(pair.Turn, Vector3.up), Quaternion.AngleAxis(pair.Lay, Vector3.right), pair.Scale);
                    Upload(layer);
                    LayerSubMesh sub = Face(layer, textured);
                    if (kind == "direction") rows.Add(Capture(evidence, pair, sub, textured, true, 0, 0, 0));
                    else if (kind == "lighting")
                    {
                        rows.Add(Capture(evidence, pair, sub, textured, false, 0, 0, 0));
                        rows.Add(Capture(evidence, pair, sub, textured, false, 0, 1, 1));
                        rows.Add(Capture(evidence, pair, sub, textured, false, 0, 2, 2));
                        rows.Add(Capture(evidence, pair, sub, textured, false, 0, 0, 0, "reset"));
                        // The legacy control keeps BinNormal(G), even when its mesh happens to contain normals.
                        rows.Add(Capture(evidence, pair, sub, textured, false, 0, 0, 1, "legacy", true));
                    }
                    else
                    {
                        rows.Add(Capture(evidence, pair, sub, textured, true, 1, 0, 1));
                        rows.Add(Capture(evidence, pair, sub, textured, true, 0, 1, 0));
                        rows.Add(Capture(evidence, pair, sub, textured, true, 0, 2, 0));
                    }
                }
            if (kind == "lighting")
            {
                // G is twenty degrees off its legacy zero-elevation bin. Exact G would read 130,
                // while BinNormal(G) reads 134. This catches removal of legacy quantization.
                var oblique = new Pair
                {
                    Name = "legacy-oblique",
                    Scale = new Vector3(1.3f, .9f, 1.1f),
                    Turn = 20,
                    N = new Vector3(.989247403381f, .140757555142f, .039710018574f),
                    G = new Vector3(.342020143326f, 0, .939692620786f),
                    Q = Vector3.right,
                    L = new Vector3(0, .8f, .6f),
                    Shades = new[] { .5852700345358998, .875, .5 }
                };
                foreach (bool textured in new[] { false, true })
                {
                    SectionLayer layer = Bake(Fixture(fixtures, textured), Quaternion.AngleAxis(20, Vector3.up), Quaternion.identity, oblique.Scale);
                    Upload(layer);
                    rows.Add(Capture(evidence, oblique, Face(layer, textured), textured, false, 0, 0, 1, "legacy", true));
                }
            }
            return rows;
        }

        private static int[] RGBA(Color32 p) => new[] { (int)p.r, p.g, p.b, p.a };
        private static int Difference(int[][] a, int[][] b) => a.Zip(b, (x, y) => Enumerable.Range(0, 3).Max(c => Math.Abs(x[c] - y[c]))).Max();

        private static object Capture(string evidence, Pair pair, LayerSubMesh sub, bool textured, bool diagnostic,
            int transportFault, int lightingFault, int expectedIndex, string suffix = "", bool legacy = false, bool ownCell = false, bool cutaway = false)
        {
            const int width = 1024, height = 256;
            string name = (diagnostic ? "direction" : "lighting") + "-" + pair.Name + (textured ? "-texture" : "-solid")
                + "-t" + transportFault + "-l" + lightingFault + suffix;
            var backend = UnityStubs.Rendering.CameraTextureRenderer.Current;
            Require(backend != null && backend.GetType().FullName == "RimWorldWaylith.Rendering.CameraTextureBaker", "Approved GPU backend unavailable");
            FieldInfo serial = backend.GetType().GetField("_frameNumber", BindingFlags.Instance | BindingFlags.NonPublic);
            int before = (int)serial.GetValue(backend);
            var owner = new GameObject("IsoRim normal framebuffer proof");
            Camera camera = owner.AddComponent<Camera>();
            camera.enabled = false; camera.orthographic = true; camera.aspect = 4; camera.cullingMask = 0;
            Vector3 rasterG = pair.G;
            camera.transform.rotation = Quaternion.LookRotation(-rasterG, Mathf.Abs(rasterG.y) > .9f ? Vector3.forward : Vector3.up);
            camera.transform.position = rasterG * 10;
            // Freeze a physical-space camera. The view matrix cancels only the artificial Y expansion.
            camera.worldToCameraMatrix = camera.worldToCameraMatrix * Matrix4x4.Scale(new Vector3(1, View3D.Squash, 1));
            Vector3[] referencePositions = sub.mesh.vertices;
            if (cutaway)
                referencePositions = referencePositions.Select(p => new Vector3(p.x,
                    (.06f + (p.y * View3D.Squash - .06f) * (2f - .06f) / (4f - .06f)) / View3D.Squash, p.z)).ToArray();
            Vector3 centroid = referencePositions.Aggregate(Vector3.zero, (sum, v) => sum + v) / 3;
            camera.orthographicSize = referencePositions.Max(v => Vector3.Scale(v - centroid, new Vector3(1, View3D.Squash, 1)).magnitude) * 1.25f;
            camera.clearFlags = CameraClearFlags.SolidColor; camera.backgroundColor = Color.magenta;
            RenderTexture target = RenderTexture.GetTemporary(width, height, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            camera.targetTexture = target;
            Require(sub.material.GetFloat("_NormalSpace") == 1, "The framebuffer received a nonstatic normal-space material");
            Material actual = new Material(sub.material);
            actual.SetFloat("_NormalSpace", legacy ? 0 : 1);
            actual.SetFloat("_LightMode", 1); actual.SetFloat("_GlowMode", 0); actual.SetFloat("_MarkPass", 0);
            actual.SetFloat("_NormalView", diagnostic ? 1 : 0);
            actual.SetFloat("_NormalTransportFault", transportFault); actual.SetFloat("_NormalLightingFault", lightingFault);
            actual.SetVector("_IsoSun", new Vector4(pair.L.x, pair.L.y, pair.L.z, 0));
            actual.SetVector("_IsoView", new Vector4(pair.G.x, pair.G.y, pair.G.z, View3D.Squash));
            actual.SetVector("_IsoSky", Vector4.one); actual.SetVector("_IsoCellMap", Vector4.zero);
            Require(actual.GetFloat("_NormalView") == (diagnostic ? 1 : 0) && actual.GetFloat("_NormalTransportFault") == transportFault
                && actual.GetFloat("_NormalLightingFault") == lightingFault && actual.GetFloat("_LightMode") == 1, "Requested shader state was not observed");
            Mesh reference = new Mesh { vertices = referencePositions, triangles = sub.mesh.triangles, colors32 = Enumerable.Repeat(new Color32(255, 255, 255, 255), 3).ToArray() };
            reference.RecalculateBounds();
            double[] referenceShades = ownCell ? new[] { pair.Shades[0] * 64 / 255, pair.Shades[0] * 192 / 255, pair.Shades[0] } : pair.Shades;
            Material[] refs = new Material[3];
            Vector3[] directions = { pair.N, pair.G, pair.Q };
            for (int i = 0; i < 3; i++)
            {
                refs[i] = new Material(R3DMaterials.Solid);
                refs[i].SetFloat("_LightMode", 0); refs[i].SetFloat("_GlowMode", 0); refs[i].SetFloat("_NormalView", 0);
                refs[i].SetVector("_IsoCellMap", Vector4.zero);
                refs[i].color = diagnostic ? new Color(directions[i].x * .5f + .5f, directions[i].y * .5f + .5f, directions[i].z * .5f + .5f)
                    : new Color((float)(.6 * referenceShades[i]), (float)(.6 * referenceShades[i]), (float)(.6 * referenceShades[i]));
            }
            Vector3 axis = camera.transform.right;
            Vector3 rightAxis = new Vector3(axis.x, axis.y / View3D.Squash, axis.z);
            Matrix4x4[] placements = Enumerable.Range(0, 4).Select(i => Matrix4x4.Translate(-centroid + rightAxis * ((i - 1.5f) * camera.orthographicSize * 2))).ToArray();
            Texture2D controlTexture = null;
            if (ownCell || cutaway)
            {
                controlTexture = new Texture2D(ownCell ? 2 : 1, 1, TextureFormat.RGBA32, false, true);
                controlTexture.filterMode = FilterMode.Point; controlTexture.wrapMode = TextureWrapMode.Clamp;
                controlTexture.SetPixels32(ownCell ? new[] { new Color32(64, 64, 64, 0), new Color32(192, 192, 192, 0) } : new[] { new Color32(64, 0, 255, 0) });
                controlTexture.Apply();
                if (ownCell)
                {
                    actual.SetTexture("_IsoGlowTex", controlTexture); actual.SetFloat("_GlowMode", 3);
                    actual.SetVector("_IsoSky", Vector4.zero);
                    Vector3 origin = placements[0].MultiplyPoint3x4(centroid);
                    actual.SetVector("_IsoGlowMap", new Vector4(origin.x + .5f - .2f, 0, 2, 0));
                }
                else
                {
                    actual.SetTexture("_IsoCellTex", controlTexture);
                    actual.SetVector("_IsoCellMap", new Vector4(4, 5, 1, 1));
                    actual.SetVector("_IsoLevel", new Vector4(0, .5f, 0, 0));
                }
            }
            Camera.CameraCallback draw = active =>
            {
                if (active != camera) return;
                Require(actual.SetPass(0), "Owned actual shader did not compile/bind");
                Graphics.DrawMeshNow(sub.mesh, placements[0]);
                for (int i = 0; i < 3; i++)
                {
                    Require(refs[i].SetPass(0), "Owned reference shader did not compile/bind");
                    Graphics.DrawMeshNow(reference, placements[i + 1]);
                }
            };
            RenderTexture previous = RenderTexture.active;
            Texture2D image = null;
            try
            {
                Camera.onPreRender += draw; camera.Render(); Camera.onPreRender -= draw;
                int completed = (int)serial.GetValue(backend);
                Require(completed == before + 1, "No unique completed GPU frame");
                Require(backend.TryReadPixels(target, out byte[] raw) && raw.Length == width * height * 4, "Actual GPU readback failed");
                RenderTexture.active = target;
                image = new Texture2D(width, height, TextureFormat.RGBA32, false);
                image.ReadPixels(new Rect(0, 0, width, height), 0, 0); image.Apply();
                byte[] png = image.EncodeToPNG();
                File.WriteAllBytes(Path.Combine(evidence, name + ".rgba"), raw);
                File.WriteAllBytes(Path.Combine(evidence, name + ".png"), png);
                Color32[] pixels = image.GetPixels32();
                int[][][] patches = Enumerable.Range(0, 4).Select(slot => Enumerable.Range(-1, 3).SelectMany(dy =>
                    Enumerable.Range(-1, 3).Select(dx => RGBA(pixels[(height / 2 + dy) * width + slot * 256 + 128 + dx]))).ToArray()).ToArray();
                Require(patches.All(patch => patch.All(p => p[3] == 255 && !(p[0] == 255 && p[1] == 0 && p[2] == 255))), "Empty, clipped or nonopaque patch");
                if (!diagnostic)
                    for (int i = 1; i < 4; i++) for (int j = i + 1; j < 4; j++)
                        Require(Enumerable.Range(0, 3).All(c => Math.Abs(patches[i][0][c] - patches[j][0][c]) >= 12), "Production references do not separate by 12/255 in every RGB channel");
                int[] differences = Enumerable.Range(1, 3).Select(i => Difference(patches[0], patches[i])).ToArray();
                var row = new
                {
                    name,
                    width,
                    height,
                    backend = backend.GetType().FullName,
                    gpuFrameBefore = before,
                    gpuFrameCompleted = completed,
                    unityFrame = Time.frameCount,
                    shader = actual.shader.name,
                    textured,
                    diagnostic,
                    transportFault,
                    lightingFault,
                    observedDiagnostic = actual.GetFloat("_NormalView"),
                    observedTransportFault = actual.GetFloat("_NormalTransportFault"),
                    observedLightingFault = actual.GetFloat("_NormalLightingFault"),
                    lightMode = actual.GetFloat("_LightMode"),
                    normalSpace = actual.GetFloat("_NormalSpace"),
                    glowMode = actual.GetFloat("_GlowMode"),
                    sky = V(actual.GetVector("_IsoSky")),
                    textureName = actual.mainTexture != null ? actual.mainTexture.name : null,
                    textureWidth = actual.mainTexture != null ? actual.mainTexture.width : 0,
                    textureHeight = actual.mainTexture != null ? actual.mainTexture.height : 0,
                    squash = View3D.Squash,
                    expectedN = V(pair.N),
                    expectedG = V(pair.G),
                    expectedBinN = V(pair.Q),
                    sun = V(pair.L),
                    expectedShades = referenceShades,
                    ownCell,
                    cutaway,
                    cameraView = camera.worldToCameraMatrix.ToString(),
                    cameraProjection = camera.projectionMatrix.ToString(),
                    baseRgb = .6,
                    patches,
                    differences,
                    expectedIndex,
                    authoredComparisonPassed = differences[0] <= 2,
                    rawPath = name + ".rgba",
                    rawHash = Convert.ToHexString(SHA256.HashData(raw)),
                    pngPath = name + ".png",
                    pngHash = Convert.ToHexString(SHA256.HashData(png))
                };
                File.WriteAllText(Path.Combine(evidence, name + ".json"), JsonSerializer.Serialize(row, Json));
                Require(differences[expectedIndex] <= 2, name + " mismatched its expected framebuffer reference: " + string.Join(",", differences));
                if (expectedIndex != 0) Require(differences[0] > 2, "Fault or legacy control silently passed the authored oracle");
                return row;
            }
            finally
            {
                Camera.onPreRender -= draw; RenderTexture.active = previous; camera.targetTexture = null;
                RenderTexture.ReleaseTemporary(target);
                if (image != null) UnityEngine.Object.Destroy(image);
                if (controlTexture != null) UnityEngine.Object.Destroy(controlTexture);
                UnityEngine.Object.Destroy(reference); UnityEngine.Object.Destroy(actual);
                foreach (Material material in refs) UnityEngine.Object.Destroy(material);
                UnityEngine.Object.Destroy(owner);
            }
        }
    }
}
