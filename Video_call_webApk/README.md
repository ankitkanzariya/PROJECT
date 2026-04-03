# Video Calling Web Application

A full-stack video calling application with room-based meetings, password protection, and approval system.

## Features

- Create password-protected video rooms with unique IDs
- Search and join rooms using room ID
- Password-protected join requests
- Room creator approval/rejection system
- WebRTC video communication
- Real-time signaling with Socket.IO
- Video tiles UI for participants
- Mute, camera toggle, and leave room controls

## Tech Stack

- **Frontend**: React with TypeScript
- **Backend**: FastAPI with Python
- **Real-time**: Socket.IO
- **Database**: PostgreSQL
- **Deployment**: Vercel (Frontend), Render (Backend)

## Project Structure

```
Video_call_webApk/
├── backend/                 # FastAPI backend
│   ├── app/
│   │   ├── __init__.py
│   │   ├── main.py         # FastAPI app entry point
│   │   ├── database.py     # Database configuration
│   │   ├── models/         # SQLAlchemy models
│   │   ├── schemas/        # Pydantic schemas
│   │   ├── routes/         # API routes
│   │   └── socketio/       # Socket.IO handlers
│   ├── requirements.txt
│   └── Dockerfile
├── frontend/               # React frontend
│   ├── src/
│   │   ├── components/     # React components
│   │   ├── pages/          # Page components
│   │   ├── hooks/          # Custom hooks
│   │   ├── services/       # API and Socket.IO services
│   │   └── utils/          # Utility functions
│   ├── package.json
│   └── Dockerfile
├── docker-compose.yml      # Local development
└── docs/                   # Additional documentation
```

## Local Development

### Prerequisites

- Node.js (v18+)
- Python (v3.9+)
- PostgreSQL
- Docker (optional)

### Backend Setup

```bash
cd backend
python -m venv venv
source venv/bin/activate  # On Windows: venv\Scripts\activate
pip install -r requirements.txt

# Set environment variables
DATABASE_URL=postgresql://username:password@localhost:5432/video_call_db
SECRET_KEY=your-secret-key

# Run database migrations
alembic upgrade head

# Start the server
uvicorn app.main:app --reload --host 0.0.0.0 --port 8000
```

### Frontend Setup

```bash
cd frontend
npm install

# Start the development server
npm start
```

### Using Docker

```bash
docker-compose up -d
```

## Deployment

### Frontend (Vercel)

1. Connect your GitHub repository to Vercel
2. Set the root directory to `frontend`
3. Configure environment variables
4. Deploy

### Backend (Render)

1. Connect your GitHub repository to Render
2. Set the root directory to `backend`
3. Configure PostgreSQL database
4. Set environment variables
5. Deploy

## Environment Variables

### Backend
- `DATABASE_URL`: PostgreSQL connection string
- `SECRET_KEY`: JWT secret key
- `CORS_ORIGINS`: Allowed frontend origins

### Frontend
- `REACT_APP_API_URL`: Backend API URL
- `REACT_APP_SOCKET_URL`: Socket.IO server URL

## API Documentation

Once the backend is running, visit `http://localhost:8000/docs` for interactive API documentation.

## License

MIT License
