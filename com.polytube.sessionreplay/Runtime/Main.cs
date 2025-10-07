using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Polytube.SessionReplay
{
    public class AppCreds
    {
        public string apiId;
        public string apiKey;
        public string sessionId;
    }

    public static class Main
    {
        private static string SessionTempDir = "";
        private static string SessionStreamingAssetDir = "";
        private static string ReplayExePath = "";
        private static Process exeProcess;
        private static StreamWriter exeWriter;

        private static AppCreds Creds = new();

        // --------------------------
        // Entry point for auto-load
        // --------------------------
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Load()
        {
            if (!InitializeEnvironment()) return;

            StartReplayProcess(new List<string> { "--load" }, memorize: false);
        }

        // --------------------------
        // Manual start with credentials
        // --------------------------
        public static void Start(string apiId = "", string apiKey = "")
        {
            if (exeProcess != null) return; // already started

            if (!InitializeEnvironment()) return;

            Creds.apiId = apiId;
            Creds.apiKey = apiKey;

            var args = new List<string>
            {
                "--title", $"\"{Application.productName}\"",
                "--out", $"\"{SessionTempDir}\""
            };

            if (!string.IsNullOrEmpty(apiId)) args.AddRange(new[] { "--api-id", apiId });
            if (!string.IsNullOrEmpty(apiKey)) args.AddRange(new[] { "--api-key", apiKey });

            StartReplayProcess(args, memorize: true);

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
                UnityEngine.Debug.LogWarning("[SessionReplay] Replay only supported on Windows.");
                return false;
            }

            SessionTempDir = Path.Combine(Application.temporaryCachePath, "com.polytube.sessionreplay");
            SessionStreamingAssetDir = Application.streamingAssetsPath;
            ReplayExePath = Path.Combine(SessionStreamingAssetDir, "replay.exe");

            if (!File.Exists(ReplayExePath))
            {
                UnityEngine.Debug.LogError($"[SessionReplay] replay.exe not found at: {ReplayExePath}");
                return false;
            }

            return true;
        }

        // --------------------------
        // Start process helper
        // --------------------------
        private static void StartReplayProcess(List<string> args, bool memorize)
        {
            var psi = new ProcessStartInfo
            {
                FileName = ReplayExePath,
                Arguments = string.Join(" ", args),
                UseShellExecute = false,
                RedirectStandardInput = memorize,
                CreateNoWindow = true
            };

            var process = new Process { StartInfo = psi, EnableRaisingEvents = memorize };
            process.Start();

            UnityEngine.Debug.Log($"[SessionReplay] Started replay.exe (PID {process.Id}) with args: {psi.Arguments}");

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
                UnityEngine.Debug.LogWarning($"[SessionReplay] Failed to write log to replay.exe: {ex.Message}");
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

                UnityEngine.Debug.Log("[SessionReplay] Closed log pipe, replay.exe will finish uploads and exit.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[SessionReplay] OnQuit cleanup error: {ex.Message}");
            }
        }
    }
}
