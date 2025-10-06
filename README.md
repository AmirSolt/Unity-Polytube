<h1 align="center">
  <img width="500" src="/logo_name.png" alt="Session Replay">
</h1>

# Session Replay
<p align="center">
  <p align="center">
  This tool <b>records gameplay sessions</b> for playtesting and debugging. It uses <b>GPU duplication</b> for high performance and minimal impact on system resources, and can automatically upload recordings to the <b>cloud</b> when valid credentials are provided.</p>
</p>



## Instructions

1. **Add the package** to your Unity project.
   It will automatically add `replay.exe` to your `StreamingAssets` inside the `Assets` folder.
   If it’s not added, close and reopen Unity. Do not modify or delete the generated files.

2. **Start recording** by calling:

   ```csharp
   Polytube.SessionReplay.Main.Start();
   ```

   This starts recording the game window and saves the files to the local `Application.temporaryCachePath`.
   Make sure to obtain proper consent and respect player privacy.

3. **Do not close or kill the program manually.**
   It will shut down automatically when the game window is closed.

4. **Cloud upload (optional):**
   If you provide an `ApiId` and an `ApiKey`, the files will be uploaded automatically.
   You can sign up for access here: [https://www.polytube.io/](https://www.polytube.io/)

   Example:

   ```csharp
   Polytube.SessionReplay.Main.Start("<API_ID>", "<API_KEY>");
   ```

