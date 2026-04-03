from pydantic import BaseModel
from datetime import datetime
from typing import Optional

class RoomCreate(BaseModel):
    name: str
    password: str
    creator_id: str
    creator_username: str

class RoomResponse(BaseModel):
    id: str
    name: str
    creator_id: str
    is_active: bool
    created_at: datetime
    
    class Config:
        from_attributes = True

class RoomJoin(BaseModel):
    room_id: str
    password: str
    user_id: str
    username: str
    message: Optional[str] = None
