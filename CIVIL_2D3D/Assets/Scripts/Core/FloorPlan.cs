using System;
using System.Collections.Generic;
using UnityEngine;

namespace Civil2D3D.Core
{
    [Serializable]
    public class FloorPlan
    {
        public List<Room> Rooms = new List<Room>();
        public List<Wall> Walls = new List<Wall>();
        public List<Door> Doors = new List<Door>();
        public List<Window> Windows = new List<Window>();
        public Vector2 Dimensions;
        public float Scale = 1.0f; // pixels per unit
        public string OriginalFilePath;
        public DateTime CreatedDate = DateTime.Now;
    }

    [Serializable]
    public class Room
    {
        public string Name;
        public string Type; // Bedroom, Kitchen, LivingRoom, etc.
        public Vector2 Position;
        public Vector2 Size;
        public List<Vector2> Vertices = new List<Vector2>();
        public List<FurnitureItem> Furniture = new List<FurnitureItem>();
        public MaterialScheme Materials;
        public List<Opening> Openings = new List<Opening>();
    }

    [Serializable]
    public class Wall
    {
        public Vector2 Start;
        public Vector2 End;
        public float Thickness = 0.15f; // meters
        public float Height = 3.0f; // meters
        public WallType Type;
        public Material Material;
    }

    [Serializable]
    public class Door
    {
        public Vector2 Position;
        public float Width = 0.8f;
        public float Height = 2.1f;
        public DoorType Type;
        public float OpenAngle = 90f;
        public Vector2 Direction; // which way door opens
    }

    [Serializable]
    public class Window
    {
        public Vector2 Position;
        public float Width = 1.2f;
        public float Height = 1.5f;
        public float SillHeight = 1.0f; // from floor
        public WindowType Type;
        public Vector2 Direction; // which wall it's on
    }

    [Serializable]
    public class FurnitureItem
    {
        public string Name;
        public FurnitureType Type;
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        public string ModelPath;
        public Material Material;
        public bool IsLocked = false;
    }

    [Serializable]
    public class MaterialScheme
    {
        public Material WallMaterial;
        public Material FloorMaterial;
        public Material CeilingMaterial;
        public Color WallColor = Color.white;
        public Color FloorColor = Color.gray;
        public Color CeilingColor = Color.white;
    }

    [Serializable]
    public class Opening
    {
        public OpeningType Type;
        public Vector2 Position;
        public float Width;
        public float Height;
        public Vector2 Direction;
    }

    public enum WallType { Interior, Exterior, Structural }
    public enum DoorType { Single, Double, Sliding, French }
    public enum WindowType { Single, Double, Bay, Sliding }
    public enum FurnitureType { Bed, Sofa, Table, Chair, Wardrobe, TV, AC, Desk, DiningTable, KitchenCounter }
    public enum OpeningType { Door, Window, Garage }
}
