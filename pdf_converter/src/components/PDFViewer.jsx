import { useState } from "react";
import { AlertCircle, Loader2 } from "lucide-react";

function PDFViewer({ file }) {
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(true);

  const handleLoad = () => {
    setLoading(false);
    setError(null);
  };

  const handleError = () => {
    setLoading(false);
    setError("Failed to load PDF file");
  };

  if (error) {
    return (
      <div className="flex flex-col items-center justify-center min-h-[400px]">
        <AlertCircle className="w-16 h-16 text-red-500 mb-4" />
        <p className="text-red-400 text-lg">{error}</p>
        <p className="text-gray-400 text-sm mt-2">
          Please try uploading the PDF again
        </p>
      </div>
    );
  }

  return (
    <div className="flex flex-col items-center w-full">
      {loading && (
        <div className="flex flex-col items-center justify-center min-h-[400px]">
          <Loader2 className="w-16 h-16 text-purple-400 mb-4 animate-spin" />
          <p className="text-purple-200 text-lg">Loading PDF...</p>
        </div>
      )}

      <iframe
        src={file}
        className="w-full h-[700px] rounded-xl shadow-2xl border-0"
        onLoad={handleLoad}
        onError={handleError}
        title="PDF Viewer"
      />
    </div>
  );
}

export default PDFViewer;
