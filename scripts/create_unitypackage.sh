#!/bin/sh -ex

SCRIPTDIR=`dirname $0`
SCRIPTDIR=`cd $SCRIPTDIR && pwd -P`
BASEDIR=${SCRIPTDIR}/..
BASEDIR=`cd ${BASEDIR} && pwd -P`

UNITY_APP=/Applications/Unity/Unity.app

pushd ${BASEDIR} > /dev/null
/bin/rm -rf build_unity
/bin/mkdir -p build_unity

src_array=`find ${BASEDIR}/net/SimonSpeckNet/Speck -name '*.cs'`

COMPILER=${UNITY_APP}/Contents/MonoBleedingEdge/bin/mcs
${COMPILER} -sdk:2 -r:${UNITY_APP}/Contents/Managed/UnityEngine.dll -r:${UNITY_APP}/Contents/Managed/UnityEditor.dll -target:library -optimize+ -out:SimonSpeckNet_Static.dll -unsafe+ -define:LIBSPECK_STATIC ${src_array}
${COMPILER} -sdk:2 -r:${UNITY_APP}/Contents/Managed/UnityEngine.dll -r:${UNITY_APP}/Contents/Managed/UnityEditor.dll -target:library -optimize+ -out:SimonSpeckNet.dll -unsafe+ ${src_array}

pushd build_unity > /dev/null
/bin/mkdir -p Assets
/bin/mkdir -p Assets/Plugins

# copy native libraries
/bin/cp -r ${BASEDIR}/net/plugins/* Assets/Plugins
# copy dll files
/bin/cp -r ${BASEDIR}/SimonSpeckNet_Static.dll Assets/Plugins/iOS/
/bin/cp -r ${BASEDIR}/SimonSpeckNet.dll Assets/Plugins/Android/
/bin/cp -r ${BASEDIR}/SimonSpeckNet.dll Assets/Plugins/x64/
/bin/rm -f ${BASEDIR}/SimonSpeckNet_Static.dll ${BASEDIR}/SimonSpeckNet.dll

# remove stub files
/bin/rm -rf Assets/Plugins/Android/libs/arm64-v8a
/bin/rm -rf Assets/Plugins/Android/libs/armeabi
/bin/rm -rf Assets/Plugins/Android/libs/x86_64
/bin/rm -rf Assets/Plugins/x64/libspeck.dylib

# copy meta files
/bin/cp -r ${BASEDIR}/unity/SimonSpeckNet.dll.meta Assets/Plugins/x64/
/bin/cp -r ${BASEDIR}/unity/libspeck.so.meta Assets/Plugins/x64/
/bin/cp -r ${BASEDIR}/unity/speck.bundle.meta Assets/Plugins/x64/

/bin/cp -r ${BASEDIR}/unity/SpeckSample Assets/

# create unity package
${UNITY_APP}/Contents/MacOS/Unity -quit -batchmode -nographics -projectPath $(pwd) -logFile /dev/stdout -exportPackage Assets/Plugins Assets/SpeckSample SimonSpeckForUnity.unitypackage

popd > /dev/null # build_unity

# 
/bin/rm -rf out
/bin/mkdir -p out
/bin/cp build_unity/SimonSpeckForUnity.unitypackage out/

popd > /dev/null # ${BASEDIR}
