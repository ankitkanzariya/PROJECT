from sqlalchemy import Column, String, DateTime, Boolean, Text
from sqlalchemy.sql import func
from ..database import Base
import uuid

def generate_room_id():
    return str(uuid.uuid4())[:8].upper()

class Room(Base):
    __tablename__ = "rooms"

    id = Column(String, primary_key=True, default=generate_room_id)
    name = Column(String, nullable=False)
    password = Column(String, nullable=False)
    creator_id = Column(String, nullable=False)
    is_active = Column(Boolean, default=True)
    created_at = Column(DateTime(timezone=True), server_default=func.now())
    updated_at = Column(DateTime(timezone=True), onupdate=func.now())
