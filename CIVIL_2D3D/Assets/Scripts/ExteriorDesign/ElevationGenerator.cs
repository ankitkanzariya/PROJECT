using System;
using System.Collections.Generic;
using UnityEngine;

namespace Civil2D3D.ExteriorDesign
{
    public class ElevationGenerator : MonoBehaviour
    {
        [Header("Elevation Settings")]
        public float defaultFloorHeight = 3.0f;
        public float foundationHeight = 0.5f;
        public float roofOverhang = 0.3f;
        public Material defaultExteriorMaterial;
        public Material defaultRoofMaterial;
        public Material defaultWindowMaterial;
        public Material defaultDoorMaterial;

        [Header("Style Options")]
        public ElevationStyle[] availableStyles;
        public ElevationStyle currentStyle;

        [Header("Facade Elements")]
        public GameObject[] windowPrefabs;
        public GameObject[] doorPrefabs;
        public GameObject[] balconyPrefabs;
        public GameObject[] railingPrefabs;

        [Header("Lighting")]
        public Light[] exteriorLights;
        public Material emissiveMaterial;

        private GameObject elevationParent;
        private FloorPlan currentFloorPlan;

        [Serializable]
        public class ElevationStyle
        {
            public string styleName;
            public Material wallMaterial;
            public Material roofMaterial;
            public Material trimMaterial;
            public Color wallColor;
            public Color trimColor;
            public float windowFrameWidth = 0.1f;
            public float doorFrameWidth = 0.15f;
            public bool hasBalconies = true;
            public bool hasDecorativeElements = true;
            public RoofStyle roofStyle;
        }

        public enum RoofStyle
        {
            Flat,
            Gable,
            Hip,
            Mansard,
            Shed
        }

        public void GenerateElevation(FloorPlan floorPlan)
        {
            currentFloorPlan = floorPlan;
            
            if (elevationParent != null)
            {
                Destroy(elevationParent);
            }

            elevationParent = new GameObject("Elevation");
            
            // Apply default style if none selected
            if (currentStyle == null && availableStyles.Length > 0)
            {
                currentStyle = availableStyles[0];
            }

            GenerateFoundation();
            GenerateWalls();
            GenerateRoof();
            GenerateOpenings();
            GenerateFacadeElements();
            ApplyMaterials();
            AddLighting();
        }

        private void GenerateFoundation()
        {
            GameObject foundation = new GameObject("Foundation");
            foundation.transform.SetParent(elevationParent.transform);

            // Calculate building footprint
            Bounds buildingBounds = CalculateBuildingFootprint();
            
            // Create foundation mesh
            MeshFilter meshFilter = foundation.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = foundation.AddComponent<MeshRenderer>();
            
            Mesh foundationMesh = CreateFoundationMesh(buildingBounds);
            meshFilter.mesh = foundationMesh;
            
            // Apply material
            Material foundationMaterial = currentStyle?.wallMaterial ?? defaultExteriorMaterial;
            meshRenderer.material = foundationMaterial;
            
            // Position foundation
            foundation.transform.position = new Vector3(buildingBounds.center.x, foundationHeight * 0.5f, buildingBounds.center.z);
        }

        private void GenerateWalls()
        {
            GameObject wallsParent = new GameObject("ExteriorWalls");
            wallsParent.transform.SetParent(elevationParent.transform);

            if (currentFloorPlan?.Walls == null) return;

            foreach (var wall in currentFloorPlan.Walls)
            {
                // Only create exterior walls for elevation
                if (wall.Type == WallType.Exterior || wall.Type == WallType.Structural)
                {
                    GameObject wallObj = CreateExteriorWall(wall);
                    wallObj.transform.SetParent(wallsParent.transform);
                }
            }
        }

