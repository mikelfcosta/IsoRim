using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using UnityEngine;
using Verse;

// The runner opts into this CLR hook before the engine's first SetDirectory call.
// No hook runs in a normal game process. The hook only uses framework APIs.
public static class StartupHook
{
    public static void Initialize()
    {
        try
        {
            InitializeRequest();
        }
        catch (Exception error)
        {
            Console.Error.WriteLine("Normal startup refused: " + error);
            Environment.Exit(2);
        }
    }

    private static void InitializeRequest()
    {
        string requestPath = Environment.GetEnvironmentVariable("ISORIM_NORMAL_REQUEST");
        if (string.IsNullOrEmpty(requestPath))
        {
            throw new InvalidOperationException("The normal startup hook requires a request");
        }
        var request = JsonSerializer.Deserialize<IsoRim.ProbeNormals.Request>(File.ReadAllText(requestPath));
        string output = Path.GetFullPath(request.outputDirectory).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string assembly = typeof(StartupHook).Assembly.Location;
        string actualHash;
        using (FileStream file = File.OpenRead(assembly))
        {
            actualHash = Convert.ToHexString(SHA256.HashData(file));
        }
        if (!Guid.TryParseExact(request.nonce, "N", out _) || new DirectoryInfo(output).Name != request.nonce
            || !Path.GetFullPath(requestPath).StartsWith(output + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            || actualHash != request.assemblyHash || DateTimeOffset.UtcNow - request.createdUtc > TimeSpan.FromMinutes(15)
            || request.createdUtc > DateTimeOffset.UtcNow)
        {
            throw new InvalidOperationException("The normal startup request is invalid: nonce=" + request.nonce
                + " output=" + output + " request=" + Path.GetFullPath(requestPath)
                + " hash=" + actualHash + " expected=" + request.assemblyHash
                + " created=" + request.createdUtc.ToString("O") + " now=" + DateTime.UtcNow.ToString("O"));
        }
        Assembly engine = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "Waylith.dll"));
        engine.GetType("Waylith.Logging.FileLog", true).GetMethod("SetDirectory").Invoke(null, new object[] { output });
    }
}

namespace IsoRim
{
    // Only Probe.Begin can start this controller, after its three opt-in guards pass.
    internal static class ProbeNormals
    {
        private static bool started;

        internal sealed class Request
        {
            public string nonce { get; set; }
            public string head { get; set; }
            public string assemblyHash { get; set; }
            public string outputDirectory { get; set; }
            public string fixtureDirectory { get; set; }
            public string[] caseIds { get; set; }
            public DateTimeOffset createdUtc { get; set; }
            public string packagedHash { get; set; }
            public string compilerManifestHash { get; set; }
            public string contractHash { get; set; }
        }

        private static string Hash(string path)
        {
            using (FileStream file = File.OpenRead(path))
            {
                return Convert.ToHexString(SHA256.HashData(file));
            }
        }

