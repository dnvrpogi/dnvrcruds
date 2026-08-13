@echo off
REM Wrapper to run the PowerShell start script from cmd.exe
SET scriptDir=%~dp0
powershell -NoProfile -ExecutionPolicy Bypass -File "%scriptDir%start.ps1" %*