        private GameObject CreateExteriorWall(Wall wall)
        {
            GameObject wallObj = new GameObject("ExteriorWall");
            
            // Calculate wall dimensions
            Vector3 start = new Vector3(wall.Start.x, foundationHeight, wall.Start.y);
            Vector3 end = new Vector3(wall.End.x, foundationHeight, wall.End.y);
            Vector3 direction = (end - start).normalized;
            float length = Vector3.Distance(start, end);
            
            // Create wall mesh
            MeshFilter meshFilter = wallObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = wallObj.AddComponent<MeshRenderer>();
            
            Mesh mesh = CreateWallMesh(length, wall.Height, wall.Thickness);
            meshFilter.mesh = mesh;
            
            // Position and rotation
            wallObj.transform.position = start + direction * (length * 0.5f) + Vector3.up * (wall.Height * 0.5f);
            wallObj.transform.LookAt(start + direction);
            wallObj.transform.Rotate(0, 90, 0);
            
            return wallObj;
        }

        private void GenerateRoof()
        {
            GameObject roofParent = new GameObject("Roof");
            roofParent.transform.SetParent(elevationParent.transform);

            RoofStyle roofStyle = currentStyle?.roofStyle ?? RoofStyle.Flat;

            switch (roofStyle)
            {
                case RoofStyle.Flat:
                    GenerateFlatRoof(roofParent);
                    break;
                case RoofStyle.Gable:
                    GenerateGableRoof(roofParent);
                    break;
                case RoofStyle.Hip:
                    GenerateHipRoof(roofParent);
                    break;
                case RoofStyle.Mansard:
                    GenerateMansardRoof(roofParent);
                    break;
                case RoofStyle.Shed:
                    GenerateShedRoof(roofParent);
                    break;
            }
        }

        private void GenerateFlatRoof(GameObject parent)
        {
            Bounds buildingBounds = CalculateBuildingFootprint();
            
            GameObject roof = new GameObject("FlatRoof");
            roof.transform.SetParent(parent.transform);
            
            MeshFilter meshFilter = roof.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = roof.AddComponent<MeshRenderer>();
            
            // Create simple flat roof with overhang
            Vector3 size = buildingBounds.size + Vector3.one * roofOverhang * 2f;
            Mesh roofMesh = CreateFlatRoofMesh(size.x, size.z, 0.2f);
            meshFilter.mesh = roofMesh;
            
            // Position roof
            roof.transform.position = new Vector3(buildingBounds.center.x, defaultFloorHeight + foundationHeight, buildingBounds.center.z);
            
            // Apply material
            Material roofMaterial = currentStyle?.roofMaterial ?? defaultRoofMaterial;
            meshRenderer.material = roofMaterial;
        }

        private void GenerateGableRoof(GameObject parent)
        {
            Bounds buildingBounds = CalculateBuildingFootprint();
            
            GameObject roof = new GameObject("GableRoof");
            roof.transform.SetParent(parent.transform);
            
            MeshFilter meshFilter = roof.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = roof.AddComponent<MeshRenderer>();
            
            // Create gable roof mesh
            Mesh roofMesh = CreateGableRoofMesh(buildingBounds.size.x, buildingBounds.size.z, roofOverhang);
            meshFilter.mesh = roofMesh;
            
            // Position roof
            roof.transform.position = new Vector3(buildingBounds.center.x, defaultFloorHeight + foundationHeight, buildingBounds.center.z);
            
            // Apply material
            Material roofMaterial = currentStyle?.roofMaterial ?? defaultRoofMaterial;
            meshRenderer.material = roofMaterial;
        }

        private void GenerateHipRoof(GameObject parent)
        {
            // Similar to gable but with four slopes
            GenerateGableRoof(parent); // Placeholder
        }

        private void GenerateMansardRoof(GameObject parent)
        {
            // Complex roof with multiple slopes
            GenerateGableRoof(parent); // Placeholder
        }

        private void GenerateShedRoof(GameObject parent)
        {
            // Single slope roof
            GenerateFlatRoof(parent); // Placeholder
        }

        private void GenerateOpenings()
        {
            GenerateWindows();
            GenerateDoors();
        }

        private void GenerateWindows()
        {
            if (currentFloorPlan?.Windows == null) return;

            GameObject windowsParent = new GameObject("ExteriorWindows");
            windowsParent.transform.SetParent(elevationParent.transform);

            foreach (var window in currentFloorPlan.Windows)
            {
                GameObject windowObj = CreateExteriorWindow(window);
                if (windowObj != null)
                {
                    windowObj.transform.SetParent(windowsParent.transform);
                }
            }
        }

