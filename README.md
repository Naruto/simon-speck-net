# simon-speck-net

c# binding of [Naruto/simon-speck-c](https://github.com/Naruto/simon-speck-c)

# preapre

- mono

# get source

```bash
git clone --recursive git@github.com:Naruto/simon-speck-net.git
```

# build

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


# build unity package

```bash
cd /path/to/simon-speck-net
./scripts/create_unitypackage.sh
```

SimonSpeckNet unitypackage is outputted to `out` directory.

