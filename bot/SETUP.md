# 🚀 Setup Guide for Telegram Mobile Shop Bot

## Step-by-Step Installation and Configuration

### 📋 Prerequisites
- Python 3.8 or higher
- Git
- Telegram account
- Basic command line knowledge

---

## 🔧 Step 1: Get Telegram Bot Token

1. Open Telegram and search for **@BotFather**
2. Send `/start` to BotFather
3. Send `/newbot` to create a new bot
4. Follow the prompts:
   - Choose a name for your bot (e.g., "Mobile Shop Bot")
   - Choose a username (must end with `_bot`, e.g., `mobile_shop_bot`)
5. BotFather will give you a **token** - copy this token
6. Keep this token secure - it's like a password for your bot

---

## 💻 Step 2: Set Up the Project

### Option A: Clone from Repository
```bash
git clone <your-repository-url>
cd bot
```

### Option B: Create New Project
```bash
mkdir telegram-mobile-shop
cd telegram-mobile-shop
# Copy all the provided files into this directory
```

---

## 🐍 Step 3: Set Up Python Environment

```bash
# Create virtual environment
python -m venv venv

# Activate virtual environment
# Windows:
venv\Scripts\activate
# macOS/Linux:
source venv/bin/activate

# Verify activation (you should see (venv) in your prompt)
```

---

## 📦 Step 4: Install Dependencies

```bash
# Install required packages
pip install -r requirements.txt

# Verify installation
pip list
```

---

## ⚙️ Step 5: Configure Environment

```bash
# Copy the example environment file
cp .env.example .env

# Edit the .env file with a text editor
# Notepad (Windows) or nano (macOS/Linux)
notepad .env  # Windows
nano .env      # macOS/Linux
```

**Edit .env file with your settings:**
```env
TELEGRAM_BOT_TOKEN=paste_your_bot_token_here
SECRET_KEY=generate-a-random-secret-key-here
DEBUG=True
SHOP_OWNER_IDS=  # Leave empty to allow anyone to be shop owner
```

**To generate a SECRET_KEY:**
```bash
python -c "from django.core.management.utils import get_random_secret_key; print(get_random_secret_key())"
```

---

## 🗄️ Step 6: Set Up Database

```bash
# Create database migrations
python manage.py makemigrations

# Apply migrations to create tables
python manage.py migrate

# (Optional) Create admin user to access Django admin
python manage.py createsuperuser
# Follow prompts to create username, email, and password
```

---

## 🚀 Step 7: Start the Bot

```bash
# Make sure you're in the project directory with activated venv
python bot.py
```

**You should see output like:**
```
2024-01-01 12:00:00,000 - root - INFO - Starting Telegram bot...
```

---

## 📱 Step 8: Test Your Bot

1. Open Telegram
2. Search for your bot using its username
3. Send `/start` to your bot
4. You should see the welcome message with buttons

**Expected behavior:**
- Bot responds immediately
- Shows menu with ➕ Add Phone, 📱 View Phones, etc.
- All buttons work correctly

---

## 🔍 Step 9: Verify Everything Works

### Test Shop Owner Functions:
1. Click "➕ Add Phone"
2. Follow the conversation flow:
   - Enter phone model: "iPhone 12"
   - Enter price: "599.99"
   - Enter RAM/Storage: "6GB/128GB"
   - Select condition: "Good"
   - Enter warranty: "6 months warranty"
   - Select guarantee: "Yes"
   - Enter description: "Excellent condition iPhone 12"
   - Upload a photo or send "Skip"

3. Click "📱 View Phones" - should show your added phone
4. Click "✅ Mark as Sold" - select your phone to mark as sold
5. Click "❌ Delete Phone" - select a phone to delete

### Test Customer View:
1. Have a different user (or yourself in a different account) send `/start`
2. Should see all available phones from all shops
3. Should not see sold phones

---

## 🛠️ Common Issues & Solutions

### Issue: "ModuleNotFoundError: No module named 'django'"
**Solution:**
```bash
# Make sure virtual environment is activated
venv\Scripts\activate  # Windows
source venv/bin/activate  # macOS/Linux

# Reinstall requirements
pip install -r requirements.txt
```

### Issue: "TELEGRAM_BOT_TOKEN environment variable not set!"
**Solution:**
1. Check that `.env` file exists
2. Verify token is correctly copied
3. Ensure no extra spaces or quotes around the token

### Issue: Bot doesn't respond to messages
**Solution:**
1. Verify bot.py is running (check terminal)
2. Check internet connection
3. Verify bot token is correct
4. Try restarting the bot

### Issue: Database errors
**Solution:**
```bash
# Reset database (WARNING: This deletes all data)
rm db.sqlite3
python manage.py makemigrations
python manage.py migrate
```

---

## 🌐 Optional: Webhook Configuration (Production)

For production deployment, you'll want to use webhooks instead of polling:

1. **Get a domain and SSL certificate**
2. **Configure your firewall** to allow HTTPS traffic
3. **Update bot.py** to use webhooks instead of polling

```python
# In bot.py, replace the last lines with:
application.run_webhook(
    listen="0.0.0.0",
    port=8443,
    url_path="your-secret-path",
    webhook_url=f"https://your-domain.com/your-secret-path"
)
```

---

## 📊 Monitoring and Maintenance

### Check Bot Logs
```bash
# Bot logs are displayed in the terminal when running
# For production, consider logging to a file
```

### Database Backup
```bash
# Backup SQLite database
cp db.sqlite3 backup_$(date +%Y%m%d).sqlite3
```

### Update Dependencies
```bash
# Check for updates
pip list --outdated

# Update specific packages
pip install --upgrade package-name
```

---

## 🎯 Next Steps

Once your bot is running:

1. **Customize messages** - Edit bot.py to match your brand
2. **Add features** - Implement search, filtering, etc.
3. **Set up monitoring** - Add logging and error tracking
4. **Deploy to production** - Use Docker or cloud hosting
5. **Add multiple shops** - Configure SHOP_OWNER_IDS for specific users

---

## 🆘 Need Help?

If you encounter issues:

1. Check the troubleshooting section above
2. Review the terminal output for error messages
3. Verify all configuration steps were completed
4. Check that your Telegram bot token is valid
5. Ensure your virtual environment is activated

---

## ✅ Success Checklist

- [ ] Telegram bot token obtained from @BotFather
- [ ] Python virtual environment created and activated
- [ ] Dependencies installed from requirements.txt
- [ ] .env file configured with bot token
- [ ] Database migrations applied
- [ ] Bot starts without errors
- [ ] Bot responds to /start command
- [ ] Add phone flow works correctly
- [ ] View/delete/mark sold functions work
- [ ] Customer view shows available phones

Your Telegram Mobile Shop Bot is now ready for use! 🎉
