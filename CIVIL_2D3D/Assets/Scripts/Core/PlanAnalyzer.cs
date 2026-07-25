using System;
using System.Collections.Generic;
using UnityEngine;
using OpenCvSharp;
using System.Linq;

namespace Civil2D3D.Core
{
    public class PlanAnalyzer : MonoBehaviour
    {
        [Header("Analysis Settings")]
        public float MinWallThickness = 5f; // pixels
        public float MaxWallThickness = 20f; // pixels
        public float MinRoomArea = 1000f; // square pixels
        public float DoorWidthRange = 30f; // pixels
        public float WindowWidthRange = 40f; // pixels

        public FloorPlan AnalyzeImage(string imagePath)
        {
            try
            {
                Mat image = Cv2.ImRead(imagePath, ImreadModes.Grayscale);
                if (image.Empty())
                {
                    Debug.LogError($"Failed to load image: {imagePath}");
                    return null;
                }

                FloorPlan floorPlan = new FloorPlan
                {
                    OriginalFilePath = imagePath
                };

                // Preprocess image
                Mat processed = PreprocessImage(image);
                
                // Detect walls
                List<Wall> walls = DetectWalls(processed);
                floorPlan.Walls = walls;
                
                // Identify rooms
                List<Room> rooms = IdentifyRooms(walls, processed);
                floorPlan.Rooms = rooms;
                
                // Find openings (doors and windows)
                List<Opening> openings = FindOpenings(processed, walls);
                ClassifyOpenings(openings, floorPlan);
                
                // Calculate dimensions
                floorPlan.Dimensions = CalculateDimensions(image);
                
                // Clean up
                image.Dispose();
                processed.Dispose();

                return floorPlan;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error analyzing plan: {ex.Message}");
                return null;
            }
        }

        private Mat PreprocessImage(Mat image)
        {
            Mat processed = new Mat();
            
            // Apply Gaussian blur to reduce noise
            Cv2.GaussianBlur(image, processed, new OpenCvSharp.Size(5, 5), 0);
            
            // Adaptive threshold for better edge detection
            Cv2.AdaptiveThreshold(processed, processed, 255, 
                AdaptiveThresholdTypes.MeanC, ThresholdTypes.Binary, 11, 2);
            
            // Morphological operations to clean up
            Mat kernel = Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(3, 3));
            Cv2.MorphologyEx(processed, processed, MorphTypes.Close, kernel);
            
            return processed;
        }

        private List<Wall> DetectWalls(Mat image)
        {
            List<Wall> walls = new List<Wall>();
            
            // Edge detection
            Mat edges = new Mat();
            Cv2.Canny(image, edges, 50, 150, 3, false);
            
            // Hough line detection
            LineSegmentP[] lines = Cv2.HoughLinesP(edges, 1, Math.PI / 180, 
                50, MinWallThickness, MaxWallThickness);
            
            foreach (var line in lines)
            {
                Wall wall = new Wall
                {
                    Start = new Vector2(line.P1.X, line.P1.Y),
                    End = new Vector2(line.P2.X, line.P2.Y),
                    Type = WallType.Interior // Default, will be refined later
                };
                
                walls.Add(wall);
            }
            
            // Filter and merge similar lines
            walls = FilterAndMergeWalls(walls);
            
            edges.Dispose();
            return walls;
        }

        private List<Wall> FilterAndMergeWalls(List<Wall> walls)
        {
            List<Wall> filtered = new List<Wall>();
            float mergeThreshold = 10f; // pixels
            
            for (int i = 0; i < walls.Count; i++)
            {
                bool merged = false;
                for (int j = i + 1; j < walls.Count; j++)
                {
                    if (ShouldMergeWalls(walls[i], walls[j], mergeThreshold))
                    {
                        walls[i] = MergeWalls(walls[i], walls[j]);
                        merged = true;
                        walls.RemoveAt(j);
                        j--;
                    }
                }
                
                if (!merged)
                {
                    filtered.Add(walls[i]);
                }
            }
            
            return filtered;
        }

