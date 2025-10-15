<h1 align="center">
  <img width="800" src="/unity_banner.png" alt="Polytube Logo">
</h1>
<p align="center">
  Polytube is an analytics tool that <b>records gameplay sessions</b> for playtesting and debugging. It uses <b>GPU duplication</b> for high performance and minimal impact on system resources, and can automatically upload recordings to the <b>cloud</b> when valid credentials are provided. This Unity packge provides an easy to use wrapper around polytube.exe.
</p>

<h4 align="center">
  <a href="#documentation">Documentation</a> |
  <a href="https://polytube.io/">Polytube Cloud Storage</a> |
  <a href="https://dev.polytube.io/polytube.exe">Polytube.exe Download</a>
</h4>


<p align="center">
  <img src="preview_gif.gif" alt="Demo" style="width:600px;">
</p>



---

## Unity Instructions (4 Steps)

### 1. Add the package

1. **Download the project** as a ZIP file from GitHub:
   [https://github.com/AmirSolt/Unity-Polytube/archive/refs/heads/main.zip](https://github.com/AmirSolt/Unity-Polytube/archive/refs/heads/main.zip)

2. **Extract the ZIP** and place the folder named `com.polytube.sessionrecorder` inside your Unity project’s `Packages/` directory.


### 2. Add the polytube.exe

1. **Download the latest executable**: [https://github.com/AmirSolt/Polytube/releases](https://github.com/AmirSolt/Polytube/releases)

2. **Place** the downloaded `polytube.exe` file inside your Unity project’s `Assets/StreamingAssets/` directory.

> **Note:**  
> The `polytube.exe` software is licensed under [Creative Commons Attribution-NoDerivatives 4.0 International (CC BY-ND 4.0)](https://creativecommons.org/licenses/by-nd/4.0/).  
> It is **free to use and redistribute for commercial use**, but **modification or redistribution of modified versions is not permitted**.



### 3. Start recording

Call the following method from your code:

```csharp
using Polytube.SessionRecorder;

Polytube.SessionRecorder.Main.Start();
```

See [documentation](#documentation)

This starts recording the game window and saves the files to `Application.temporaryCachePath`. 
The recording will close immediately inside the editor since there's no game window.

> **🚨 IMPORTANT:** Make sure to obtain proper consent agreements before recording. Respect user's privacy,



### 4. (Optional) Enable cloud uploads

If you provide an `ApiId` and `ApiKey`, the recordings will automatically upload to the cloud.

You can sign up for access at: [https://polytube.io/](https://polytube.io/)

See [documentation](#documentation)

Example:

```csharp
using Polytube.SessionRecorder;

Polytube.SessionRecorder.Main.Start(new Dictionary<string, string>{
  {"--api-id", "<YOUR API ID>"},
  {"--api-key", "<YOUR API KEY>"}
});
```
---

## Notes


### Do not close the program manually

The recorder is designed to shut down automatically when the target window closes. Do not kill the process abruptly.

### Proper consent forms

Make sure you comply with privacy laws in every region where your software will be released. If your application uses cloud storage and doesn't meet these regulations, it could be banned or restricted.

### It's designed to record till the last momment

Since the recorder, `polytube.exe` runs in a different process it will keep recording even if your game crashes. Once the game window is closed for any reason it will stop recording and close all services.

### Wipes out previous recordings

Currently, the recorder wipes out any previous recordings, so you don't have to worry about storage space.

---


## Documentation


### ***Functions***


### `Start(Dictionary<string, string> userFlags=null)`

This function starts recording the game window and saves the files to `Application.temporaryCachePath`.
It uses `Application.productName` to find the game window. This function won't work in the editor.

You can add flag parameters to control the behaviour of the recorder. See below for flags.


---

### ***Flags***


### [Optional] `--api-id "<ID>"`

**Description:**
Sets the API ID used for authentication when communicating with the cloud storage endpoint.
**Details:**
* Currently it only works with [polytube.io](https://polytube.io) cloud storage.
**Example:**

```bash
{"--api-id", "<YOUR API ID>"},
```


### [Optional] `--api-key "<Key>"`

**Description:**
Sets the API key used for authentication with the cloud storage endpoint.
**Details:**
* Currently it only works with [polytube.io](https://polytube.io) cloud storage.
**Example:**

```bash
{"--api-key", "<YOUR API KEY>"},
```

---

## Upcoming features
- Add an option to disable wiping out old sessions.
- A `Stop()` function to stop the recording manually.
