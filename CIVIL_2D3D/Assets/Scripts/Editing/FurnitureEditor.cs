using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Civil2D3D.Editing
{
    public class FurnitureEditor : MonoBehaviour
    {
        [Header("Editor Settings")]
        public LayerMask furnitureLayer;
        public LayerMask floorLayer;
        public float snapToGridSize = 0.1f;
        public float rotationSnapAngle = 45f;
        public bool enableGridSnapping = true;
        public bool enableRotationSnapping = true;

        [Header("Visual Feedback")]
        public Material highlightMaterial;
        public Material selectedMaterial;
        public Color validPlacementColor = Color.green;
        public Color invalidPlacementColor = Color.red;

        [Header("UI References")]
        public GameObject propertiesPanel;
        public UnityEngine.UI.Slider rotationSlider;
        public UnityEngine.UI.InputField xPositionInput;
        public UnityEngine.UI.InputField yPositionInput;
        public UnityEngine.UI.InputField zPositionInput;
        public UnityEngine.UI.Button deleteButton;
        public UnityEngine.UI.Button duplicateButton;

        private GameObject selectedFurniture;
        private Camera mainCamera;
        private bool isDragging = false;
        private bool isRotating = false;
        private Vector3 dragOffset;
        private Material originalMaterial;
        private List<GameObject> highlightedObjects = new List<GameObject>();

        private FloorPlan currentFloorPlan;
        private BuildingGenerator buildingGenerator;

        public event Action<FurnitureItem> onFurnitureSelected;
        public event Action<FurnitureItem> onFurnitureModified;
        public event Action<FurnitureItem> onFurnitureDeleted;

        private void Start()
        {
            mainCamera = Camera.main;
            buildingGenerator = FindObjectOfType<BuildingGenerator>();
            
            InitializeUI();
        }

        private void InitializeUI()
        {
            if (propertiesPanel != null)
                propertiesPanel.SetActive(false);

            if (rotationSlider != null)
                rotationSlider.onValueChanged.AddListener(OnRotationSliderChanged);

            if (deleteButton != null)
                deleteButton.onClick.AddListener(DeleteSelectedFurniture);

            if (duplicateButton != null)
                duplicateButton.onClick.AddListener(DuplicateSelectedFurniture);

            if (xPositionInput != null)
                xPositionInput.onEndEdit.AddListener(OnPositionInputChanged);

            if (yPositionInput != null)
                yPositionInput.onEndEdit.AddListener(OnPositionInputChanged);

            if (zPositionInput != null)
                zPositionInput.onEndEdit.AddListener(OnPositionInputChanged);
        }

        private void Update()
        {
            HandleMouseInput();
            HandleKeyboardInput();
            UpdateSelectedFurnitureHighlight();
        }

        private void HandleMouseInput()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            // Left click - select or drag
            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick();
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                HandleDrag();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            // Right click - rotate
            if (Input.GetMouseButtonDown(1) && selectedFurniture != null)
            {
                isRotating = true;
            }
            else if (Input.GetMouseButton(1) && isRotating)
            {
                HandleRotation();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                isRotating = false;
            }
        }

        private void HandleMouseClick()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, furnitureLayer))
            {
                SelectFurniture(hit.collider.gameObject);
                isDragging = true;
                dragOffset = hit.transform.position - hit.point;
            }
            else
            {
                DeselectFurniture();
            }
        }

        private void HandleDrag()
        {
            if (selectedFurniture == null) return;

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, floorLayer))
            {
                Vector3 newPosition = hit.point + dragOffset;

                if (enableGridSnapping)
                {
                    newPosition = SnapToGrid(newPosition);
                }

                if (IsValidPlacement(newPosition))
                {
                    selectedFurniture.transform.position = newPosition;
                    UpdatePositionInputs();
                    OnFurnitureModified();
                }
            }
        }

        private void HandleRotation()
        {
            if (selectedFurniture == null) return;

            float mouseX = Input.GetAxis("Mouse X");
            Vector3 currentRotation = selectedFurniture.transform.rotation.eulerAngles;
            
            currentRotation.y += mouseX * 2f;

            if (enableRotationSnapping)
            {
                currentRotation.y = Mathf.Round(currentRotation.y / rotationSnapAngle) * rotationSnapAngle;
            }

            selectedFurniture.transform.rotation = Quaternion.Euler(currentRotation);
            
            if (rotationSlider != null)
            {
                rotationSlider.value = currentRotation.y;
            }

            OnFurnitureModified();
        }

        private void HandleKeyboardInput()
        {
            if (selectedFurniture == null) return;

            // Delete key
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                DeleteSelectedFurniture();
            }

            // Duplicate key
            if (Input.GetKeyDown(KeyCode.D) && Input.GetKey(KeyCode.LeftControl))
            {
                DuplicateSelectedFurniture();
            }

            // Arrow keys for fine movement
            Vector3 moveDirection = Vector3.zero;
            float moveSpeed = 0.1f;

            if (Input.GetKey(KeyCode.LeftArrow))
                moveDirection.x -= moveSpeed;
            if (Input.GetKey(KeyCode.RightArrow))
                moveDirection.x += moveSpeed;
            if (Input.GetKey(KeyCode.UpArrow))
                moveDirection.z += moveSpeed;
            if (Input.GetKey(KeyCode.DownArrow))
                moveDirection.z -= moveSpeed;

            if (moveDirection != Vector3.zero)
            {
                Vector3 newPosition = selectedFurniture.transform.position + moveDirection;
                
                if (enableGridSnapping)
                {
                    newPosition = SnapToGrid(newPosition);
                }

                if (IsValidPlacement(newPosition))
                {
                    selectedFurniture.transform.position = newPosition;
                    UpdatePositionInputs();
                    OnFurnitureModified();
                }
            }

            // Q/E for rotation
            if (Input.GetKey(KeyCode.Q))
            {
                RotateSelectedFurniture(-1f);
            }
            if (Input.GetKey(KeyCode.E))
            {
                RotateSelectedFurniture(1f);
            }
        }

        private void SelectFurniture(GameObject furniture)
        {
            DeselectFurniture();

            selectedFurniture = furniture;
            originalMaterial = selectedFurniture.GetComponent<Renderer>().material;

            // Apply selection material
            if (selectedMaterial != null)
            {
                selectedFurniture.GetComponent<Renderer>().material = selectedMaterial;
            }

            // Show properties panel
            if (propertiesPanel != null)
            {
                propertiesPanel.SetActive(true);
                UpdatePropertiesPanel();
            }

            // Trigger event
            FurnitureItem furnitureItem = GetFurnitureItem(furniture);
            onFurnitureSelected?.Invoke(furnitureItem);
        }

        private void DeselectFurniture()
        {
            if (selectedFurniture != null)
            {
                // Restore original material
                if (originalMaterial != null)
                {
                    selectedFurniture.GetComponent<Renderer>().material = originalMaterial;
                }

                selectedFurniture = null;
            }

            // Hide properties panel
            if (propertiesPanel != null)
            {
                propertiesPanel.SetActive(false);
            }
        }

        private void UpdateSelectedFurnitureHighlight()
        {
            // Clear previous highlights
            ClearHighlights();

            if (selectedFurniture == null)
            {
                // Highlight furniture under mouse
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, furnitureLayer))
                {
                    HighlightFurniture(hit.collider.gameObject);
                }
            }
        }

        private void HighlightFurniture(GameObject furniture)
        {
            Renderer renderer = furniture.GetComponent<Renderer>();
            if (renderer != null && highlightMaterial != null)
            {
                renderer.material = highlightMaterial;
                highlightedObjects.Add(furniture);
            }
        }

        private void ClearHighlights()
        {
            foreach (var obj in highlightedObjects)
            {
                if (obj != null)
                {
                    FurnitureItem item = GetFurnitureItem(obj);
                    if (item != null && item.Material != null)
                    {
                        obj.GetComponent<Renderer>().material = item.Material;
                    }
                }
            }
            highlightedObjects.Clear();
        }

        private Vector3 SnapToGrid(Vector3 position)
        {
            position.x = Mathf.Round(position.x / snapToGridSize) * snapToGridSize;
            position.y = Mathf.Round(position.y / snapToGridSize) * snapToGridSize;
            position.z = Mathf.Round(position.z / snapToGridSize) * snapToGridSize;
            return position;
        }

        private bool IsValidPlacement(Vector3 position)
        {
            // Check if position is within building bounds
            if (buildingGenerator == null) return true;

            // This is a simplified validation
            // In a full implementation, you'd check room boundaries and collisions
            
            return true;
        }

        private void UpdatePropertiesPanel()
        {
            if (selectedFurniture == null) return;

            Vector3 position = selectedFurniture.transform.position;
            Vector3 rotation = selectedFurniture.transform.rotation.eulerAngles;

            if (xPositionInput != null)
                xPositionInput.text = position.x.ToString("F2");

            if (yPositionInput != null)
                yPositionInput.text = position.y.ToString("F2");

            if (zPositionInput != null)
                zPositionInput.text = position.z.ToString("F2");

            if (rotationSlider != null)
                rotationSlider.value = rotation.y;
        }

        private void UpdatePositionInputs()
        {
            if (selectedFurniture == null) return;

            Vector3 position = selectedFurniture.transform.position;

            if (xPositionInput != null)
                xPositionInput.text = position.x.ToString("F2");

            if (yPositionInput != null)
                yPositionInput.text = position.y.ToString("F2");

            if (zPositionInput != null)
                zPositionInput.text = position.z.ToString("F2");
        }

        private void OnRotationSliderChanged(float value)
        {
            if (selectedFurniture == null) return;

            Vector3 rotation = selectedFurniture.transform.rotation.eulerAngles;
            rotation.y = value;
            selectedFurniture.transform.rotation = Quaternion.Euler(rotation);

            OnFurnitureModified();
        }

        private void OnPositionInputChanged(string value)
        {
            if (selectedFurniture == null) return;

            float x, y, z;
            if (float.TryParse(xPositionInput.text, out x) &&
                float.TryParse(yPositionInput.text, out y) &&
                float.TryParse(zPositionInput.text, out z))
            {
                Vector3 newPosition = new Vector3(x, y, z);

                if (enableGridSnapping)
                {
                    newPosition = SnapToGrid(newPosition);
                }

                if (IsValidPlacement(newPosition))
                {
                    selectedFurniture.transform.position = newPosition;
                    OnFurnitureModified();
                }
            }
        }

        private void DeleteSelectedFurniture()
        {
            if (selectedFurniture == null) return;

            FurnitureItem furnitureItem = GetFurnitureItem(selectedFurniture);
            
            // Remove from floor plan
            RemoveFurnitureFromFloorPlan(furnitureItem);

            // Trigger event
            onFurnitureDeleted?.Invoke(furnitureItem);

            // Destroy GameObject
            Destroy(selectedFurniture);
            selectedFurniture = null;

            // Hide properties panel
            if (propertiesPanel != null)
            {
                propertiesPanel.SetActive(false);
            }
        }

        private void DuplicateSelectedFurniture()
        {
            if (selectedFurniture == null) return;

            GameObject duplicate = Instantiate(selectedFurniture);
            duplicate.transform.position = selectedFurniture.transform.position + Vector3.right * 1f;
            duplicate.transform.rotation = selectedFurniture.transform.rotation;

            // Add to floor plan
            FurnitureItem originalItem = GetFurnitureItem(selectedFurniture);
            FurnitureItem duplicateItem = CreateFurnitureItemFromGameObject(duplicate, originalItem);
            AddFurnitureToFloorPlan(duplicateItem);

            SelectFurniture(duplicate);
        }

        private void RotateSelectedFurniture(float direction)
        {
            if (selectedFurniture == null) return;

            Vector3 rotation = selectedFurniture.transform.rotation.eulerAngles;
            rotation.y += direction * (enableRotationSnapping ? rotationSnapAngle : 1f);
            selectedFurniture.transform.rotation = Quaternion.Euler(rotation);

            if (rotationSlider != null)
            {
                rotationSlider.value = rotation.y;
            }

            OnFurnitureModified();
        }

        private FurnitureItem GetFurnitureItem(GameObject furnitureObj)
        {
            // Find the corresponding FurnitureItem in the floor plan
            if (currentFloorPlan == null) return null;

            foreach (var room in currentFloorPlan.Rooms)
            {
                foreach (var furniture in room.Furniture)
                {
                    // This is a simplified matching - in a real implementation,
                    // you'd need a better way to match GameObjects to FurnitureItems
                    if (furniture.Name == furnitureObj.name)
                    {
                        return furniture;
                    }
                }
            }

            return null;
        }

        private void RemoveFurnitureFromFloorPlan(FurnitureItem furnitureItem)
        {
            if (currentFloorPlan == null || furnitureItem == null) return;

            foreach (var room in currentFloorPlan.Rooms)
            {
                if (room.Furniture.Remove(furnitureItem))
                {
                    break;
                }
            }
        }

        private void AddFurnitureToFloorPlan(FurnitureItem furnitureItem)
        {
            if (currentFloorPlan == null) return;

            // Find the appropriate room based on position
            Room targetRoom = FindRoomForPosition(furnitureItem.Position);
            if (targetRoom != null)
            {
                targetRoom.Furniture.Add(furnitureItem);
            }
        }

        private Room FindRoomForPosition(Vector3 position)
        {
            if (currentFloorPlan == null) return null;

            Vector2 pos2D = new Vector2(position.x, position.z);

            foreach (var room in currentFloorPlan.Rooms)
            {
                if (IsPointInRoom(pos2D, room))
                {
                    return room;
                }
            }

            return null;
        }

        private bool IsPointInRoom(Vector2 point, Room room)
        {
            bool inside = false;
            int j = room.Vertices.Count - 1;

            for (int i = 0; i < room.Vertices.Count; i++)
            {
                if (((room.Vertices[i].y > point.y) != (room.Vertices[j].y > point.y)) &&
                    (point.x < (room.Vertices[j].x - room.Vertices[i].x) * (point.y - room.Vertices[i].y) / 
                    (room.Vertices[j].y - room.Vertices[i].y) + room.Vertices[i].x))
                {
                    inside = !inside;
                }
                j = i;
            }

            return inside;
        }

        private FurnitureItem CreateFurnitureItemFromGameObject(GameObject obj, FurnitureItem template)
        {
            FurnitureItem item = new FurnitureItem
            {
                Name = obj.name,
                Type = template.Type,
                Position = obj.transform.position,
                Rotation = obj.transform.rotation.eulerAngles,
                Scale = obj.transform.localScale,
                Material = template.Material,
                ModelPath = template.ModelPath
            };

            return item;
        }

        private void OnFurnitureModified()
        {
            if (selectedFurniture == null) return;

            FurnitureItem furnitureItem = GetFurnitureItem(selectedFurniture);
            if (furnitureItem != null)
            {
                // Update the FurnitureItem with current GameObject state
                furnitureItem.Position = selectedFurniture.transform.position;
                furnitureItem.Rotation = selectedFurniture.transform.rotation.eulerAngles;
                furnitureItem.Scale = selectedFurniture.transform.localScale;

                onFurnitureModified?.Invoke(furnitureItem);
            }
        }

        public void SetCurrentFloorPlan(FloorPlan floorPlan)
        {
            currentFloorPlan = floorPlan;
        }

        public void EnableEditing(bool enable)
        {
            this.enabled = enable;
            
            if (!enable)
            {
                DeselectFurniture();
                ClearHighlights();
            }
        }

        public void SetGridSnapEnabled(bool enabled)
        {
            enableGridSnapping = enabled;
        }

        public void SetRotationSnapEnabled(bool enabled)
        {
            enableRotationSnapping = enabled;
        }

        public void SetGridSize(float size)
        {
            snapToGridSize = size;
        }

        public void SetRotationSnapAngle(float angle)
        {
            rotationSnapAngle = angle;
        }
    }
}
