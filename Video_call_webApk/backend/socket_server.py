import socketio
import uvicorn
import sys
import os
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

# Add app directory to path
sys.path.append(os.path.join(os.path.dirname(__file__), 'app'))

# Create Socket.IO server
sio = socketio.AsyncServer(
    async_mode="asgi",
    cors_allowed_origins="*",
    logger=True,
    engineio_logger=True
)

# Store user connections
user_connections = {}
room_users = {}

# Register handlers
from app.socketio.handlers import register_handlers
register_handlers(sio)

# Create ASGI app
app = socketio.ASGIApp(sio)

# Create FastAPI app for HTTP endpoints
fastapi_app = FastAPI()

# Add CORS middleware
fastapi_app.add_middleware(
    CORSMiddleware,
    allow_origins="*",
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

@sio.event
async def connect(sid, environ):
    print(f"🔌 Client connected: {sid}")
    print(f"🔌 Connection environ: {environ}")

@sio.event
async def disconnect(sid):
    print(f"🔌 Client disconnected: {sid}")
    # Remove from user connections
    if sid in user_connections:
        user_id = user_connections[sid]
        del user_connections[sid]
        print(f"👋 User {user_id} disconnected")
        print(f"📊 Remaining connections: {len(user_connections)}")
        
        # Remove from room users
        for room_id, users in room_users.items():
            if user_id in users:
                users.remove(user_id)
                # Notify room that user left
                await sio.emit("user_left", {"user_id": user_id}, room=room_id)

@sio.event
async def register_user(sid, data):
    user_id = data.get("user_id")
    username = data.get("username")
    
    user_connections[sid] = user_id
    print(f"👤 User registered: {username} ({user_id}) -> {sid}")
    print(f"📊 Total active connections: {len(user_connections)}")

@sio.event
async def join_room(sid, data):
    room_id = data.get("room_id")
    user_id = data.get("user_id")
    username = data.get("username")
    
    # Join socket room
    await sio.enter_room(sid, room_id)
    
    # Track room users
    if room_id not in room_users:
        room_users[room_id] = []
    if user_id not in room_users[room_id]:
        room_users[room_id].append(user_id)
    
    # Notify room that user joined
    await sio.emit("user_joined", {
        "user_id": user_id,
        "username": username
    }, room=room_id)
    
    print(f"User {username} joined room {room_id}")

@sio.event
async def leave_room(sid, data):
    room_id = data.get("room_id")
    user_id = data.get("user_id")
    
    # Leave socket room
    await sio.leave_room(sid, room_id)
    
    # Remove from room users
    if room_id in room_users and user_id in room_users[room_id]:
        room_users[room_id].remove(user_id)
    
    # Notify room that user left
    await sio.emit("user_left", {"user_id": user_id}, room=room_id)
    
    print(f"User {user_id} left room {room_id}")

@sio.event
async def send_signal(sid, data):
    target_user_id = data.get("target_user_id")
    signal_data = data.get("signalData")
    signal_type = data.get("type")
    
    # Find target user's socket
    target_sid = None
    for sid_key, uid in user_connections.items():
        if uid == target_user_id:
            target_sid = sid_key
            break
    
    if target_sid:
        await sio.emit("receive_signal", {
            "sender_id": data.get("sender_id"),
            "signalData": signal_data,
            "type": signal_type
        }, room=target_sid)
        print(f"Signal sent from {sid} to {target_sid}")

@fastapi_app.post("/join-request-notification")
async def join_request_notification(data: dict):
    """Receive join request notification from API server"""
    try:
        room_id = data.get("room_id")
        request_data = data.get("request")
        
        # Find room creator's socket and send notification
        creator_sid = None
        for sid_key, uid in user_connections.items():
            # Check if this user is the room creator
            # You might need to query the database here to verify
            await sio.emit("new_join_request", request_data, room=room_id)
        
        return {"status": "notification sent"}
    except Exception as e:
        return {"status": "error", "message": str(e)}

@fastapi_app.post("/send-approval")
async def send_approval(data: dict):
    """Send approval notification to specific user"""
    try:
        user_id = data.get("user_id")
        response = data.get("response")  # "approved" or "rejected"
        
        print(f"🔔 Sending approval notification to user {user_id}: {response}")
        
        # Find user's socket
        target_sid = None
        for sid_key, uid in user_connections.items():
            if uid == user_id:
                target_sid = sid_key
                print(f"📍 Found user {user_id} at socket {sid_key}")
                break
        
        if target_sid:
            await sio.emit("join_request_response", {
                "user_id": user_id,
                "response": response
            }, room=target_sid)
            print(f"✅ Approval notification sent to {target_sid}")
            return {"status": "approval sent"}
        else:
            print(f"❌ User {user_id} not found in active connections")
            print(f"Active connections: {user_connections}")
            return {"status": "error", "message": "User not found"}
    except Exception as e:
        print(f"❌ Error sending approval: {e}")
        return {"status": "error", "message": str(e)}

# Mount FastAPI app for HTTP endpoints
socketio_app = socketio.ASGIApp(sio)
fastapi_app.mount("/", socketio_app)

if __name__ == "__main__":
    uvicorn.run(fastapi_app, host="0.0.0.0", port=8002)
