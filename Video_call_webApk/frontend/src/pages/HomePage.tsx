import React from 'react';
import { useNavigate } from 'react-router-dom';
import { VideoCameraIcon, UserGroupIcon } from '@heroicons/react/24/outline';

const HomePage: React.FC = () => {
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-900 to-purple-900">
      <div className="text-center max-w-2xl mx-auto p-8">
        <h1 className="text-6xl font-bold mb-4 bg-clip-text text-transparent bg-gradient-to-r from-blue-400 to-purple-400">
          Video Call App
        </h1>
        <p className="text-xl mb-12 text-gray-300">
          Create secure video rooms with password protection and approval system
        </p>
        
        <div className="grid md:grid-cols-2 gap-8">
          <button
            onClick={() => navigate('/create-room')}
            className="group bg-blue-600 hover:bg-blue-700 text-white p-8 rounded-2xl transition-all transform hover:scale-105 shadow-xl"
          >
            <VideoCameraIcon className="w-16 h-16 mx-auto mb-4 group-hover:animate-pulse" />
            <h2 className="text-2xl font-semibold mb-2">Create Room</h2>
            <p className="text-blue-200">
              Start a new video call room with password protection
            </p>
          </button>
          
          <button
            onClick={() => navigate('/join-room')}
            className="group bg-purple-600 hover:bg-purple-700 text-white p-8 rounded-2xl transition-all transform hover:scale-105 shadow-xl"
          >
            <UserGroupIcon className="w-16 h-16 mx-auto mb-4 group-hover:animate-pulse" />
            <h2 className="text-2xl font-semibold mb-2">Join Room</h2>
            <p className="text-purple-200">
              Enter a room ID to join an existing video call
            </p>
          </button>
        </div>
        
        <div className="mt-12 text-gray-400">
          <p className="text-sm">
            Features: WebRTC video calling • Password protection • Approval system • Real-time signaling
          </p>
        </div>
      </div>
    </div>
  );
};

export default HomePage;
