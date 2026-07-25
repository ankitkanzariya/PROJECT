@echo off
echo ========================================
echo Civil 2D3D Uninstaller
echo ========================================
echo.

:: Confirm uninstallation
set /p confirm="Are you sure you want to uninstall Civil 2D3D? (Y/N): "
if /i not "%confirm%"=="Y" (
    echo Uninstallation cancelled.
    pause
    exit /b 0
)

echo.
echo Uninstalling Civil 2D3D...
echo.

:: Remove application directory
if exist "C:\Civil2D3D" (
    echo Removing application files...
    rmdir /s /q "C:\Civil2D3D"
    echo Application files removed.
) else (
    echo Application directory not found.
)

:: Remove desktop shortcut
if exist "%USERPROFILE%\Desktop\Civil 2D3D.lnk" (
    echo Removing desktop shortcut...
    del "%USERPROFILE%\Desktop\Civil 2D3D.lnk"
    echo Desktop shortcut removed.
) else (
    echo Desktop shortcut not found.
)

:: Remove start menu entry
if exist "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D\Civil 2D3D.lnk" (
    echo Removing Start Menu entry...
    del "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D\Civil 2D3D.lnk"
    rmdir "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D" 2>nul
    echo Start Menu entry removed.
) else (
    echo Start Menu entry not found.
)

:: Remove taskbar shortcut (if exists)
if exist "%APPDATA%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar\Civil 2D3D.lnk" (
    echo Removing taskbar shortcut...
    del "%APPDATA%\Microsoft\Internet Explorer\Quick Launch\User Pinned\TaskBar\Civil 2D3D.lnk"
    echo Taskbar shortcut removed.
)

:: Clean up registry entries (optional)
echo Cleaning up registry entries...
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.pdf\UserChoice" /f 2>nul
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.jpg\UserChoice" /f 2>nul
reg delete "HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\.png\UserChoice" /f 2>nul

echo.
echo ========================================
echo Uninstallation Complete!
echo ========================================
echo.
echo Civil 2D3D has been completely removed from your system.
echo.
echo Note: Your project files and exports in:
echo - C:\Civil2D3D\Projects\
echo - C:\Civil2D3D\Exports\
echo have been deleted along with the application.
echo.
echo If you want to keep your work, make sure to backup
echo these folders before uninstalling next time.
echo.
echo Thank you for using Civil 2D3D Architectural Visualizer!
echo.
pause
