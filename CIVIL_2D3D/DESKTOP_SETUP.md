# Desktop Setup Guide - Civil 2D3D Application

## Option 1: Quick Desktop Installation (Recommended)

### Step 1: Build the Application
1. Open Unity Hub
2. Open the project: `c:\Users\Ankit\Desktop\PROJECTS\CIVIL_2D3D`
3. Go to **File > Build Settings**
4. Select **Windows** platform
5. Click **Switch Platform** (if not already selected)
6. Click **Build**
7. Choose location: `C:\Civil2D3D\` (create this folder)
8. Wait for build to complete

### Step 2: Create Desktop Shortcut
1. Navigate to `C:\Civil2D3D\`
2. Find `Civil2D3D.exe`
3. Right-click on `Civil2D3D.exe`
4. Select **Send to > Desktop (create shortcut)**
5. Rename shortcut to "Civil 2D3D Architectural Visualizer"

### Step 3: Run from Desktop
- Double-click the desktop shortcut
- Application will launch directly

---

## Option 2: Portable Installation (USB/External Drive)

### Step 1: Build Portable Version
1. Follow steps 1-8 from Option 1
2. Copy the entire `C:\Civil2D3D\` folder to USB drive
3. Create shortcut on desktop pointing to USB drive location

### Step 2: Run from USB
- Insert USB drive
- Double-click the shortcut
- Application runs without installation

---

## Option 3: Developer Setup (Run from Source)

### Prerequisites
- Unity 2023.3.0f1 or higher
- Visual Studio 2022
- Windows 10/11

### Setup Steps
1. **Double-click `QUICK_START.bat`** in project folder
2. Unity will open with the project
3. Press **Play** button to run
4. For desktop access, create a shortcut to `QUICK_START.bat`

---

## Automatic Setup Script

### One-Click Desktop Setup
1. Download and run: `SETUP_DESKTOP.bat` (create this file)
2. Script will automatically:
   - Build the application
   - Create desktop shortcut
   - Set up file associations
   - Create start menu entry

### Create SETUP_DESKTOP.bat
```batch
@echo off
echo ========================================
echo Civil 2D3D Desktop Setup
echo ========================================
echo.

:: Check if Unity is installed
where Unity >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ERROR: Unity not found!
    echo Please install Unity first
    pause
    exit /b 1
)

:: Create build directory
if not exist "C:\Civil2D3D" mkdir "C:\Civil2D3D"

:: Build application
echo Building Civil 2D3D application...
start /wait "" "Unity" -batchmode -quit -projectPath "%~dp0" -buildWindowsPlayer "C:\Civil2D3D\Civil2D3D.exe" -logFile

if not exist "C:\Civil2D3D\Civil2D3D.exe" (
    echo ERROR: Build failed!
    pause
    exit /b 1
)

:: Create desktop shortcut
echo Creating desktop shortcut...
powershell "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%USERPROFILE%\Desktop\Civil 2D3D.lnk'); $Shortcut.TargetPath = 'C:\Civil2D3D\Civil2D3D.exe'; $Shortcut.Save()"

:: Create start menu entry
if not exist "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D" mkdir "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D"
powershell "$WshShell = New-Object -comObject WScript.Shell; $Shortcut = $WshShell.CreateShortcut('%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D\Civil 2D3D.lnk'); $Shortcut.TargetPath = 'C:\Civil2D3D\Civil2D3D.exe'; $Shortcut.Save()"

