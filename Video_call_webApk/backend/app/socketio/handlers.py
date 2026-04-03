from socketio import AsyncServer
from sqlalchemy.orm import Session
from typing import Dict, List
import json

from ..database import SessionLocal
from ..models.room import Room
from ..models.join_request import JoinRequest

# Store active connections and rooms
active_connections: Dict[str, str] = {}  # socket_id -> user_id
room_participants: Dict[str, List[str]] = {}  # room_id -> [socket_ids]

def register_handlers(sio: AsyncServer):
    
    @sio.event
    async def connect(sid, environ):
        print(f"Client connected: {sid}")
    
    @sio.event
    async def disconnect(sid):
        print(f"Client disconnected: {sid}")
        
        # Remove from active connections
        if sid in active_connections:
            user_id = active_connections[sid]
            del active_connections[sid]
            
            # Remove from room participants
            for room_id, participants in room_participants.items():
                if sid in participants:
                    participants.remove(sid)
                    await sio.emit("user_left", {
                        "user_id": user_id,
                        "participant_count": len(participants)
                    }, room=room_id)
                    
                    # Clean up empty rooms
                    if not participants:
                        del room_participants[room_id]
        
        # Set user offline in database
        db = SessionLocal()
        try:
            from ..models.user import User
            user = db.query(User).filter(User.id == user_id).first()
            if user:
                user.is_online = False
                db.commit()
        finally:
            db.close()
    
    @sio.event
    async def register_user(sid, data):
        user_id = data.get("user_id")
        username = data.get("username")
        
        # Store connection
        active_connections[sid] = user_id
        
        # Set user online in database
        db = SessionLocal()
        try:
            from ..models.user import User
            user = db.query(User).filter(User.id == user_id).first()
            if user:
                user.is_online = True
                db.commit()
            else:
                # Create user if doesn't exist
                new_user = User(id=user_id, username=username)
                db.add(new_user)
                db.commit()
        finally:
            db.close()
        
        await sio.emit("user_registered", {"user_id": user_id})
    
    @sio.event
    async def join_room(sid, data):
        room_id = data.get("room_id")
        user_id = data.get("user_id")
        username = data.get("username")
        
        # Add to room participants
        if room_id not in room_participants:
            room_participants[room_id] = []
        
        if sid not in room_participants[room_id]:
            room_participants[room_id].append(sid)
        
        # Join socket.io room
        await sio.enter_room(sid, room_id)
        
        # Notify others in room
        await sio.emit("user_joined", {
            "user_id": user_id,
            "username": username,
            "participant_count": len(room_participants[room_id])
        }, room=room_id)
        
        # Send current participants to new user
        await sio.emit("room_participants", {
            "participants": room_participants[room_id],
            "count": len(room_participants[room_id])
        }, room=sid)
    
    @sio.event
    async def leave_room(sid, data):
        room_id = data.get("room_id")
        user_id = data.get("user_id")
        
        # Remove from room participants
        if room_id in room_participants and sid in room_participants[room_id]:
            room_participants[room_id].remove(sid)
            
            # Leave socket.io room
            await sio.leave_room(sid, room_id)
            
            # Notify others
            await sio.emit("user_left", {
                "user_id": user_id,
                "participant_count": len(room_participants[room_id])
            }, room=room_id)
            
            # Clean up empty rooms
            if not room_participants[room_id]:
                del room_participants[room_id]
    
    @sio.event
    async def send_signal(sid, data):
        target_user_id = data.get("target_user_id")
        signal_data = data.get("data")
        signal_type = data.get("type")
        
        # Find target socket
        target_socket = None
        for socket_id, user_id in active_connections.items():
            if user_id == target_user_id:
                target_socket = socket_id
                break
        
        if target_socket:
            await sio.emit("receive_signal", {
                "sender_id": active_connections[sid],
                "data": signal_data,
                "type": signal_type
            }, room=target_socket)
    
    @sio.event
    async def join_request_notification(sid, data):
        room_id = data.get("room_id")
        request_data = data.get("request")
        
        # Notify room creator
        db = SessionLocal()
        try:
            room = db.query(Room).filter(Room.id == room_id).first()
            if room:
                creator_socket = None
                for socket_id, user_id in active_connections.items():
                    if user_id == room.creator_id:
                        creator_socket = socket_id
                        break
                
                if creator_socket:
                    await sio.emit("new_join_request", request_data, room=creator_socket)
        finally:
            db.close()
    
    @sio.event
    async def join_request_response(sid, data):
        user_id = data.get("user_id")
        response = data.get("response")  # approved or rejected
        
        # Find user socket
        user_socket = None
        for socket_id, uid in active_connections.items():
            if uid == user_id:
                user_socket = socket_id
                break
        
        if user_socket:
            await sio.emit("join_request_status", {
                "status": response
            }, room=user_socket)
