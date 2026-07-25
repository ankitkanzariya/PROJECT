using System;
using System.Collections.Generic;
using UnityEngine;

namespace Civil2D3D.Core
{
    public class BuildingGenerator : MonoBehaviour
    {
        [Header("Generation Settings")]
        public Material defaultWallMaterial;
        public Material defaultFloorMaterial;
        public Material defaultCeilingMaterial;
        public float wallHeight = 3.0f;
        public float floorThickness = 0.1f;
        public float ceilingThickness = 0.1f;

        [Header("Prefabs")]
        public GameObject doorPrefab;
        public GameObject windowPrefab;
        public GameObject[] furniturePrefabs;

        private GameObject buildingParent;
        private Dictionary<string, Material> materialCache = new Dictionary<string, Material>();

        public GameObject Generate3D(FloorPlan plan)
        {
            // Create parent object for the building
            buildingParent = new GameObject($"Building_{DateTime.Now:yyyyMMdd_HHmmss}");
            
            // Generate structure
            CreateFloors(plan);
            CreateWalls(plan);
            CreateCeilings(plan);
            
            // Add openings
            PlaceDoors(plan);
            PlaceWindows(plan);
            
            // Add furniture
            PlaceFurniture(plan);
            
            return buildingParent;
        }

        private void CreateFloors(FloorPlan plan)
        {
            GameObject floorsParent = new GameObject("Floors");
            floorsParent.transform.SetParent(buildingParent.transform);

            foreach (var room in plan.Rooms)
            {
                GameObject floor = CreateFloor(room);
                floor.transform.SetParent(floorsParent.transform);
            }
        }

        private GameObject CreateFloor(Room room)
        {
            GameObject floor = new GameObject($"Floor_{room.Name}");
            
            // Create mesh
            MeshFilter meshFilter = floor.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = floor.AddComponent<MeshRenderer>();
            
            Mesh mesh = CreateRoomMesh(room.Vertices, floorThickness);
            meshFilter.mesh = mesh;
            
            // Apply material
            Material floorMaterial = room.Materials?.FloorMaterial ?? defaultFloorMaterial;
            meshRenderer.material = floorMaterial;
            
            // Add collider for interaction
            MeshCollider collider = floor.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            
            // Position
            floor.transform.position = new Vector3(0, 0, 0);
            
            return floor;
        }

        private void CreateWalls(FloorPlan plan)
        {
            GameObject wallsParent = new GameObject("Walls");
            wallsParent.transform.SetParent(buildingParent.transform);

            foreach (var wall in plan.Walls)
            {
                GameObject wallObj = CreateWall(wall);
                wallObj.transform.SetParent(wallsParent.transform);
            }
        }

        private GameObject CreateWall(Wall wall)
        {
            GameObject wallObj = new GameObject("Wall");
            
            // Calculate wall dimensions
            Vector3 start = new Vector3(wall.Start.x, 0, wall.Start.y);
            Vector3 end = new Vector3(wall.End.x, 0, wall.End.y);
            Vector3 direction = (end - start).normalized;
            float length = Vector3.Distance(start, end);
            
            // Create wall mesh
            MeshFilter meshFilter = wallObj.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = wallObj.AddComponent<MeshRenderer>();
            
            Mesh mesh = CreateWallMesh(length, wall.Height, wall.Thickness);
            meshFilter.mesh = mesh;
            
            // Apply material
            Material wallMaterial = wall.Material ?? defaultWallMaterial;
            meshRenderer.material = wallMaterial;
            
            // Position and rotation
            wallObj.transform.position = start + direction * (length * 0.5f);
            wallObj.transform.LookAt(start + direction);
            wallObj.transform.Rotate(0, 90, 0);
            wallObj.transform.position = new Vector3(wallObj.transform.position.x, wallHeight * 0.5f, wallObj.transform.position.z);
            
            // Add collider
            MeshCollider collider = wallObj.AddComponent<MeshCollider>();
            collider.sharedMesh = mesh;
            
            return wallObj;
        }

