from pydantic import BaseModel
from datetime import datetime
from typing import Optional

class UserCreate(BaseModel):
    id: str
    username: str
    email: Optional[str] = None

class UserResponse(BaseModel):
    id: str
    username: str
    email: Optional[str]
    is_online: bool
    last_seen: datetime
    
    class Config:
        from_attributes = True
