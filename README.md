[![Build Status](https://travis-ci.org/Naruto/simon-speck-net.svg?branch=develop)](https://travis-ci.org/Naruto/simon-speck-net?branch=develop)
[![Windows Build status](https://ci.appveyor.com/api/projects/status/uulln99smanqi05k?svg=true)](https://ci.appveyor.com/project/Naruto/simon-speck-net)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/13827/badge.svg)](https://scan.coverity.com/projects/13827)
[![NuGet version](https://badge.fury.io/nu/simonspecknet.svg)](https://badge.fury.io/nu/simonspecknet)
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net?ref=badge_shield)
# simon-speck-net

c# binding of [Naruto/simon-speck-c](https://github.com/Naruto/simon-speck-c)

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


## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FNaruto%2Fsimon-speck-net?ref=badge_large)