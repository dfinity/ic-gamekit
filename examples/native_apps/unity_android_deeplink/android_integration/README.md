# Unity Android sample
This sample demonstrates one way to integrate Identity Integration with Unity on Android.

## Overview

In this example, you can learn how to communicate with the IC from C#. As we described in [Internet Identity Integration](/examples/native_apps/unity_android_deeplink/README.md#workflow), the game mainly focuses on:

1. Provide an Android Java plugin which can be a bridge between C# and the browser.
2. Register the DeepLink in the Android manifest file.
3. Generate the Ed25519KeyIdentity in C# and pass the public key to the Web Brower.
4. Receive the DelegationIdentity from the Web Brower and pass it from Java to C#.
5. Use the DelegationIdentity to communicate with the backend canister.