        private Mesh CreateWallMesh(float width, float height, float thickness)
        {
            Mesh mesh = new Mesh();
            
            // Define vertices for a wall segment
            Vector3[] vertices = new Vector3[24];
            
            // Front face
            vertices[0] = new Vector3(-width * 0.5f, 0, thickness * 0.5f);
            vertices[1] = new Vector3(width * 0.5f, 0, thickness * 0.5f);
            vertices[2] = new Vector3(width * 0.5f, height, thickness * 0.5f);
            vertices[3] = new Vector3(-width * 0.5f, height, thickness * 0.5f);
            
            // Back face
            vertices[4] = new Vector3(-width * 0.5f, 0, -thickness * 0.5f);
            vertices[5] = new Vector3(width * 0.5f, 0, -thickness * 0.5f);
            vertices[6] = new Vector3(width * 0.5f, height, -thickness * 0.5f);
            vertices[7] = new Vector3(-width * 0.5f, height, -thickness * 0.5f);
            
            // Top face
            vertices[8] = new Vector3(-width * 0.5f, height, thickness * 0.5f);
            vertices[9] = new Vector3(width * 0.5f, height, thickness * 0.5f);
            vertices[10] = new Vector3(width * 0.5f, height, -thickness * 0.5f);
            vertices[11] = new Vector3(-width * 0.5f, height, -thickness * 0.5f);
            
            // Bottom face
            vertices[12] = new Vector3(-width * 0.5f, 0, thickness * 0.5f);
            vertices[13] = new Vector3(width * 0.5f, 0, thickness * 0.5f);
            vertices[14] = new Vector3(width * 0.5f, 0, -thickness * 0.5f);
            vertices[15] = new Vector3(-width * 0.5f, 0, -thickness * 0.5f);
            
            // Left face
            vertices[16] = new Vector3(-width * 0.5f, 0, thickness * 0.5f);
            vertices[17] = new Vector3(-width * 0.5f, 0, -thickness * 0.5f);
            vertices[18] = new Vector3(-width * 0.5f, height, -thickness * 0.5f);
            vertices[19] = new Vector3(-width * 0.5f, height, thickness * 0.5f);
            
            // Right face
            vertices[20] = new Vector3(width * 0.5f, 0, thickness * 0.5f);
            vertices[21] = new Vector3(width * 0.5f, 0, -thickness * 0.5f);
            vertices[22] = new Vector3(width * 0.5f, height, -thickness * 0.5f);
            vertices[23] = new Vector3(width * 0.5f, height, thickness * 0.5f);
            
            // Define triangles
            int[] triangles = new int[36];
            
            // Front face
            triangles[0] = 0; triangles[1] = 2; triangles[2] = 1;
            triangles[3] = 0; triangles[4] = 3; triangles[5] = 2;
            
            // Back face
            triangles[6] = 4; triangles[7] = 5; triangles[8] = 6;
            triangles[9] = 4; triangles[10] = 6; triangles[11] = 7;
            
            // Top face
            triangles[12] = 8; triangles[13] = 10; triangles[14] = 9;
            triangles[15] = 8; triangles[16] = 11; triangles[17] = 10;
            
            // Bottom face
            triangles[18] = 12; triangles[19] = 13; triangles[20] = 14;
            triangles[21] = 12; triangles[22] = 14; triangles[23] = 15;
            
            // Left face
            triangles[24] = 16; triangles[25] = 18; triangles[26] = 17;
            triangles[27] = 16; triangles[28] = 19; triangles[29] = 18;
            
            // Right face
            triangles[30] = 20; triangles[31] = 22; triangles[32] = 21;
            triangles[33] = 20; triangles[34] = 23; triangles[35] = 22;
            
            // Define UVs
            Vector2[] uvs = new Vector2[24];
            
            for (int i = 0; i < 24; i++)
            {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].y);
            }
            
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            
            return mesh;
        }

        private Mesh CreateRoomMesh(List<Vector2> vertices, float thickness)
        {
            Mesh mesh = new Mesh();
            
            if (vertices.Count < 3)
            {
                Debug.LogError("Room must have at least 3 vertices");
                return mesh;
            }
            
            // Convert 2D vertices to 3D
            Vector3[] vertices3D = new Vector3[vertices.Count * 2];
            
            // Top vertices
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices3D[i] = new Vector3(vertices[i].x, thickness, vertices[i].y);
            }
            
            // Bottom vertices
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices3D[vertices.Count + i] = new Vector3(vertices[i].x, 0, vertices[i].y);
            }
            
            // Create triangles for top face
            int[] triangles = new int[(vertices.Count - 2) * 6];
            int triangleIndex = 0;
            
            // Top face triangles
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles[triangleIndex++] = 0;
                triangles[triangleIndex++] = i;
                triangles[triangleIndex++] = i + 1;
            }
            
            // Bottom face triangles (reversed winding)
            for (int i = 1; i < vertices.Count - 1; i++)
            {
                triangles[triangleIndex++] = vertices.Count;
                triangles[triangleIndex++] = vertices.Count + i + 1;
                triangles[triangleIndex++] = vertices.Count + i;
            }
            
            // Side faces
            for (int i = 0; i < vertices.Count; i++)
            {
                int next = (i + 1) % vertices.Count;
                
                // Side quad triangles
                triangles[triangleIndex++] = i;
                triangles[triangleIndex++] = vertices.Count + i;
                triangles[triangleIndex++] = next;
                
                triangles[triangleIndex++] = next;
                triangles[triangleIndex++] = vertices.Count + i;
                triangles[triangleIndex++] = vertices.Count + next;
            }
            
            // Generate UVs
            Vector2[] uvs = new Vector2[vertices3D.Length];
            for (int i = 0; i < vertices3D.Length; i++)
            {
                uvs[i] = new Vector2(vertices3D[i].x, vertices3D[i].z);
            }
            
            mesh.vertices = vertices3D;
            mesh.triangles = triangles;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            
            return mesh;
        }

        private void CreateCeilings(FloorPlan plan)
        {
            GameObject ceilingsParent = new GameObject("Ceilings");
            ceilingsParent.transform.SetParent(buildingParent.transform);

            foreach (var room in plan.Rooms)
            {
                GameObject ceiling = CreateCeiling(room);
                ceiling.transform.SetParent(ceilingsParent.transform);
            }
        }

        private GameObject CreateCeiling(Room room)
        {
            GameObject ceiling = new GameObject($"Ceiling_{room.Name}");
            
            // Create mesh (similar to floor but at ceiling height)
            MeshFilter meshFilter = ceiling.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = ceiling.AddComponent<MeshRenderer>();
            
            Mesh mesh = CreateRoomMesh(room.Vertices, ceilingThickness);
            meshFilter.mesh = mesh;
            
            // Apply material
            Material ceilingMaterial = room.Materials?.CeilingMaterial ?? defaultCeilingMaterial;
            meshRenderer.material = ceilingMaterial;
            
            // Position at ceiling height
            ceiling.transform.position = new Vector3(0, wallHeight, 0);
            
            return ceiling;
        }

        private void PlaceDoors(FloorPlan plan)
        {
            GameObject doorsParent = new GameObject("Doors");
            doorsParent.transform.SetParent(buildingParent.transform);

            foreach (var door in plan.Doors)
            {
                if (doorPrefab != null)
                {
                    GameObject doorObj = Instantiate(doorPrefab, doorsParent.transform);
                    PositionDoor(doorObj, door);
                }
                else
                {
                    GameObject doorObj = CreatePrimitiveDoor(door);
                    doorObj.transform.SetParent(doorsParent.transform);
                }
            }
        }

        private void PositionDoor(GameObject doorObj, Door door)
        {
            Vector3 position = new Vector3(door.Position.x, door.Height * 0.5f, door.Position.y);
            doorObj.transform.position = position;
            
            // Rotate to face the correct direction
            Vector3 direction = new Vector3(door.Direction.x, 0, door.Direction.y);
            doorObj.transform.rotation = Quaternion.LookRotation(direction);
            
            // Scale to door dimensions
            Vector3 scale = doorObj.transform.localScale;
            scale.x = door.Width;
            scale.y = door.Height;
            scale.z = 0.1f; // Door thickness
            doorObj.transform.localScale = scale;
        }

        private GameObject CreatePrimitiveDoor(Door door)
        {
            GameObject doorObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doorObj.name = "Door";
            
            PositionDoor(doorObj, door);
            
            return doorObj;
        }

        private void PlaceWindows(FloorPlan plan)
        {
            GameObject windowsParent = new GameObject("Windows");
            windowsParent.transform.SetParent(buildingParent.transform);

            foreach (var window in plan.Windows)
            {
                if (windowPrefab != null)
                {
                    GameObject windowObj = Instantiate(windowPrefab, windowsParent.transform);
                    PositionWindow(windowObj, window);
                }
                else
                {
                    GameObject windowObj = CreatePrimitiveWindow(window);
                    windowObj.transform.SetParent(windowsParent.transform);
                }
            }
        }

        private void PositionWindow(GameObject windowObj, Window window)
        {
            Vector3 position = new Vector3(window.Position.x, window.SillHeight + window.Height * 0.5f, window.Position.y);
            windowObj.transform.position = position;
            
            // Rotate to face the correct direction
            Vector3 direction = new Vector3(window.Direction.x, 0, window.Direction.y);
            windowObj.transform.rotation = Quaternion.LookRotation(direction);
            
            // Scale to window dimensions
            Vector3 scale = windowObj.transform.localScale;
            scale.x = window.Width;
            scale.y = window.Height;
            scale.z = 0.1f; // Window thickness
            windowObj.transform.localScale = scale;
        }

        private GameObject CreatePrimitiveWindow(Window window)
        {
            GameObject windowObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            windowObj.name = "Window";
            
            PositionWindow(windowObj, window);
            
            return windowObj;
        }

        private void PlaceFurniture(FloorPlan plan)
        {
            GameObject furnitureParent = new GameObject("Furniture");
            furnitureParent.transform.SetParent(buildingParent.transform);

            foreach (var room in plan.Rooms)
            {
                foreach (var furniture in room.Furniture)
                {
                    GameObject furnitureObj = CreateFurniture(furniture);
                    furnitureObj.transform.SetParent(furnitureParent.transform);
                }
            }
        }

        private GameObject CreateFurniture(FurnitureItem furniture)
        {
            GameObject furnitureObj;
            
            // Try to find appropriate prefab
            GameObject prefab = GetFurniturePrefab(furniture.Type);
            if (prefab != null)
            {
                furnitureObj = Instantiate(prefab);
            }
            else
            {
                // Create primitive as fallback
                furnitureObj = CreatePrimitiveFurniture(furniture);
            }
            
            // Set position, rotation, and scale
            furnitureObj.transform.position = furniture.Position;
            furnitureObj.transform.rotation = Quaternion.Euler(furniture.Rotation);
            furnitureObj.transform.localScale = furniture.Scale;
            
            // Apply material if specified
            if (furniture.Material != null)
            {
                Renderer renderer = furnitureObj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material = furniture.Material;
                }
            }
            
            return furnitureObj;
        }

        private GameObject GetFurniturePrefab(FurnitureType type)
        {
            if (furniturePrefabs == null) return null;
            
            foreach (var prefab in furniturePrefabs)
            {
                if (prefab.name.Contains(type.ToString()))
                {
                    return prefab;
                }
            }
            
            return null;
        }

        private GameObject CreatePrimitiveFurniture(FurnitureItem furniture)
        {
            GameObject obj;
            
            switch (furniture.Type)
            {
                case FurnitureType.Bed:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(2f, 0.5f, 1.5f);
                    break;
                case FurnitureType.Sofa:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(2f, 0.8f, 0.8f);
                    break;
                case FurnitureType.Table:
                case FurnitureType.Desk:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(1.2f, 0.75f, 0.6f);
                    break;
                case FurnitureType.Chair:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(0.5f, 1f, 0.5f);
                    break;
                case FurnitureType.Wardrobe:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(1.5f, 2.2f, 0.6f);
                    break;
                case FurnitureType.TV:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = new Vector3(1.2f, 0.8f, 0.1f);
                    break;
                default:
                    obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    obj.transform.localScale = Vector3.one;
                    break;
            }
            
            obj.name = furniture.Type.ToString();
            return obj;
        }
    }
}
