@echo off
rem Builds 1.5 and 1.6 from Source\; RimWorld 1.7 from Source17\ (Waylith port).
rem Needs the free .NET SDK and an internet connection the first time.
set "ROOT=%~dp0"

where dotnet >nul 2>nul
if errorlevel 1 (
  echo The .NET SDK is not installed.
  echo Download ".NET SDK" from https://dotnet.microsoft.com/download , install it, then run this file again.
  pause
  exit /b 1
)

if exist "%ROOT%Assemblies" rmdir /s /q "%ROOT%Assemblies"

cd /d "%ROOT%Source"
if exist "obj" rmdir /s /q "obj"

for %%V in (1.6 1.5) do (
  echo.
  echo ===== Building RimWorld %%V from Source =====
  dotnet build -c Release -nologo -p:RimWorldVersion=%%V
  if errorlevel 1 (
    echo.
    echo BUILD FAILED for RimWorld %%V. See the errors above.
    pause
    exit /b 1
  )
)

cd /d "%ROOT%Source17"
if exist "obj" rmdir /s /q "obj"

echo.
echo ===== Building RimWorld 1.7 from Source17 =====
dotnet build -c Release -nologo
if errorlevel 1 (
  echo.
  echo BUILD FAILED for RimWorld 1.7. See the errors above.
  pause
  exit /b 1
)

echo.
echo Done. 1.5\Assemblies, 1.6\Assemblies, and 1.7\Assemblies are ready. Start RimWorld and enable the mod.
pause
