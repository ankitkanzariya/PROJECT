using System;
using System.Collections.Generic;
using UnityEngine;

namespace Civil2D3D.InteriorDesign
{
    public class FurniturePlanner : MonoBehaviour
    {
        [Header("Furniture Settings")]
        public float minimumWalkwayWidth = 0.8f; // meters
        public float doorClearance = 1.2f; // meters
        public float wallDistance = 0.1f; // meters from wall
        
        [Header("Standard Furniture Sizes")]
        public Vector3 singleBedSize = new Vector3(0.9f, 0.6f, 1.9f);
        public Vector3 doubleBedSize = new Vector3(1.4f, 0.6f, 1.9f);
        public Vector3 kingBedSize = new Vector3(1.8f, 0.6f, 2.0f);
        public Vector3 sofaSize = new Vector3(2.0f, 0.8f, 0.8f);
        public Vector3 diningTableSize = new Vector3(1.2f, 0.75f, 0.8f);
        public Vector3 wardrobeSize = new Vector3(0.6f, 2.2f, 2.4f);
        public Vector3 tvSize = new Vector3(1.2f, 0.8f, 0.1f);
        public Vector3 deskSize = new Vector3(1.2f, 0.75f, 0.6f);
        public Vector3 kitchenCounterSize = new Vector3(0.6f, 0.9f, 2.4f);

        [Header("Room Configuration")]
        public RoomConfiguration[] roomConfigurations;

        [Serializable]
        public class RoomConfiguration
        {
            public string roomType;
            public FurnitureRequirement[] requiredFurniture;
            public bool autoPlace = true;
        }

        [Serializable]
        public class FurnitureRequirement
        {
            public FurnitureType type;
            public bool required;
            public Vector3? customSize;
            public string placementHint; // "against_wall", "center", "corner", "near_window"
        }

        public void PlanFurnitureForRoom(Room room)
        {
            RoomConfiguration config = GetRoomConfiguration(room.Type);
            if (config == null)
            {
                Debug.LogWarning($"No configuration found for room type: {room.Type}");
                return;
            }

            room.Furniture.Clear();

            foreach (var requirement in config.requiredFurniture)
            {
                if (requirement.required)
                {
                    FurnitureItem furniture = CreateFurnitureItem(requirement, room);
                    if (furniture != null)
                    {
                        if (config.autoPlace)
                        {
                            Vector3 position = FindOptimalPosition(furniture, room);
                            furniture.Position = position;
                        }
                        
                        room.Furniture.Add(furniture);
                    }
                }
            }
        }

        private RoomConfiguration GetRoomConfiguration(string roomType)
        {
            foreach (var config in roomConfigurations)
            {
                if (config.roomType.Equals(roomType, StringComparison.OrdinalIgnoreCase))
                {
                    return config;
                }
            }
            return null;
        }

        private FurnitureItem CreateFurnitureItem(FurnitureRequirement requirement, Room room)
        {
            FurnitureItem furniture = new FurnitureItem
            {
                Name = requirement.type.ToString(),
                Type = requirement.type,
                Rotation = Vector3.zero,
                Scale = GetFurnitureSize(requirement.type, requirement.customSize)
            };

            return furniture;
        }

        private Vector3 GetFurnitureSize(FurnitureType type, Vector3? customSize)
        {
            if (customSize.HasValue)
                return customSize.Value;

            switch (type)
            {
                case FurnitureType.Bed:
                    return singleBedSize; // Default to single, can be upgraded
                case FurnitureType.Sofa:
                    return sofaSize;
                case FurnitureType.Table:
                    return diningTableSize;
                case FurnitureType.Chair:
                    return new Vector3(0.5f, 1.0f, 0.5f);
                case FurnitureType.Wardrobe:
                    return wardrobeSize;
                case FurnitureType.TV:
                    return tvSize;
                case FurnitureType.Desk:
                    return deskSize;
                case FurnitureType.DiningTable:
                    return diningTableSize;
                case FurnitureType.KitchenCounter:
                    return kitchenCounterSize;
                default:
                    return Vector3.one;
            }
        }

        private Vector3 FindOptimalPosition(FurnitureItem furniture, Room room)
        {
            RoomConfiguration config = GetRoomConfiguration(room.Type);
            FurnitureRequirement requirement = GetRequirementForFurniture(config, furniture.Type);
            
            string placementHint = requirement?.placementHint ?? "against_wall";
            
            switch (placementHint)
            {
                case "against_wall":
                    return FindWallPosition(furniture, room);
                case "center":
                    return FindCenterPosition(furniture, room);
                case "corner":
                    return FindCornerPosition(furniture, room);
                case "near_window":
                    return FindNearWindowPosition(furniture, room);
                default:
                    return FindWallPosition(furniture, room);
            }
        }

