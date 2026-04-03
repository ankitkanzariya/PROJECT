import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { ArrowLeftIcon, MagnifyingGlassIcon } from '@heroicons/react/24/outline';
import { v4 as uuidv4 } from 'uuid';
import { useSocket } from '../contexts/SocketContext';
import { searchRoom, requestJoinRoom } from '../services/api';

const JoinRoomPage: React.FC = () => {
  const navigate = useNavigate();
  const { registerUser } = useSocket();
  const [formData, setFormData] = useState({
    roomId: '',
    password: '',
    username: '',
    message: ''
  });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [roomFound, setRoomFound] = useState<any>(null);
  const [requestSent, setRequestSent] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSearchRoom = async () => {
    setError('');
    setRoomFound(null);
    
    if (!formData.roomId.trim()) {
      setError('Please enter a Room ID');
      return;
    }

    try {
      const room = await searchRoom(formData.roomId);
      setRoomFound(room);
    } catch (err) {
      setError('Room not found');
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    if (!roomFound) {
      setError('Please search for a room first');
      setLoading(false);
      return;
    }

    try {
      const userId = uuidv4();
      
      // Register user
      registerUser(userId, formData.username);

      // Request to join room
      const joinData = {
        room_id: formData.roomId,
        password: formData.password,
        user_id: userId,
        username: formData.username,
        message: formData.message
      };

      await requestJoinRoom(joinData);
      setRequestSent(true);
      
      // Store user info in localStorage
      localStorage.setItem('userId', userId);
      localStorage.setItem('username', formData.username);
      
    } catch (err: any) {
      if (err.response?.status === 401) {
        setError('Incorrect password');
      } else {
        setError('Failed to join room. Please try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  if (requestSent) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-purple-900 to-blue-900 p-8">
        <div className="bg-gray-800 p-8 rounded-2xl shadow-2xl max-w-md w-full text-center">
          <div className="animate-pulse">
            <div className="w-16 h-16 bg-purple-600 rounded-full mx-auto mb-4 flex items-center justify-center">
              <MagnifyingGlassIcon className="w-8 h-8 text-white" />
            </div>
          </div>
          <h2 className="text-3xl font-bold mb-4">Request Sent!</h2>
          <p className="text-gray-300 mb-6">
            Your join request has been sent to the room creator. 
            Please wait for approval.
          </p>
          <div className="bg-gray-700 p-4 rounded-lg mb-6">
            <p className="text-sm text-gray-400 mb-1">Room ID</p>
            <p className="text-xl font-mono font-bold text-purple-400">{formData.roomId}</p>
          </div>
          <button
            onClick={() => navigate('/')}
            className="w-full bg-gray-600 hover:bg-gray-700 text-white py-3 px-4 rounded-lg transition-colors"
          >
            Back to Home
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-purple-900 to-blue-900 p-8">
      <div className="bg-gray-800 p-8 rounded-2xl shadow-2xl max-w-md w-full">
        <button
          onClick={() => navigate('/')}
          className="flex items-center text-gray-400 hover:text-white mb-6 transition-colors"
        >
          <ArrowLeftIcon className="w-5 h-5 mr-2" />
          Back to Home
        </button>

        <h2 className="text-3xl font-bold mb-6">Join Room</h2>

        {error && (
          <div className="bg-red-600 text-white p-3 rounded-lg mb-4">
            {error}
          </div>
        )}

        <div className="space-y-4">
          <div className="flex gap-2">
            <input
              type="text"
              name="roomId"
              value={formData.roomId}
              onChange={handleChange}
              placeholder="Enter Room ID"
              className="flex-1 px-4 py-2 bg-gray-700 border border-gray-600 rounded-lg focus:outline-none focus:border-purple-500 text-white"
            />
            <button
              onClick={handleSearchRoom}
              className="bg-purple-600 hover:bg-purple-700 text-white px-4 py-2 rounded-lg transition-colors"
            >
              <MagnifyingGlassIcon className="w-5 h-5" />
            </button>
          </div>

          {roomFound && (
            <div className="bg-gray-700 p-4 rounded-lg mb-4">
              <h3 className="font-semibold text-green-400 mb-2">Room Found!</h3>
              <p className="text-sm text-gray-300">Name: {roomFound.name}</p>
              <p className="text-sm text-gray-300">Created by: {roomFound.creator_id}</p>
              <p className="text-sm text-gray-300">Status: {roomFound.is_active ? 'Active' : 'Inactive'}</p>
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-2">Your Name</label>
              <input
                type="text"
                name="username"
                value={formData.username}
                onChange={handleChange}
                required
                className="w-full px-4 py-2 bg-gray-700 border border-gray-600 rounded-lg focus:outline-none focus:border-purple-500 text-white"
                placeholder="Enter your name"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">Password</label>
              <input
                type="password"
                name="password"
                value={formData.password}
                onChange={handleChange}
                required
                disabled={!roomFound}
                className="w-full px-4 py-2 bg-gray-700 border border-gray-600 rounded-lg focus:outline-none focus:border-purple-500 text-white disabled:opacity-50"
                placeholder="Enter room password"
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">Message (Optional)</label>
              <textarea
                name="message"
                value={formData.message}
                onChange={handleChange}
                disabled={!roomFound}
                rows={3}
                className="w-full px-4 py-2 bg-gray-700 border border-gray-600 rounded-lg focus:outline-none focus:border-purple-500 text-white disabled:opacity-50 resize-none"
                placeholder="Send a message to the room creator..."
              />
            </div>

            <button
              type="submit"
              disabled={loading || !roomFound}
              className="w-full bg-purple-600 hover:bg-purple-700 disabled:bg-gray-600 text-white py-3 px-4 rounded-lg transition-colors"
            >
              {loading ? 'Sending Request...' : 'Request to Join'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
};

export default JoinRoomPage;
