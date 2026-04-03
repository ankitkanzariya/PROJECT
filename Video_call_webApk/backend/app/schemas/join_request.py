from pydantic import BaseModel
from datetime import datetime
from typing import Optional

class JoinRequestCreate(BaseModel):
    room_id: str
    user_id: str
    username: str
    message: Optional[str] = None

class JoinRequestResponse(BaseModel):
    id: str
    room_id: str
    user_id: str
    username: str
    status: str
    message: Optional[str]
    created_at: datetime
    
    class Config:
        from_attributes = True
