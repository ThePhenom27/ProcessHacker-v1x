@echo off

build
if not %errorlevel%==0 goto end
copy i386\kprocesshacker.sys ..\ProcessHacker\bin\Release\
:end