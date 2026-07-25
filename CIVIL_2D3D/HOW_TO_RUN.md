# How to Run Civil 2D3D Application

## Prerequisites

### System Requirements
- **Operating System**: Windows 10/11 (64-bit)
- **Unity Version**: 2023.3.0f1 or higher
- **RAM**: 8GB minimum (16GB recommended)
- **Graphics Card**: Dedicated GPU with 4GB VRAM minimum
- **Storage**: 5GB free space

### Required Software
1. **Unity Hub** (latest version)
2. **Unity Editor 2023.3.0f1+**
3. **Visual Studio 2022** (for C# development)
4. **Git** (for version control)

## Step-by-Step Setup

### 1. Install Unity
1. Download and install Unity Hub from [unity.com](https://unity.com/)
2. Install Unity Editor 2023.3.0f1 or newer
3. Select "Windows Build Support" during installation

### 2. Open Project
1. Launch Unity Hub
2. Click "Open Project"
3. Navigate to: `c:\Users\Ankit\Desktop\PROJECTS\CIVIL_2D3D`
4. Click "Select Folder"

### 3. Install Required Packages
Open Unity Package Manager (Window > Package Manager) and install:

#### Essential Packages
- **Universal RP** (for high-quality rendering)
- **UI Toolkit** (for modern UI)
- **TextMeshPro** (for text rendering)

#### External Dependencies
1. **OpenCV for Unity**:
   - Download from Unity Asset Store or GitHub
   - Import into project via Assets > Import Package

2. **PDF Library** (iTextSharp or PDFium):
   - Download .NET PDF library
   - Place in `Assets/Plugins/` folder

### 4. Configure Project Settings

#### Rendering Settings
1. Go to Edit > Project Settings > Graphics
2. Set Scriptable Render Pipeline Settings to URP Asset
3. Configure URP for high-quality rendering

#### Scripting Settings
1. Go to Edit > Project Settings > Player
2. Set API Compatibility Level to .NET Standard 2.1
3. Enable "Allow 'unsafe' Code" for OpenCV integration

#### Build Settings
1. Go to File > Build Settings
2. Select "Windows" platform
3. Click "Switch Platform"

### 5. Create Main Scene
1. Create new scene: File > New Scene
2. Save as `MainScene` in `Assets/Scenes/`
3. Add Main Camera and Directional Light

### 6. Setup Main Application
1. Create empty GameObject named "MainApplication"
2. Attach `MainApplication.cs` script
3. Configure references in Inspector:
   - Drag script components to respective fields
   - Assign UI panels and buttons
   - Set up camera references

### 7. Create UI Canvas
1. Right-click Hierarchy > UI > Canvas
2. Set Canvas Scaler to "Scale With Screen Size"
3. Reference Resolution: 1920x1080
4. Create UI panels:
   - MainMenuPanel
   - EditingPanel
   - LoadingPanel
   - PropertiesPanel

### 8. Configure Materials
1. Create materials folder: `Assets/Materials/`
2. Create basic materials:
   - DefaultWallMaterial
   - DefaultFloorMaterial
   - DefaultCeilingMaterial
   - HighlightMaterial
   - SelectedMaterial

### 9. Test Application

#### In Unity Editor
1. Press Play button
2. Verify main menu appears
3. Test file import functionality
4. Check 3D generation

#### Build for Testing
1. File > Build Settings
2. Click "Build"
3. Choose build location
4. Run executable

## Running the Application

### Launch Options

#### Method 1: Unity Editor (Development)
1. Open project in Unity
2. Open MainScene
3. Press Play button
4. Application runs in editor

#### Method 2: Standalone Build
1. Build application (see above)
2. Navigate to build folder
3. Run `Civil2D3D.exe`

### Basic Workflow

#### 1. Start Application
- Launch from Unity Editor or standalone build
- Main menu appears with options

#### 2. Import 2D Plan
- Click "New Project" or "Open Project"
- Select PDF/image file
- System analyzes plan automatically

#### 3. Configure Rooms
- For each detected room:
  - Select room type (Bedroom, Kitchen, etc.)
  - Choose furniture requirements
  - Specify bed size, TV size, etc.

#### 4. Generate 3D Building
- Click "Generate Building"
- System creates 3D model with furniture
- View in 3D viewport

#### 5. Edit & Customize
- Select furniture items
- Drag to reposition
- Use property panel for precise adjustments
- Change materials and colors

#### 6. Export Results
- Click "Export" button
- Choose export format (images, 3D models)
- Specify output location
- Wait for rendering completion

## Troubleshooting

### Common Issues

#### Build Errors
- **Missing References**: Check all script references in Inspector
- **Compilation Errors**: Check console for specific error messages
- **Package Conflicts**: Ensure compatible package versions

#### Runtime Issues
- **Import Fails**: Check file format support and permissions
- **3D Generation Issues**: Verify OpenCV installation
- **Performance Issues**: Check graphics drivers and Unity quality settings

#### UI Issues
- **Panels Not Showing**: Check canvas hierarchy and references
- **Buttons Not Working**: Verify event listeners and references
- **Layout Problems**: Check canvas scaler settings

### Debug Mode
1. Enable debug mode in MainApplication inspector
2. Check console for detailed error messages
3. Use Unity Profiler for performance analysis

## Performance Optimization

### Recommended Settings
- **Quality Settings**: Medium to High
- **Resolution**: 1920x1080 or higher
- **Anti-Aliasing**: 4x or 8x
- **Shadow Quality**: Medium or High

### For Large Projects
- Enable LOD (Level of Detail) for complex models
- Use occlusion culling for interior scenes
- Optimize texture sizes and compression

## Next Steps

### Advanced Features
- Install AI/ML packages for smart suggestions
- Add VR support packages
- Configure cloud save functionality

### Customization
- Create custom materials and textures
- Add furniture prefabs to library
- Develop custom elevation styles

---

**Support**: For technical issues, check Unity console or contact development team.
