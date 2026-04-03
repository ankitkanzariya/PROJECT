import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_URL || 'http://127.0.0.1:8000';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

export interface RoomCreateData {
  name: string;
  password: string;
  creator_id: string;
  creator_username: string;
}

export interface RoomJoinData {
  room_id: string;
  password: string;
  user_id: string;
  username: string;
  message?: string;
}

export interface Room {
  id: string;
  name: string;
  creator_id: string;
  is_active: boolean;
  created_at: string;
}

export interface JoinRequest {
  id: string;
  room_id: string;
  user_id: string;
  username: string;
  status: string;
  message?: string;
  created_at: string;
}

export const createRoom = async (data: RoomCreateData): Promise<Room> => {
  const response = await api.post('/rooms/', data);
  return response.data;
};

export const getRoom = async (roomId: string): Promise<Room> => {
  const response = await api.get(`/rooms/${roomId}`);
  return response.data;
};

export const searchRoom = async (roomId: string): Promise<Partial<Room>> => {
  const response = await api.get(`/rooms/${roomId}/search`);
  return response.data;
};

export const requestJoinRoom = async (data: RoomJoinData): Promise<{ message: string; request_id: string }> => {
  const response = await api.post('/rooms/join', data);
  return response.data;
};

export const getRoomJoinRequests = async (roomId: string): Promise<JoinRequest[]> => {
  const response = await api.get(`/join-requests/room/${roomId}`);
  return response.data;
};

export const approveJoinRequest = async (requestId: string): Promise<{ message: string }> => {
  const response = await api.put(`/join-requests/${requestId}/approve`);
  return response.data;
};

export const rejectJoinRequest = async (requestId: string): Promise<{ message: string }> => {
  const response = await api.put(`/join-requests/${requestId}/reject`);
  return response.data;
};

export const createUser = async (userData: { id: string; username: string; email?: string }) => {
  const response = await api.post('/users/', userData);
  return response.data;
};

export const setUserOnline = async (userId: string) => {
  const response = await api.put(`/users/${userId}/online`);
  return response.data;
};

export const setUserOffline = async (userId: string) => {
  const response = await api.put(`/users/${userId}/offline`);
  return response.data;
};
