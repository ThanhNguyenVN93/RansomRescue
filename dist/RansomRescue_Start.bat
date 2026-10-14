@echo off
setlocal

set "REG_KEY=HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full"
set "MIN_RELEASE=394254"

for /f "tokens=3" %%v in ('reg query "%REG_KEY%" /v Release 2^>nul') do set "RELEASE=%%v"

if not defined RELEASE goto NO_DOTNET
if %RELEASE% GEQ %MIN_RELEASE% goto RUN_APP

:NO_DOTNET
echo.
echo  +------------------------------------------------------------------+
echo  ^|  RANSOMRESCUE -- Thieu .NET Framework / .NET Framework missing  ^|
echo  +------------------------------------------------------------------+
echo  ^|                                                                  ^|
echo  ^|  [Tieng Viet]                                                    ^|
echo  ^|  Tool nay can Microsoft .NET Framework 4.6.1 tro len.           ^|
echo  ^|  Tren may nay chua co hoac phien ban qua cu.                    ^|
echo  ^|                                                                  ^|
echo  ^|  Cach cai dat:                                                   ^|
echo  ^|  1. Dung may SACH khac, tai file offline installer (~66 MB):    ^|
echo  ^|     https://aka.ms/dotnet-461-installer                         ^|
echo  ^|     (File: NDP461-KB3102436-x86-x64-AllOS-ENU.exe)             ^|
echo  ^|  2. Copy file do vao USB nay, cai tren may bi nhiem.            ^|
echo  ^|  3. Khoi dong lai roi chay RansomRescue_Start.bat lai.          ^|
echo  ^|                                                                  ^|
echo  ^|  LUU Y: Windows 10 va 11 da co san .NET 4.6.1+.                ^|
echo  ^|  Neu dung Win10/11, thu chay thang RansomRescue.exe.            ^|
echo  ^|                                                                  ^|
echo  ^|  [English]                                                       ^|
echo  ^|  .NET Framework 4.6.1+ is required but not installed.           ^|
echo  ^|  From a clean PC, download the offline installer (~66 MB):      ^|
echo  ^|     https://aka.ms/dotnet-461-installer                         ^|
echo  ^|  Copy it to this USB, install on the infected machine,          ^|
echo  ^|  restart, then run RansomRescue_Start.bat again.                ^|
echo  ^|                                                                  ^|
echo  ^|  NOTE: Windows 10 and 11 already include .NET 4.6.1+.           ^|
echo  ^|  If on Win10/11, try running RansomRescue.exe directly.         ^|
echo  ^|                                                                  ^|
echo  +------------------------------------------------------------------+
echo.
pause
exit /b 1

:RUN_APP
set "SCRIPT_DIR=%~dp0"
set "EXE=%SCRIPT_DIR%RansomRescue.exe"

if not exist "%EXE%" (
    echo.
    echo  ERROR: RansomRescue.exe not found in this folder.
    echo  Expected: %EXE%
    echo.
    pause
    exit /b 1
)

start "" "%EXE%"
exit /b 0