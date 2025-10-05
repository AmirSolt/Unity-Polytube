using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class FFmpegFileEnsurer
{
    static FFmpegFileEnsurer()
    {
        CopyFFmpeg();
    }

    private static void CopyFFmpeg()
    {
        string sourcePath = "Packages/com.polytube.sessionreplay/Editor/assets/replay.exe";
        string destDir = Path.Combine(Application.streamingAssetsPath, "com.polytube.sessionreplay");
        string destPath = Path.Combine(destDir, "replay.exe");

        if (!Directory.Exists(Application.streamingAssetsPath))
            Directory.CreateDirectory(Application.streamingAssetsPath);

        if (!Directory.Exists(destDir))
            Directory.CreateDirectory(destDir);

        if (File.Exists(sourcePath))
        {
            if (!File.Exists(destPath)) // copy only if missing
            {
                File.Copy(sourcePath, destPath, overwrite: true);
                Debug.Log($"[ReplayFileEnsurer] Copied {sourcePath} -> {destPath}. This file is needed for SessionReplay Package!");
            }
        }
        else
        {
            Debug.LogWarning($"[ReplayFileEnsurer] Source file not found: {sourcePath}. This file is needed for SessionReplay Package!");
        }
    }
}
