import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { SocketProvider } from './contexts/SocketContext';
import HomePage from './pages/HomePage';
import CreateRoomPage from './pages/CreateRoomPage';
import JoinRoomPage from './pages/JoinRoomPage';
import RoomPage from './pages/RoomPage';
import WaitingRoomPage from './pages/WaitingRoomPage';

function App() {
  return (
    <SocketProvider>
      <Router>
        <div className="min-h-screen bg-gray-900 text-white">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/create-room" element={<CreateRoomPage />} />
            <Route path="/join-room" element={<JoinRoomPage />} />
            <Route path="/room/:roomId" element={<RoomPage />} />
            <Route path="/waiting/:roomId" element={<WaitingRoomPage />} />
          </Routes>
        </div>
      </Router>
    </SocketProvider>
  );
}

export default App;