        private GameObject CreateExteriorWindow(Window window)
        {
            GameObject windowObj;
            
            // Try to use prefab
            if (windowPrefabs != null && windowPrefabs.Length > 0)
            {
                windowObj = Instantiate(windowPrefabs[UnityEngine.Random.Range(0, windowPrefabs.Length)]);
            }
            else
            {
                windowObj = CreatePrimitiveWindow(window);
            }

            // Position window
            Vector3 position = new Vector3(window.Position.x, foundationHeight + window.SillHeight + window.Height * 0.5f, window.Position.y);
            windowObj.transform.position = position;
            
            // Rotate to face outward
            Vector3 direction = new Vector3(window.Direction.x, 0, window.Direction.y);
            windowObj.transform.rotation = Quaternion.LookRotation(-direction);
            
            // Scale
            Vector3 scale = windowObj.transform.localScale;
            scale.x = window.Width;
            scale.y = window.Height;
            scale.z = 0.2f;
            windowObj.transform.localScale = scale;

            return windowObj;
        }

        private GameObject CreatePrimitiveWindow(Window window)
        {
            GameObject windowObj = new GameObject("Window");
            
            // Create window frame
            GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frame.name = "WindowFrame";
            frame.transform.SetParent(windowObj.transform);
            
            // Create window glass
            GameObject glass = GameObject.CreatePrimitive(PrimitiveType.Cube);
            glass.name = "WindowGlass";
            glass.transform.SetParent(windowObj.transform);
            
            // Position and scale glass
            float frameWidth = currentStyle?.windowFrameWidth ?? 0.1f;
            glass.transform.localScale = new Vector3(window.Width - frameWidth * 2f, window.Height - frameWidth * 2f, 0.05f);
            glass.transform.localPosition = Vector3.zero;
            
            // Apply materials
            Renderer frameRenderer = frame.GetComponent<Renderer>();
            Renderer glassRenderer = glass.GetComponent<Renderer>();
            
            if (frameRenderer != null)
                frameRenderer.material = currentStyle?.trimMaterial ?? defaultWindowMaterial;
            
            if (glassRenderer != null)
            {
                Material glassMaterial = new Material(defaultWindowMaterial);
                glassMaterial.color = new Color(0.7f, 0.85f, 1.0f, 0.3f);
                glassRenderer.material = glassMaterial;
            }
            
            return windowObj;
        }

        private void GenerateDoors()
        {
            if (currentFloorPlan?.Doors == null) return;

            GameObject doorsParent = new GameObject("ExteriorDoors");
            doorsParent.transform.SetParent(elevationParent.transform);

            foreach (var door in currentFloorPlan.Doors)
            {
                GameObject doorObj = CreateExteriorDoor(door);
                if (doorObj != null)
                {
                    doorObj.transform.SetParent(doorsParent.transform);
                }
            }
        }

        private GameObject CreateExteriorDoor(Door door)
        {
            GameObject doorObj;
            
            // Try to use prefab
            if (doorPrefabs != null && doorPrefabs.Length > 0)
            {
                doorObj = Instantiate(doorPrefabs[UnityEngine.Random.Range(0, doorPrefabs.Length)]);
            }
            else
            {
                doorObj = CreatePrimitiveDoor(door);
            }

            // Position door
            Vector3 position = new Vector3(door.Position.x, foundationHeight + door.Height * 0.5f, door.Position.y);
            doorObj.transform.position = position;
            
            // Rotate to face outward
            Vector3 direction = new Vector3(door.Direction.x, 0, door.Direction.y);
            doorObj.transform.rotation = Quaternion.LookRotation(-direction);
            
            // Scale
            Vector3 scale = doorObj.transform.localScale;
            scale.x = door.Width;
            scale.y = door.Height;
            scale.z = 0.2f;
            doorObj.transform.localScale = scale;

            return doorObj;
        }