        private bool ShouldMergeWalls(Wall wall1, Wall wall2, float threshold)
        {
            // Check if walls are collinear and close enough
            Vector2 dir1 = (wall1.End - wall1.Start).normalized;
            Vector2 dir2 = (wall2.End - wall2.Start).normalized;
            
            float angle = Vector2.Angle(dir1, dir2);
            if (angle > 10f) return false;
            
            // Check distance between lines
            float dist = DistanceBetweenLines(wall1, wall2);
            return dist < threshold;
        }

        private float DistanceBetweenLines(Wall wall1, Wall wall2)
        {
            // Simplified distance calculation
            Vector2 mid1 = (wall1.Start + wall1.End) * 0.5f;
            Vector2 mid2 = (wall2.Start + wall2.End) * 0.5f;
            return Vector2.Distance(mid1, mid2);
        }

        private Wall MergeWalls(Wall wall1, Wall wall2)
        {
            List<Vector2> points = new List<Vector2> 
            { 
                wall1.Start, wall1.End, 
                wall2.Start, wall2.End 
            };
            
            // Find extreme points
            points.Sort((a, b) => a.x.CompareTo(b.x));
            Vector2 start = points[0];
            Vector2 end = points[points.Count - 1];
            
            return new Wall
            {
                Start = start,
                End = end,
                Thickness = Mathf.Max(wall1.Thickness, wall2.Thickness),
                Type = wall1.Type
            };
        }

        private List<Room> IdentifyRooms(List<Wall> walls, Mat image)
        {
            List<Room> rooms = new List<Room>();
            
            // Find contours in the image
            Mat binary = new Mat();
            Cv2.Threshold(image, binary, 127, 255, ThresholdTypes.BinaryInv);
            
            OpenCvSharp.Point[][] contours;
            HierarchyIndex[] hierarchy;
            Cv2.FindContours(binary, out contours, out hierarchy, 
                RetrievalModes.External, ContourApproximationModes.ApproxSimple);
            
            foreach (var contour in contours)
            {
                double area = Cv2.ContourArea(contour);
                if (area < MinRoomArea) continue;
                
                // Approximate contour to polygon
                OpenCvSharp.Point[] approx = Cv2.ApproxPolyDP(contour, 
                    Cv2.ArcLength(contour, true) * 0.02, true);
                
                if (approx.Length < 4) continue; // Need at least quadrilateral
                
                Room room = new Room
                {
                    Name = $"Room_{rooms.Count + 1}",
                    Type = ClassifyRoomType(approx, area),
                    Vertices = ConvertToVector2List(approx)
                };
                
                // Calculate room bounds
                CalculateRoomBounds(room);
                
                rooms.Add(room);
            }
            
            binary.Dispose();
            return rooms;
        }

        private string ClassifyRoomType(OpenCvSharp.Point[] vertices, double area)
        {
            // Simple heuristic-based room classification
            float aspectRatio = CalculateAspectRatio(vertices);
            
            if (area > 10000)
            {
                return "LivingRoom";
            }
            else if (aspectRatio < 1.2f)
            {
                return "Bedroom";
            }
            else if (aspectRatio > 2.0f)
            {
                return "Kitchen";
            }
            else if (area < 3000)
            {
                return "Bathroom";
            }
            
            return "Room";
        }

        private float CalculateAspectRatio(OpenCvSharp.Point[] vertices)
        {
            // Find bounding rectangle
            var rect = Cv2.BoundingRect(vertices);
            return (float)rect.Width / rect.Height;
        }

        private List<Vector2> ConvertToVector2List(OpenCvSharp.Point[] points)
        {
            List<Vector2> result = new List<Vector2>();
            foreach (var point in points)
            {
                result.Add(new Vector2(point.X, point.Y));
            }
            return result;
        }

        private void CalculateRoomBounds(Room room)
        {
            if (room.Vertices.Count == 0) return;
            
            Vector2 min = room.Vertices[0];
            Vector2 max = room.Vertices[0];
            
            foreach (var vertex in room.Vertices)
            {
                min = Vector2.Min(min, vertex);
                max = Vector2.Max(max, vertex);
            }
            
            room.Position = min;
            room.Size = max - min;
        }

        private List<Opening> FindOpenings(Mat image, List<Wall> walls)
        {
            List<Opening> openings = new List<Opening>();
            
            // Look for gaps in walls that could be doors or windows
            foreach (var wall in walls)
            {
                List<Opening> wallOpenings = FindOpeningsInWall(image, wall);
                openings.AddRange(wallOpenings);
            }
            
            return openings;
        }

