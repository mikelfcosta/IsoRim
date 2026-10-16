using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
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
        // Independent literal matrix oracle, separate from generated fixture metadata.
        private const string RecoveryExpected = @"R01-unit|0|A|-
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
R29-valid|0|A|-";

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
                string[] expectedControls = { "passing", "passing-no-vn", "missing-gear", "missing-model", "missing-mtl", "malformed-mtl",
                    "zero-normal", "nonfinite-normal", "float-overflow-normal", "float-underflow-normal", "magnitude-overflow-normal",
                    "small-magnitude-normal", "normal-index", "position-index", "empty-obj", "vertices-only-obj",
                    "normalize-below", "normalize-at", "normalize-above", "normalize-next-below", "normalize-next-above",
                    "normalize-diagonal-below", "normalize-diagonal-above" };
                string[] successControls = { "passing", "passing-no-vn", "normalize-above", "normalize-next-above", "normalize-diagonal-above" };
                Require(controls.GetArrayLength() == expectedControls.Length
                    && expectedControls.All(name => controls.EnumerateArray().Count(control => control.GetProperty("name").GetString() == name) == 1),
                    "The validator omitted or duplicated positive or invalid controls");
                foreach (JsonElement control in controls.EnumerateArray())
                {
                    bool expectedSuccess = successControls.Contains(control.GetProperty("name").GetString());
                    Require(control.GetProperty("expectedSuccess").GetBoolean() == expectedSuccess
                        && control.GetProperty("exit").GetInt32() == (expectedSuccess ? 0 : 1)
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
            string external = DefDatabase<ModelDef>.GetNamed("NormalProofExternalModel").modContentPack.RootDir;
            foreach (var boundary in new[] { ("normalize-below", false, Vector3.forward), ("normalize-at", false, Vector3.forward),
                ("normalize-above", true, Vector3.right), ("normalize-next-below", false, Vector3.forward),
                ("normalize-next-above", true, Vector3.right), ("normalize-diagonal-below", false, Vector3.forward),
                ("normalize-diagonal-above", true, new Vector3(0.70710677f, 0.70710677f, 0f)) })
            {
                string file = Path.Combine(external, "1.7/Models/Diagnostics", boundary.Item1 + ".obj");
                Model model = ObjLoader.Load(file);
                Require(model != null && model.positions.Length == 3 && model.authoredNormals.Length == 3
                    && model.authoredNormals.All(value => value == boundary.Item2), "Loader authored validity differs at " + boundary.Item1);
                Require(model.shadingNormals.All(normal => (normal - boundary.Item3).sqrMagnitude < 0.000001f)
                    && model.normals.All(normal => (normal - Vector3.forward).sqrMagnitude < 0.000001f),
                    "Loader normalization or geometric fallback differs at " + boundary.Item1);
                reads.Add(new
                {
                    numericBoundary = boundary.Item1,
                    selectedFile = file,
                    objSource = File.ReadAllText(file),
                    expectedAuthored = boundary.Item2,
                    model.authoredNormals,
                    shadingNormals = model.shadingNormals.Select(normal => new[] { normal.x, normal.y, normal.z }).ToArray()
                });
            }
            RecoveryValidation(evidence, external);
        }

        private static string Bits(float value) => unchecked((uint)BitConverter.SingleToInt32Bits(value)).ToString("X8");

        private static void RecoveryValidation(string evidence, string external)
        {
            var expected = RecoveryExpected.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Trim().Split('|')).ToDictionary(parts => parts[0], StringComparer.Ordinal);
            string root = Path.Combine(external, "1.7/Models/Diagnostics/Recovery");
            using JsonDocument ledger = JsonDocument.Parse(File.ReadAllText(Path.Combine(root, "ledger.json")));
            using JsonDocument validator = JsonDocument.Parse(File.ReadAllText(Path.Combine(evidence, "validator-results.json")));
            JsonElement cases = ledger.RootElement;
            JsonElement cli = validator.RootElement.GetProperty("recovery");
            bool Complete(JsonElement rows) => rows.GetArrayLength() == expected.Count
                && expected.Keys.All(id => rows.EnumerateArray().Count(row => row.GetProperty("id").GetString() == id) == 1);
            Require(Complete(cases) && Complete(cli), "The recovery matrix omitted, duplicated or relabelled a required member");
            string[] ids = expected.Keys.ToArray();
            foreach (string[] invalid in new[] { ids.Skip(1).ToArray(), ids.Concat(new[] { ids[0] }).ToArray(),
                ids.Select((id, i) => i == 0 ? "relabeled" : id).ToArray() })
            {
                using JsonDocument fault = JsonDocument.Parse(JsonSerializer.Serialize(invalid.Select(id => new { id })));
                Require(!Complete(fault.RootElement), "The actual runtime consumer accepted an incomplete recovery set");
            }
            reads.Add(new { recoveryOmissionControls = 3, missingRejected = true, duplicateRejected = true, relabeledRejected = true });
            var observations = new List<object>();
            var failures = new List<string>();
            foreach (JsonElement item in cases.EnumerateArray())
            {
                string id = item.GetProperty("id").GetString();
                string[] oracle = expected[id];
                JsonElement control = cli.EnumerateArray().Single(row => row.GetProperty("id").GetString() == id);
                Require(item.GetProperty("cli").GetInt32() == int.Parse(oracle[1], CultureInfo.InvariantCulture)
                    && item.GetProperty("loader").GetString() == oracle[2]
                    && control.GetProperty("exit").GetInt32() == int.Parse(oracle[1], CultureInfo.InvariantCulture)
                    && control.GetProperty("diagnosticMatched").GetBoolean() && control.GetProperty("bitMatched").GetBoolean(),
                    "The full validator disagrees with the literal recovery oracle: " + id);
                string file = Path.Combine(root, id, "input.obj");
                byte[] bytes = File.ReadAllBytes(file);
                string hash = Convert.ToHexString(SHA256.HashData(bytes));
                Require(Convert.ToHexString(bytes).Equals(item.GetProperty("objHex").GetString(), StringComparison.OrdinalIgnoreCase)
                    && hash == item.GetProperty("objHash").GetString() && hash == control.GetProperty("objHash").GetString()
                    && hash == control.GetProperty("actualInputHash").GetString(),
                    "The loader and CLI did not observe the same input bytes: " + id);
                string material = Path.Combine(root, id, "sample.mtl");
                Require(item.GetProperty("mtlHex").ValueKind == JsonValueKind.Null ? !File.Exists(material)
                    : Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(material))) == item.GetProperty("mtlHash").GetString(),
                    "The recovery material input differs: " + id);
                var diagnostics = new List<string>();
                Application.LogCallback capture = (message, stack, type) => diagnostics.Add(message);
                Model model = null;
                string error = null;
                Application.logMessageReceived += capture;
                try { model = ObjLoader.Load(file); }
                catch (Exception failure) { error = failure.ToString(); }
                finally { Application.logMessageReceived -= capture; }
                string actual = error != null ? "E" : model == null ? "N" : model.authoredNormals.All(value => value) ? "A"
                    : model.authoredNormals.All(value => !value) ? "F" : "PARTIAL";
                bool flagsMatch = actual == oracle[2] && (model == null || model.positions.Length == 3);
                bool shadingMatch = true;
                if (actual == "F")
                    shadingMatch = model.shadingNormals.Select((normal, i) => normal == model.normals[i]).All(value => value);
                else if (actual == "A")
                {
                    Vector3 direction = id == "R01-original" ? new Vector3(0f, 0.6f, 0.8f) : id == "R02-negative" ? Vector3.left
                        : id == "R12-diagonal-above" ? new Vector3(0.7071068f, 0.7071068f, 0f) : Vector3.right;
                    shadingMatch = model.shadingNormals.All(normal => (normal - direction).sqrMagnitude < 0.000001f);
                }
                string token = item.GetProperty("token").GetString();
                bool parsed = token != null && float.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out _);
                float scalar = 0;
                if (token != null) float.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out scalar);
                bool bitsMatch = oracle[3] == "-" || (parsed && Bits(scalar) == oracle[3]
                    && control.GetProperty("scalarBits").EnumerateArray().Any(row => row.GetProperty("token").GetString() == token && row.GetProperty("bits").GetString() == oracle[3]));
                string indexToken = item.GetProperty("indexToken").GetString();
                int integer = 0;
                bool indexParsed = indexToken != null && int.TryParse(indexToken, NumberStyles.Integer, CultureInfo.InvariantCulture, out integer);
                if (indexToken != null)
                {
                    bool expectedParse = new[] { "0", "+0", "-0", "-2147483648", "2147483647", "4", "-4" }.Contains(indexToken);
                    if (id == "R23-leading-zeros-position") expectedParse = true;
                    bitsMatch &= indexParsed == expectedParse;
                    string expectedIndexBits = indexToken == "-2147483648" ? "80000000" : indexToken == "2147483647" ? "7FFFFFFF"
                        : indexToken == "-4" ? "FFFFFFFC" : indexToken == "4" ? "00000004" : "00000000";
                    if (id == "R23-leading-zeros-position") expectedIndexBits = "00000001";
                    bitsMatch &= unchecked((uint)integer).ToString("X8") == expectedIndexBits;
                }
                if (id.StartsWith("R22-", StringComparison.Ordinal) || id.StartsWith("R23-", StringComparison.Ordinal))
                    if (id.EndsWith("-texture", StringComparison.Ordinal)) shadingMatch &= model.uvs[0] == Vector2.zero;
                if (id.StartsWith("R25-v-", StringComparison.Ordinal)) shadingMatch &= Bits(model.positions[0].x) == Bits(scalar);
                if (id.StartsWith("R25-vt-", StringComparison.Ordinal) || (id.StartsWith("R14-", StringComparison.Ordinal) && id.EndsWith("-vt", StringComparison.Ordinal)))
                    shadingMatch &= Bits(model.uvs[0].x) == Bits(scalar);
                if (id.StartsWith("R25-rgb-", StringComparison.Ordinal)) shadingMatch &= Bits(model.colors[0].r) == Bits(scalar);
                if (id.StartsWith("R26-", StringComparison.Ordinal) || (id.StartsWith("R14-", StringComparison.Ordinal) && id.EndsWith("-Kd", StringComparison.Ordinal)))
                {
                    bool malformed = !parsed || !float.IsFinite(scalar);
                    Color color = model.materials[model.material[0]].color;
                    shadingMatch &= malformed ? diagnostics.Any(message => message.Contains("Malformed material colour") && message.Contains(material))
                        && Bits(color.r) == "3F4CCCCD" && Bits(color.g) == "3F4CCCCD" && Bits(color.b) == "3F4CCCCD"
                        : Bits(color.r) == Bits(scalar);
                }
                if (id == "R27-missing-mtl" || id == "R27-escaping-mtl")
                    shadingMatch &= diagnostics.Any(message => message.Contains("Material file not found"));
                bool passed = flagsMatch && shadingMatch && bitsMatch;
                if (!passed) failures.Add(id + ": expected=" + oracle[2] + " actual=" + actual + " bits=" + Bits(scalar));
                observations.Add(new
                {
                    id,
                    expectedCli = oracle[1],
                    expectedLoader = oracle[2],
                    actualLoader = actual,
                    passed,
                    strictProfile = item.GetProperty("strictProfile").GetBoolean(),
                    inputHex = Convert.ToHexString(bytes),
                    inputHash = hash,
                    materialHash = File.Exists(material) ? Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(material))) : null,
                    modelNull = model == null,
                    corners = model?.positions.Length ?? 0,
                    authoredNormals = model?.authoredNormals,
                    positions = model?.positions.Select(v => new[] { Bits(v.x), Bits(v.y), Bits(v.z) }).ToArray(),
                    geometricNormals = model?.normals.Select(v => new[] { Bits(v.x), Bits(v.y), Bits(v.z) }).ToArray(),
                    shadingNormals = model?.shadingNormals.Select(v => new[] { Bits(v.x), Bits(v.y), Bits(v.z) }).ToArray(),
                    uvs = model?.uvs.Select(v => new[] { Bits(v.x), Bits(v.y) }).ToArray(),
                    colors = model?.colors.Select(v => new[] { Bits(v.r), Bits(v.g), Bits(v.b), Bits(v.a) }).ToArray(),
                    materials = model?.materials.Select(v => new { v.name, color = new[] { Bits(v.color.r), Bits(v.color.g), Bits(v.color.b) } }).ToArray(),
                    token,
                    scalarParsed = parsed,
                    scalarBits = Bits(scalar),
                    indexToken,
                    indexParsed,
                    indexBits = unchecked((uint)integer).ToString("X8"),
                    diagnostics,
                    error
                });
            }
            string output = Path.Combine(evidence, "RecoveryLoader.json");
            File.WriteAllText(output, JsonSerializer.Serialize(observations));
            reads.Add(new { recoveryEvidence = output, required = expected.Count, observed = observations.Count, failures });
            Require(failures.Count == 0, "Recovery loader matrix failed: " + string.Join("; ", failures));
        }
    }
}
