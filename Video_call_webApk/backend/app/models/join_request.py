from sqlalchemy import Column, String, DateTime, Boolean, ForeignKey, Text
from sqlalchemy.sql import func
from sqlalchemy.orm import relationship
from ..database import Base

class JoinRequest(Base):
    __tablename__ = "join_requests"

    id = Column(String, primary_key=True)
    room_id = Column(String, ForeignKey("rooms.id"), nullable=False)
    user_id = Column(String, nullable=False)
    username = Column(String, nullable=False)
    status = Column(String, default="pending")  # pending, approved, rejected
    message = Column(Text, nullable=True)
    created_at = Column(DateTime(timezone=True), server_default=func.now())
    updated_at = Column(DateTime(timezone=True), onupdate=func.now())
    
    # Relationship
    room = relationship("Room", backref="join_requests")
