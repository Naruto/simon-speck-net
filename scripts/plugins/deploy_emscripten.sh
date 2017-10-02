#!/bin/bash -xe

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/../..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR} > /dev/null

# build speck by emscripten
pushd ./modules/simon-speck-c
./scripts/speck/build_emscripten.sh
popd > /dev/null # ./modules/simon-speck-c

OUTPUT_DIR=./net/plugins/WebGL
/bin/rm -rf ${OUTPUT_DIR}
/bin/mkdir -p ${OUTPUT_DIR}
unzip ./modules/simon-speck-c/libs/emscripten/libspeck.zip -d ${OUTPUT_DIR}

popd > /dev/null
