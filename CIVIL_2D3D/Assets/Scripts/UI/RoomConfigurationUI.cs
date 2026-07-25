using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Civil2D3D.InteriorDesign;

namespace Civil2D3D.UI
{
    public class RoomConfigurationUI : MonoBehaviour
    {
        [Header("UI References")]
        public GameObject roomConfigPanel;
        public TMP_Dropdown roomTypeDropdown;
        public Transform furnitureListContainer;
        public GameObject furnitureItemPrefab;
        public Button applyButton;
        public Button cancelButton;
        public Button nextRoomButton;
        public Button previousRoomButton;
        public TextMeshProUGUI currentRoomLabel;
        public TextMeshProUGUI progressText;

        [Header("Furniture Options")]
        public Toggle bedToggle;
        public Toggle sofaToggle;
        public Toggle tableToggle;
        public Toggle wardrobeToggle;
        public Toggle tvToggle;
        public Toggle acToggle;
        public Toggle deskToggle;
        public Toggle diningTableToggle;

        [Header("Bed Configuration")]
        public GameObject bedConfigPanel;
        public TMP_Dropdown bedSizeDropdown;
        public Toggle bedWithHeadboardToggle;
        public Toggle bedWithStorageToggle;

        [Header("TV Configuration")]
        public GameObject tvConfigPanel;
        public TMP_Dropdown tvSizeDropdown;
        public Toggle tvWithStandToggle;
        public Toggle tvWallMountedToggle;

        [Header("Wardrobe Configuration")]
        public GameObject wardrobeConfigPanel;
        public TMP_Dropdown wardrobeSizeDropdown;
        public Toggle wardrobeWithMirrorToggle;

        private FloorPlan currentFloorPlan;
        private int currentRoomIndex = 0;
        private List<RoomConfiguration> roomConfigurations = new List<RoomConfiguration>();

        public event Action<RoomConfiguration> onRoomConfigurationApplied;
        public event Action onAllRoomsConfigured;

        private void Start()
        {
            InitializeUI();
            Hide();
        }

        private void InitializeUI()
        {
            // Setup room type dropdown
            roomTypeDropdown.ClearOptions();
            roomTypeDropdown.AddOptions(new List<string> { 
                "Bedroom", "Living Room", "Kitchen", "Bathroom", 
                "Dining Room", "Office", "Study", "Guest Room"
            });

            // Setup bed size dropdown
            bedSizeDropdown.ClearOptions();
            bedSizeDropdown.AddOptions(new List<string> { "Single", "Double", "King" });

            // Setup TV size dropdown
            tvSizeDropdown.ClearOptions();
            tvSizeDropdown.AddOptions(new List<string> { "32\"", "43\"", "55\"", "65\"" });

            // Setup wardrobe size dropdown
            wardrobeSizeDropdown.ClearOptions();
            wardrobeSizeDropdown.AddOptions(new List<string> { "Small (2ft)", "Medium (3ft)", "Large (4ft)" });

            // Setup button listeners
            if (applyButton != null)
                applyButton.onClick.AddListener(ApplyCurrentRoomConfiguration);

            if (cancelButton != null)
                cancelButton.onClick.AddListener(Hide);

            if (nextRoomButton != null)
                nextRoomButton.onClick.AddListener(NextRoom);

            if (previousRoomButton != null)
                previousRoomButton.onClick.AddListener(PreviousRoom);

            // Setup furniture toggle listeners
            SetupFurnitureToggleListeners();

            // Hide configuration panels initially
            if (bedConfigPanel != null)
                bedConfigPanel.SetActive(false);

            if (tvConfigPanel != null)
                tvConfigPanel.SetActive(false);

            if (wardrobeConfigPanel != null)
                wardrobeConfigPanel.SetActive(false);
        }

        private void SetupFurnitureToggleListeners()
        {
            if (bedToggle != null)
                bedToggle.onValueChanged.AddListener(OnBedToggleChanged);

            if (tvToggle != null)
                tvToggle.onValueChanged.AddListener(OnTVToggleChanged);

            if (wardrobeToggle != null)
                wardrobeToggle.onValueChanged.AddListener(OnWardrobeToggleChanged);
        }

        public void Show(FloorPlan floorPlan)
        {
            currentFloorPlan = floorPlan;
            currentRoomIndex = 0;
            roomConfigurations.Clear();

            if (currentFloorPlan != null && currentFloorPlan.Rooms.Count > 0)
            {
                LoadRoomConfiguration(currentRoomIndex);
                Show();
            }
            else
            {
                Debug.LogWarning("No rooms found in floor plan");
            }
        }