        private FurnitureRequirement GetRequirementForFurniture(RoomConfiguration config, FurnitureType type)
        {
            if (config?.requiredFurniture == null) return null;
            
            foreach (var requirement in config.requiredFurniture)
            {
                if (requirement.type == type)
                    return requirement;
            }
            return null;
        }

        private Vector3 FindWallPosition(FurnitureItem furniture, Room room)
        {
            Vector3 bestPosition = Vector3.zero;
            float bestScore = float.MaxValue;

            // Try each wall
            for (int wallIndex = 0; wallIndex < room.Vertices.Count; wallIndex++)
            {
                Vector3 wallPosition = GetPositionAlongWall(furniture, room, wallIndex);
                float score = EvaluatePosition(wallPosition, furniture, room);
                
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPosition = wallPosition;
                }
            }

            return bestPosition;
        }

        private Vector3 GetPositionAlongWall(FurnitureItem furniture, Room room, int wallIndex)
        {
            Vector2 wallStart = room.Vertices[wallIndex];
            Vector2 wallEnd = room.Vertices[(wallIndex + 1) % room.Vertices.Count];
            
            Vector2 wallDirection = (wallEnd - wallStart).normalized;
            Vector2 wallNormal = new Vector2(-wallDirection.y, wallDirection.x);
            
            // Place furniture against the wall
            Vector2 furnitureCenter = wallStart + wallDirection * (Vector2.Distance(wallStart, wallEnd) * 0.5f);
            furnitureCenter += wallNormal * (furniture.Scale.x * 0.5f + wallDistance);
            
            return new Vector3(furnitureCenter.x, 0, furnitureCenter.y);
        }

        private Vector3 FindCenterPosition(FurnitureItem furniture, Room room)
        {
            Vector2 center = Vector2.zero;
            foreach (var vertex in room.Vertices)
            {
                center += vertex;
            }
            center /= room.Vertices.Count;
            
            return new Vector3(center.x, 0, center.y);
        }

        private Vector3 FindCornerPosition(FurnitureItem furniture, Room room)
        {
            Vector3 bestPosition = Vector3.zero;
            float bestScore = float.MaxValue;

            // Try each corner
            for (int cornerIndex = 0; cornerIndex < room.Vertices.Count; cornerIndex++)
            {
                Vector2 corner = room.Vertices[cornerIndex];
                Vector2 nextCorner = room.Vertices[(cornerIndex + 1) % room.Vertices.Count];
                Vector2 prevCorner = room.Vertices[(cornerIndex - 1 + room.Vertices.Count) % room.Vertices.Count];
                
                // Calculate corner position
                Vector2 cornerDirection = (nextCorner - corner).normalized;
                Vector2 prevDirection = (prevCorner - corner).normalized;
                Vector2 cornerNormal = (cornerDirection + prevDirection).normalized * 0.5f;
                
                Vector2 furniturePosition = corner + cornerNormal * (furniture.Scale.x * 0.5f + wallDistance);
                
                Vector3 position = new Vector3(furniturePosition.x, 0, furniturePosition.y);
                float score = EvaluatePosition(position, furniture, room);
                
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPosition = position;
                }
            }

