# Deployment Guide

## Production Deployment

### Frontend (Vercel)

1. **Prepare for Vercel Deployment**
   ```bash
   cd frontend
   npm run build
   ```

2. **Deploy to Vercel**
   - Connect your GitHub repository to Vercel
   - Set the root directory to `frontend`
   - Configure environment variables:
     - `REACT_APP_API_URL`: Your backend URL (e.g., `https://your-backend.onrender.com`)
     - `REACT_APP_SOCKET_URL`: Your backend URL (e.g., `https://your-backend.onrender.com`)

3. **Vercel Configuration**
   Create `frontend/vercel.json`:
   ```json
   {
     "version": 2,
     "builds": [
       {
         "src": "package.json",
         "use": "@vercel/static-build",
         "config": {
           "distDir": "build"
         }
       }
     ],
     "routes": [
       {
         "src": "/(.*)",
         "dest": "/index.html"
       }
     ]
   }
   ```

### Backend (Render)

1. **Prepare for Render Deployment**
   - Ensure all dependencies are in `requirements.txt`
   - Add environment variables to `.env`

2. **Deploy to Render**
   - Connect your GitHub repository to Render
   - Set the root directory to `backend`
   - Choose "Web Service" type
   - Configure:
     - Runtime: Python 3.9
     - Build Command: `pip install -r requirements.txt`
     - Start Command: `uvicorn app.main:app --host 0.0.0.0 --port $PORT`
     - Environment Variables:
       - `DATABASE_URL`: PostgreSQL connection string provided by Render
       - `SECRET_KEY`: Generate a secure random string
       - `CORS_ORIGINS`: Your Vercel frontend URL

3. **Database Setup**
   - Create a PostgreSQL database on Render
   - Get the connection string and add it to environment variables

### Environment Variables

#### Backend Environment Variables
- `DATABASE_URL`: PostgreSQL connection string
- `SECRET_KEY`: JWT secret key (generate with: `openssl rand -hex 32`)
- `CORS_ORIGINS`: Comma-separated list of allowed origins

#### Frontend Environment Variables
- `REACT_APP_API_URL`: Backend API URL
- `REACT_APP_SOCKET_URL`: Socket.IO server URL

## Local Development with Docker

### Prerequisites
- Docker and Docker Compose installed

### Running the Application
```bash
# Start all services
docker-compose up -d

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Database Migrations
If you need to run database migrations:
```bash
# Access the backend container
docker-compose exec backend bash

# Run migrations (if using Alembic)
alembic upgrade head
```

## SSL/HTTPS Configuration

### For Production
Both Vercel and Render automatically provide SSL certificates. Ensure:
1. All API calls use HTTPS URLs
2. Socket.IO connects over HTTPS (WebSocket Secure)
3. Environment variables use HTTPS URLs

### Custom Domain Setup
1. Configure custom domains in both Vercel and Render dashboards
2. Update CORS origins to include your custom domain
3. Update frontend environment variables

## Monitoring and Logging

### Backend Monitoring
- Use Render's built-in metrics
- Consider adding logging with Python's `logging` module
- Monitor error rates and response times

### Frontend Monitoring
- Vercel Analytics for performance metrics
- Consider adding error tracking (Sentry, etc.)
- Monitor Core Web Vitals

## Scaling Considerations

### Backend Scaling
- Render automatically scales web services
- Consider database connection pooling
- Implement rate limiting for API endpoints

### Frontend Scaling
- Vercel's edge network handles global distribution
- Optimize bundle size and loading performance
- Consider CDN for static assets

## Security Best Practices

1. **Environment Variables**: Never commit secrets to git
2. **CORS**: Restrict to specific origins in production
3. **Database**: Use connection pooling and secure credentials
4. **API**: Implement rate limiting and input validation
5. **WebRTC**: Use secure TURN servers for production if needed

## Troubleshooting

### Common Issues

1. **CORS Errors**
   - Check CORS origins configuration
   - Ensure frontend URL is included in allowed origins

2. **Socket.IO Connection Issues**
   - Verify Socket.IO server URL
   - Check firewall/proxy settings
   - Ensure WebSocket protocol is allowed

3. **Database Connection Issues**
   - Verify database URL format
   - Check database is running and accessible
   - Ensure proper credentials

4. **WebRTC Issues**
   - Check STUN/TURN server configuration
   - Verify camera/microphone permissions
   - Test with different browsers

### Debug Commands
```bash
# Check backend logs
docker-compose logs backend

# Check database connection
docker-compose exec backend python -c "from app.database import engine; print(engine.execute('SELECT 1').scalar())"

# Test API endpoints
curl http://localhost:8000/health

# Check frontend build
cd frontend && npm run build
```
