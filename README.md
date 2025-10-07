<h1 align="center">
  <img width="800" src="/logo_name.png" alt="Session Replay">
</h1>
<p align="center">
  This tool <b>records gameplay sessions</b> for playtesting and debugging. It uses <b>GPU duplication</b> for high performance and minimal impact on system resources, and can automatically upload recordings to the <b>cloud</b> when valid credentials are provided.
</p>

<h4 align="center">
  <a href="https://polytube.io/">Cloud Storage</a> |
  <a href="https://assetstore.unity.com/packages/slug/338050">Unity Asset Store</a> |
  <a href="https://polytube.itch.io/">Itch.io Page</a>
</h4>

---

## Instructions (Unity)

### 1. Add the package

1. **Download the project** as a ZIP file from GitHub:
   [https://github.com/AmirSolt/Unity-Session-Replay/archive/refs/tags/recording.zip](https://github.com/AmirSolt/Unity-Session-Replay/archive/refs/tags/recording.zip)

2. **Extract the ZIP** and place the folder named `com.polytube.sessionreplay` inside your Unity project’s `Packages/` directory.

3. **Download the executable** from the latest release:
   [https://github.com/AmirSolt/Unity-Session-Replay/releases/download/recording/replay.exe](https://github.com/AmirSolt/Unity-Session-Replay/releases/download/recording/replay.exe)

4. **Place** the downloaded `replay.exe` file inside your Unity project’s `Assets/StreamingAssets/` directory.

---

### 2. Start recording

Call the following method from your code:

```csharp
Polytube.SessionReplay.Main.Start();
```

This starts recording the game window and saves the files to `Application.temporaryCachePath`.

> Make sure to obtain proper consent and respect player privacy.

---

### 3. Do not close the program manually

The recorder will shut down automatically when the game window closes. Do **not** kill the process manually.

---

### 4. (Optional) Enable cloud uploads

If you provide an `ApiId` and `ApiKey`, the recordings will automatically upload to the cloud.

You can sign up for access at: [https://www.polytube.io/](https://www.polytube.io/)

Example:

```csharp
Polytube.SessionReplay.Main.Start("<API_ID>", "<API_KEY>");
```
