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
    internal static class AssetIntakeCases
    {
        private static readonly BindingFlags Hidden = BindingFlags.Static | BindingFlags.NonPublic;
        private static readonly List<object> reads = new List<object>();

        internal static string Run(string id, string evidence)
        {
            reads.Clear();
            switch (id)
            {
                case "AssetIntakeCases.InternalPreference":
                    InternalPreference();
                    break;
                case "AssetIntakeCases.DefinitionRoot":
                    DefinitionRoot();
                    break;
                case "AssetIntakeCases.AdjacentMaterials":
                    AdjacentMaterials();
                    break;
                case "AssetIntakeCases.FolderDiscoveryFreeze":
                    FolderDiscoveryFreeze(evidence);
                    break;
                case "AssetIntakeCases.PreparedValidation":
                    PreparedValidation(evidence);
                    break;
                default:
                    throw new InvalidOperationException("Unknown intake case: " + id);
            }
            string path = Path.Combine(evidence, id + ".json");
            File.WriteAllText(path, JsonSerializer.Serialize(reads));
            return path;
        }

        private static void Require(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        private static void Selected(string reader, Model model, string path, int width)
        {
            Require(model != null && model.maxX == width, reader + " selected the wrong geometry: " + path);
            Require(object.ReferenceEquals(model, ModelLibrary.LoadFile(path)), reader + " did not cache by the selected absolute path");
            reads.Add(new { reader, selectedFile = path, model.id, model.maxX });
        }

        private static void InternalPreference()
        {
            string originalRoot = IsoRimMod.RootDir;
            var saved = new Dictionary<FieldInfo, object>();
            foreach (Type type in new[] { typeof(ShipRoofs), typeof(Turbines), typeof(Doors), typeof(Effects) })
            {
                foreach (FieldInfo field in type.GetFields(Hidden))
                {
                    if (field.FieldType == typeof(Model) || field.Name == "tried" || field.Name == "loaded")
                    {
                        saved[field] = field.GetValue(null);
                    }
                }
            }
            bool models = IsoRimMod.Settings.models3D, effects = IsoRimMod.Settings.effects3D;
            try
            {
                IsoRimMod.Settings.models3D = IsoRimMod.Settings.effects3D = true;
                foreach (bool preferred in new[] { true, false })
                {
                    string root = Path.Combine(originalRoot, "1.7", "Models", "Diagnostics", preferred ? "InternalPreferred" : "InternalFallback");
                    IsoRimMod.RootDir = root;
                    string directory = Path.Combine(root, preferred ? "1.7/Models" : "Models");
                    int width = preferred ? 2 : 3;
                    foreach (FieldInfo field in saved.Keys)
                    {
                        if (field.FieldType == typeof(bool))
                        {
                            field.SetValue(null, false);
                        }
                    }
                    Selected("People.Part", People.Part("probe"), Path.Combine(directory, "probe.obj"), width);
                    Require(HairParts.Exists("hair_probe"), "HairParts.Exists bypassed the preferred-only or fallback hair file");
                    reads.Add(new { reader = "HairParts.Exists", selectedFile = Path.Combine(directory, "hair_probe.obj") });
                    Require(HairParts.Exists("hair_version_only") == preferred, "Hair availability cache is not keyed by the resolved absolute file");
                    reads.Add(new { reader = "HairParts.Exists version-only", exists = preferred, selectedFile = preferred ? Path.Combine(directory, "hair_version_only.obj") : null });
                    // The packet names ShipRoofs.Load. Core-only acceptance does not enter Odyssey rendering.
                    typeof(ShipRoofs).GetMethod("Load", Hidden).Invoke(null, null);
                    foreach (var piece in new[] { ("square", "roof_square.obj"), ("triangle", "roof_triangle.obj"), ("rim", "roof_rim.obj"), ("rim90", "roof_rim90.obj"), ("rimDiagonal", "roof_rim_diagonal.obj") })
                    {
                        Selected("ShipRoofs.Load " + piece.Item1, (Model)typeof(ShipRoofs).GetField(piece.Item1, Hidden).GetValue(null), Path.Combine(directory, piece.Item2), width);
                    }
                    var turbine = ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("WindTurbine")) as ThingWithComps;
                    Turbines.Draw(turbine.GetComp<CompPowerTrader>());
                    Selected("Turbines.Draw", (Model)typeof(Turbines).GetField("rotor", Hidden).GetValue(null), Path.Combine(directory, "wind_rotor.obj"), width);
                    Doors.Draw((Building_Door)ThingMaker.MakeThing(ThingDefOf.Door), 0);
                    foreach (var piece in new[] { ("leaf", "door_leaf.obj"), ("lintel", "door_lintel.obj"), ("gateLeaf", "gate_leaf.obj"), ("gatePosts", "gate_posts.obj") })
                    {
                        Selected("Doors.Draw " + piece.Item1, (Model)typeof(Doors).GetField(piece.Item1, Hidden).GetValue(null), Path.Combine(directory, piece.Item2), width);
                    }
                    Effects.DrawAll();
                    foreach (var piece in new[] { ("flame", "fx_flame.obj"), ("flameCore", "fx_flame_core.obj"), ("puff", "fx_puff.obj") })
                    {
                        Selected("Effects.DrawAll " + piece.Item1, (Model)typeof(Effects).GetField(piece.Item1, Hidden).GetValue(null), Path.Combine(directory, piece.Item2), width);
                    }
                }
            }
            finally
            {
                IsoRimMod.RootDir = originalRoot;
                IsoRimMod.Settings.models3D = models;
                IsoRimMod.Settings.effects3D = effects;
                foreach (var pair in saved)
                {
                    pair.Key.SetValue(null, pair.Value);
                }
            }
        }

        private static void DefinitionRoot()
        {
            foreach (string prefix in new[] { "Internal", "External" })
            {
                ModelDef def = DefDatabase<ModelDef>.GetNamed("NormalProof" + prefix + "Model");
                ThingDef thing = DefDatabase<ThingDef>.GetNamed("NormalProof" + prefix + "Thing");
                Require(thing.statBases != null && thing.statBases.Any(stat => stat.stat == StatDefOf.Mass && stat.value == 1f), "The diagnostic item has no authored mass");
                ModelEntry entry;
                Require(ModelLibrary.TryGet(thing, out entry) && entry.def == def, "The fixture binding was not discovered: " + prefix);
                string owner = def.modContentPack.RootDir;
                int width = prefix == "External" ? 7 : 2;
                Model model = entry.variants[0];
                Require(model.positions[1].x == width, "A definition model escaped its defining root: " + owner);
                if (prefix == "External")
                {
                    Require(owner != IsoRimMod.RootDir && def.modContentPack.PackageId == "isorim.normalproof.external", "External definition ownership changed");
                    Require(model.positions.Length == 6 && model.positions[4].x == 8, "Definition parts escaped their defining root");
                }
                WornGearDef gear = DefDatabase<WornGearDef>.GetNamed("NormalProof" + prefix + "Gear");
                Require(gear.modContentPack == def.modContentPack, "Model and gear fixture owners differ");
                Color tint;
                Selected(prefix + " gear", WornGear.ModelFor(gear, null, out tint), Path.Combine(owner, gear.model), width);
                reads.Add(new
                {
                    definition = def.defName,
                    fileName = def.fileName,
                    gearFileName = gear.fileName,
                    owner,
                    packageId = def.modContentPack.PackageId,
                    selectedFile = Path.Combine(owner, def.model),
                    parts = def.parts,
                    firstCornerX = model.positions[1].x
                });
            }
        }

        private static void AdjacentMaterials()
        {
            string owner = DefDatabase<ModelDef>.GetNamed("NormalProofExternalModel").modContentPack.RootDir;
            string directory = Path.Combine(owner, "1.7/Models/Diagnostics");
            Model selected = ObjLoader.Load(Path.Combine(directory, "shared.obj"));
            Color color = selected.materials[selected.material[0]].color;
            Require(Mathf.Abs(color.r - 0.2f) < 0.00001f && Mathf.Abs(color.g - 0.3f) < 0.00001f && Mathf.Abs(color.b - 0.4f) < 0.00001f,
                "The selected OBJ borrowed a material library from another directory");
            reads.Add(new { selectedFile = Path.Combine(directory, "shared.obj"), materialLibrary = Path.Combine(directory, "sample.mtl"), color = new[] { color.r, color.g, color.b } });
            var diagnostics = new List<string>();
            Application.LogCallback capture = (message, stack, type) => diagnostics.Add(message);
            Application.logMessageReceived += capture;
            try
            {
                Require(ObjLoader.Load(Path.Combine(directory, "absent.obj")) == null, "Missing OBJ did not fail");
                ObjLoader.Load(Path.Combine(directory, "missing-material.obj"));
                ObjLoader.Load(Path.Combine(directory, "malformed.obj"));
            }
            finally
            {
                Application.logMessageReceived -= capture;
            }
            Require(diagnostics.Any(line => line.Contains("Model file not found") && line.Contains("absent.obj")), "Missing OBJ has no diagnostic");
            Require(diagnostics.Any(line => line.Contains("Material file not found") && line.Contains("absent.mtl")), "Missing adjacent library has no diagnostic");
            Require(diagnostics.Any(line => line.Contains("Malformed material") && line.Contains("malformed.mtl")), "Malformed adjacent library has no diagnostic");
            reads.Add(new { diagnostics });
        }

        private static void FolderDiscoveryFreeze(string evidence)
        {
            ModContentPack pack = LoadedModManager.RunningModsListForReading.Single(mod => mod.PackageId == "connor.isorim");
            Require(!File.Exists(Path.Combine(pack.RootDir, "LoadFolders.xml")), "The inactive About configuration was migrated to the mod root");
            using (JsonDocument expected = JsonDocument.Parse(File.ReadAllText(Path.Combine(evidence, "intake-frozen-inputs.json"))))
            {
                Require(expected.RootElement.GetProperty("nonce").GetString() == new DirectoryInfo(evidence).Name, "The frozen-input descriptor is stale");
                foreach (JsonProperty file in expected.RootElement.GetProperty("files").EnumerateObject())
                {
                    string path = Path.Combine(pack.RootDir, file.Name);
                    Require(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))) == file.Value.GetString(), "A frozen effective input changed: " + path);
                    reads.Add(new { frozenFile = path, sha256 = file.Value.GetString() });
                }
                var modelNames = new HashSet<string>(DefDatabase<ModelDef>.AllDefs.Where(def => def.modContentPack == pack && !def.defName.StartsWith("NormalProof")).Select(def => def.defName));
                var gearNames = new HashSet<string>(DefDatabase<WornGearDef>.AllDefs.Where(def => def.modContentPack == pack && !def.defName.StartsWith("NormalProof")).Select(def => def.defName));
                Require(modelNames.SetEquals(expected.RootElement.GetProperty("models").EnumerateArray().Select(item => item.GetString())), "The frozen loaded ModelDef set changed");
                Require(gearNames.SetEquals(expected.RootElement.GetProperty("gear").EnumerateArray().Select(item => item.GetString())), "The frozen loaded WornGearDef set changed");
                reads.Add(new { loadedModelDefs = modelNames.OrderBy(name => name).ToArray(), loadedGearDefs = gearNames.OrderBy(name => name).ToArray() });
                reads.Add(new
                {
                    definitionOrigins = pack.AllDefs.Where(def => def is ModelDef || def is WornGearDef)
                    .Select(def => new { def.defName, def.fileName, owner = def.modContentPack.RootDir }).ToArray()
                });
            }
            ModContentPack external = DefDatabase<ModelDef>.GetNamed("NormalProofExternalModel").modContentPack;
            Require(File.ReadAllText(Path.Combine(external.RootDir, "About/LoadFolders.xml")).Contains("IgnoredAboutFolder"), "The inactive-carrier counterfactual is missing");
            foreach (ModContentPack mod in new[] { pack, external })
            {
                var folders = ((IEnumerable<string>)typeof(ModContentPack).GetField("foldersToLoadDescendingOrder", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(mod))
                    .Select(folder => Path.GetFullPath(folder).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)).ToArray();
                Require(folders.Contains(Path.GetFullPath(mod.RootDir)) && folders.Contains(Path.GetFullPath(Path.Combine(mod.RootDir, "1.7"))), "Default root/version discovery failed");
                Require(!folders.Any(folder => folder.Contains("IgnoredAboutFolder") || folder.EndsWith("About")), "The About carrier became active");
                reads.Add(new { packageId = mod.PackageId, owner = mod.RootDir, actualFolders = folders });
            }
        }

        private static void PreparedValidation(string evidence)
        {
            string path = Path.Combine(evidence, "validator-results.json");
            using (JsonDocument report = JsonDocument.Parse(File.ReadAllText(path)))
            {
                Require(report.RootElement.GetProperty("nonce").GetString() == new DirectoryInfo(evidence).Name, "The validator results are stale");
                JsonElement controls = report.RootElement.GetProperty("controls");
                Require(controls.GetArrayLength() == 9, "The validator omitted positive or invalid controls");
                foreach (JsonElement control in controls.EnumerateArray())
                {
                    Require((control.GetProperty("exit").GetInt32() == 0) == control.GetProperty("expectedSuccess").GetBoolean()
                        && control.GetProperty("diagnosticMatched").GetBoolean(), "The staged validator accepted an invalid input or rejected the passing control: " + control.GetProperty("name").GetString());
                }
                var checkedPaths = controls[0].GetProperty("checkedPaths").EnumerateArray().Select(item => item.GetString()).ToArray();
                Require(checkedPaths.Contains("1.7/Defs/Diagnostics/WornGear.xml") && checkedPaths.Contains("1.7/Models/Diagnostics/internal.obj")
                    && checkedPaths.Contains("1.7/Models/Diagnostics/legacy.obj"), "The passing validator did not read the fixture and geometric fallback");
                reads.Add(new { validatorReport = path, controls = JsonSerializer.Deserialize<object>(controls.GetRawText()) });
            }
            string directory = Path.Combine(IsoRimMod.RootDir, "1.7/Models/Diagnostics");
            Model authored = ObjLoader.Load(Path.Combine(directory, "internal.obj"));
            Model fallback = ObjLoader.Load(Path.Combine(directory, "legacy.obj"));
            Require(authored.positions.Length == 3 && fallback.positions.Length == 3
                && authored.authoredNormals.Length == 3 && fallback.authoredNormals.Length == 3
                && authored.authoredNormals.All(value => value) && fallback.authoredNormals.All(value => !value), "Authored validity or missing-normal fallback changed");
            Require(fallback.shadingNormals.All(normal => (normal - Vector3.forward).sqrMagnitude < 0.000001f), "Missing normals do not use geometric fallback");
            reads.Add(new { authoredFile = Path.Combine(directory, "internal.obj"), fallbackFile = Path.Combine(directory, "legacy.obj") });
        }
    }
}