        private GameObject CreatePrimitiveDoor(Door door)
        {
            GameObject doorObj = new GameObject("Door");
            
            // Create door panel
            GameObject panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.name = "DoorPanel";
            panel.transform.SetParent(doorObj.transform);
            
            // Create door frame
            GameObject frame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            frame.name = "DoorFrame";
            frame.transform.SetParent(doorObj.transform);
            
            // Position and scale frame
            float frameWidth = currentStyle?.doorFrameWidth ?? 0.15f;
            frame.transform.localScale = new Vector3(door.Width + frameWidth * 2f, door.Height + frameWidth * 2f, 0.3f);
            frame.transform.localPosition = Vector3.zero;
            
            // Position and scale panel
            panel.transform.localScale = new Vector3(door.Width, door.Height, 0.1f);
            panel.transform.localPosition = Vector3.forward * 0.1f;
            
            // Apply materials
            Renderer panelRenderer = panel.GetComponent<Renderer>();
            Renderer frameRenderer = frame.GetComponent<Renderer>();
            
            if (panelRenderer != null)
                panelRenderer.material = currentStyle?.doorMaterial ?? defaultDoorMaterial;
            
            if (frameRenderer != null)
                frameRenderer.material = currentStyle?.trimMaterial ?? defaultDoorMaterial;
            
            return doorObj;
        }

        private void GenerateFacadeElements()
        {
            if (currentStyle?.hasBalconies == true)
            {
                GenerateBalconies();
            }

            if (currentStyle?.hasDecorativeElements == true)
            {
                GenerateDecorativeElements();
            }
        }

        private void GenerateBalconies()
        {
            // Generate balconies for upper floors
            GameObject balconiesParent = new GameObject("Balconies");
            balconiesParent.transform.SetParent(elevationParent.transform);

            // Simple balcony generation - place on some exterior walls
            if (currentFloorPlan?.Walls != null)
            {
                int balconyCount = 0;
                foreach (var wall in currentFloorPlan.Walls)
                {
                    if (wall.Type == WallType.Exterior && balconyCount < 2) // Limit balconies
                    {
                        GameObject balcony = CreateBalcony(wall);
                        if (balcony != null)
                        {
                            balcony.transform.SetParent(balconiesParent.transform);
                            balconyCount++;
                        }
                    }
                }
            }
        }

        private GameObject CreateBalcony(Wall wall)
        {
            GameObject balconyObj;
            
            // Try to use prefab
            if (balconyPrefabs != null && balconyPrefabs.Length > 0)
            {
                balconyObj = Instantiate(balconyPrefabs[0]);
            }
            else
            {
                balconyObj = CreatePrimitiveBalcony(wall);
            }

            // Position balcony
            Vector3 wallCenter = (new Vector3(wall.Start.x, 0, wall.Start.y) + new Vector3(wall.End.x, 0, wall.End.y)) * 0.5f;
            Vector3 wallDirection = (new Vector3(wall.End.x, 0, wall.End.y) - new Vector3(wall.Start.x, 0, wall.Start.y)).normalized;
            Vector3 outward = new Vector3(-wallDirection.z, 0, wallDirection.x);
            
            balconyObj.transform.position = wallCenter + outward * 2f + Vector3.up * (defaultFloorHeight - 0.5f);
            balconyObj.transform.rotation = Quaternion.LookRotation(wallDirection);

            return balconyObj;
        }

        private GameObject CreatePrimitiveBalcony(Wall wall)
        {
            GameObject balconyObj = new GameObject("Balcony");
            
            // Create balcony floor
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "BalconyFloor";
            floor.transform.SetParent(balconyObj.transform);
            floor.transform.localScale = new Vector3(3f, 0.1f, 2f);
            floor.transform.localPosition = Vector3.zero;
            
            // Create railing
            GameObject railing = new GameObject("Railing");
            railing.transform.SetParent(balconyObj.transform);
            
            // Simple railing posts
            for (int i = 0; i < 5; i++)
            {
                GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                post.name = $"RailingPost_{i}";
                post.transform.SetParent(railing.transform);
                post.transform.localScale = new Vector3(0.05f, 1f, 0.05f);
                post.transform.localPosition = new Vector3(-1.2f + i * 0.6f, 0.5f, 0.8f);
            }
            
            // Railing top rail
            GameObject topRail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            topRail.name = "TopRail";
            topRail.transform.SetParent(railing.transform);
            topRail.transform.localScale = new Vector3(3.2f, 0.05f, 0.05f);
            topRail.transform.localPosition = new Vector3(0f, 1f, 0.8f);

            return balconyObj;
        }

