@echo off
echo ========================================
echo Civil 2D3D Desktop Setup
echo ========================================
echo.

:: Check if Unity is installed
where Unity >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Unity not found!
    echo Please install Unity from unity.com first
    echo.
    echo Download Unity Hub: https://unity.com/download
    echo Install Unity Editor 2023.3.0f1 or newer
    echo.
    pause
    exit /b 1
)

:: Create build directory
echo Creating build directory...
if not exist "C:\Civil2D3D" mkdir "C:\Civil2D3D"

:: Build application
echo.
echo Building Civil 2D3D application...
echo This may take several minutes...
echo.

:: Get current directory
set PROJECT_PATH=%~dp0

:: Build in batch mode
start /wait "" "Unity" -batchmode -quit -projectPath "%PROJECT_PATH%" -buildWindowsPlayer "C:\Civil2D3D\Civil2D3D.exe" -logFile

:: Check if build was successful
if not exist "C:\Civil2D3D\Civil2D3D.exe" (
    echo.
    echo ERROR: Build failed!
    echo.
    echo Possible solutions:
    echo 1. Check Unity console for errors
    echo 2. Make sure all required packages are installed
    echo 3. Verify project settings
    echo.
    echo For manual build instructions, read DESKTOP_SETUP.md
    echo.
    pause
    exit /b 1
)

:: Create desktop shortcut
echo.
echo Creating desktop shortcut...
powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%USERPROFILE%\Desktop\Civil 2D3D.lnk'); $Shortcut.TargetPath = 'C:\Civil2D3D\Civil2D3D.exe'; $Shortcut.WorkingDirectory = 'C:\Civil2D3D'; $Shortcut.IconLocation = 'C:\Civil2D3D\Civil2D3D.exe, 0'; $Shortcut.Save()"

:: Create start menu entry
echo Creating Start Menu entry...
if not exist "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D" mkdir "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D"
powershell -Command "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D\Civil 2D3D.lnk'); $Shortcut.TargetPath = 'C:\Civil2D3D\Civil2D3D.exe'; $Shortcut.WorkingDirectory = 'C:\Civil2D3D'; $Shortcut.IconLocation = 'C:\Civil2D3D\Civil2D3D.exe, 0'; $Shortcut.Save()"

:: Create quick launch folder
echo Creating quick launch folder...
if not exist "C:\Civil2D3D\Projects" mkdir "C:\Civil2D3D\Projects"
if not exist "C:\Civil2D3D\Exports" mkdir "C:\Civil2D3D\Exports"
if not exist "C:\Civil2D3D\Temp" mkdir "C:\Civil2D3D\Temp"

:: Create README file
echo Creating user guide...
echo Civil 2D3D Architectural Visualizer > "C:\Civil2D3D\README.txt"
echo. >> "C:\Civil2D3D\README.txt"
echo Quick Start: >> "C:\Civil2D3D\README.txt"
echo 1. Double-click Civil2D3D.exe to launch >> "C:\Civil2D3D\README.txt"
echo 2. Click "New Project" to import 2D plan >> "C:\Civil2D3D\README.txt"
echo 3. Configure rooms and furniture >> "C:\Civil2D3D\README.txt"
echo 4. Generate 3D building >> "C:\Civil2D3D\README.txt"
echo 5. Edit and export results >> "C:\Civil2D3D\README.txt"
echo. >> "C:\Civil2D3D\README.txt"
echo For detailed help, visit the project folder: >> "C:\Civil2D3D\README.txt"
echo %PROJECT_PATH% >> "C:\Civil2D3D\README.txt"

echo.
echo ========================================
echo Setup Complete!
echo ========================================
echo.
echo Civil 2D3D has been installed on your desktop!
echo.
echo You can now run the application from:
echo - Desktop shortcut: "Civil 2D3D"
echo - Start Menu: Programs > Civil2D3D
echo - Direct file: C:\Civil2D3D\Civil2D3D.exe
echo.
echo Project files will be saved in: C:\Civil2D3D\Projects\
echo Exported files will be saved in: C:\Civil2D3D\Exports\
echo.
echo Need help? Read DESKTOP_SETUP.md in the project folder
echo.
echo Enjoy using Civil 2D3D Architectural Visualizer!
echo.

:: Ask if user wants to launch now
set /p launch="Launch Civil 2D3D now? (Y/N): "
if /i "%launch%"=="Y" (
    echo.
    echo Launching Civil 2D3D...
    start "" "C:\Civil2D3D\Civil2D3D.exe"
)

echo.
echo Setup completed successfully!
pause