        private List<Opening> FindOpeningsInWall(Mat image, Wall wall)
        {
            List<Opening> openings = new List<Opening>();
            
            // Sample points along the wall
            Vector2 direction = (wall.End - wall.Start).normalized;
            float wallLength = Vector2.Distance(wall.Start, wall.End);
            int samples = Mathf.FloorToInt(wallLength / 5f); // Sample every 5 pixels
            
            List<bool> isWall = new List<bool>();
            
            for (int i = 0; i <= samples; i++)
            {
                Vector2 samplePoint = wall.Start + direction * (i * wallLength / samples);
                bool wallPresent = IsWallPresent(image, samplePoint, direction);
                isWall.Add(wallPresent);
            }
            
            // Find gaps in the wall
            FindGapsInWall(isWall, wall, openings);
            
            return openings;
        }

        private bool IsWallPresent(Mat image, Vector2 point, Vector2 direction)
        {
            // Check perpendicular direction for wall material
            Vector2 perpendicular = new Vector2(-direction.y, direction.x);
            
            for (int offset = -10; offset <= 10; offset++)
            {
                Vector2 checkPoint = point + perpendicular * offset;
                if (checkPoint.x < 0 || checkPoint.x >= image.Width ||
                    checkPoint.y < 0 || checkPoint.y >= image.Height)
                    continue;
                
                byte pixel = image.At<byte>((int)checkPoint.y, (int)checkPoint.x);
                if (pixel > 128) // White pixel indicates wall
                    return true;
            }
            
            return false;
        }

        private void FindGapsInWall(List<bool> isWall, Wall wall, List<Opening> openings)
        {
            bool inGap = false;
            int gapStart = -1;
            
            for (int i = 0; i < isWall.Count; i++)
            {
                if (!isWall[i] && !inGap)
                {
                    // Start of a gap
                    gapStart = i;
                    inGap = true;
                }
                else if (isWall[i] && inGap)
                {
                    // End of a gap
                    int gapEnd = i - 1;
                    int gapLength = gapEnd - gapStart + 1;
                    
                    if (gapLength >= 3) // Minimum gap size
                    {
                        Opening opening = CreateOpeningFromGap(wall, gapStart, gapEnd, isWall.Count);
                        openings.Add(opening);
                    }
                    
                    inGap = false;
                }
            }
        }

        private Opening CreateOpeningFromGap(Wall wall, int startIdx, int endIdx, int totalSamples)
        {
            Vector2 direction = (wall.End - wall.Start).normalized;
            float wallLength = Vector2.Distance(wall.Start, wall.End);
            
            float startDistance = (startIdx * wallLength) / totalSamples;
            float endDistance = (endIdx * wallLength) / totalSamples;
            
            Vector2 position = wall.Start + direction * ((startDistance + endDistance) * 0.5f);
            float width = endDistance - startDistance;
            
            return new Opening
            {
                Position = position,
                Width = width,
                Height = 2.1f, // Default height
                Direction = direction,
                Type = OpeningType.Door // Default, will be refined
            };
        }

        private void ClassifyOpenings(List<Opening> openings, FloorPlan floorPlan)
        {
            foreach (var opening in openings)
            {
                // Simple heuristic: wider openings are doors, narrower are windows
                if (opening.Width > DoorWidthRange)
                {
                    opening.Type = OpeningType.Door;
                    
                    Door door = new Door
                    {
                        Position = opening.Position,
                        Width = opening.Width,
                        Height = opening.Height,
                        Direction = opening.Direction,
                        Type = DoorType.Single
                    };
                    
                    floorPlan.Doors.Add(door);
                }
                else
                {
                    opening.Type = OpeningType.Window;
                    
                    Window window = new Window
                    {
                        Position = opening.Position,
                        Width = opening.Width,
                        Height = 1.5f, // Standard window height
                        SillHeight = 1.0f,
                        Direction = opening.Direction,
                        Type = WindowType.Single
                    };
                    
                    floorPlan.Windows.Add(window);
                }
            }
        }

        private Vector2 CalculateDimensions(Mat image)
        {
            return new Vector2(image.Width, image.Height);
        }
    }
}