        private void GenerateDecorativeElements()
        {
            // Add decorative trim, cornices, etc.
            GameObject decorativeParent = new GameObject("DecorativeElements");
            decorativeParent.transform.SetParent(elevationParent.transform);

            // Add cornice at roof line
            GenerateCornice(decorativeParent);
        }

        private void GenerateCornice(GameObject parent)
        {
            Bounds buildingBounds = CalculateBuildingFootprint();
            
            GameObject cornice = new GameObject("Cornice");
            cornice.transform.SetParent(parent.transform);
            
            MeshFilter meshFilter = cornice.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = cornice.AddComponent<MeshRenderer>();
            
            // Create cornice mesh
            Mesh corniceMesh = CreateCorniceMesh(buildingBounds.size.x, buildingBounds.size.z);
            meshFilter.mesh = corniceMesh;
            
            // Position cornice
            cornice.transform.position = new Vector3(buildingBounds.center.x, defaultFloorHeight + foundationHeight - 0.2f, buildingBounds.center.z);
            
            // Apply trim material
            if (currentStyle?.trimMaterial != null)
            {
                meshRenderer.material = currentStyle.trimMaterial;
            }
        }

        private void ApplyMaterials()
        {
            if (currentStyle == null) return;

            // Apply materials to all exterior elements
            Renderer[] renderers = elevationParent.GetComponentsInChildren<Renderer>();
            
            foreach (var renderer in renderers)
            {
                if (renderer.gameObject.name.Contains("Wall"))
                {
                    renderer.material = currentStyle.wallMaterial;
                }
                else if (renderer.gameObject.name.Contains("Roof"))
                {
                    renderer.material = currentStyle.roofMaterial;
                }
                else if (renderer.gameObject.name.Contains("Frame") || renderer.gameObject.name.Contains("Trim"))
                {
                    renderer.material = currentStyle.trimMaterial;
                }
            }
        }

        private void AddLighting()
        {
            if (exteriorLights == null) return;

            GameObject lightingParent = new GameObject("ExteriorLighting");
            lightingParent.transform.SetParent(elevationParent.transform);

            // Add exterior lights at strategic positions
            Bounds buildingBounds = CalculateBuildingFootprint();
            
            foreach (var light in exteriorLights)
            {
                GameObject lightObj = new GameObject("ExteriorLight");
                lightObj.transform.SetParent(lightingParent.transform);
                
                Light lightComponent = lightObj.AddComponent<Light>();
                lightComponent.type = light.type;
                lightComponent.intensity = light.intensity;
                lightComponent.color = light.color;
                lightComponent.range = light.range;
                
                // Position lights around the building
                Vector3[] lightPositions = {
                    new Vector3(buildingBounds.min.x - 2f, defaultFloorHeight, buildingBounds.center.z),
                    new Vector3(buildingBounds.max.x + 2f, defaultFloorHeight, buildingBounds.center.z),
                    new Vector3(buildingBounds.center.x, defaultFloorHeight, buildingBounds.min.z - 2f),
                    new Vector3(buildingBounds.center.x, defaultFloorHeight, buildingBounds.max.z + 2f)
                };
                
                int lightIndex = UnityEngine.Random.Range(0, lightPositions.Length);
                lightObj.transform.position = lightPositions[lightIndex];
                lightObj.transform.LookAt(buildingBounds.center);
            }
        }

        private Bounds CalculateBuildingFootprint()
        {
            if (currentFloorPlan?.Walls == null || currentFloorPlan.Walls.Count == 0)
            {
                return new Bounds(Vector3.zero, Vector3.one * 10f);
            }

            Vector3 min = new Vector3(float.MaxValue, 0, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, 0, float.MinValue);

            foreach (var wall in currentFloorPlan.Walls)
            {
                min.x = Mathf.Min(min.x, wall.Start.x, wall.End.x);
                min.z = Mathf.Min(min.z, wall.Start.y, wall.End.y);
                max.x = Mathf.Max(max.x, wall.Start.x, wall.End.x);
                max.z = Mathf.Max(max.z, wall.Start.y, wall.End.y);
            }

            Vector3 center = (min + max) * 0.5f;
            Vector3 size = max - min;

            return new Bounds(center, size);
        }

