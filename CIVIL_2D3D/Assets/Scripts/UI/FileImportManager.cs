using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace Civil2D3D.UI
{
    public class FileImportManager : MonoBehaviour
    {
        [Header("UI References")]
        public Button importButton;
        public Button browseButton;
        public TMP_Dropdown fileTypeDropdown;
        public TextMeshProUGUI statusText;
        public Image previewImage;
        public GameObject loadingPanel;

        [Header("Import Settings")]
        public Vector2 maxImageSize = new Vector2(2048, 2048);
        public string[] supportedImageFormats = { ".jpg", ".jpeg", ".png", ".bmp", ".tiff" };
        public string[] supportedPDFFormats = { ".pdf" };
        public string[] supportedCADFormats = { ".dwg", ".dxf" };

        [Header("Events")]
        public UnityEvent<FloorPlan> onPlanImported;
        public UnityEvent<string> onImportError;

        private PlanAnalyzer planAnalyzer;
        private string currentFilePath;

        private void Start()
        {
            InitializeUI();
            planAnalyzer = FindObjectOfType<PlanAnalyzer>();
        }

        private void InitializeUI()
        {
            // Setup dropdown
            fileTypeDropdown.ClearOptions();
            fileTypeDropdown.AddOptions(new List<string> { "Image Files", "PDF Files", "CAD Files" });
            fileTypeDropdown.value = 0;

            // Setup button listeners
            if (importButton != null)
                importButton.onClick.AddListener(ImportCurrentFile);

            if (browseButton != null)
                browseButton.onClick.AddListener(BrowseForFile);

            // Initialize UI state
            UpdateStatus("Ready to import file. Click 'Browse' to select a file.");
            loadingPanel.SetActive(false);
        }

        private void BrowseForFile()
        {
            string[] extensions = GetSupportedExtensions();
            string filePath = FileBrowser.OpenFile("Select Architectural Plan", extensions);

            if (!string.IsNullOrEmpty(filePath))
            {
                currentFilePath = filePath;
                LoadFilePreview(filePath);
                UpdateStatus($"File selected: {Path.GetFileName(filePath)}");
            }
        }

        private string[] GetSupportedExtensions()
        {
            List<string> extensions = new List<string>();

            switch (fileTypeDropdown.value)
            {
                case 0: // Image Files
                    foreach (string format in supportedImageFormats)
                        extensions.Add($"Image Files|*{format}");
                    break;
                case 1: // PDF Files
                    foreach (string format in supportedPDFFormats)
                        extensions.Add($"PDF Files|*{format}");
                    break;
                case 2: // CAD Files
                    foreach (string format in supportedCADFormats)
                        extensions.Add($"CAD Files|*{format}");
                    break;
            }

            return extensions.ToArray();
        }

        private void LoadFilePreview(string filePath)
        {
            try
            {
                string extension = Path.GetExtension(filePath).ToLower();

                if (Array.Exists(supportedImageFormats, ext => ext == extension))
                {
                    LoadImagePreview(filePath);
                }
                else if (Array.Exists(supportedPDFFormats, ext => ext == extension))
                {
                    LoadPDFPreview(filePath);
                }
                else if (Array.Exists(supportedCADFormats, ext => ext == extension))
                {
                    LoadCADPreview(filePath);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Error loading preview: {ex.Message}");
            }
        }

        private void LoadImagePreview(string filePath)
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            
            if (texture.LoadImage(fileData))
            {
                // Resize texture if needed
                texture = ResizeTexture(texture, (int)maxImageSize.x, (int)maxImageSize.y);
                
                if (previewImage != null)
                {
                    previewImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                    previewImage.preserveAspect = true;
                }
            }
            else
            {
                UpdateStatus("Failed to load image preview");
            }
        }

        private void LoadPDFPreview(string filePath)
        {
            // For PDF, we'll need to extract the first page as an image
            // This would require a PDF library like iTextSharp or PDFium
            Texture2D pdfPreview = ExtractFirstPageFromPDF(filePath);
            
            if (pdfPreview != null && previewImage != null)
            {
                previewImage.sprite = Sprite.Create(pdfPreview, new Rect(0, 0, pdfPreview.width, pdfPreview.height), Vector2.zero);
                previewImage.preserveAspect = true;
            }
            else
            {
                UpdateStatus("PDF preview not available");
            }
        }

        private void LoadCADPreview(string filePath)
        {
            // For CAD files, we'd need to convert to image format
            // This would require AutoCAD API or similar
            Texture2D cadPreview = ConvertCADToImage(filePath);
            
            if (cadPreview != null && previewImage != null)
            {
                previewImage.sprite = Sprite.Create(cadPreview, new Rect(0, 0, cadPreview.width, cadPreview.height), Vector2.zero);
                previewImage.preserveAspect = true;
            }
            else
            {
                UpdateStatus("CAD preview not available");
            }
        }

        private Texture2D ExtractFirstPageFromPDF(string filePath)
        {
            // Placeholder implementation
            // In a real implementation, you would use a PDF library to render the first page
            Debug.LogWarning("PDF extraction not implemented yet");
            return null;
        }

        private Texture2D ConvertCADToImage(string filePath)
        {
            // Placeholder implementation
            // In a real implementation, you would use AutoCAD API or similar
            Debug.LogWarning("CAD conversion not implemented yet");
            return null;
        }

        private void ImportCurrentFile()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                UpdateStatus("No file selected. Please browse for a file first.");
                return;
            }

            StartCoroutine(ImportFileCoroutine(currentFilePath));
        }

        private System.Collections.IEnumerator ImportFileCoroutine(string filePath)
        {
            ShowLoading(true);
            UpdateStatus("Analyzing plan...");

            try
            {
                string extension = Path.GetExtension(filePath).ToLower();

                if (Array.Exists(supportedImageFormats, ext => ext == extension))
                {
                    yield return StartCoroutine(ImportImageFile(filePath));
                }
                else if (Array.Exists(supportedPDFFormats, ext => ext == extension))
                {
                    yield return StartCoroutine(ImportPDFFile(filePath));
                }
                else if (Array.Exists(supportedCADFormats, ext => ext == extension))
                {
                    yield return StartCoroutine(ImportCADFile(filePath));
                }
                else
                {
                    UpdateStatus("Unsupported file format");
                    onImportError?.Invoke("Unsupported file format");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Import failed: {ex.Message}");
                onImportError?.Invoke(ex.Message);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private System.Collections.IEnumerator ImportImageFile(string filePath)
        {
            yield return new WaitForEndOfFrame();

            if (planAnalyzer != null)
            {
                FloorPlan plan = planAnalyzer.AnalyzeImage(filePath);
                
                if (plan != null)
                {
                    UpdateStatus("Plan successfully imported!");
                    onPlanImported?.Invoke(plan);
                }
                else
                {
                    UpdateStatus("Failed to analyze plan");
                    onImportError?.Invoke("Plan analysis failed");
                }
            }
            else
            {
                UpdateStatus("Plan analyzer not found");
                onImportError?.Invoke("Plan analyzer not found");
            }
        }

        private System.Collections.IEnumerator ImportPDFFile(string filePath)
        {
            UpdateStatus("PDF import not implemented yet");
            yield return null;
        }

        private System.Collections.IEnumerator ImportCADFile(string filePath)
        {
            UpdateStatus("CAD import not implemented yet");
            yield return null;
        }

        private Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
            RenderTexture.active = rt;

            Graphics.Blit(source, rt);

            Texture2D result = new Texture2D(targetWidth, targetHeight);
            result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
            result.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            return result;
        }

        private void ShowLoading(bool show)
        {
            if (loadingPanel != null)
                loadingPanel.SetActive(show);
        }

        private void UpdateStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
            
            Debug.Log($"[FileImportManager] {message}");
        }

        public void ClearCurrentFile()
        {
            currentFilePath = null;
            if (previewImage != null)
                previewImage.sprite = null;
            UpdateStatus("File cleared. Ready to import new file.");
        }

        public string GetCurrentFilePath()
        {
            return currentFilePath;
        }

        public bool HasFileSelected()
        {
            return !string.IsNullOrEmpty(currentFilePath);
        }
    }

    // Simple file browser helper (would need to be implemented based on platform)
    public static class FileBrowser
    {
        public static string OpenFile(string title, string[] extensions)
        {
            // This is a placeholder implementation
            // In a real application, you would use platform-specific file dialogs
            // For Windows: System.Windows.Forms.OpenFileDialog
            // For Mac: NSOpenPanel
            // For cross-platform: Unity's native file dialog or third-party asset
            
            Debug.LogWarning("FileBrowser.OpenFile not implemented - using placeholder");
            
            // For now, return a sample path for testing
            return Application.streamingAssetsPath + "/SamplePlan.jpg";
        }
    }
}
