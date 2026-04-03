# 📱 Telegram Mobile Shop SaaS Bot

A production-ready Telegram bot built with Django and python-telegram-bot that allows mobile shop owners to list and sell used phones.

## 🎯 Features

### For Shop Owners
- ✅ Auto-registration via Telegram ID
- ✅ Add phones with detailed information (model, price, specs, condition, warranty, etc.)
- ✅ Upload phone photos
- ✅ View all available phones
- ✅ Mark phones as sold
- ✅ Delete phone listings
- ✅ Multi-user support (each shop owner sees only their data)

### For Customers
- ✅ Browse all available phones from all shops
- ✅ View detailed phone information
- ✅ Contact shop owners
- ✅ Clean, user-friendly interface

## 🏗️ Architecture

- **Backend**: Django 4.2 with SQLite
- **Bot Framework**: python-telegram-bot 20.7 (async)
- **Database**: SQLite (development ready, easily switchable to PostgreSQL)
- **Deployment**: Production-ready with environment variables

## 🚀 Quick Start

### 1. Prerequisites
- Python 3.8+
- Telegram Bot Token (get from [@BotFather](https://t.me/botfather))

### 2. Installation

```bash
# Clone the repository
git clone <your-repo-url>
cd bot

# Create virtual environment
python -m venv venv

# Activate virtual environment
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# Install dependencies
pip install -r requirements.txt
```

### 3. Configuration

```bash
# Copy environment file
cp .env.example .env

# Edit .env file with your settings
TELEGRAM_BOT_TOKEN=your_telegram_bot_token_here
SECRET_KEY=your-secret-key-here
DEBUG=True
```

### 4. Database Setup

```bash
# Run migrations
python manage.py makemigrations
python manage.py migrate

# Create superuser (optional, for admin access)
python manage.py createsuperuser
```

### 5. Start the Bot

```bash
python bot.py
```

## 📋 Bot Commands & Flows

### Shop Owner Commands

#### `/start`
- Automatically registers shop owner
- Shows main menu with options:
  - ➕ Add Phone
  - 📱 View Phones
  - ❌ Delete Phone
  - ✅ Mark as Sold

#### Add Phone Flow
1. Click "➕ Add Phone"
2. Enter phone model name
3. Enter price
4. Enter RAM/Storage (e.g., "8GB/256GB")
5. Select condition (Excellent/Good/Average)
6. Enter warranty details (or "No")
7. Select guarantee (Yes/No)
8. Enter description (optional)
9. Upload photo (optional)

#### View Phones
- Shows all unsold phones from your shop
- Displays model, price, specs, and condition

#### Delete Phone
- Shows list of your phones
- Click to delete permanently

#### Mark as Sold
- Shows list of available phones
- Click to mark as sold (hides from customers)

### Customer Flow

#### `/start`
- Shows all available phones from all shops
- Displays phone details with shop information
- Contact button for each shop

## 🗄️ Database Models

### Shop
- `owner_telegram_id`: Unique Telegram user ID
- `shop_name`: Shop display name
- `is_active`: Shop status
- `created_at`: Registration timestamp

### Product (Phone)
- `shop`: Foreign key to Shop
- `name`: Phone model name
- `price`: Decimal price
- `ram_storage`: RAM and storage specs
- `condition`: Good/Excellent/Average
- `warranty`: Warranty details
- `guarantee`: Boolean for guarantee availability
- `description`: Product description
- `image`: Telegram file_id for photo
- `is_sold`: Sold status
- `created_at`: Listing timestamp

## 🔧 Configuration Options

### Environment Variables

```bash
# Required
TELEGRAM_BOT_TOKEN=your_bot_token

# Optional
SECRET_KEY=your_django_secret_key
DEBUG=True/False
SHOP_OWNER_IDS=123456789,987654321  # Comma-separated Telegram IDs
```

### Production Settings

For production deployment:

1. Set `DEBUG=False`
2. Use a secure `SECRET_KEY`
3. Configure proper database (PostgreSQL recommended)
4. Set up webhooks instead of polling
5. Configure domain and SSL

## 🚀 Deployment

### Using Docker (Recommended)

```dockerfile
# Dockerfile
FROM python:3.11-slim

WORKDIR /app

COPY requirements.txt .
RUN pip install -r requirements.txt

COPY . .

EXPOSE 8000

CMD ["python", "bot.py"]
```

### Using systemd (Linux)

```ini
# /etc/systemd/system/telegram-bot.service
[Unit]
Description=Telegram Mobile Shop Bot
After=network.target

[Service]
Type=simple
User=your-user
WorkingDirectory=/path/to/bot
Environment=PATH=/path/to/bot/venv/bin
ExecStart=/path/to/bot/venv/bin/python bot.py
Restart=always

[Install]
WantedBy=multi-user.target
```

```bash
# Enable and start
sudo systemctl enable telegram-bot
sudo systemctl start telegram-bot
```

## 🧪 Testing

```bash
# Run Django tests
python manage.py test

# Run specific app tests
python manage.py test shop
```

## 📊 Admin Interface

Access Django admin at `http://localhost:8000/admin/` (if running with `runserver`)

- View and manage shops
- View and manage products
- Monitor bot activity

## 🔒 Security Considerations

1. **Bot Token**: Keep your Telegram bot token secure
2. **Database**: Use proper database credentials in production
3. **Input Validation**: All user inputs are validated
4. **Rate Limiting**: Consider implementing rate limiting for production
5. **HTTPS**: Use HTTPS for webhook URLs in production

## 🐛 Troubleshooting

### Common Issues

1. **Bot doesn't respond**
   - Check TELEGRAM_BOT_TOKEN is correct
   - Ensure bot.py is running
   - Check internet connection

2. **Database errors**
   - Run migrations: `python manage.py migrate`
   - Check database file permissions

3. **Import errors**
   - Activate virtual environment
   - Install requirements: `pip install -r requirements.txt`

### Debug Mode

Enable debug logging by setting environment variable:
```bash
export PYTHONPATH=/path/to/your/project
python bot.py
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

## 🆘 Support

For issues and questions:
- Create an issue in the repository
- Check the troubleshooting section
- Review the logs for error messages

---

## 🎉 Features in Detail

### Multi-Shop Support
- Each Telegram user gets their own shop
- Complete data isolation between shops
- Scalable architecture for unlimited shops

### Rich Product Information
- Phone model and pricing
- Technical specifications (RAM/Storage)
- Condition ratings
- Warranty and guarantee information
- Photo uploads via Telegram
- Detailed descriptions

### User Experience
- Intuitive conversation flows
- Inline keyboards for easy navigation
- Clear error messages
- Responsive design for mobile

### Production Ready
- Comprehensive error handling
- Logging for monitoring
- Database migrations
- Admin interface
- Environment-based configuration
- Docker support

This bot is designed to be immediately deployable and scalable for production use while maintaining clean, maintainable code.
