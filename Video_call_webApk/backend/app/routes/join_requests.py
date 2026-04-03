from fastapi import APIRouter, Depends, HTTPException, status
from sqlalchemy.orm import Session
from typing import List

from ..database import get_db
from ..models.join_request import JoinRequest
from ..models.room import Room
from ..schemas.join_request import JoinRequestResponse

router = APIRouter(prefix="/join-requests", tags=["join_requests"])

@router.get("/room/{room_id}", response_model=List[JoinRequestResponse])
async def get_room_join_requests(room_id: str, db: Session = Depends(get_db)):
    # Verify room exists
    room = db.query(Room).filter(Room.id == room_id).first()
    if not room:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Room not found"
        )
    
    # Get pending join requests
    requests = db.query(JoinRequest).filter(
        JoinRequest.room_id == room_id,
        JoinRequest.status == "pending"
    ).all()
    
    return requests

@router.put("/{request_id}/approve")
async def approve_join_request(request_id: str, db: Session = Depends(get_db)):
    join_request = db.query(JoinRequest).filter(JoinRequest.id == request_id).first()
    if not join_request:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Join request not found"
        )
    
    join_request.status = "approved"
    db.commit()
    
    # Send approval notification via socket
    try:
        import requests
        socket_response = requests.post(
            "http://localhost:8002/send-approval",
            json={
                "user_id": join_request.user_id,
                "response": "approved"
            }
        )
        print(f"Approval notification sent: {socket_response.status_code}")
    except Exception as e:
        print(f"Approval notification failed: {e}")
    
    return {"message": "Join request approved"}

@router.put("/{request_id}/reject")
async def reject_join_request(request_id: str, db: Session = Depends(get_db)):
    join_request = db.query(JoinRequest).filter(JoinRequest.id == request_id).first()
    if not join_request:
        raise HTTPException(
            status_code=status.HTTP_404_NOT_FOUND,
            detail="Join request not found"
        )
    
    join_request.status = "rejected"
    db.commit()
    
    return {"message": "Join request rejected"}

@router.get("/user/{user_id}", response_model=List[JoinRequestResponse])
async def get_user_join_requests(user_id: str, db: Session = Depends(get_db)):
    requests = db.query(JoinRequest).filter(
        JoinRequest.user_id == user_id
    ).all()
    
    return requests
