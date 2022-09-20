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

This configs the asset canister name in the dfx.json file for the generated IC project.

### Enable IC Connector

You can turn on/off the post-build callback. With this checked, a folder name `ic-project` will be generated under the Unity WebGL build ouput folder after building.

### Deployment

Please follow the [Unity WebGL Sample](https://github.com/dfinity/examples/tree/master/hosting/unity-webgl-template) to deploy the generated IC project to the Internet Computer.
