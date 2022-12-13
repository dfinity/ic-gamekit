# Internet Computer Agent for Unity

The Internet Computer(IC) Agent is an open-source toolkit for Unity game developers who want to develop games on the Internet Computer, it's based on [ICP.NET](https://github.com/edjCase/ICP.NET) which provides the Internet Computer Protocol(ICP) libraries for .NET.

## ICP.NET Integration
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
