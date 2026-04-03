import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { ArrowLeftIcon, ClockIcon } from "@heroicons/react/24/outline";
import { useSocket } from "../contexts/SocketContext";

const WaitingRoomPage: React.FC = () => {
  const { roomId } = useParams<{ roomId: string }>();
  const navigate = useNavigate();
  const { socket, userId, username } = useSocket();

  const [requestStatus, setRequestStatus] = useState<
    "pending" | "approved" | "rejected"
  >("pending");
  const [countdown, setCountdown] = useState(5);

  useEffect(() => {
    if (!socket) return;

    socket.on("join_request_status", (data: any) => {
      setRequestStatus(data.status);

      if (data.status === "approved") {
        // Start countdown to redirect
        let count = 5;
        const timer = setInterval(() => {
          count--;
          setCountdown(count);

          if (count === 0) {
            clearInterval(timer);
            navigate(`/room/${roomId}`);
          }
        }, 1000);
      } else if (data.status === "rejected") {
        // Show rejection message with host info
        setTimeout(() => {
          navigate("/");
        }, 3000);
      }
    });

    return () => {
      socket.off("join_request_status");
    };
  }, [socket, roomId, navigate]);

  const handleGoBack = () => {
    navigate("/");
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-orange-900 to-red-900 p-8">
      <div className="bg-gray-800 p-8 rounded-2xl shadow-2xl max-w-md w-full text-center">
        <button
          onClick={handleGoBack}
          className="flex items-center text-gray-400 hover:text-white mb-6 transition-colors justify-center"
        >
          <ArrowLeftIcon className="w-5 h-5 mr-2" />
          Back to Home
        </button>

        <div className="mb-6">
          {requestStatus === "pending" && (
            <div className="animate-pulse">
              <div className="w-20 h-20 bg-orange-600 rounded-full mx-auto mb-4 flex items-center justify-center">
                <ClockIcon className="w-10 h-10 text-white animate-spin" />
              </div>
            </div>
          )}

          {requestStatus === "approved" && (
            <div className="w-20 h-20 bg-green-600 rounded-full mx-auto mb-4 flex items-center justify-center">
              <svg
                className="w-10 h-10 text-white"
                fill="currentColor"
                viewBox="0 0 20 20"
              >
                <path
                  fillRule="evenodd"
                  d="M16.707 5.293a1 1 0 010 1.414l-8 8a1 1 0 01-1.414 0l-4-4a1 1 0 011.414-1.414L8 12.586l7.293-7.293a1 1 0 011.414 0z"
                  clipRule="evenodd"
                />
              </svg>
            </div>
          )}

          {requestStatus === "rejected" && (
            <div className="w-20 h-20 bg-red-600 rounded-full mx-auto mb-4 flex items-center justify-center">
              <svg
                className="w-10 h-10 text-white"
                fill="currentColor"
                viewBox="0 0 20 20"
              >
                <path
                  fillRule="evenodd"
                  d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                  clipRule="evenodd"
                />
              </svg>
            </div>
          )}
        </div>

        <h2 className="text-3xl font-bold mb-4">
          {requestStatus === "pending" && "Waiting for Approval"}
          {requestStatus === "approved" && "Request Approved!"}
          {requestStatus === "rejected" && "Request Rejected"}
        </h2>

        <p className="text-gray-300 mb-6">
          {requestStatus === "pending" &&
            "Your join request has been sent to the room creator. Please wait for their approval."}
          {requestStatus === "approved" &&
            `You can now enter the room! Redirecting in ${countdown} seconds...`}
          {requestStatus === "rejected" &&
            "Your join request was rejected by the host. Redirecting to home..."}
        </p>

        <div className="bg-gray-700 p-4 rounded-lg mb-6">
          <p className="text-sm text-gray-400 mb-1">Room ID</p>
          <p className="text-xl font-mono font-bold text-orange-400">
            {roomId}
          </p>
        </div>

        {requestStatus === "rejected" && (
          <button
            onClick={handleGoBack}
            className="w-full bg-gray-600 hover:bg-gray-700 text-white py-3 px-4 rounded-lg transition-colors"
          >
            Back to Home
          </button>
        )}
      </div>
    </div>
  );
};

export default WaitingRoomPage;
