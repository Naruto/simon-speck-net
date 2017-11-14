[![Build Status](https://travis-ci.org/Naruto/simon-speck-net.svg?branch=develop)](https://travis-ci.org/Naruto/simon-speck-net?branch=develop)
[![Windows Build status](https://ci.appveyor.com/api/projects/status/uulln99smanqi05k?svg=true)](https://ci.appveyor.com/project/Naruto/simon-speck-net)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/13827/badge.svg)](https://scan.coverity.com/projects/13827)
[![NuGet version](https://badge.fury.io/nu/simonspecknet.svg)](https://badge.fury.io/nu/simonspecknet)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net?ref=badge_shield)
# simon-speck-net

c# wrapper implement of [Naruto/simon-speck-c](https://github.com/Naruto/simon-speck-c)

simon and speck are lightweight block cipher algorithms, published by NSA.([iadgov/simon-speck](https://github.com/iadgov/simon-speck))

support platforms are Desktop Platforms(Windows, macOS, Linux), Xamarin iOS and MonoAndroid.

# Supports

- algorithms and block sizes, key sizes and modes
    - speck ECB
        - 128/128
        - 128/192
        - 128/256
    - speck CTR
        - 128/128
        - 128/192
        - 128/256
- platforms, architectures
    - windows x64
    - macOS x64
    - Xamarin iOS
    - MonoAndroid


# Sample

simple encrypt and decrypt code.

```csharp
String plainText = "test text abcdefg.";
byte[] plainByte = System.Text.Encoding.ASCII.GetBytes(plainText);

// Speck ECB mode
using (SymmetricAlgorithm algo = new Speck())
{
    algo.BlockSize = 128;
    algo.KeySize = 128;
    
    algo.GenerateKey();
    using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
    {
        byte[] plainEnc = encryptor.TransformFinalBlock(plainByte, 0, plainByte.Length);
        byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
        Console.WriteLine(System.Text.Encoding.ASCII.GetString(plainDec));
        Console.WriteLine();
    }
}

// Speck CTR mode
using (SymmetricAlgorithm algo = new SpeckCTR())
{
    algo.BlockSize = 128;
    algo.KeySize = 128;
    
    algo.GenerateIV();
    algo.GenerateKey();
    using (ICryptoTransform encryptor = algo.CreateEncryptor() , decryptor = algo.CreateDecryptor())
    {
        byte[] plainEnc = encryptor.TransformFinalBlock(plainByte, 0, plainByte.Length);
        byte[] plainDec = decryptor.TransformFinalBlock(plainEnc, 0, plainEnc.Length);
        Console.WriteLine(System.Text.Encoding.ASCII.GetString(plainDec));
        Console.WriteLine();
    }
}
```

# development
## preapre

- mono
- Unity (if create unity package)

## get source

```bash
git clone --recursive git@github.com:Naruto/simon-speck-net.git
```

## build

build simon-speck-net and nunit test.

```bash
cd /path/to/simon-speck-net
cd ./net
mono .nuget/nuget.exe restore
xbuild /p:TargetFrameworkVersion="v4.5" /p:Configuration=Release
mono ./packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe ./speckTest/bin/Release/speckTest.dll
```

or

```bash
cd /path/to/simon-speck-net
./scripts/build.sh
```

## create nuget package

```bash
cd /path/to/simon-speck-net
./scripts/create_nuget.sh
```

SimonSpeckNet nuget package file is outputted to `out/net` directory.

## create unity package

```bash
cd /path/to/simon-speck-net
./scripts/create_unitypackage.sh
```

SimonSpeckNet unitypackage file is outputted to `out/unity` directory.

## update native library

update simon-speck-c sumobule

```
git submodule update --init --recursive
```

### Android

```
export NDK_ROOT=/path/to/android-ndk-path
./scripts/plugins/deploy_android.sh
```

deploy each archtectures library files to `net/plugins/Android/libs`

### iOS
```
./scripts/pluginsdeploy_ios.sh
```

deploy fat library file to `net/plugins/iOS`

### Linux

```
./scripts/plugins/deploy_linux.sh
```
deploy .so file to `net/plugins/x64`

### macOS

```
./scripts/plugins/deploy_mac.sh
```

deploy .dylib and .bundle files to `net/plugins/x64`

### Windows
T.B.D

# License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net?ref=badge_large)
