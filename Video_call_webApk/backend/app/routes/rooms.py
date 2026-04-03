from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List
import uuid
from passlib.context import CryptContext

from ..database import get_db
from ..models.room import Room
from ..models.join_request import JoinRequest
from ..schemas.room import RoomCreate, RoomResponse, RoomJoin

router = APIRouter(prefix="/rooms", tags=["rooms"])
pwd_context = CryptContext(schemes=["bcrypt"], deprecated="auto")

@router.post("/", response_model=RoomResponse, status_code=status.HTTP_201_CREATED)
async def create_room(room: RoomCreate, db: Session = Depends(get_db)):
    # Ensure password is string and truncate to prevent bcrypt error (max 72 bytes)
    password = str(room.password)[:72]
    print(f"Creating room with name: {room.name}, creator_id: {room.creator_id}, password length: {len(password)}")
    
    # Hash the password
    try:
        hashed_password = pwd_context.hash(password)
        print("Password hashed successfully")
    except Exception as e:
        print(f"Error hashing password: {e}")
        raise HTTPException(status_code=500, detail="Password hashing failed")
    
    # Create room
    db_room = Room(
        name=room.name,
        password=hashed_password,
        creator_id=room.creator_id
    )
    
    try:
        db.add(db_room)
        db.commit()
        db.refresh(db_room)
        print(f"Room created successfully with ID: {db_room.id}")
    except Exception as e:
        print(f"Error creating room: {e}")
        db.rollback()
        raise HTTPException(status_code=500, detail="Room creation failed")
    
    return db_room

@router.get("/{room_id}", response_model=RoomResponse)
async def get_room(room_id: str, db: Session = Depends(get_db)):
    room = db.query(Room).filter(Room.id == room_id).first()
    if not room:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Room not found"
        )
    return room

@router.post("/join")
async def request_join_room(room_join: RoomJoin, db: Session = Depends(get_db)):
    # Check if room exists
    room = db.query(Room).filter(Room.id == room_join.room_id).first()
    if not room:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Room not found"
        )
    
    # Verify password
    if not pwd_context.verify(room_join.password, room.password):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Incorrect password"
        )
    
    # Check for existing pending request from this user
    existing_request = db.query(JoinRequest).filter(
        JoinRequest.room_id == room_join.room_id,
        JoinRequest.user_id == room_join.user_id,
        JoinRequest.status.in_(["pending", "approved"])
    ).first()
    
    if existing_request:
        if existing_request.status == "pending":
            raise HTTPException(
                status_code=status.HTTP_400_BAD_REQUEST,
                detail="Join request already pending"
            )
        elif existing_request.status == "approved":
            # User already approved, return success
            return {
                "message": "Already approved to join room",
                "request_id": existing_request.id,
                "status": "approved"
            }
    
    # Create join request
    join_request = JoinRequest(
        id=str(uuid.uuid4()),
        room_id=room_join.room_id,
        user_id=room_join.user_id,
        username=room_join.username,
        message=room_join.message
    )
    
    db.add(join_request)
    db.commit()
    db.refresh(join_request)
    
    # Emit socket notification to room creator
    try:
        import requests
        # Notify socket server about new join request
        socket_response = requests.post(
            "http://localhost:8002/join-request-notification",
            json={
                "room_id": room_join.room_id,
                "request": {
                    "id": join_request.id,
                    "user_id": join_request.user_id,
                    "username": join_request.username,
                    "message": join_request.message,
                    "created_at": join_request.created_at.isoformat()
                }
            }
        )
        print(f"Socket notification sent: {socket_response.status_code}")
    except Exception as e:
        print(f"Socket notification failed: {e}")
    
    return {"message": "Join request sent successfully", "request_id": join_request.id}

@router.post("/join-approved")
async def join_approved_room(room_join: RoomJoin, db: Session = Depends(get_db)):
    # Check if room exists
    room = db.query(Room).filter(Room.id == room_join.room_id).first()
    if not room:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Room not found"
        )
    
    # Check if user has approved join request
    join_request = db.query(JoinRequest).filter(
        JoinRequest.room_id == room_join.room_id,
        JoinRequest.user_id == room_join.user_id,
        JoinRequest.status == "approved"
    ).first()
    
    if not join_request:
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="No approved join request found"
        )
    
    # Verify password
    if not pwd_context.verify(room_join.password, room.password):
        raise HTTPException(
            status_code=status.HTTP_401_UNAUTHORIZED,
            detail="Incorrect password"
        )
    
    return {
        "message": "Successfully joined room",
        "room": {
            "id": room.id,
            "name": room.name,
            "creator_id": room.creator_id
        }
    }
@router.get("/{room_id}/search")
async def search_room(room_id: str, db: Session = Depends(get_db)):
    room = db.query(Room).filter(Room.id == room_id).first()
    if not room:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Room not found"
        )
    
    return {
        "id": room.id,
        "name": room.name,
        "creator_id": room.creator_id,
        "is_active": room.is_active
    }
