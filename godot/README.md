# Internet Computer GameKit for Godot
Internet Computer GameKit is an open-source toolkit for Godot game developers who want to develop games on the Internet Computer. It's under `godot/addons/ic-gamekit` folder which contains a Godot plugin for Internet Computer.

## Installing
You can just simply copy `addons/ic-gamekit` folder under you Godot project folder. Make sure you enable it via `Project Settings -> Plugins`. It's compatible with Godot 3.x.

## IC Connector

Internet computer GameKit contains IC Connector as the first supported feature. It registers an EditorExportPlugin which generates an IC project for deployment to the Internet Computer.

After installing the package successfully, you can go to `Project Settings` -> `IC Settings` to config Internet Computer settings. We only support two basic options for now, will add more in the next release.

### Canister Name

This configs the asset canister name in the `dfx.json` file for the generated IC project.

### Enable IC Connector

You can turn on/off the post-build callback. With this checked, a folder named `ic-project` will be generated under the Godot HTML5 export path. Now only `Export Project` is supported, `Export PCK/Zip` will be supported in the future.

### Deployment

Please follow the [Godot HTML5 Sample](https://github.com/dfinity/examples/tree/master/hosting/godot-html5-template) to deploy the generated IC project to the Internet Computer.

## Troubleshooting

### Get 500 error while browsing the game

If you get 500 error while accessing the game in the browsers, try to use `raw` keyword in the URL like https://\<canister-id\>.raw.ic0.app. Unity generates pretty large `.data` and `.wasm` files which can only be served using `raw` keyword.