        private void LoadRoomConfiguration(int roomIndex)
        {
            if (currentFloorPlan == null || roomIndex >= currentFloorPlan.Rooms.Count)
                return;

            Room currentRoom = currentFloorPlan.Rooms[roomIndex];
            
            // Update UI
            if (currentRoomLabel != null)
                currentRoomLabel.text = $"Room {roomIndex + 1}: {currentRoom.Name}";

            if (progressText != null)
                progressText.text = $"{roomIndex + 1} / {currentFloorPlan.Rooms.Count}";

            // Set room type
            string roomType = GetRoomTypeDisplayName(currentRoom.Type);
            int dropdownIndex = roomTypeDropdown.options.FindIndex(option => option.text == roomType);
            if (dropdownIndex >= 0)
                roomTypeDropdown.value = dropdownIndex;

            // Load existing configuration or create new one
            RoomConfiguration config = GetOrCreateRoomConfiguration(currentRoom);
            LoadConfigurationToUI(config);

            // Update navigation buttons
            UpdateNavigationButtons();
        }

        private RoomConfiguration GetOrCreateRoomConfiguration(Room room)
        {
            // Check if we already have a configuration for this room
            foreach (var config in roomConfigurations)
            {
                if (config.roomName == room.Name)
                {
                    return config;
                }
            }

            // Create new configuration
            RoomConfiguration newConfig = new RoomConfiguration
            {
                roomName = room.Name,
                roomType = room.Type
            };

            roomConfigurations.Add(newConfig);
            return newConfig;
        }

        private void LoadConfigurationToUI(RoomConfiguration config)
        {
            // Reset all toggles
            if (bedToggle != null) bedToggle.isOn = config.hasBed;
            if (sofaToggle != null) sofaToggle.isOn = config.hasSofa;
            if (tableToggle != null) tableToggle.isOn = config.hasTable;
            if (wardrobeToggle != null) wardrobeToggle.isOn = config.hasWardrobe;
            if (tvToggle != null) tvToggle.isOn = config.hasTV;
            if (acToggle != null) acToggle.isOn = config.hasAC;
            if (deskToggle != null) deskToggle.isOn = config.hasDesk;
            if (diningTableToggle != null) diningTableToggle.isOn = config.hasDiningTable;

            // Load bed configuration
            if (bedSizeDropdown != null)
            {
                int bedSizeIndex = GetBedSizeIndex(config.bedSize);
                if (bedSizeIndex >= 0)
                    bedSizeDropdown.value = bedSizeIndex;
            }

            if (bedWithHeadboardToggle != null)
                bedWithHeadboardToggle.isOn = config.bedWithHeadboard;

            if (bedWithStorageToggle != null)
                bedWithStorageToggle.isOn = config.bedWithStorage;

            // Load TV configuration
            if (tvSizeDropdown != null)
            {
                int tvSizeIndex = GetTVSizeIndex(config.tvSize);
                if (tvSizeIndex >= 0)
                    tvSizeDropdown.value = tvSizeIndex;
            }

            if (tvWithStandToggle != null)
                tvWithStandToggle.isOn = config.tvWithStand;

            if (tvWallMountedToggle != null)
                tvWallMountedToggle.isOn = config.tvWallMounted;

            // Load wardrobe configuration
            if (wardrobeSizeDropdown != null)
            {
                int wardrobeSizeIndex = GetWardrobeSizeIndex(config.wardrobeSize);
                if (wardrobeSizeIndex >= 0)
                    wardrobeSizeDropdown.value = wardrobeSizeIndex;
            }

            if (wardrobeWithMirrorToggle != null)
                wardrobeWithMirrorToggle.isOn = config.wardrobeWithMirror;
        }

        private void ApplyCurrentRoomConfiguration()
        {
            if (currentFloorPlan == null || currentRoomIndex >= currentFloorPlan.Rooms.Count)
                return;

            Room currentRoom = currentFloorPlan.Rooms[currentRoomIndex];
            RoomConfiguration config = GetOrCreateRoomConfiguration(currentRoom);

            // Save configuration from UI
            config.roomType = GetRoomTypeFromDropdown();
            config.hasBed = bedToggle != null && bedToggle.isOn;
            config.hasSofa = sofaToggle != null && sofaToggle.isOn;
            config.hasTable = tableToggle != null && tableToggle.isOn;
            config.hasWardrobe = wardrobeToggle != null && wardrobeToggle.isOn;
            config.hasTV = tvToggle != null && tvToggle.isOn;
            config.hasAC = acToggle != null && acToggle.isOn;
            config.hasDesk = deskToggle != null && deskToggle.isOn;
            config.hasDiningTable = diningTableToggle != null && diningTableToggle.isOn;

            // Save bed configuration
            config.bedSize = GetBedSizeFromDropdown();
            config.bedWithHeadboard = bedWithHeadboardToggle != null && bedWithHeadboardToggle.isOn;
            config.bedWithStorage = bedWithStorageToggle != null && bedWithStorageToggle.isOn;

            // Save TV configuration
            config.tvSize = GetTVSizeFromDropdown();
            config.tvWithStand = tvWithStandToggle != null && tvWithStandToggle.isOn;
            config.tvWallMounted = tvWallMountedToggle != null && tvWallMountedToggle.isOn;

            // Save wardrobe configuration
            config.wardrobeSize = GetWardrobeSizeFromDropdown();
            config.wardrobeWithMirror = wardrobeWithMirrorToggle != null && wardrobeWithMirrorToggle.isOn;

            // Update room type
            currentRoom.Type = config.roomType;

            // Trigger event
            onRoomConfigurationApplied?.Invoke(config);

            // Move to next room or finish
            if (currentRoomIndex < currentFloorPlan.Rooms.Count - 1)
            {
                NextRoom();
            }
            else
            {
                onAllRoomsConfigured?.Invoke();
                Hide();
            }
        }

