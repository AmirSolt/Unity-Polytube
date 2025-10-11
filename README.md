<h1 align="center">
  <img width="800" src="/logo_name.png" alt="Session Replay">
</h1>
<p align="center">
  Replay is an analytics tool that <b>records gameplay sessions</b> for playtesting and debugging. It uses <b>GPU duplication</b> for high performance and minimal impact on system resources, and can automatically upload recordings to the <b>cloud</b> when valid credentials are provided. This Unity packge provides an easy to use wrapper around Replay.
</p>

<h4 align="center">
  <a href="https://polytube.io/">Cloud Storage</a> |
  <a href="https://assetstore.unity.com/packages/slug/338050">Unity Asset Store</a> |
  <a href="https://polytube.itch.io/">Itch.io Page</a>
</h4>

---

## Unity Instructions (4 Steps)

### 1. Add the package

1. **Download the project** as a ZIP file from GitHub:
   [https://github.com/AmirSolt/Unity-Session-Replay/archive/refs/heads/main.zip](https://github.com/AmirSolt/Unity-Session-Replay/archive/refs/heads/main.zip)

2. **Extract the ZIP** and place the folder named `com.polytube.sessionreplay` inside your Unity project’s `Packages/` directory.


### 2. Add the Replay.exe

1. **Download the executable** from the latest release: [https://github.com/AmirSolt/replay/releases](https://github.com/AmirSolt/replay/releases)

2. **Place** the downloaded `replay.exe` file inside your Unity project’s `Assets/StreamingAssets/` directory.


### 3. Start recording

Call the following method from your code:

```csharp
using Polytube.SessionReplay;

Polytube.SessionReplay.Main.Start();
```

This starts recording the game window and saves the files to `Application.temporaryCachePath`.

> **🚨 IMPORTANT:** Make sure to obtain proper consent agreements before recording. Respect user's privacy,



### 4. (Optional) Enable cloud uploads

If you provide an `ApiId` and `ApiKey`, the recordings will automatically upload to the cloud.

You can sign up for access at: [https://www.polytube.io/](https://www.polytube.io/)

Example:

```csharp
using Polytube.SessionReplay;

Polytube.SessionReplay.Main.Start(new Dictionary<string, string>{
  {"--api-id", "<YOUR API ID>"},
  {"--api-key", "<YOUR API KEY>"}
});
```
---

## Notes


### Do not close the program manually

The recorder is designed to shut down automatically when the target window closes. Do not kill the process abruptly.

### Proper Consent Forms

Make sure you comply with privacy laws in every region where your software will be released. If your application uses cloud storage and doesn't meet these regulations, it could be banned or restricted.

---

## Replay Docs


This package is a thin wrapper around 'Replay.exe'. For more information visit: [https://github.com/AmirSolt/replay](https://github.com/AmirSolt/replay)
