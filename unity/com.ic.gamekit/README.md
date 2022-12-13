# Internet Computer GameKit for Unity
The Internet Computer(IC) GameKit is an open-source toolkit for Unity game developers who want to develop games on the Internet Computer.

## IC Connector

The Internet Computer GameKit contains the IC Connector as the first supported feature. It registers a WebGL post-build callback which generates an IC project ready for deployment to the Internet Computer.

After installing the package successfully, you can go to `Project Settings` -> `Internet Computer` to configure Internet Computer settings. We only support two basic options for now, and will add more in the next release.

### Canister Name

This configures the asset canister name in the `dfx.json` file for the generated IC project.

### Enable IC Connector

You can turn on/off the post-build callback. With this checked, a folder named `ic-project` will be generated under the Unity WebGL build output folder after building.

### Deployment

Please follow the [Unity WebGL Sample](https://github.com/dfinity/examples/tree/master/hosting/unity-webgl-template) to deploy the generated IC project to the Internet Computer.

## IC Agent

The Internet Computer Agent is based on [ICP.NET](https://github.com/edjCase/ICP.NET) which provides the Internet Computer Protocol(ICP) libraries for .NET.

### ICP.NET Integration
Here describes how the [ICP.NET](https://github.com/edjCase/ICP.NET) is integrated, basically explains how the managed dlls under `ICP.NET` folder are generated.

- `EdjCase.ICP.Agent.dll` & `EdjCase.ICP.Candid.dll` are compiled from [ICP.NET](https://github.com/edjCase/ICP.NET) directly, they're targeted to .NET standard 2.0, so it’s okay to use them directly.
- For all the other dependencies, including  
    - Chaos.NaCl.dll (1.0.0)  
    - Dahomey.Cbor.dll (1.16.1)  
    - System.Collections.Immutable (6.0.0)  
    - System.Runtime.CompilerServices.Unsafe (6.0.0)  
    - System.IO.Pipelines (6.0.1)  
    - Microsoft.Bcl.HashCode.dll (1.1.1)  

  Download the packages with the right version from https://www.nuget.org/packages and choose the dll with netstandard2.0 version.

## Troubleshooting

### Get 500 error while browsing the game

If you get 500 error while accessing the game in the browsers, try to use `raw` keyword in the URL, like https://\<canister-id\>.raw.ic0.app. Unity generates pretty large `.data` and `.wasm` files which can only be served using `raw` keyword.

### Can't load the game successfully with Compression Enabled for WebGL

While building to WebGL, unity provides with `Gzip/Brotli` compression format in the `Publishing Settings`. With compression enabled, you may get the below error message in the browser Console while accessing the deployed game.

```
Unable to parse Build/webgl.framework.js.gz! This can happen if build compression was enabled but web server hosting the content was misconfigured to not serve the file with HTTP Response Header "Content-Encoding: gzip" present. Check browser Console and Devtools Network tab to debug.
```

The reason is the `.wasm` and `.framework.js` files are compressed as `.gz`(`.br` if you use Brotli), but it needs to be treated as content-type `application/wasm` or `application/javascript` rather than `application/gzip`. Please check unity document [WebGL: Server configuration code samples](https://docs.unity3d.com/2020.1/Documentation/Manual/webgl-server-configuration-code-samples.html) for more info. 

This is only supported by the dfx `0.12.0` and above. An workaround is using `Decompression fallback` provided by Unity, please check [WebGL: Compressed builds and server configuration](https://docs.unity3d.com/2020.1/Documentation/Manual/webgl-deploying.html) for details.

### Can't load the icons correctly with the default WebGL template

While building to WebGL, unity provides `Default` and `Minimal` templates. If you choose `Default` template, you may get the below error while testing the deployment locally by the url like `http://127.0.0.1:8000/?canisterId=rrkah-fqaaa-aaaaa-aaaaq-cai`.

```
Failed to load resource: the server responded with a status of 400 (Bad Request)            unity-logo-dark.png:1 
...
```

This is because the `TemplateData\style.css` references the urls of some icon images which can't be served correctly. Please use url like `http://<canisterid>.localhost:8000/` to browse your game instead.
