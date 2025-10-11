using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Polytube.SessionRecorder
{

    public static class Main
    {
        private static string SessionTempDir = "";
        private static string ExePath = "";
        private static Process exeProcess;
        private static StreamWriter exeWriter;


        // --------------------------
        // Entry point for auto-load
        // --------------------------
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Load()
        {
            if (!InitializeEnvironment()) return;

            var args = new List<string>
            {
                "--load",
                "--out", $"\"{SessionTempDir}\""
            };


            StartExeProcess(args, memorize: false);

        }

        // --------------------------
        // Manual start with credentials
        // --------------------------
        public static void Start(Dictionary<string, string> userFlags=null)
        {

            userFlags ??= new Dictionary<string, string>();
            
            if (exeProcess != null) return; // already started

            if (!InitializeEnvironment()) return;

            
            Dictionary<string, string> defaultFlags = new Dictionary<string, string>
            {
                {"--title",$"\"{Application.productName}\""},
                {"--app-name",$"\"{Application.productName}\""},
                {"--app-version",$"\"{Application.version}\""},
                {"--out",$"\"{SessionTempDir}\""},
            };

            Dictionary<string, string> flags = MergeFlags(defaultFlags, userFlags);

            List<string> args = flags.Select(kvp => $"{kvp.Key} {kvp.Value}").ToList();

            StartExeProcess(args, memorize: true);

            // Subscribe to logs and quit
            Application.logMessageReceived += OnConsoleLog;
            Application.quitting += OnQuit;
        }

        // --------------------------
        // Common environment setup
        // --------------------------
        private static bool InitializeEnvironment()
        {
            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                UnityEngine.Debug.LogWarning("[SessionRecorder] Polytube only supported on Windows.");
                return false;
            }

            SessionTempDir = Path.Combine(Application.temporaryCachePath, "com.polytube.sessionrecorder");
            ExePath = Path.Combine(Application.streamingAssetsPath, "polytube.exe");

            if (!File.Exists(ExePath))
            {
                UnityEngine.Debug.LogError($"[SessionRecorder] polytube.exe not found at: {ExePath}");
                return false;
            }

            return true;
        }

        // --------------------------
        // Start process helper
        // --------------------------
        private static void StartExeProcess(List<string> args, bool memorize)
        {
            var psi = new ProcessStartInfo
            {
                FileName = ExePath,
                Arguments = string.Join(" ", args),
                UseShellExecute = false,
                RedirectStandardInput = memorize,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = psi, EnableRaisingEvents = memorize };
            process.Start();

            UnityEngine.Debug.Log($"[SessionRecorder] Started polytube.exe (PID {process.Id}) with args: {psi.Arguments}");

            if (memorize)
            {
                exeProcess = process;
                exeWriter = process.StandardInput;
            }
        }

        // --------------------------
        // Logging pipe
        // --------------------------
        private static void OnConsoleLog(string logString, string stackTrace, LogType type)
        {
            if (exeWriter == null) return;

            string level = type.ToString().ToUpperInvariant();
            string timestamp = DateTime.UtcNow.ToString("o");
            string content = $"[{timestamp}] [{level}] {logString}";

            try
            {
                exeWriter.WriteLine(content);
                exeWriter.Flush();
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[SessionRecorder] Failed to write log to polytube.exe: {ex.Message}");
            }
        }

        // --------------------------
        // Cleanup on quit
        // --------------------------
        private static void OnQuit()
        {
            try
            {
                exeWriter?.Close();
                exeProcess = null;
                exeWriter = null;

                UnityEngine.Debug.Log("[SessionRecorder] Closed log pipe, polytube.exe will finish uploads and exit.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[SessionRecorder] OnQuit cleanup error: {ex.Message}");
            }
        }

        // --------------------------
        // Merge flags
        // --------------------------
        private static Dictionary<string, string> MergeFlags(
            Dictionary<string, string> defaultFlags,
            Dictionary<string, string> userFlags)
        {
            // Create a copy so we don't mutate the original
            Dictionary<string, string> merged = new Dictionary<string, string>(defaultFlags);

            foreach (var kvp in userFlags)
            {
                if (merged.ContainsKey(kvp.Key))
                    merged[kvp.Key] = kvp.Value; // Replace existing
                else
                    merged.Add(kvp.Key, kvp.Value); // Add new flag
            }

            return merged;
        }

    }
}