        private void NextRoom()
        {
            if (currentRoomIndex < currentFloorPlan.Rooms.Count - 1)
            {
                currentRoomIndex++;
                LoadRoomConfiguration(currentRoomIndex);
            }
        }

        private void PreviousRoom()
        {
            if (currentRoomIndex > 0)
            {
                currentRoomIndex--;
                LoadRoomConfiguration(currentRoomIndex);
            }
        }

        private void UpdateNavigationButtons()
        {
            if (previousRoomButton != null)
                previousRoomButton.interactable = currentRoomIndex > 0;

            if (nextRoomButton != null)
                nextRoomButton.interactable = currentRoomIndex < currentFloorPlan.Rooms.Count - 1;

            if (applyButton != null)
                applyButton.GetComponentInChildren<TextMeshProUGUI>().text = 
                    (currentRoomIndex == currentFloorPlan.Rooms.Count - 1) ? "Finish" : "Apply & Next";
        }

        private string GetRoomTypeDisplayName(string roomType)
        {
            switch (roomType)
            {
                case "Bedroom": return "Bedroom";
                case "LivingRoom": return "Living Room";
                case "Kitchen": return "Kitchen";
                case "Bathroom": return "Bathroom";
                case "DiningRoom": return "Dining Room";
                case "Office": return "Office";
                case "Study": return "Study";
                case "GuestRoom": return "Guest Room";
                default: return roomType;
            }
        }

        private string GetRoomTypeFromDropdown()
        {
            string selected = roomTypeDropdown.options[roomTypeDropdown.value].text;
            
            switch (selected)
            {
                case "Bedroom": return "Bedroom";
                case "Living Room": return "LivingRoom";
                case "Kitchen": return "Kitchen";
                case "Bathroom": return "Bathroom";
                case "Dining Room": return "DiningRoom";
                case "Office": return "Office";
                case "Study": return "Study";
                case "Guest Room": return "GuestRoom";
                default: return selected.Replace(" ", "");
            }
        }

        private void OnBedToggleChanged(bool isOn)
        {
            if (bedConfigPanel != null)
                bedConfigPanel.SetActive(isOn);
        }

        private void OnTVToggleChanged(bool isOn)
        {
            if (tvConfigPanel != null)
                tvConfigPanel.SetActive(isOn);
        }

        private void OnWardrobeToggleChanged(bool isOn)
        {
            if (wardrobeConfigPanel != null)
                wardrobeConfigPanel.SetActive(isOn);
        }

        private int GetBedSizeIndex(string bedSize)
        {
            switch (bedSize)
            {
                case "Single": return 0;
                case "Double": return 1;
                case "King": return 2;
                default: return 0;
            }
        }

        private int GetTVSizeIndex(string tvSize)
        {
            switch (tvSize)
            {
                case "32\"": return 0;
                case "43\"": return 1;
                case "55\"": return 2;
                case "65\"": return 3;
                default: return 0;
            }
        }

        private int GetWardrobeSizeIndex(string wardrobeSize)
        {
            switch (wardrobeSize)
            {
                case "Small": return 0;
                case "Medium": return 1;
                case "Large": return 2;
                default: return 1;
            }
        }

        private string GetBedSizeFromDropdown()
        {
            return bedSizeDropdown.options[bedSizeDropdown.value].text;
        }

        private string GetTVSizeFromDropdown()
        {
            return tvSizeDropdown.options[tvSizeDropdown.value].text;
        }

        private string GetWardrobeSizeFromDropdown()
        {
            return wardrobeSizeDropdown.options[wardrobeSizeDropdown.value].text;
        }

        public void Show()
        {
            if (roomConfigPanel != null)
                roomConfigPanel.SetActive(true);
        }

        public void Hide()
        {
            if (roomConfigPanel != null)
                roomConfigPanel.SetActive(false);
        }

        public List<RoomConfiguration> GetRoomConfigurations()
        {
            return roomConfigurations;
        }

        public bool IsConfiguring()
        {
            return roomConfigPanel != null && roomConfigPanel.activeInHierarchy;
        }
    }

    [Serializable]
    public class RoomConfiguration
    {
        public string roomName;
        public string roomType;

        // Furniture requirements
        public bool hasBed;
        public bool hasSofa;
        public bool hasTable;
        public bool hasWardrobe;
        public bool hasTV;
        public bool hasAC;
        public bool hasDesk;
        public bool hasDiningTable;

        // Bed configuration
        public string bedSize = "Single";
        public bool bedWithHeadboard;
        public bool bedWithStorage;

        // TV configuration
        public string tvSize = "32\"";
        public bool tvWithStand;
        public bool tvWallMounted;

        // Wardrobe configuration
        public string wardrobeSize = "Medium";
        public bool wardrobeWithMirror;
    }
}