echo.
echo ========================================
echo Setup Complete!
echo ========================================
echo.
echo You can now run Civil 2D3D from:
echo - Desktop shortcut
echo - Start Menu > Civil2D3D
echo - Direct: C:\Civil2D3D\Civil2D3D.exe
echo.
pause
```

---

## Manual Desktop Installation Steps

### Step 1: Install Unity (if not installed)
1. Download Unity Hub from unity.com
2. Install Unity Editor 2023.3.0f1
3. Select "Windows Build Support"

### Step 2: Build Application
1. Open Unity Hub
2. Click "Add" > "Add project from disk"
3. Select: `c:\Users\Ankit\Desktop\PROJECTS\CIVIL_2D3D`
4. Wait for project to load
5. Go to **File > Build Settings**
6. Select **Windows**
7. Click **Build**
8. Save as: `C:\Civil2D3D\Civil2D3D.exe`

### Step 3: Create Desktop Access
#### Method A: Desktop Shortcut
1. Go to `C:\Civil2D3D\`
2. Right-click `Civil2D3D.exe`
3. **Send to > Desktop (create shortcut)**

#### Method B: Start Menu
1. Right-click `Civil2D3D.exe`
2. **Pin to Start**
3. Or drag to Start Menu

#### Method C: Taskbar
1. Right-click desktop shortcut
2. **Pin to taskbar**

---

## File Associations (Optional)

### Associate File Types
1. Right-click any PDF/image file
2. **Open with > Choose another app**
3. Select `Civil2D3D.exe`
4. Check "Always use this app"
5. Click **OK**

### Supported File Types
- PDF files
- JPG, PNG, BMP images
- DWG, DXF CAD files (if supported)

---

## Troubleshooting

### Build Issues
- **Unity not found**: Install Unity Hub and Unity Editor
- **Build fails**: Check Unity console for errors
- **Missing dependencies**: Install required packages

### Runtime Issues
- **Application won't start**: Run as Administrator
- **Missing DLLs**: Install Microsoft Visual C++ Redistributable
- **Graphics issues**: Update graphics drivers

### Performance Issues
- **Slow startup**: Disable antivirus scanning of app folder
- **Lagging**: Lower graphics quality in settings
- **Crashes**: Check system requirements

---

## System Requirements

### Minimum Requirements
- **OS**: Windows 10 (64-bit)
- **CPU**: Intel i5 or AMD Ryzen 5
- **RAM**: 8GB
- **GPU**: GTX 1050 or equivalent
- **Storage**: 2GB free space

### Recommended Requirements
- **OS**: Windows 11 (64-bit)
- **CPU**: Intel i7 or AMD Ryzen 7
- **RAM**: 16GB
- **GPU**: GTX 1660 or better
- **Storage**: 5GB free space

---

## Update Process

### Method 1: Automatic Updates
1. Run `UPDATE_DESKTOP.bat` (create this)
2. Script downloads and installs updates

### Method 2: Manual Updates
1. Download new version
2. Replace `C:\Civil2D3D\` contents
3. Restart application

### Create UPDATE_DESKTOP.bat
```batch
@echo off
echo Updating Civil 2D3D...
echo.

:: Backup current version
if exist "C:\Civil2D3D\backup" rmdir /s /q "C:\Civil2D3D\backup"
move "C:\Civil2D3D" "C:\Civil2D3D\backup"

:: Build new version
start /wait "" "Unity" -batchmode -quit -projectPath "%~dp0" -buildWindowsPlayer "C:\Civil2D3D\Civil2D3D.exe"

echo Update complete!
pause
```

---

## Uninstallation

### Method 1: Manual Removal
1. Delete folder: `C:\Civil2D3D\`
2. Remove desktop shortcut
3. Remove start menu entry

### Method 2: Uninstall Script
Create `UNINSTALL.bat`:
```batch
@echo off
echo Uninstalling Civil 2D3D...
echo.

:: Remove application
if exist "C:\Civil2D3D" rmdir /s /q "C:\Civil2D3D"

:: Remove shortcuts
del "%USERPROFILE%\Desktop\Civil 2D3D.lnk" 2>nul
del "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D\Civil 2D3D.lnk" 2>nul
if exist "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D" rmdir "%APPDATA%\Microsoft\Windows\Start Menu\Programs\Civil2D3D"

echo Civil 2D3D uninstalled successfully!
pause
```

---

## Quick Start Summary

1. **Install Unity** (if needed)
2. **Open project** in Unity
3. **Build application** to `C:\Civil2D3D\`
4. **Create desktop shortcut**
5. **Run from desktop**

Your Civil 2D3D application is now ready to use directly from your desktop!
