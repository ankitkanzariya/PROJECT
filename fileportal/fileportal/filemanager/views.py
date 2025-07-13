from django.shortcuts import render, redirect
from django.contrib.auth import login, logout
from .models import User, FileUpload
from django.contrib.auth.decorators import login_required
from django.http import HttpResponse
import random
import os
from twilio.rest import Client
from dotenv import load_dotenv

# ✅ Load environment variables from .env file
load_dotenv()

def send_otp(phone):
    otp = str(random.randint(1000, 9999))

    # ✅ Twilio credentials from .env file
    account_sid = os.getenv("TWILIO_ACCOUNT_SID")
    auth_token = os.getenv("TWILIO_AUTH_TOKEN")
    twilio_number = os.getenv("TWILIO_PHONE_NUMBER")

    # ✅ Send OTP using Twilio
    client = Client(account_sid, auth_token)
    message = client.messages.create(
        body=f"🔐 Your OTP for login is: {otp}",
        from_=twilio_number,
        to=f"+91{phone}"  # You can adjust this for your country code
    )

    print("✅ OTP sent:", otp)
    return otp

def login_view(request):
    if request.method == 'POST':
        phone = request.POST.get('phone')
        otp = request.POST.get('otp')

        if not otp:
            request.session['otp'] = send_otp(phone)
            request.session['phone'] = phone
            return render(request, 'filemanager/otp.html')

        if otp == request.session.get('otp'):
            user, _ = User.objects.get_or_create(phone=request.session['phone'])
            login(request, user)
            return redirect('upload')
        else:
            return HttpResponse("❌ Invalid OTP")

    return render(request, 'filemanager/login.html')

@login_required
def upload_file(request):
    if request.method == 'POST':
        uploaded_file = request.FILES['file']
        FileUpload.objects.create(user=request.user, file=uploaded_file)
    files = FileUpload.objects.filter(user=request.user)
    return render(request, 'filemanager/upload.html', {'files': files})

def logout_view(request):
    logout(request)
    return redirect('login')
