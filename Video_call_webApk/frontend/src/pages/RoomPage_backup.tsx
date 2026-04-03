import React, { useState, useEffect, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ArrowLeftIcon, MicrophoneIcon, VideoCameraIcon, PhoneXMarkIcon } from "@heroicons/react/24/outline";
import { useSocket } from "../contexts/SocketContext";
import VideoTile from "../components/VideoTile";
import { getRoom, getRoomJoinRequests } from "../services/api";

interface Participant {
  id: string;
  username: string;
  stream?: MediaStream | null;
}

const RoomPage: React.FC = () => {
  const { roomId } = useParams<{ roomId: string }>();
  const navigate = useNavigate();
  const { socket, userId, username, joinRoom, leaveRoom, sendSignal } = useSocket();

  const [room, setRoom] = useState<any>(null);
  const [participants, setParticipants] = useState<Participant[]>([]);
  const [joinRequests, setJoinRequests] = useState<any[]>([]);
  const [isMuted, setIsMuted] = useState(false);
  const [isCameraOff, setIsCameraOff] = useState(false);
  const [localStream, setLocalStream] = useState<MediaStream | null>(null);
  const [peerConnections, setPeerConnections] = useState<Map<string, RTCPeerConnection>>(new Map());
  const [isCreator, setIsCreator] = useState(false);
  const [showSidebar, setShowSidebar] = useState(false);

  const localVideoRef = useRef<HTMLVideoElement>(null);

  useEffect(() => {
    if (!roomId || !userId || !username) return;

    loadRoomInfo();
    setupLocalMedia();
    joinRoom(roomId, userId, username);

    return () => {
      cleanup();
    };
  }, [roomId, userId, username]);

  useEffect(() => {
    if (!socket) return;

    socket.on("user_joined", handleUserJoined);
    socket.on("user_left", handleUserLeft);
    socket.on("receive_signal", handleReceiveSignal);
    socket.on("new_join_request", handleNewJoinRequest);
    socket.on("join_request_response", handleJoinRequestResponse);

    return () => {
      socket.off("user_joined");
      socket.off("user_left");
      socket.off("receive_signal");
      socket.off("new_join_request");
      socket.off("join_request_response");
    };
  }, [socket]);

  const loadRoomInfo = async () => {
    try {
      const roomData = await getRoom(roomId!);
      setRoom(roomData);
      if (roomData.creator_id === userId) {
        setIsCreator(true);
        loadJoinRequests();
      }
    } catch (error) {
      console.error("Failed to load room info:", error);
    }
  };

  const loadJoinRequests = async () => {
    try {
      const requests = await getRoomJoinRequests(roomId!);
      setJoinRequests(requests);
    } catch (error) {
      console.error("Failed to load join requests:", error);
    }
  };

  const setupLocalMedia = async () => {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({
        video: true,
        audio: true,
      });
      setLocalStream(stream);
      if (localVideoRef.current) {
        localVideoRef.current.srcObject = stream;
      }
    } catch (error) {
      console.error("Failed to access media devices:", error);
    }
  };

  const handleUserJoined = (data: any) => {
    if (data.user_id === userId) return;
    
    const newParticipant: Participant = {
      id: data.user_id,
      username: data.username,
    };
    setParticipants((prev) => [...prev, newParticipant]);
    createPeerConnection(data.user_id);
  };

  const handleUserLeft = (data: any) => {
    setParticipants((prev) => prev.filter((p) => p.id !== data.user_id));
    const pc = peerConnections.get(data.user_id);
    if (pc) {
      pc.close();
      peerConnections.delete(data.user_id);
    }
  };

  const handleNewJoinRequest = (request: any) => {
    setJoinRequests((prev) => [...prev, request]);
    setShowSidebar(true);
  };

  const handleJoinRequestResponse = (data: any) => {
    if (data.response === "approved") {
      alert("Join request approved! Joining room...");
      joinRoom(roomId!, userId!, username!);
    } else if (data.response === "rejected") {
      alert("Join request rejected by host.");
      navigate("/");
    }
  };

  const createPeerConnection = async (participantId: string) => {
    const configuration = {
      iceServers: [{ urls: "stun:stun.l.google.com:19302" }],
    };
    const pc = new RTCPeerConnection(configuration);

    if (localStream) {
      localStream.getTracks().forEach((track) => {
        pc.addTrack(track, localStream!);
      });
    }

    pc.onicecandidate = (event) => {
      if (event.candidate) {
        sendSignal(participantId, event.candidate, "ice-candidate");
      }
    };

    pc.ontrack = (event) => {
      const [remoteStream] = event.streams;
      setParticipants((prev) =>
        prev.map((p) =>
          p.id === participantId ? { ...p, stream: remoteStream } : p,
        ),
      );
    };

    if (isCreator) {
      const offer = await pc.createOffer();
      await pc.setLocalDescription(offer);
      sendSignal(participantId, offer, "offer");
    }

    peerConnections.set(participantId, pc);
  };

  const handleReceiveSignal = async (data: any) => {
    const { sender_id, signalData, type } = data;
    let pc = peerConnections.get(sender_id);

    if (!pc) {
      await createPeerConnection(sender_id);
      pc = peerConnections.get(sender_id);
    }

    if (!pc) return;

    if (type === "offer") {
      await pc.setRemoteDescription(new RTCSessionDescription(signalData));
      const answer = await pc.createAnswer();
      await pc.setLocalDescription(answer);
      sendSignal(sender_id, answer, "answer");
    } else if (type === "answer") {
      await pc.setRemoteDescription(new RTCSessionDescription(signalData));
    } else if (type === "ice-candidate") {
      await pc.addIceCandidate(new RTCIceCandidate(signalData));
    }
  };

  const handleApproveRequest = async (requestId: string, requestUserId: string) => {
    try {
      await fetch(`http://127.0.0.1:8000/join-requests/${requestId}/approve`, {
        method: "PUT",
      });
      setJoinRequests((prev) => prev.filter((req) => req.id !== requestId));
      if (socket) {
        socket.emit("join_request_response", {
          user_id: requestUserId,
          response: "approved",
        });
      }
    } catch (error) {
      console.error("Failed to approve request:", error);
    }
  };

  const handleRejectRequest = async (requestId: string, requestUserId: string) => {
    try {
      await fetch(`http://127.0.0.1:8000/join-requests/${requestId}/reject`, {
        method: "PUT",
      });
      setJoinRequests((prev) => prev.filter((req) => req.id !== requestId));
      if (socket) {
        socket.emit("join_request_response", {
          user_id: requestUserId,
          response: "rejected",
        });
      }
    } catch (error) {
      console.error("Failed to reject request:", error);
    }
  };

  const cleanup = () => {
    if (localStream) {
      localStream.getTracks().forEach((track) => track.stop());
    }
    peerConnections.forEach((pc) => pc.close());
  };

  const leaveRoomHandler = () => {
    leaveRoom(roomId!, userId!);
    cleanup();
    navigate("/");
  };

  if (!room) {
    return <div>Loading...</div>;
  }

  return (
    <div className="flex h-screen bg-gray-900 text-white">
      {/* Main Content */}
      <div className="flex-1 flex flex-col">
        {/* Header */}
        <div className="flex items-center justify-between p-4 bg-gray-800">
          <div className="flex items-center space-x-4">
            <button
              onClick={leaveRoomHandler}
              className="p-2 hover:bg-gray-700 rounded-lg"
            >
              <ArrowLeftIcon className="w-5 h-5" />
            </button>
            <div>
              <h1 className="text-xl font-semibold">{room.name}</h1>
              <p className="text-sm text-gray-400">Room ID: {roomId}</p>
            </div>
          </div>
          <div className="flex items-center space-x-2">
            {isCreator && joinRequests.length > 0 && (
              <button
                onClick={() => setShowSidebar(!showSidebar)}
                className="relative p-2 bg-orange-600 hover:bg-orange-700 rounded-full"
              >
                <svg className="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
                  <path d="M10 2a6 6 0 00-6 6v3.586l-.707.707A1 1 0 004 14h12a1 1 0 00.707-1.707L16 11.586V8a6 6 0 00-6-6zM10 18a3 3 0 01-3-3h6a3 3 0 01-3 3z" />
                </svg>
                <span className="absolute -top-1 -right-1 bg-red-600 text-white text-xs rounded-full w-5 h-5 flex items-center justify-center">
                  {joinRequests.length}
                </span>
              </button>
            )}
          </div>
        </div>

        {/* Video Grid */}
        <div className="flex-1 p-4">
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 h-full">
            <VideoTile
              username={`${username} (You)`}
              stream={localStream}
              isMuted={isMuted}
              isCameraOff={isCameraOff}
              isLocal={true}
            />
            {participants.map((participant) => (
              <VideoTile
                key={participant.id}
                username={participant.username}
                stream={participant.stream}
                isMuted={false}
                isCameraOff={false}
                isLocal={false}
              />
            ))}
          </div>
        </div>

        {/* Controls */}
        <div className="flex items-center justify-center space-x-4 p-4 bg-gray-800">
          <button
            onClick={() => setIsMuted(!isMuted)}
            className={`p-3 rounded-full ${isMuted ? 'bg-red-600' : 'bg-gray-700'} hover:bg-gray-600`}
          >
            <MicrophoneIcon className="w-6 h-6" />
          </button>
          <button
            onClick={() => setIsCameraOff(!isCameraOff)}
            className={`p-3 rounded-full ${isCameraOff ? 'bg-red-600' : 'bg-gray-700'} hover:bg-gray-600`}
          >
            <VideoCameraIcon className="w-6 h-6" />
          </button>
          <button
            onClick={leaveRoomHandler}
            className="p-3 bg-red-600 rounded-full hover:bg-red-700"
          >
            <PhoneXMarkIcon className="w-6 h-6" />
          </button>
        </div>
      </div>

      {/* Join Requests Sidebar */}
      {isCreator && showSidebar && (
        <div className="w-80 bg-gray-800 p-4 overflow-y-auto">
          <div className="flex items-center justify-between mb-4">
            <h2 className="text-lg font-semibold">Join Requests</h2>
            <button
              onClick={() => setShowSidebar(false)}
              className="text-gray-400 hover:text-white"
            >
              ×
            </button>
          </div>
          {joinRequests.map((request) => (
            <div key={request.id} className="bg-gray-700 rounded-lg p-3 mb-3">
              <div className="flex items-center justify-between mb-2">
                <span className="font-medium">{request.username}</span>
                <span className="text-xs text-gray-400">
                  {new Date(request.created_at).toLocaleTimeString()}
                </span>
              </div>
              <p className="text-sm text-gray-300 mb-3">{request.message}</p>
              <div className="flex space-x-2">
                <button
                  onClick={() => handleApproveRequest(request.id, request.user_id)}
                  className="flex-1 bg-green-600 hover:bg-green-700 px-3 py-1 rounded text-sm"
                >
                  Approve
                </button>
                <button
                  onClick={() => handleRejectRequest(request.id, request.user_id)}
                  className="flex-1 bg-red-600 hover:bg-red-700 px-3 py-1 rounded text-sm"
                >
                  Reject
                </button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default RoomPage;
