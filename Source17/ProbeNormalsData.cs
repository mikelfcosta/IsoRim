using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace IsoRim
{
    internal static class NormalDataCases
    {
        private static readonly Vector3[] CornerNormals = { new Vector3(0, 0.6f, 0.8f), new Vector3(0.6f, 0, 0.8f), Vector3.forward };
        internal static void Run(string id, string output)
        {
            switch (id)
            {
                case "NormalDataCases.AuthoredIndices":
                    AuthoredIndices(output);
                    break;
                case "NormalDataCases.InvalidTriangleFallback":
                    InvalidTriangleFallback(output);
                    break;
                case "NormalDataCases.GeometricChannelsPreserved":
                    GeometricChannelsPreserved(output);
                    break;
                case "NormalDataCases.DerivedModels":
                    DerivedModels(output);
                    break;
                default:
                    throw new InvalidOperationException("Unknown normal case: " + id);
            }
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        private static Model Load(string output, string name, string text)
        {
            string path = Path.Combine(output, name + ".obj");
            File.WriteAllText(path, text);
            Model model = ObjLoader.Load(path);
            Require(model != null, "The fixture did not load");
            return model;
        }

        private static Vector3[] Shading(Model model)
        {
            // Reflection lets the witness run red against the unmodified model contract.
            var field = typeof(Model).GetField("shadingNormals");
            Require(field != null, "The loader drops authored normals");
            return (Vector3[])field.GetValue(model);
        }

        private static void Equal(Vector3 actual, Vector3 expected, string message)
        {
            Require((actual - expected).sqrMagnitude < 0.000001f, message + ": " + actual + " != " + expected);
        }

        private static void AuthoredIndices(string output)
        {
            // One cell per unit. Ground is y=0. Pivot is the origin. Z is reflected on import.
            string vertices = "v 0 0 0\nv 2 0 0\nv 0 1 0\nvt 0 0\nvt 1 0\nvt 0 1\nvn 0 0 -1\nvn 0 0.6 -0.8\nvn 0.6 0 -0.8\n";
            foreach (string face in new[] { "f 1/1/3 2/2/1 3/3/2\n", "f 1//-1 2//-3 3//-2\n" })
            {
                Model model = Load(output, "indices", vertices + face);
                Vector3[] shading = Shading(model);
                Equal(shading[0], new Vector3(0.6f, 0, 0.8f), "First corner lost its normal index");
                Equal(shading[1], Vector3.forward, "Second corner lost its normal index");
                Equal(shading[2], new Vector3(0, 0.6f, 0.8f), "Third corner lost its normal index");
            }
            // This closed tetrahedron has negative volume after Z reflection. Its outward
            // authored normals already face the repaired orientation and must not be negated.
            string tetra = "v 0 0 0\nv 2 0 0\nv 0 1 0\nv 0 0 3\nvn 0 0 -1\nvn 0 0.6 -0.8\nvn 0.6 0 -0.8\n"
                + "f 1//1 3//2 2//3\nf 1//1 2//1 4//1\nf 1//1 4//1 3//1\nf 2//1 3//1 4//1\n";
            Model repaired = Load(output, "winding", tetra);
            Equal(repaired.positions[1], new Vector3(2, 0, 0), "The winding control did not repair");
            Equal(Shading(repaired)[1], new Vector3(0.6f, 0, 0.8f), "Winding repair lost or negated an outward authored normal");
            Model inward = Load(output, "inward", "v 0 0 0\nv 2 0 0\nv 0 1 0\nvn 0 -0.6 0.8\nf 1//1 2//1 3//1\n");
            Equal(Shading(inward)[0], new Vector3(0, 0.6f, 0.8f), "An authored normal in the opposite hemisphere was not oriented to the final face");
        }

        private static void InvalidTriangleFallback(string output)
        {
            string vertices = "v 0 0 0\nv 2 0 0\nv 0 1 0\nvn 0 0.6 -0.8\n";
            foreach (string invalid in new[] { "vn 0 0 0\n", "vn NaN 0 -1\n", "vn 0 Infinity -1\n", "vn broken 0 -1\n", "vn 0 1\n" })
            {
                Model model = Load(output, "invalid-vector", vertices + invalid + "f 1//1 2//2 3//1\n");
                TriangleFallback(model, "Invalid vector retained a partially authored triangle");
            }
            foreach (string corner in new[] { "2", "2//0", "2//99", "2//-2", "2//broken" })
            {
                Model model = Load(output, "invalid-index", vertices + "f 1//1 " + corner + " 3//1\n");
                TriangleFallback(model, "Missing or invalid index retained a partially authored triangle");
            }
            Model missing = Load(output, "missing", "v 0 0 0\nv 2 0 0\nv 0 1 0\nf 1 2 3\n");
            TriangleFallback(missing, "Missing normals became authored up defaults");
            // A bad triangle must not discard the independent valid triangle after it.
            Model mixed = Load(output, "mixed", vertices + "f 1//1 2 3//1\nf 1//1 2//1 3//1\n");
            TriangleFallback(mixed, "Partial triangle retained authored data");
            for (int i = 3; i < 6; i++)
            {
                Require(mixed.authoredNormals[i], "An invalid triangle discarded the next valid triangle");
                Equal(Shading(mixed)[i], new Vector3(0, 0.6f, 0.8f), "The valid triangle lost its authored normal");
            }
        }

        private static void TriangleFallback(Model model, string message)
        {
            for (int i = 0; i < 3; i++)
            {
                Require(!model.authoredNormals[i], message);
                Equal(Shading(model)[i], Vector3.forward, "Fallback is not the geometric face normal");
                Equal(model.normals[i], Vector3.forward, "Fallback overwrote the geometric stream");
            }
        }

        private static Model ChannelsFixture(string output, bool authored)
        {
            File.WriteAllText(Path.Combine(output, "channels.mtl"), "newmtl plain\nKd 0.2 0.3 0.4\nnewmtl stuff_metal\nKd 1 1 1\nnewmtl textured\nKd 0.5 0.6 0.7\nmap_Kd probe.png\nnewmtl glow\nKd 0.7 0.3 0.1\n");
            // Four independent asymmetric triangles. Ground y=0, pivot at origin, one cell/unit.
            string text = "mtllib channels.mtl\nv 0 0 0 0.1 0.2 0.3\nv 2 0 0\nv 0 1 0\nvt 0.1 0.2\nvt 0.8 0.2\nvt 0.1 0.9\n";
            if (authored)
            {
                text += "vn 0 0.6 -0.8\nvn 0.6 0 -0.8\nvn 0 0 -1\n";
            }
            foreach (string material in new[] { "plain", "stuff_metal", "textured", "glow" })
            {
                text += "usemtl " + material + "\nf 1/1" + (authored ? "/1" : "") + " 2/2" + (authored ? "/2" : "") + " 3/3" + (authored ? "/3" : "") + "\n";
            }
            return Load(output, authored ? "channels-authored" : "channels-geometric", text);
        }

        private static void GeometricChannelsPreserved(string output)
        {
            Model model = ChannelsFixture(output, true);
            Model control = ChannelsFixture(output, false);
            Require(model.positions.Length == 12, "The fixture lost triangles");
            for (int i = 0; i < 12; i++)
            {
                Equal(model.positions[i], control.positions[i], "Normal import moved a corner");
                Equal(model.normals[i], Vector3.forward, "Authored data overwrote the geometric normal");
                Equal(model.hull[i], control.hull[i], "The hull used shading normals");
                Require(model.uvs[i] == control.uvs[i], "Normal import changed UVs");
                Require(model.colors[i] == control.colors[i], "Normal import changed colours");
                Require(model.tinted[i] == control.tinted[i], "Normal import changed tint flags");
                Require(model.material[i] == control.material[i], "Normal import changed material slots");
                Equal(Shading(model)[i], CornerNormals[i % 3], "The geometric control did not exercise authored data");
            }
            Require(model.maxY == 1 && model.halfDepth == 0 && model.minX == 0 && model.maxX == 2 && model.minZ == 0 && model.maxZ == 0,
                "Normal import changed the model bounds");
            Require(model.materials.Length == control.materials.Length, "Normal import changed the materials");
            for (int i = 0; i < model.materials.Length; i++)
            {
                ModelMaterial actual = model.materials[i], expected = control.materials[i];
                Require(actual.name == expected.name && actual.color == expected.color && actual.tinted == expected.tinted
                    && actual.glow == expected.glow && actual.texture == expected.texture, "Normal import changed material data");
            }
        }

        private static void DerivedModels(string output)
        {
            Model source = ObjLoader.Merge(new List<Model> { ChannelsFixture(output, true), ChannelsFixture(output, false) });
            Vector3[] original = (Vector3[])Shading(source).Clone();
            Color ignored;
            Model[] copies = {
                ObjLoader.Relit(source, "plain", new List<Color> { Color.red, Color.blue }),
                ObjLoader.Marked(source, new List<string> { "plain" }, out ignored),
                ObjLoader.Glowing(source, new List<string> { "plain" }, out ignored),
                ObjLoader.Weathered(source, new Vector3(0.2f, 0.3f, 0.4f), null),
                ObjLoader.WithoutSmallOutlines(source, 0.1f)
            };
            foreach (Model copy in copies)
            {
                Require(copy != null && copy.shadingNormals != null && copy.authoredNormals != null, "A derived model dropped authored data");
                Require(copy.shadingNormals.Length == source.positions.Length && copy.authoredNormals.Length == source.positions.Length,
                    "A derived stream lost corner alignment");
                for (int i = 0; i < source.positions.Length; i++)
                {
                    Equal(copy.shadingNormals[i], i < 12 ? CornerNormals[i % 3] : Vector3.forward, "A derived model changed a shading corner");
                    Require(copy.authoredNormals[i] == (i < 12), "A derived model discarded authored validity");
                    Equal(source.shadingNormals[i], original[i], "A copy mutated its source");
                }
            }
            Model box = ObjLoader.Box(new Vector3(-1, 0, -0.5f), new Vector3(1, 2, 0.5f));
            Require(box.shadingNormals == null && box.authoredNormals == null, "A synthetic box invented authored normals");
            Model control = ChannelsFixture(output, false);
            List<Model> parts = new List<Model> { source, box, control, source };
            Model merged = ObjLoader.Merge(parts);
            int offset = 0;
            foreach (Model part in parts)
            {
                for (int i = 0; i < part.positions.Length; i++)
                {
                    bool authored = part == source && i < 12;
                    Require(merged.authoredNormals[offset + i] == authored, "Merge shifted authored validity");
                    Equal(merged.shadingNormals[offset + i], authored ? CornerNormals[i % 3] : part.normals[i], "Merge shifted a shading corner");
                    Equal(merged.normals[offset + i], part.normals[i], "Merge changed a geometric corner");
                }
                offset += part.positions.Length;
            }
            Require(merged.shadingNormals.Length == offset && merged.authoredNormals.Length == offset, "Merge lost stream alignment");
        }
    }
}
