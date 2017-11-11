@echo off
setlocal

set SCRIPTDIR=%~dp0
set BASEDIR=%SCRIPTDIR%..
set OUTPUT_DIR=net\plugins

cd %BASEDIR%

@echo on
cd %BASEDIR%\net
.nuget\nuget.exe restore
msbuild /target:clean
msbuild /p:TargetFrameworkVersion="v4.5" /p:Configuration=Release
packages\NUnit.ConsoleRunner.3.7.0\tools\nunit3-console.exe speckTest\bin\Release\speckTest.dll
cd %BASEDIR%
