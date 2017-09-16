#!/bin/sh -ex

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR} > /dev/null
pushd ./net > /dev/null
mono .nuget/nuget.exe restore
xbuild /p:TargetFrameworkVersion="v4.5"
popd > /dev/null # ./net
popd > /dev/null # ${BASEDIR}
