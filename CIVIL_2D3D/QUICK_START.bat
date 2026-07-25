@echo off
echo ========================================
echo Civil 2D3D - Quick Start Script
echo ========================================
echo.

:: Check if Unity Hub is installed
where unity-hub >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Unity Hub not found!
    echo Please install Unity Hub from https://unity.com/
    echo.
    pause
    exit /b 1
)

:: Check if Unity Editor is installed
where Unity >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Unity Editor not found!
    echo Please install Unity Editor 2023.3.0f1 or newer
    echo.
    pause
    exit /b 1
)

:: Open project in Unity
echo Opening Civil 2D3D project in Unity...
echo.

:: Get current directory
set PROJECT_PATH=%~dp0

:: Launch Unity with project
start "" "Unity" -projectPath "%PROJECT_PATH%" -logFile

echo Project should open in Unity Editor...
echo.
echo Next steps:
echo 1. Wait for Unity to load the project
echo 2. Open MainScene from Assets/Scenes/
echo 3. Press Play button to run application
echo.
echo For detailed setup instructions, read HOW_TO_RUN.md
echo.

pause
