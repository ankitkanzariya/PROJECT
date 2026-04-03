import React, {
  createContext,
  useContext,
  useEffect,
  useState,
  useRef,
} from "react";
import { io, Socket } from "socket.io-client";

interface SocketContextType {
  socket: Socket | null;
  isConnected: boolean;
  userId: string | null;
  username: string | null;
  registerUser: (userId: string, username: string) => void;
  joinRoom: (roomId: string, userId: string, username: string) => void;
  leaveRoom: (roomId: string, userId: string) => void;
  sendSignal: (targetUserId: string, data: any, type: string) => void;
}

const SocketContext = createContext<SocketContextType | undefined>(undefined);

export const useSocket = () => {
  const context = useContext(SocketContext);
  if (!context) {
    throw new Error("useSocket must be used within a SocketProvider");
  }
  return context;
};

interface SocketProviderProps {
  children: React.ReactNode;
}

export const SocketProvider: React.FC<SocketProviderProps> = ({ children }) => {
  const [socket, setSocket] = useState<Socket | null>(null);
  const [isConnected, setIsConnected] = useState(false);
  const [userId, setUserId] = useState<string | null>(null);
  const [username, setUsername] = useState<string | null>(null);
  const reconnectAttempts = useRef(0);
  const maxReconnectAttempts = 5;

  const createSocket = () => {
    const socketUrl =
      process.env.REACT_APP_SOCKET_URL || "http://localhost:8002";
    console.log("Creating socket connection to:", socketUrl);

    const newSocket = io(socketUrl, {
      transports: ["polling"], // Start with polling only
      timeout: 20000,
      reconnection: true,
      reconnectionAttempts: 10,
      reconnectionDelay: 2000,
      reconnectionDelayMax: 5000,
    });

    newSocket.on("connect", () => {
      console.log("✅ Connected to server");
      setIsConnected(true);
      reconnectAttempts.current = 0;

      // Re-register user if we had one
      if (userId && username) {
        console.log("Re-registering user:", username);
        newSocket.emit("register_user", {
          user_id: userId,
          username: username,
        });
      }
    });

    newSocket.on("disconnect", (reason) => {
      console.log("❌ Disconnected from server:", reason);
      setIsConnected(false);
    });

    newSocket.on("connect_error", (error) => {
      console.error("🔴 Connection error:", error.message);
      reconnectAttempts.current++;

      if (reconnectAttempts.current >= maxReconnectAttempts) {
        console.error("🔴 Max reconnection attempts reached");
        newSocket.disconnect();
      }
    });

    return newSocket;
  };

  useEffect(() => {
    const newSocket = createSocket();
    setSocket(newSocket);

    return () => {
      newSocket.disconnect();
    };
  }, []);

  const registerUser = (newUserId: string, newUsername: string) => {
    if (socket) {
      socket.emit("register_user", {
        user_id: newUserId,
        username: newUsername,
      });
      setUserId(newUserId);
      setUsername(newUsername);
    }
  };

  const joinRoom = (roomId: string, newUserId: string, newUsername: string) => {
    if (socket) {
      console.log(`Joining room ${roomId} as ${newUsername} (${newUserId})`);
      socket.emit("join_room", {
        room_id: roomId,
        user_id: newUserId,
        username: newUsername,
      });
    } else {
      console.error("Socket not available for joinRoom");
    }
  };

  const leaveRoom = (roomId: string, newUserId: string) => {
    if (socket) {
      socket.emit("leave_room", { room_id: roomId, user_id: newUserId });
    }
  };

  const sendSignal = (targetUserId: string, data: any, type: string) => {
    if (socket && userId) {
      socket.emit("send_signal", {
        target_user_id: targetUserId,
        sender_id: userId,
        signalData: data,
        type: type,
      });
    }
  };

  return (
    <SocketContext.Provider
      value={{
        socket,
        isConnected,
        userId,
        username,
        registerUser,
        joinRoom,
        leaveRoom,
        sendSignal,
      }}
    >
      {children}
    </SocketContext.Provider>
  );
};
