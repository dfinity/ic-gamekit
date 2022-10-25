# Internet Computer GameKit for Unity
Internet Computer(IC) GameKit is an open-source toolkit for Unity game developers who want to develop games on the Internet Computer. It's under `Packages/com.dfinity.gamekit` folder which contains a Unity package for Internet Computer.

## Installing
You can install the package from `Unity Package Manager` via using `Add package from git URL...` menu, by inputing the following URL.

```
https://github.com/dfinity/ic-gamekit.git?path=/unity/Packages/com.ic.gamekit
```

If you want to try the version in development, using the following URL to specify the `dev` branch.

```
https://github.com/dfinity/ic-gamekit.git?path=/unity/Packages/com.ic.gamekit#dev
```

## IC Connector

Internet computer GameKit contains IC Connector as the first supported feature. It registers a WebGL post-build callback which generates an IC project for deployment to the Internet Computer.

After installing the package successfully, you can go to `Project Settings` -> `Internet Computer` to config Internet Computer settings. We only support two basic options for now, will add more in the next release.

### Canister Name

This configs the asset canister name in the `dfx.json` file for the generated IC project.

### Enable IC Connector

You can turn on/off the post-build callback. With this checked, a folder named `ic-project` will be generated under the Unity WebGL build ouput folder after building.

### Deployment

Please follow the [Unity WebGL Sample](https://github.com/dfinity/examples/tree/master/hosting/unity-webgl-template) to deploy the generated IC project to the Internet Computer.

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