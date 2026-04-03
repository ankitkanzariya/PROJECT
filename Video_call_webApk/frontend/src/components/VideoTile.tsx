import React, { useRef, useEffect } from "react";
import { VideoCameraIcon, MicrophoneIcon } from "@heroicons/react/24/outline";

interface VideoTileProps {
  username: string;
  stream?: MediaStream | null;
  isMuted?: boolean;
  isCameraOff?: boolean;
  isLocal?: boolean;
}

const VideoTile: React.FC<VideoTileProps> = ({
  username,
  stream,
  isMuted = false,
  isCameraOff = false,
  isLocal = false,
}) => {
  const videoRef = useRef<HTMLVideoElement>(null);

  useEffect(() => {
    if (videoRef.current && stream) {
      videoRef.current.srcObject = stream;
    }
  }, [stream]);

  return (
    <div className="video-tile relative aspect-video bg-gray-800 rounded-lg overflow-hidden">
      {/* Video Element */}
      {!isCameraOff ? (
        <video
          ref={videoRef}
          autoPlay
          playsInline
          muted={isLocal}
          className="w-full h-full object-cover"
        />
      ) : (
        <div className="w-full h-full flex items-center justify-center bg-gray-900">
          <VideoCameraIcon className="w-16 h-16 text-gray-600" />
        </div>
      )}

      {/* Username Badge */}
      <div className="absolute bottom-2 left-2 bg-black bg-opacity-50 px-2 py-1 rounded text-sm text-white">
        {username}
      </div>

      {/* Status Indicators */}
      <div className="absolute top-2 right-2 flex space-x-1">
        {isMuted && (
          <div className="bg-red-600 p-1 rounded-full">
            <MicrophoneIcon className="w-4 h-4 text-white" />
          </div>
        )}
        {isCameraOff && (
          <div className="bg-gray-600 p-1 rounded-full">
            <VideoCameraIcon className="w-4 h-4 text-white" />
          </div>
        )}
      </div>
    </div>
  );
};

export default VideoTile;