        internal static void Tick()
        {
            if (!Probe.On || started || Find.CurrentMap == null || !View3D.Ready)
            {
                return;
            }
            string path = Path.Combine(GenFilePaths.SaveDataFolderPath, "IsoRimNormalRequest.json");
            if (!File.Exists(path))
            {
                return;
            }
            started = true;
            try
            {
                Request request = JsonSerializer.Deserialize<Request>(File.ReadAllText(path));
                string output = Path.GetFullPath(request.outputDirectory);
                string save = Path.GetFullPath(GenFilePaths.SaveDataFolderPath);
                if (DateTimeOffset.UtcNow - request.createdUtc > TimeSpan.FromMinutes(15) || request.createdUtc > DateTimeOffset.UtcNow)
                {
                    throw new InvalidOperationException("The normal request is stale: " + request.createdUtc.ToString("O") + " now " + DateTime.UtcNow.ToString("O"));
                }
                if (!Guid.TryParseExact(request.nonce, "N", out _) || string.IsNullOrEmpty(request.head))
                {
                    throw new InvalidOperationException("The normal request has an invalid nonce or head");
                }
                if (!save.StartsWith(output + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("The savedata path is outside the evidence directory: " + save + " vs " + output);
                }
                if (Hash(typeof(ProbeNormals).Assembly.Location) != request.assemblyHash)
                {
                    throw new InvalidOperationException("The loaded product differs: " + typeof(ProbeNormals).Assembly.Location + " " + Hash(typeof(ProbeNormals).Assembly.Location) + " vs " + request.assemblyHash);
                }
                Type fileLog = Assembly.Load("Waylith").GetType("Waylith.Logging.FileLog", true);
                string engineLogRoot = (string)fileLog.GetProperty("StartedRoot").GetValue(null);
                if (!string.Equals(Path.GetFullPath(engineLogRoot), output, StringComparison.OrdinalIgnoreCase) || (bool)fileLog.GetProperty("Disabled").GetValue(null))
                {
                    throw new InvalidOperationException("The engine log is not isolated: " + engineLogRoot);
                }
                Dictionary<string, string> modules = new Dictionary<string, string>();
                string runtimeRoot = Path.GetDirectoryName(typeof(object).Assembly.Location);
                foreach (string name in new[] { "RimWorld.dll", "Assembly-CSharp.dll", "UnityStubs.dll", "Waylith.dll", "Waylith.UnityCompat.dll", "System.Private.CoreLib.dll", "coreclr.dll" })
                {
                    modules[name] = Hash(Path.Combine(runtimeRoot, name));
                }
                modules["0Harmony.dll"] = Hash(typeof(HarmonyLib.Harmony).Assembly.Location);
                using (Process process = Process.GetCurrentProcess())
                {
                    bool foundClr = false;
                    foreach (ProcessModule module in process.Modules)
                    {
                        if (string.Equals(module.ModuleName, "coreclr.dll", StringComparison.OrdinalIgnoreCase))
                        {
                            modules["coreclr.dll"] = Hash(module.FileName);
                            foundClr = true;
                        }
                    }
                    if (!foundClr)
                    {
                        throw new InvalidOperationException("The runtime did not load the approved CLR module");
                    }
                }
                // Loaded assemblies must come from the private copy, not another installed game.
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (modules.ContainsKey(assembly.GetName().Name + ".dll") && !assembly.IsDynamic)
                    {
                        string name = assembly.GetName().Name + ".dll";
                        if (!Path.GetFullPath(assembly.Location).StartsWith(runtimeRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new InvalidOperationException("A proof module loaded outside the sandbox: " + assembly.Location);
                        }
                        modules[name] = Hash(assembly.Location);
                    }
                }
                string runtime = typeof(object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion.Split('+')[0];
                HashSet<string> seen = new HashSet<string>();
                foreach (string id in request.caseIds.OrderBy(id => id.StartsWith("IntegratedNormalCases.", StringComparison.Ordinal) ? 1 : 0))
                {
                    bool passed = false;
                    string details = "";
                    string[] evidencePaths = Array.Empty<string>();
                    try
                    {
                        if (!seen.Add(id))
                        {
                            throw new InvalidOperationException("Duplicate normal case");
                        }
                        if (id.StartsWith("IntegratedNormalCases.", StringComparison.Ordinal))
                        {
                            evidencePaths = new[] { IntegratedNormalCases.Run(id, request) };
                        }
                        else if (id.StartsWith("MovingNormalCases.", StringComparison.Ordinal))
                        {
                            evidencePaths = new[] { MovingNormalCases.Run(id, output, request.fixtureDirectory) };
                        }
                        else if (id.StartsWith("StaticNormalCases.", StringComparison.Ordinal))
                        {
                            evidencePaths = new[] { StaticNormalCases.Run(id, output, request.fixtureDirectory) };
                        }
                        else if (id.StartsWith("AssetIntakeCases.", StringComparison.Ordinal))
                        {
                            evidencePaths = new[] { AssetIntakeCases.Run(id, output) };
                        }
                        else
                        {
                            NormalDataCases.Run(id, request.fixtureDirectory);
                        }
                        passed = true;
                    }
                    catch (Exception error)
                    {
                        details = error.ToString();
                    }
                    string record = JsonSerializer.Serialize(new
                    {
                        request.nonce,
                        request.head,
                        assemblyHash = Hash(typeof(ProbeNormals).Assembly.Location),
                        caseId = id,
                        passed,
                        details,
                        modules,
                        runtime,
                        gameVersion = Assembly.Load("RimWorld").GetName().Version.ToString(),
                        processId = Environment.ProcessId,
                        engineLogRoot,
                        evidencePaths
                    });
                    string destination = Path.Combine(output, "NormalProof-" + seen.Count + ".json");
                    File.WriteAllText(destination + ".tmp", record);
                    File.Move(destination + ".tmp", destination);
                }
            }
            catch (Exception error)
            {
                Log.Error("[IsoRim normal proof] " + error);
            }
            Application.Quit();
        }
    }
}
