import { useState } from "react";
import { FileText, Languages, Upload, X } from "lucide-react";
import PDFViewer from "./components/PDFViewer";
import LanguageSelector from "./components/LanguageSelector";

function App() {
  const [pdfFile, setPdfFile] = useState(null);
  const [selectedLanguage, setSelectedLanguage] = useState("gu");
  const [translatedText, setTranslatedText] = useState("");
  const [isTranslating, setIsTranslating] = useState(false);

  const languages = [
    { code: "gu", name: "Gujarati" },
    { code: "hi", name: "Hindi" },
    { code: "bn", name: "Bengali" },
    { code: "ta", name: "Tamil" },
    { code: "te", name: "Telugu" },
    { code: "mr", name: "Marathi" },
    { code: "kn", name: "Kannada" },
    { code: "ml", name: "Malayalam" },
    { code: "pa", name: "Punjabi" },
    { code: "es", name: "Spanish" },
    { code: "fr", name: "French" },
    { code: "de", name: "German" },
    { code: "ja", name: "Japanese" },
    { code: "ko", name: "Korean" },
    { code: "zh", name: "Chinese" },
    { code: "ar", name: "Arabic" },
    { code: "ru", name: "Russian" },
    { code: "pt", name: "Portuguese" },
    { code: "it", name: "Italian" },
    { code: "nl", name: "Dutch" },
  ];

  const handleFileUpload = (event) => {
    const file = event.target.files[0];
    if (file && file.type === "application/pdf") {
      const fileUrl = URL.createObjectURL(file);
      setPdfFile(fileUrl);
      setTranslatedText("");
    }
  };

  const handleTranslate = async () => {
    if (!pdfFile) return;

    setIsTranslating(true);

    try {
      // Extract text from PDF
      console.log("Step 1: Extracting text from PDF");
      const text = await extractTextFromPDF(pdfFile);

      if (!text || text.length === 0) {
        throw new Error("No text could be extracted from PDF");
      }

      console.log("Step 2: Translating text");
      // Translate text using MyMemory Translation API
      const translated = await translateText(text, selectedLanguage);

      setTranslatedText(translated);
    } catch (error) {
      console.error("Translation error:", error);
      console.error("Error details:", error.message);
      alert(`Translation failed: ${error.message}. Please try again.`);
    } finally {
      setIsTranslating(false);
    }
  };

  const extractTextFromPDF = async (fileUrl) => {
    try {
      console.log("Starting PDF extraction from:", fileUrl);
      const pdfjsLib = await import("pdfjs-dist");
      // ✅ Set the worker source using Vite's import.meta.url
      pdfjsLib.GlobalWorkerOptions.workerSrc = new URL(
        "pdfjs-dist/build/pdf.worker.mjs",
        import.meta.url,
      ).href;

      console.log("Fetching PDF file...");
      const response = await fetch(fileUrl);
      const arrayBuffer = await response.arrayBuffer();
      console.log("PDF fetched, size:", arrayBuffer.byteLength);

      console.log("Loading PDF document...");
      const pdf = await pdfjsLib.getDocument(arrayBuffer).promise;
      console.log("PDF loaded, pages:", pdf.numPages);

      let fullText = "";

      for (let i = 1; i <= pdf.numPages; i++) {
        console.log(`Extracting text from page ${i}/${pdf.numPages}`);
        const page = await pdf.getPage(i);
        const textContent = await page.getTextContent();
        const pageText = textContent.items.map((item) => item.str).join(" ");
        fullText += pageText + "\n\n";
      }

      console.log("Text extraction completed. Total length:", fullText.length);
      return fullText;
    } catch (error) {
      console.error("PDF extraction error:", error);
      throw error;
    }
  };

  const translateText = async (text, targetLang) => {
    console.log("Starting translation for language:", targetLang);
    console.log("Text length:", text.length);
    console.log(
      "Text snippet for translation:",
      text.substring(0, 200) + "...",
    );

    // Split text into smaller chunks to avoid API limits
    const chunks = text.match(/.{1,500}/g) || [text];
    const translatedChunks = [];

    for (let i = 0; i < chunks.length; i++) {
      const chunk = chunks[i];
      if (!chunk.trim()) continue;

      try {
        console.log(`Translating chunk ${i + 1}/${chunks.length}`);

        // Using MyMemory Translation API (free, no API key required)
        const url = `https://api.mymemory.translated.net/get?q=${encodeURIComponent(chunk)}&langpair=en|${targetLang}`;

        const response = await fetch(url);

        if (!response.ok) {
          console.error("HTTP error:", response.status);
          throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        if (data && data.responseData && data.responseData.translatedText) {
          const translatedChunk = data.responseData.translatedText;
          translatedChunks.push(translatedChunk);
          console.log(`Chunk ${i + 1} translated successfully`);
        } else {
          console.error("Invalid response data:", data);
          translatedChunks.push(chunk);
        }

        // Add delay to avoid rate limiting
        await new Promise((resolve) => setTimeout(resolve, 300));
      } catch (error) {
        console.error("Translation chunk error:", error);
        // If a chunk fails, include original text
        translatedChunks.push(chunk);
      }
    }

    if (translatedChunks.length === 0) {
      throw new Error("Translation failed - no content was translated");
    }

    console.log(
      "Translation completed. Total chunks:",
      translatedChunks.length,
    );
    return translatedChunks.join(" ");
  };

  const clearPDF = () => {
    if (pdfFile && typeof pdfFile === "string") {
      URL.revokeObjectURL(pdfFile);
    }
    setPdfFile(null);
    setTranslatedText("");
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-900 via-purple-900 to-slate-900">
      <div className="container mx-auto px-4 py-8">
        {/* Header */}
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold text-white mb-2 flex items-center justify-center gap-3">
            <FileText className="w-10 h-10" />
            PDF Translator
          </h1>
          <p className="text-purple-200">
            Import PDF, translate to any language, view in tabs
          </p>
        </div>

        {/* Upload Section */}
        {!pdfFile && (
          <div className="max-w-2xl mx-auto mb-8">
            <div className="bg-white/10 backdrop-blur-lg rounded-2xl p-8 border border-white/20">
              <div className="border-2 border-dashed border-purple-400 rounded-xl p-12 text-center hover:border-purple-300 transition-colors">
                <Upload className="w-16 h-16 mx-auto mb-4 text-purple-400" />
                <p className="text-white text-lg mb-4">
                  Drop your PDF here or click to upload
                </p>
                <input
                  type="file"
                  accept=".pdf"
                  onChange={handleFileUpload}
                  className="hidden"
                  id="pdf-upload"
                />
                <label
                  htmlFor="pdf-upload"
                  className="inline-block bg-purple-600 hover:bg-purple-700 text-white px-6 py-3 rounded-lg cursor-pointer transition-colors"
                >
                  Select PDF File
                </label>
              </div>
            </div>
          </div>
        )}

        {/* Main Content */}
        {pdfFile && (
          <div className="bg-white/10 backdrop-blur-lg rounded-2xl border border-white/20 overflow-hidden">
            {/* Control Bar */}
            <div className="bg-white/5 p-4 border-b border-white/10 flex items-center justify-between flex-wrap gap-4">
              <div className="flex items-center gap-4">
                <span className="text-white font-medium">
                  PDF:{" "}
                  {typeof pdfFile === "string"
                    ? pdfFile.split("/").pop()
                    : pdfFile.name}
                </span>
                <button
                  onClick={clearPDF}
                  className="text-red-400 hover:text-red-300 transition-colors"
                >
                  <X className="w-5 h-5" />
                </button>
              </div>

              <div className="flex items-center gap-4">
                <LanguageSelector
                  languages={languages}
                  selectedLanguage={selectedLanguage}
                  onLanguageChange={setSelectedLanguage}
                />
                <button
                  onClick={handleTranslate}
                  disabled={isTranslating}
                  className="flex items-center gap-2 bg-purple-600 hover:bg-purple-700 disabled:bg-purple-800 text-white px-4 py-2 rounded-lg transition-colors"
                >
                  <Languages className="w-4 h-4" />
                  {isTranslating ? "Translating..." : "Translate"}
                </button>
              </div>
            </div>

            {/* Content Area - Side by Side */}
            <div className="p-6 min-h-[600px]">
              <div className="grid grid-cols-2 gap-6">
                {/* Original PDF */}
                <div className="bg-white rounded-xl p-4">
                  <h3 className="text-lg font-bold text-gray-800 mb-4 flex items-center gap-2">
                    <FileText className="w-5 h-5" />
                    Original PDF
                  </h3>
                  <PDFViewer file={pdfFile} />
                </div>

                {/* Translated Content */}
                <div className="bg-white rounded-xl p-4">
                  <h3 className="text-lg font-bold text-gray-800 mb-4 flex items-center gap-2">
                    <Languages className="w-5 h-5" />
                    Translated (
                    {languages.find((l) => l.code === selectedLanguage)?.name ||
                      selectedLanguage}
                    )
                  </h3>
                  <div className="min-h-[600px] max-h-[700px] overflow-y-auto">
                    {translatedText ? (
                      <div className="whitespace-pre-wrap text-gray-800 leading-relaxed">
                        {translatedText}
                      </div>
                    ) : (
                      <p className="text-gray-500 text-center py-20">
                        Select a language and click Translate to see the
                        translated content
                      </p>
                    )}
                  </div>
                </div>
              </div>
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;
