#!/bin/bash -xe

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR} > /dev/null

# build speck for iOS
pushd ./modules/simon-speck-c
./scripts/speck/build_ios.sh
popd > /dev/null # ./modules/simon-speck-c

OUTPUT_DIR=./net/plugins/iOS
/bin/rm -rf ./net/plugins/iOS
/bin/mkdir -p ./net/plugins/iOS
/bin/cp ./modules/simon-speck-c/libs/ios/libspeck.a ${OUTPUT_DIR}

popd > /dev/null
