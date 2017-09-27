#!/bin/sh -ex

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR} > /dev/null
# clean nupkg
/bin/rm -rf ./net/SimonSpeckNet.*.nupkg

# build library
./scripts/build.sh

pushd ./net > /dev/null
# pack nuget
mono .nuget/nuget.exe pack NuGet/SimonSpeckNet.nuspec -BasePath $(pwd)
popd > /dev/null # ./net

/bin/rm -rf out/net
/bin/mkdir -p out/net
cp net/SimonSpeckNet.*.nupkg ./out/net/
popd > /dev/null # ${BASEDIR}