        private Mesh CreateFoundationMesh(Bounds bounds)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[24];
            int[] triangles = new int[36];
            
            // Create box vertices
            Vector3 size = bounds.size;
            Vector3[] corners = {
                new Vector3(-size.x * 0.5f, 0, -size.z * 0.5f),
                new Vector3(size.x * 0.5f, 0, -size.z * 0.5f),
                new Vector3(size.x * 0.5f, 0, size.z * 0.5f),
                new Vector3(-size.x * 0.5f, 0, size.z * 0.5f),
                new Vector3(-size.x * 0.5f, -foundationHeight, -size.z * 0.5f),
                new Vector3(size.x * 0.5f, -foundationHeight, -size.z * 0.5f),
                new Vector3(size.x * 0.5f, -foundationHeight, size.z * 0.5f),
                new Vector3(-size.x * 0.5f, -foundationHeight, size.z * 0.5f)
            };

            // Create box mesh
            mesh.vertices = corners;
            mesh.triangles = new int[] {
                0,2,1, 0,3,2, // Top
                4,5,6, 4,6,7, // Bottom
                0,4,7, 0,7,3, // Left
                1,2,6, 1,6,5, // Right
                0,1,5, 0,5,4, // Front
                2,3,7, 2,7,6  // Back
            };
            
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateWallMesh(float width, float height, float thickness)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[24];
            int[] triangles = new int[36];
            
            // Create wall vertices
            vertices[0] = new Vector3(-width * 0.5f, 0, thickness * 0.5f);
            vertices[1] = new Vector3(width * 0.5f, 0, thickness * 0.5f);
            vertices[2] = new Vector3(width * 0.5f, height, thickness * 0.5f);
            vertices[3] = new Vector3(-width * 0.5f, height, thickness * 0.5f);
            
            vertices[4] = new Vector3(-width * 0.5f, 0, -thickness * 0.5f);
            vertices[5] = new Vector3(width * 0.5f, 0, -thickness * 0.5f);
            vertices[6] = new Vector3(width * 0.5f, height, -thickness * 0.5f);
            vertices[7] = new Vector3(-width * 0.5f, height, -thickness * 0.5f);
            
            // Create wall triangles
            mesh.vertices = vertices;
            mesh.triangles = new int[] {
                0,2,1, 0,3,2, // Front
                4,5,6, 4,6,7, // Back
                0,4,7, 0,7,3, // Left
                1,2,6, 1,6,5, // Right
                2,3,7, 2,7,6, // Top
                0,1,5, 0,5,4  // Bottom
            };
            
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateFlatRoofMesh(float width, float depth, float thickness)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[24];
            int[] triangles = new int[36];
            
            // Create roof vertices
            vertices[0] = new Vector3(-width * 0.5f, 0, -depth * 0.5f);
            vertices[1] = new Vector3(width * 0.5f, 0, -depth * 0.5f);
            vertices[2] = new Vector3(width * 0.5f, 0, depth * 0.5f);
            vertices[3] = new Vector3(-width * 0.5f, 0, depth * 0.5f);
            
            vertices[4] = new Vector3(-width * 0.5f, -thickness, -depth * 0.5f);
            vertices[5] = new Vector3(width * 0.5f, -thickness, -depth * 0.5f);
            vertices[6] = new Vector3(width * 0.5f, -thickness, depth * 0.5f);
            vertices[7] = new Vector3(-width * 0.5f, -thickness, depth * 0.5f);
            
