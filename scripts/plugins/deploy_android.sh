#!/bin/bash -xe

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

pushd ${BASEDIR} > /dev/null

# build speck for macos
pushd ./modules/simon-speck-c
./scripts/speck/build_android.sh
popd > /dev/null # ./modules/simon-speck-c

OUTPUT_DIR=./net/plugins/Android/libs
/bin/rm -rf ${OUTPUT_DIR}
/bin/mkdir -p ${OUTPUT_DIR}
/bin/cp -r ./modules/simon-speck-c/libs/android/* ${OUTPUT_DIR}

popd > /dev/null
