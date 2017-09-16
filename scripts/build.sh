#!/bin/sh -ex

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR}/net > /dev/null
mono .nuget/nuget.exe restore
xbuild /p:TargetFrameworkVersion="v4.5"
mono ./packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe ./speckTest/bin/Debug/speckTest.dll
popd > /dev/null # ${BASEDIR}
