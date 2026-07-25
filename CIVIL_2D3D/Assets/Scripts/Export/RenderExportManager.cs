using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Civil2D3D.Export
{
    public class RenderExportManager : MonoBehaviour
    {
        [Header("Render Settings")]
        public int renderWidth = 1920;
        public int renderHeight = 1080;
        public float renderScale = 1.0f;
        public bool enableAntiAliasing = true;
        public int antiAliasingQuality = 4;

        [Header("Camera Settings")]
        public Camera[] exportCameras;
        public bool renderAllCameras = true;
        public float cameraDistance = 10f;
        public float cameraHeight = 5f;

        [Header("Export Settings")]
        public string exportFolder = "Exports";
        public string fileNamePrefix = "ArchitecturalRender";
        public ImageFormat imageFormat = ImageFormat.PNG;
        public bool includeTimestamp = true;

        [Header("3D Export Settings")]
        public bool export3DModels = true;
        public bool exportTextures = true;
        public ModelFormat modelFormat = ModelFormat.OBJ;

        [Header("PDF Export Settings")]
        public bool exportPDF = true;
        public bool includeFloorPlan = true;
        public bool includeElevation = true;
        public bool includeMaterialList = true;

        private UniversalAdditionalCameraData[] cameraData;
        private RenderTexture renderTexture;
        private Camera renderCamera;
        private GameObject buildingParent;

        public enum ImageFormat
        {
            PNG,
            JPG,
            EXR
        }

        public enum ModelFormat
        {
            OBJ,
            FBX,
            GLTF
        }

        private void Start()
        {
            InitializeRenderSystem();
            CreateExportDirectory();
        }

        private void InitializeRenderSystem()
        {
            // Get camera data for URP
            List<UniversalAdditionalCameraData> cameraDataList = new List<UniversalAdditionalCameraData>();
            
            if (exportCameras != null)
            {
                foreach (var cam in exportCameras)
                {
                    if (cam != null)
                    {
                        var data = cam.GetComponent<UniversalAdditionalCameraData>();
                        if (data != null)
                            cameraDataList.Add(data);
                    }
                }
            }
            
            cameraData = cameraDataList.ToArray();

            // Create render texture
            renderTexture = new RenderTexture(renderWidth, renderHeight, 24, RenderTextureFormat.ARGB32);
            renderTexture.antiAliasing = enableAntiAliasing ? antiAliasingQuality : 1;
            renderTexture.name = "ExportRenderTexture";
        }

        private void CreateExportDirectory()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, exportFolder);
            if (!Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
        }

        public void SetBuildingParent(GameObject building)
        {
            buildingParent = building;
        }

        public void RenderAllViews()
        {
            StartCoroutine(RenderAllViewsCoroutine());
        }

        private System.Collections.IEnumerator RenderAllViewsCoroutine()
        {
            Debug.Log("Starting render process...");

            if (buildingParent == null)
            {
                Debug.LogError("No building parent set for rendering");
                yield break;
            }

            List<string> renderedImages = new List<string>();

            // Render from each camera
            if (renderAllCameras && exportCameras != null)
            {
                for (int i = 0; i < exportCameras.Length; i++)
                {
                    if (exportCameras[i] != null)
                    {
                        string imagePath = yield return StartCoroutine(RenderFromCamera(exportCameras[i], $"View_{i + 1}"));
                        if (!string.IsNullOrEmpty(imagePath))
                            renderedImages.Add(imagePath);
                        
                        yield return new WaitForSeconds(0.1f); // Small delay between renders
                    }
                }
            }
            else
            {
                // Render from default positions
                renderedImages.AddRange(yield return StartCoroutine(RenderFromDefaultPositions()));
            }

            // Export 3D models if enabled
            if (export3DModels)
            {
                yield return StartCoroutine(Export3DModels());
            }

            // Export PDF if enabled
            if (exportPDF)
            {
                yield return StartCoroutine(ExportPDF(renderedImages));
            }

            Debug.Log("Render process completed");
        }

        private System.Collections.IEnumerator RenderFromCamera(Camera camera, string viewName)
        {
            string fileName = GenerateFileName(viewName);
            string filePath = Path.Combine(Application.persistentDataPath, exportFolder, fileName);

            // Setup camera for rendering
            Camera originalCamera = camera;
            renderCamera = originalCamera;

            // Set target texture
            renderCamera.targetTexture = renderTexture;

            // Render the frame
            renderCamera.Render();

            yield return new WaitForEndOfFrame();

            // Read pixels and save
            Texture2D renderImage = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            renderImage.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            renderImage.Apply();
            RenderTexture.active = null;

            // Save to file
            byte[] fileData = EncodeImage(renderImage);
            File.WriteAllBytes(filePath, fileData);

            // Cleanup
            renderCamera.targetTexture = null;
            Destroy(renderImage);

            Debug.Log($"Rendered view: {viewName} -> {filePath}");
            yield return filePath;
        }

        private System.Collections.IEnumerator RenderFromDefaultPositions()
        {
            List<string> images = new List<string>();

            // Create temporary camera for rendering
            GameObject tempCameraObj = new GameObject("TempRenderCamera");
            Camera tempCamera = tempCameraObj.AddComponent<Camera>();
            tempCamera.fieldOfView = 60f;
            tempCamera.nearClipPlane = 0.1f;
            tempCamera.farClipPlane = 100f;

            if (buildingParent != null)
            {
                Bounds bounds = CalculateBuildingBounds(buildingParent);
                Vector3 center = bounds.center;

                // Generate camera positions around the building
                Vector3[] positions = {
                    new Vector3(center.x + cameraDistance, center.y + cameraHeight, center.z), // Front
                    new Vector3(center.x - cameraDistance, center.y + cameraHeight, center.z), // Back
                    new Vector3(center.x, center.y + cameraHeight, center.z + cameraDistance), // Right
                    new Vector3(center.x, center.y + cameraHeight, center.z - cameraDistance), // Left
                    new Vector3(center.x, center.y + cameraDistance * 2, center.z)  // Top
                };

                Vector3[] targets = {
                    center,
                    center,
                    center,
                    center,
                    center
                };

                string[] viewNames = { "Front", "Back", "Right", "Left", "Top" };

                for (int i = 0; i < positions.Length; i++)
                {
                    tempCamera.transform.position = positions[i];
                    tempCamera.transform.LookAt(targets[i]);

                    string imagePath = yield return StartCoroutine(RenderFromCamera(tempCamera, viewNames[i]));
                    if (!string.IsNullOrEmpty(imagePath))
                        images.Add(imagePath);
                    
                    yield return new WaitForSeconds(0.1f);
                }
            }

            // Cleanup
            Destroy(tempCameraObj);

            yield return images;
        }

        private Bounds CalculateBuildingBounds(GameObject building)
        {
            Renderer[] renderers = building.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
            {
                return new Bounds(building.transform.position, Vector3.one);
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            return bounds;
        }

        private byte[] EncodeImage(Texture2D texture)
        {
            switch (imageFormat)
            {
                case ImageFormat.PNG:
                    return texture.EncodeToPNG();
                case ImageFormat.JPG:
                    return texture.EncodeToJPG(90);
                case ImageFormat.EXR:
                    return texture.EncodeToEXR();
                default:
                    return texture.EncodeToPNG();
            }
        }

        private string GenerateFileName(string viewName)
        {
            string fileName = fileNamePrefix;
            
            if (includeTimestamp)
            {
                fileName += $"_{DateTime.Now:yyyyMMdd_HHmmss}";
            }
            
            fileName += $"_{viewName}";
            
            switch (imageFormat)
            {
                case ImageFormat.PNG:
                    fileName += ".png";
                    break;
                case ImageFormat.JPG:
                    fileName += ".jpg";
                    break;
                case ImageFormat.EXR:
                    fileName += ".exr";
                    break;
            }
            
            return fileName;
        }

        private System.Collections.IEnumerator Export3DModels()
        {
            if (buildingParent == null) yield break;

            string fileName = GenerateFileName("3DModel");
            string filePath = Path.Combine(Application.persistentDataPath, exportFolder, fileName);

            switch (modelFormat)
            {
                case ModelFormat.OBJ:
                    yield return StartCoroutine(ExportToOBJ(filePath));
                    break;
                case ModelFormat.FBX:
                    yield return StartCoroutine(ExportToFBX(filePath));
                    break;
                case ModelFormat.GLTF:
                    yield return StartCoroutine(ExportToGLTF(filePath));
                    break;
            }
        }

        private System.Collections.IEnumerator ExportToOBJ(string filePath)
        {
            // Simple OBJ export implementation
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("# OBJ exported from Civil 2D3D");
                writer.WriteLine($"# Generated: {DateTime.Now}");

                MeshFilter[] meshFilters = buildingParent.GetComponentsInChildren<MeshFilter>();
                int vertexOffset = 0;

                foreach (var meshFilter in meshFilters)
                {
                    Mesh mesh = meshFilter.sharedMesh;
                    if (mesh == null) continue;

                    Transform transform = meshFilter.transform;

                    // Write vertices
                    for (int i = 0; i < mesh.vertices.Length; i++)
                    {
                        Vector3 vertex = transform.TransformPoint(mesh.vertices[i]);
                        writer.WriteLine($"v {vertex.x:F6} {vertex.y:F6} {vertex.z:F6}");
                    }

                    // Write UVs
                    for (int i = 0; i < mesh.uv.Length; i++)
                    {
                        Vector2 uv = mesh.uv[i];
                        writer.WriteLine($"vt {uv.x:F6} {uv.y:F6}");
                    }

                    // Write normals
                    for (int i = 0; i < mesh.normals.Length; i++)
                    {
                        Vector3 normal = transform.TransformDirection(mesh.normals[i]);
                        writer.WriteLine($"vn {normal.x:F6} {normal.y:F6} {normal.z:F6}");
                    }

                    // Write faces
                    for (int i = 0; i < mesh.triangles.Length; i += 3)
                    {
                        int v1 = mesh.triangles[i] + vertexOffset + 1;
                        int v2 = mesh.triangles[i + 1] + vertexOffset + 1;
                        int v3 = mesh.triangles[i + 2] + vertexOffset + 1;

                        writer.WriteLine($"f {v1}/{v1}/{v1} {v2}/{v2}/{v2} {v3}/{v3}/{v3}");
                    }

                    vertexOffset += mesh.vertices.Length;
                }
            }

            Debug.Log($"Exported 3D model to: {filePath}");
            yield return null;
        }

        private System.Collections.IEnumerator ExportToFBX(string filePath)
        {
            // FBX export would require a third-party library
            Debug.LogWarning("FBX export not implemented - requires third-party library");
            yield return null;
        }

        private System.Collections.IEnumerator ExportToGLTF(string filePath)
        {
            // GLTF export would require a third-party library
            Debug.LogWarning("GLTF export not implemented - requires third-party library");
            yield return null;
        }

        private System.Collections.IEnumerator ExportPDF(List<string> renderedImages)
        {
            // PDF export would require a PDF library like iTextSharp
            Debug.LogWarning("PDF export not implemented - requires PDF library");
            yield return null;
        }

        public void SetRenderResolution(int width, int height)
        {
            renderWidth = width;
            renderHeight = height;

            if (renderTexture != null)
            {
                renderTexture.Release();
                renderTexture.width = width;
                renderTexture.height = height;
                renderTexture.Create();
            }
        }

        public void SetImageFormat(ImageFormat format)
        {
            imageFormat = format;
        }

        public void SetModelFormat(ModelFormat format)
        {
            modelFormat = format;
        }

        public void SetAntiAliasing(bool enabled, int quality = 4)
        {
            enableAntiAliasing = enabled;
            antiAliasingQuality = quality;

            if (renderTexture != null)
            {
                renderTexture.antiAliasing = enabled ? quality : 1;
            }
        }

        public string GetExportFolder()
        {
            return Path.Combine(Application.persistentDataPath, exportFolder);
        }

        public void ClearExportFolder()
        {
            string folderPath = GetExportFolder();
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                foreach (var file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (var dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
        }

        private void OnDestroy()
        {
            if (renderTexture != null)
            {
                renderTexture.Release();
                Destroy(renderTexture);
            }
        }
    }
}
