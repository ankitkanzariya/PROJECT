from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from decouple import config

from .database import engine, Base
from .routes import rooms_router, users_router, join_requests_router

# Create database tables
Base.metadata.create_all(bind=engine)

# FastAPI app
app = FastAPI(
    title="Video Call API",
    description="Backend API for video calling application",
    version="1.0.0"
)

# CORS middleware
app.add_middleware(
    CORSMiddleware,
    allow_origins=config("CORS_ORIGINS", default="http://127.0.0.1:3000,http://localhost:3000,http://127.0.0.1:3001,http://localhost:3001").split(","),
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# Include API routes
app.include_router(rooms_router)
app.include_router(users_router)
app.include_router(join_requests_router)

@app.get("/")
async def root():
    return {"message": "Video Call API is running"}

@app.get("/health")
async def health_check():
    return {"status": "healthy"}

if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)