            return bestPosition;
        }

        private Vector3 FindNearWindowPosition(FurnitureItem furniture, Room room)
        {
            // Find windows in the room
            List<Vector2> windowPositions = GetWindowPositionsInRoom(room);
            
            if (windowPositions.Count == 0)
            {
                return FindCenterPosition(furniture, room);
            }

            Vector3 bestPosition = Vector3.zero;
            float bestScore = float.MaxValue;

            foreach (var windowPos in windowPositions)
            {
                Vector3 position = new Vector3(windowPos.x, 0, windowPos.y);
                // Place furniture at a reasonable distance from window
                position.z -= furniture.Scale.z * 0.5f + 0.5f;
                
                float score = EvaluatePosition(position, furniture, room);
                
                if (score < bestScore)
                {
                    bestScore = score;
                    bestPosition = position;
                }
            }

            return bestPosition;
        }

        private List<Vector2> GetWindowPositionsInRoom(Room room)
        {
            List<Vector2> windowPositions = new List<Vector2>();
            
            // This would need access to the floor plan's windows
            // For now, return empty list
            // In a full implementation, you'd check which windows are within this room's bounds
            
            return windowPositions;
        }

        private float EvaluatePosition(Vector3 position, FurnitureItem furniture, Room room)
        {
            float score = 0f;

            // Check if position is within room bounds
            if (!IsPositionInRoom(position, room))
            {
                score += 1000f; // Heavy penalty for being outside
            }

            // Check collisions with other furniture
            float collisionPenalty = CheckCollisions(position, furniture, room);
            score += collisionPenalty;

            // Check walkway clearance
            float walkwayPenalty = CheckWalkwayClearance(position, furniture, room);
            score += walkwayPenalty;

            // Check door clearance
            float doorPenalty = CheckDoorClearance(position, furniture, room);
            score += doorPenalty;

            return score;
        }

        private bool IsPositionInRoom(Vector3 position, Room room)
        {
            Vector2 pos2D = new Vector2(position.x, position.z);
            return IsPointInPolygon(pos2D, room.Vertices);
        }

        private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
        {
            bool inside = false;
            int j = polygon.Count - 1;

            for (int i = 0; i < polygon.Count; i++)
            {
                if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                    (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) / (polygon[j].y - polygon[i].y) + polygon[i].x))
                {
                    inside = !inside;
                }
                j = i;
            }

            return inside;
        }

        private float CheckCollisions(Vector3 position, FurnitureItem furniture, Room room)
        {
            float penalty = 0f;

            foreach (var otherFurniture in room.Furniture)
            {
                if (otherFurniture == furniture) continue;

                float distance = Vector3.Distance(position, otherFurniture.Position);
                float minDistance = (furniture.Scale.x + otherFurniture.Scale.x) * 0.5f + minimumWalkwayWidth;

                if (distance < minDistance)
                {
                    penalty += (minDistance - distance) * 10f;
                }
            }

            return penalty;
        }

        private float CheckWalkwayClearance(Vector3 position, FurnitureItem furniture, Room room)
        {
            // Check if furniture blocks main walkways
            // This is a simplified implementation
            Vector3 center = FindCenterPosition(furniture, room);
            float distanceFromCenter = Vector3.Distance(position, center);
            
            // Penalty for blocking center area
            if (distanceFromCenter < minimumWalkwayWidth)
            {
                return (minimumWalkwayWidth - distanceFromCenter) * 5f;
            }

            return 0f;
        }

        private float CheckDoorClearance(Vector3 position, FurnitureItem furniture, Room room)
        {
            float penalty = 0f;

            // This would need access to door positions
            // For now, return 0
            // In a full implementation, you'd check distance to doors and apply penalties

            return penalty;
        }

        public void UpgradeBedSize(Room room, BedSize bedSize)
        {
            foreach (var furniture in room.Furniture)
            {
                if (furniture.Type == FurnitureType.Bed)
                {
                    switch (bedSize)
                    {
                        case BedSize.Single:
                            furniture.Scale = singleBedSize;
                            break;
                        case BedSize.Double:
                            furniture.Scale = doubleBedSize;
                            break;
                        case BedSize.King:
                            furniture.Scale = kingBedSize;
                            break;
                    }
                    
                    // Re-optimize position
                    Vector3 newPosition = FindOptimalPosition(furniture, room);
                    furniture.Position = newPosition;
                    break;
                }
            }
        }

        public void AddFurnitureToRoom(Room room, FurnitureType type, Vector3 position)
        {
            FurnitureItem furniture = new FurnitureItem
            {
                Name = type.ToString(),
                Type = type,
                Position = position,
                Rotation = Vector3.zero,
                Scale = GetFurnitureSize(type, null)
            };

            room.Furniture.Add(furniture);
        }

        public void RemoveFurnitureFromRoom(Room room, FurnitureItem furniture)
        {
            room.Furniture.Remove(furniture);
        }

        public bool ValidateFurnitureLayout(Room room)
        {
            foreach (var furniture in room.Furniture)
            {
                if (!IsPositionInRoom(furniture.Position, room))
                {
                    Debug.LogWarning($"Furniture {furniture.Name} is outside room bounds");
                    return false;
                }

                float collisionPenalty = CheckCollisions(furniture.Position, furniture, room);
                if (collisionPenalty > 0)
                {
                    Debug.LogWarning($"Furniture {furniture.Name} has collision issues");
                    return false;
                }
            }

            return true;
        }
    }

    public enum BedSize
    {
        Single,
        Double,
        King
    }
}
