import React, { useState, useEffect, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  ArrowLeftIcon,
  MicrophoneIcon,
  VideoCameraIcon,
  PhoneXMarkIcon,
} from "@heroicons/react/24/outline";
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
  const { socket, userId, username, joinRoom, leaveRoom, sendSignal } =
    useSocket();

  const [room, setRoom] = useState<any>(null);
  const [participants, setParticipants] = useState<Participant[]>([]);
  const [joinRequests, setJoinRequests] = useState<any[]>([]);
  const [isMuted, setIsMuted] = useState(false);
  const [isCameraOff, setIsCameraOff] = useState(false);
  const [localStream, setLocalStream] = useState<MediaStream | null>(null);
  const [peerConnections, setPeerConnections] = useState<
    Map<string, RTCPeerConnection>
  >(new Map());
  const [isCreator, setIsCreator] = useState(false);

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
    if (room && userId && room.creator_id === userId) {
      setIsCreator(true);
      loadJoinRequests();
    }
  }, [room, userId]);

  useEffect(() => {
    if (!socket) return;

    socket.on("user_joined", handleUserJoined);
    socket.on("user_left", handleUserLeft);
    socket.on("receive_signal", handleReceiveSignal);
    socket.on("new_join_request", handleNewJoinRequest);

    return () => {
      socket.off("user_joined");
      socket.off("user_left");
      socket.off("receive_signal");
      socket.off("new_join_request");
    };
  }, [socket]);

  const loadRoomInfo = async () => {
    try {
      const roomData = await getRoom(roomId!);
      setRoom(roomData);
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

  const toggleMute = () => {
    if (localStream) {
      const audioTrack = localStream.getAudioTracks()[0];
      if (audioTrack) {
        audioTrack.enabled = !isMuted;
        setIsMuted(!isMuted);
      }
    }
  };

  const toggleCamera = () => {
    if (localStream) {
      const videoTrack = localStream.getVideoTracks()[0];
      if (videoTrack) {
        videoTrack.enabled = !isCameraOff;
        setIsCameraOff(!isCameraOff);
      }
    }
  };

  const leaveRoomHandler = () => {
    if (roomId && userId) {
      leaveRoom(roomId, userId);
    }
    cleanup();
    navigate("/");
  };

  const cleanup = () => {
    if (localStream) {
      localStream.getTracks().forEach((track) => track.stop());
    }

    peerConnections.forEach((pc) => pc.close());
    setPeerConnections(new Map());
  };

  const handleApproveRequest = async (
    requestId: string,
    requestUserId: string,
  ) => {
    try {
      const response = await fetch(
        `http://127.0.0.1:8000/join-requests/${requestId}/approve`,
        {
          method: "PUT",
          headers: {
            'Content-Type': 'application/json',
          },
        },
      );

      if (!response.ok) {
        throw new Error('Failed to approve request');
      }

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

  const handleRejectRequest = async (
    requestId: string,
    requestUserId: string,
  ) => {
    try {
      const response = await fetch(
        `http://127.0.0.1:8000/join-requests/${requestId}/reject`,
        {
          method: "PUT",
          headers: {
            'Content-Type': 'application/json',
          },
        },
      );

      if (!response.ok) {
        throw new Error('Failed to reject request');
      }

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

  return (
    <div className="min-h-screen bg-gray-900 text-white">
      <div className="flex flex-col h-screen">
        {/* Header */}
        <div className="bg-gray-800 p-4 flex items-center justify-between">
          <div className="flex items-center space-x-4">
            <button
              onClick={leaveRoomHandler}
              className="p-2 hover:bg-gray-700 rounded-lg transition-colors"
            >
              <ArrowLeftIcon className="w-5 h-5" />
            </button>
            <div>
              <h1 className="text-xl font-semibold">{room?.name || "Room"}</h1>
              <p className="text-sm text-gray-400">Room ID: {roomId}</p>
            </div>
          </div>

          <div className="flex items-center space-x-2">
            <span className="text-sm text-gray-400">
              {participants.length + 1} participants
            </span>
          </div>
        </div>

        {/* Main Content */}
        <div className="flex-1 flex">
          {/* Video Grid */}
          <div className="flex-1 p-4">
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4 h-full">
              {/* Local Video */}
              <VideoTile
                username={`${username} (You)`}
                stream={localStream}
                isMuted={isMuted}
                isCameraOff={isCameraOff}
                isLocal={true}
              />

              {/* Participant Videos */}
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

          {/* Sidebar for join requests (only for creator) */}
          {isCreator && (
            <div className="w-80 bg-gray-800 p-4 border-l border-gray-700">
              <h3 className="text-lg font-semibold mb-4">Join Requests</h3>
              <div className="space-y-3">
                {joinRequests.length > 0 ? (
                  joinRequests.map((request) => (
                    <div key={request.id} className="bg-gray-700 p-3 rounded-lg">
                      <p className="font-medium">{request.username}</p>
                      {request.message && (
                        <p className="text-sm text-gray-400 mt-1">
                          {request.message}
                        </p>
                      )}
                      <div className="flex space-x-2 mt-3">
                        <button
                          onClick={() =>
                            handleApproveRequest(request.id, request.user_id)
                          }
                          className="flex-1 bg-green-600 hover:bg-green-700 text-white py-1 px-2 rounded text-sm transition-colors"
                        >
                          Approve
                        </button>
                        <button
                          onClick={() =>
                            handleRejectRequest(request.id, request.user_id)
                          }
                          className="flex-1 bg-red-600 hover:bg-red-700 text-white py-1 px-2 rounded text-sm transition-colors"
                        >
                          Reject
                        </button>
                      </div>
                    </div>
                  ))
                ) : (
                  <p className="text-gray-400">No pending join requests</p>
                )}
              </div>
            </div>
          )}
        </div>

        {/* Controls */}
        <div className="bg-gray-800 p-4 flex items-center justify-center space-x-4">
          <button
            onClick={toggleMute}
            className={`p-3 rounded-full transition-colors ${
              isMuted ? "bg-red-600" : "bg-gray-700 hover:bg-gray-600"
            }`}
          >
            <MicrophoneIcon className="w-6 h-6" />
          </button>

          <button
            onClick={toggleCamera}
            className={`p-3 rounded-full transition-colors ${
              isCameraOff ? "bg-red-600" : "bg-gray-700 hover:bg-gray-600"
            }`}
          >
            <VideoCameraIcon className="w-6 h-6" />
          </button>

          <button
            onClick={leaveRoomHandler}
            className="p-3 rounded-full bg-red-600 hover:bg-red-700 transition-colors"
          >
            <PhoneXMarkIcon className="w-6 h-6" />
          </button>
        </div>
      </div>

      {/* Hidden local video element */}
      <video ref={localVideoRef} autoPlay muted className="hidden" />
    </div>
  );
};

export default RoomPage;