            // Create roof triangles
            mesh.vertices = vertices;
            mesh.triangles = new int[] {
                0,2,1, 0,3,2, // Top
                4,5,6, 4,6,7, // Bottom
                0,4,7, 0,7,3, // Left
                1,2,6, 1,6,5, // Right
                2,3,7, 2,7,6, // Front
                0,1,5, 0,5,4  // Back
            };
            
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateGableRoofMesh(float width, float depth, float overhang)
        {
            Mesh mesh = new Mesh();
            
            float actualWidth = width + overhang * 2f;
            float actualDepth = depth + overhang * 2f;
            float roofHeight = actualWidth * 0.3f; // Roof pitch
            
            Vector3[] vertices = new Vector3[12];
            
            // Roof vertices
            vertices[0] = new Vector3(-actualWidth * 0.5f, 0, -actualDepth * 0.5f); // Front left
            vertices[1] = new Vector3(actualWidth * 0.5f, 0, -actualDepth * 0.5f);  // Front right
            vertices[2] = new Vector3(actualWidth * 0.5f, 0, actualDepth * 0.5f);   // Back right
            vertices[3] = new Vector3(-actualWidth * 0.5f, 0, actualDepth * 0.5f);  // Back left
            
            // Ridge vertices
            vertices[4] = new Vector3(-actualWidth * 0.5f, roofHeight, 0); // Left ridge
            vertices[5] = new Vector3(actualWidth * 0.5f, roofHeight, 0);  // Right ridge
            
            // Create triangles for gable roof
            mesh.vertices = vertices;
            mesh.triangles = new int[] {
                // Front slope
                0,4,5, 0,5,1,
                // Back slope
                3,2,5, 3,5,4,
                // Left side
                0,3,4,
                // Right side
                1,5,2
            };
            
            mesh.RecalculateNormals();
            return mesh;
        }

        private Mesh CreateCorniceMesh(float width, float depth)
        {
            Mesh mesh = new Mesh();
            
            Vector3[] vertices = new Vector3[16];
            
            // Create cornice profile vertices
            float corniceHeight = 0.3f;
            float corniceDepth = 0.2f;
            
            // Top face
            vertices[0] = new Vector3(-width * 0.5f, 0, -depth * 0.5f);
            vertices[1] = new Vector3(width * 0.5f, 0, -depth * 0.5f);
            vertices[2] = new Vector3(width * 0.5f, 0, depth * 0.5f);
            vertices[3] = new Vector3(-width * 0.5f, 0, depth * 0.5f);
            
            // Bottom face
            vertices[4] = new Vector3(-width * 0.5f, -corniceHeight, -depth * 0.5f);
            vertices[5] = new Vector3(width * 0.5f, -corniceHeight, -depth * 0.5f);
            vertices[6] = new Vector3(width * 0.5f, -corniceHeight, depth * 0.5f);
            vertices[7] = new Vector3(-width * 0.5f, -corniceHeight, depth * 0.5f);
            
            // Front lip
            vertices[8] = new Vector3(-width * 0.5f, -corniceHeight, -depth * 0.5f - corniceDepth);
            vertices[9] = new Vector3(width * 0.5f, -corniceHeight, -depth * 0.5f - corniceDepth);
            vertices[10] = new Vector3(width * 0.5f, -corniceHeight * 0.5f, -depth * 0.5f - corniceDepth);
            vertices[11] = new Vector3(-width * 0.5f, -corniceHeight * 0.5f, -depth * 0.5f - corniceDepth);
            
            // Create cornice triangles
            mesh.vertices = vertices;
            mesh.triangles = new int[] {
                0,2,1, 0,3,2, // Top
                4,5,6, 4,6,7, // Bottom
                0,4,7, 0,7,3, // Left
                1,2,6, 1,6,5, // Right
                2,3,7, 2,7,6, // Back
                0,1,5, 0,5,4, // Front
                8,10,9, 8,11,10 // Front lip
            };
            
            mesh.RecalculateNormals();
            return mesh;
        }

        public void SetElevationStyle(ElevationStyle style)
        {
            currentStyle = style;
            
            // Regenerate elevation with new style
            if (currentFloorPlan != null)
            {
                GenerateElevation(currentFloorPlan);
            }
        }

        public ElevationStyle[] GetAvailableStyles()
        {
            return availableStyles;
        }

        public GameObject GetElevationObject()
        {
            return elevationParent;
        }
    }
}
