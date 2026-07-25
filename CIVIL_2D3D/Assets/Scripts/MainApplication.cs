using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Civil2D3D
{
    public class MainApplication : MonoBehaviour
    {
        [Header("Core Components")]
        public PlanAnalyzer planAnalyzer;
        public BuildingGenerator buildingGenerator;
        public FurniturePlanner furniturePlanner;
        public FurnitureEditor furnitureEditor;
        public RenderExportManager exportManager;
        public FileImportManager fileImportManager;

        [Header("UI References")]
        public GameObject mainMenuPanel;
        public GameObject editingPanel;
        public GameObject loadingPanel;
        public Button newProjectButton;
        public Button openProjectButton;
        public Button saveProjectButton;
        public Button exportButton;
        public Button furnitureModeButton;
        public Button renderModeButton;
        public TextMeshProUGUI statusText;
        public Slider progressSlider;

        [Header("Application State")]
        public FloorPlan currentFloorPlan;
        public GameObject currentBuilding;
        public ApplicationMode currentMode = ApplicationMode.Menu;

        public enum ApplicationMode
        {
            Menu,
            Import,
            Editing,
            Furniture,
            Rendering,
            Export
        }

        private void Start()
        {
            InitializeApplication();
            SetupEventListeners();
            SwitchToMode(ApplicationMode.Menu);
        }

        private void InitializeApplication()
        {
            Debug.Log("Initializing Civil 2D3D Application...");

            // Find components if not assigned
            if (planAnalyzer == null)
                planAnalyzer = FindObjectOfType<PlanAnalyzer>();

            if (buildingGenerator == null)
                buildingGenerator = FindObjectOfType<BuildingGenerator>();

            if (furniturePlanner == null)
                furniturePlanner = FindObjectOfType<FurniturePlanner>();

            if (furnitureEditor == null)
                furnitureEditor = FindObjectOfType<FurnitureEditor>();

            if (exportManager == null)
                exportManager = FindObjectOfType<RenderExportManager>();

            if (fileImportManager == null)
                fileImportManager = FindObjectOfType<FileImportManager>();

            // Initialize components
            InitializeComponents();

            Debug.Log("Application initialized successfully");
        }

        private void InitializeComponents()
        {
            // Initialize furniture planner with default configurations
            if (furniturePlanner != null)
            {
                furniturePlanner.roomConfigurations = GetDefaultRoomConfigurations();
            }

            // Initialize export manager
            if (exportManager != null)
            {
                exportManager.SetBuildingParent(currentBuilding);
            }

            // Initialize furniture editor
            if (furnitureEditor != null)
            {
                furnitureEditor.SetCurrentFloorPlan(currentFloorPlan);
            }
        }

        private void SetupEventListeners()
        {
            // Main menu buttons
            if (newProjectButton != null)
                newProjectButton.onClick.AddListener(StartNewProject);

            if (openProjectButton != null)
                openProjectButton.onClick.AddListener(OpenProject);

            if (saveProjectButton != null)
                saveProjectButton.onClick.AddListener(SaveProject);

            if (exportButton != null)
                exportButton.onClick.AddListener(ExportProject);

            if (furnitureModeButton != null)
                furnitureModeButton.onClick.AddListener(() => SwitchToMode(ApplicationMode.Furniture));

            if (renderModeButton != null)
                renderModeButton.onClick.AddListener(() => SwitchToMode(ApplicationMode.Rendering));

            // File import events
            if (fileImportManager != null)
            {
                fileImportManager.onPlanImported.AddListener(OnPlanImported);
                fileImportManager.onImportError.AddListener(OnImportError);
            }
        }

        private void StartNewProject()
        {
            Debug.Log("Starting new project...");
            SwitchToMode(ApplicationMode.Import);
        }

        private void OpenProject()
        {
            Debug.Log("Opening existing project...");
            // Implementation for opening saved projects
            StartCoroutine(LoadProjectCoroutine());
        }

        private System.Collections.IEnumerator LoadProjectCoroutine()
        {
            UpdateStatus("Loading project...");
            SetLoadingActive(true);

            // Placeholder for project loading
            yield return new WaitForSeconds(1f);

            UpdateStatus("Project loaded successfully");
            SetLoadingActive(false);
            SwitchToMode(ApplicationMode.Editing);
        }

        private void SaveProject()
        {
            if (currentFloorPlan == null)
            {
                UpdateStatus("No project to save");
                return;
            }

            Debug.Log("Saving project...");
            StartCoroutine(SaveProjectCoroutine());
        }

        private System.Collections.IEnumerator SaveProjectCoroutine()
        {
            UpdateStatus("Saving project...");
            SetLoadingActive(true);

            // Save floor plan data
            string projectData = SerializeFloorPlan(currentFloorPlan);
            string fileName = $"Project_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, "Projects", fileName);

            // Ensure directory exists
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));

            // Write project file
            System.IO.File.WriteAllText(filePath, projectData);

            yield return new WaitForSeconds(0.5f);

            UpdateStatus($"Project saved to: {fileName}");
            SetLoadingActive(false);
        }

        private void ExportProject()
        {
            if (currentBuilding == null)
            {
                UpdateStatus("No building to export");
                return;
            }

            Debug.Log("Exporting project...");
            SwitchToMode(ApplicationMode.Export);
            
            if (exportManager != null)
            {
                exportManager.RenderAllViews();
            }
        }

        private void OnPlanImported(FloorPlan plan)
        {
            Debug.Log("Plan imported successfully");
            currentFloorPlan = plan;
            
            UpdateStatus("Processing imported plan...");
            StartCoroutine(ProcessImportedPlan(plan));
        }

        private void OnImportError(string error)
        {
            Debug.LogError($"Import error: {error}");
            UpdateStatus($"Import failed: {error}");
            SwitchToMode(ApplicationMode.Menu);
        }

        private System.Collections.IEnumerator ProcessImportedPlan(FloorPlan plan)
        {
            UpdateStatus("Generating 3D building...");
            SetLoadingActive(true);

            yield return new WaitForSeconds(0.1f);

            // Generate 3D building
            if (buildingGenerator != null)
            {
                if (currentBuilding != null)
                {
                    Destroy(currentBuilding);
                }

                currentBuilding = buildingGenerator.Generate3D(plan);
                
                if (exportManager != null)
                {
                    exportManager.SetBuildingParent(currentBuilding);
                }
            }

            yield return new WaitForSeconds(0.1f);

            // Plan furniture
            UpdateStatus("Planning furniture layout...");
            if (furniturePlanner != null)
            {
                foreach (var room in plan.Rooms)
                {
                    furniturePlanner.PlanFurnitureForRoom(room);
                }
            }

            yield return new WaitForSeconds(0.1f);

            // Regenerate building with furniture
            if (buildingGenerator != null)
            {
                if (currentBuilding != null)
                {
                    Destroy(currentBuilding);
                }
                currentBuilding = buildingGenerator.Generate3D(plan);
            }

            yield return new WaitForSeconds(0.1f);

            UpdateStatus("Project ready for editing");
            SetLoadingActive(false);
            SwitchToMode(ApplicationMode.Editing);
        }

        private void SwitchToMode(ApplicationMode mode)
        {
            Debug.Log($"Switching to mode: {mode}");
            currentMode = mode;

            // Hide all panels
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);
            if (editingPanel != null)
                editingPanel.SetActive(false);
            if (loadingPanel != null)
                loadingPanel.SetActive(false);

            // Show appropriate panel and setup mode
            switch (mode)
            {
                case ApplicationMode.Menu:
                    if (mainMenuPanel != null)
                        mainMenuPanel.SetActive(true);
                    break;

                case ApplicationMode.Import:
                    // Import mode handled by FileImportManager
                    break;

                case ApplicationMode.Editing:
                    if (editingPanel != null)
                        editingPanel.SetActive(true);
                    EnableEditing(true);
                    break;

                case ApplicationMode.Furniture:
                    if (editingPanel != null)
                        editingPanel.SetActive(true);
                    EnableFurnitureMode(true);
                    break;

                case ApplicationMode.Rendering:
                    // Rendering mode handled by RenderExportManager
                    break;

                case ApplicationMode.Export:
                    // Export mode handled by RenderExportManager
                    break;
            }
        }

        private void EnableEditing(bool enable)
        {
            if (furnitureEditor != null)
            {
                furnitureEditor.EnableEditing(enable);
            }
        }

        private void EnableFurnitureMode(bool enable)
        {
            if (furnitureEditor != null)
            {
                furnitureEditor.EnableEditing(enable);
            }

            UpdateStatus(enable ? "Furniture editing mode enabled" : "Furniture editing mode disabled");
        }

        private void UpdateStatus(string message)
        {
            if (statusText != null)
            {
                statusText.text = message;
            }
            Debug.Log($"[Status] {message}");
        }

        private void SetLoadingActive(bool active)
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(active);
            }

            if (progressSlider != null)
            {
                progressSlider.gameObject.SetActive(active);
                if (active)
                {
                    StartCoroutine(AnimateProgressSlider());
                }
            }
        }

        private System.Collections.IEnumerator AnimateProgressSlider()
        {
            float progress = 0f;
            while (progressSlider != null && progressSlider.gameObject.activeInHierarchy)
            {
                progress += Time.deltaTime * 0.5f;
                if (progress > 1f) progress = 0f;
                progressSlider.value = progress;
                yield return null;
            }
        }

        private string SerializeFloorPlan(FloorPlan plan)
        {
            return JsonUtility.ToJson(plan, true);
        }

        private FloorPlan DeserializeFloorPlan(string json)
        {
            return JsonUtility.FromJson<FloorPlan>(json);
        }

        private FurniturePlanner.RoomConfiguration[] GetDefaultRoomConfigurations()
        {
            return new FurniturePlanner.RoomConfiguration[]
            {
                new FurniturePlanner.RoomConfiguration
                {
                    roomType = "Bedroom",
                    autoPlace = true,
                    requiredFurniture = new FurniturePlanner.FurnitureRequirement[]
                    {
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.Bed,
                            required = true,
                            placementHint = "against_wall"
                        },
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.Wardrobe,
                            required = true,
                            placementHint = "against_wall"
                        },
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.Desk,
                            required = false,
                            placementHint = "near_window"
                        }
                    }
                },
                new FurniturePlanner.RoomConfiguration
                {
                    roomType = "LivingRoom",
                    autoPlace = true,
                    requiredFurniture = new FurniturePlanner.FurnitureRequirement[]
                    {
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.Sofa,
                            required = true,
                            placementHint = "against_wall"
                        },
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.Table,
                            required = true,
                            placementHint = "center"
                        },
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.TV,
                            required = false,
                            placementHint = "against_wall"
                        }
                    }
                },
                new FurniturePlanner.RoomConfiguration
                {
                    roomType = "Kitchen",
                    autoPlace = true,
                    requiredFurniture = new FurniturePlanner.FurnitureRequirement[]
                    {
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.KitchenCounter,
                            required = true,
                            placementHint = "against_wall"
                        },
                        new FurniturePlanner.FurnitureRequirement
                        {
                            type = FurnitureType.Table,
                            required = false,
                            placementHint = "center"
                        }
                    }
                }
            };
        }

        public void RegenerateBuilding()
        {
            if (currentFloorPlan == null || buildingGenerator == null)
            {
                UpdateStatus("No floor plan to regenerate");
                return;
            }

            StartCoroutine(RegenerateBuildingCoroutine());
        }

        private System.Collections.IEnumerator RegenerateBuildingCoroutine()
        {
            UpdateStatus("Regenerating building...");
            SetLoadingActive(true);

            yield return new WaitForSeconds(0.1f);

            if (currentBuilding != null)
            {
                Destroy(currentBuilding);
            }

            currentBuilding = buildingGenerator.Generate3D(currentFloorPlan);
            
            if (exportManager != null)
            {
                exportManager.SetBuildingParent(currentBuilding);
            }

            yield return new WaitForSeconds(0.1f);

            UpdateStatus("Building regenerated successfully");
            SetLoadingActive(false);
        }

        public void ClearCurrentProject()
        {
            currentFloorPlan = null;
            
            if (currentBuilding != null)
            {
                Destroy(currentBuilding);
                currentBuilding = null;
            }

            if (exportManager != null)
            {
                exportManager.SetBuildingParent(null);
            }

            if (furnitureEditor != null)
            {
                furnitureEditor.SetCurrentFloorPlan(null);
            }

            UpdateStatus("Project cleared");
            SwitchToMode(ApplicationMode.Menu);
        }

        private void Update()
        {
            // Handle keyboard shortcuts
            HandleKeyboardShortcuts();
        }

        private void HandleKeyboardShortcuts()
        {
            // Ctrl+N - New project
            if (Input.GetKeyDown(KeyCode.N) && Input.GetKey(KeyCode.LeftControl))
            {
                StartNewProject();
            }

            // Ctrl+O - Open project
            if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftControl))
            {
                OpenProject();
            }

            // Ctrl+S - Save project
            if (Input.GetKeyDown(KeyCode.S) && Input.GetKey(KeyCode.LeftControl))
            {
                SaveProject();
            }

            // Ctrl+E - Export
            if (Input.GetKeyDown(KeyCode.E) && Input.GetKey(KeyCode.LeftControl))
            {
                ExportProject();
            }

            // Escape - Return to menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (currentMode != ApplicationMode.Menu)
                {
                    SwitchToMode(ApplicationMode.Menu);
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up resources
            if (currentBuilding != null)
            {
                Destroy(currentBuilding);
            }
        }
    }
}
