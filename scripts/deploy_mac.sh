#!/bin/bash -xe

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR} > /dev/null

# build speck for macos
pushd ./modules/simon-speck-c
./scripts/speck/build_mac.sh
popd > /dev/null # ./modules/simon-speck-c

OUTPUT_DIR=./net/plugins/x64
/bin/mkdir -p ./net/plugins/x64
/bin/cp ./modules/simon-speck-c/libs/macos/libspeck.dylib ${OUTPUT_DIR}
/bin/cp -r ./modules/simon-speck-c/libs/macos/speck.bundle ${OUTPUT_DIR}

popd > /dev/null
