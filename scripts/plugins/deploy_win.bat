@echo off
setlocal

set SCRIPTDIR=%~dp0
set BASEDIR=%SCRIPTDIR%..\..\
set OUTPUT_DIR=net\plugins

cd %BASEDIR%

@echo on

cd modules\simon-speck-c
call scripts\speck\build_win.bat
cd %BASEDIR%

mkdir %OUTPUT_DIR%
copy modules\simon-speck-c\libs\windows\win64\speck.dll %OUTPUT_DIR%\x64
