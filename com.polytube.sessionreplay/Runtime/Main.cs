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


        public static void Start(string apiId="", string apiKey="")
        {
            if (exeProcess != null) return;

            if (SystemInfo.operatingSystemFamily != OperatingSystemFamily.Windows)
            {
                UnityEngine.Debug.LogWarning("Replay only supported on Windows.");
                return;
            }

            SessionTempDir = Path.Combine(Application.temporaryCachePath, "com.polytube.sessionreplay");
            SessionStreamingAssetDir = Application.streamingAssetsPath;
            ReplayExePath = Path.Combine(SessionStreamingAssetDir, "replay.exe");

            Creds.apiId = apiId;
            Creds.apiKey = apiKey;

            StartExe();

            // Subscribe to Unity logs
            Application.logMessageReceived += OnConsoleLog;

            // When Unity quits, we close stdin (don’t kill process)
            Application.quitting += OnQuit;
        }

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

        private static void StartExe()
        {
            string title = Application.productName;

            var args = new List<string>
            {
                "--title", $"\"{title}\"",
                "--out", $"\"{SessionTempDir}\"",
                "--api-id", Creds.apiId,
                "--api-key", Creds.apiKey
            };

            var psi = new ProcessStartInfo
            {
                FileName = ReplayExePath,
                Arguments = string.Join(" ", args),
                UseShellExecute = false,
                RedirectStandardInput = true, // 👈 for piping logs
                CreateNoWindow = true
            };

            exeProcess = new Process { StartInfo = psi, EnableRaisingEvents = true };
            exeProcess.Start();

            exeWriter = exeProcess.StandardInput;

            UnityEngine.Debug.Log($"[SessionReplay] Started replay.exe with PID {exeProcess.Id}");
        }

        private static void OnQuit()
        {
            try
            {
                exeWriter?.Close();

                exeProcess = null;
                exeWriter = null;

                // process will close itself after cleaning up tasks

                UnityEngine.Debug.Log("[SessionReplay] Closed log pipe, replay.exe will finish uploads and exit.");
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning($"[SessionReplay] OnQuit cleanup error: {ex.Message}");
            }
        }
    }
}
