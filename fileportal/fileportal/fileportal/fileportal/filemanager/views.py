from django.shortcuts import render, redirect, get_object_or_404
from django.contrib.auth import login, logout
from django.contrib.auth.decorators import login_required
from django.http import HttpResponse, FileResponse, HttpResponseForbidden
from .models import User, FileUpload
from pathlib import Path
import random
import os
from twilio.rest import Client
from dotenv import load_dotenv

load_dotenv()

# ✅ OTP Sending via Twilio
def send_otp(phone):
    otp = str(random.randint(1000, 9999))
    account_sid = os.getenv("TWILIO_ACCOUNT_SID")
    auth_token = os.getenv("TWILIO_AUTH_TOKEN")
    twilio_number = os.getenv("TWILIO_PHONE_NUMBER")

    client = Client(account_sid, auth_token)
    message = client.messages.create(
        body=f"🔐 Your OTP for login is: {otp}",
        from_=twilio_number,
        to=f"+91{phone}"
    )
    print("✅ OTP sent:", otp)
    return otp

# ✅ OTP Login View
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

# ✅ Upload File + Display Files (newest first)
@login_required
def upload_file(request):
    if request.method == 'POST':
        uploaded_file = request.FILES['file']
        FileUpload.objects.create(user=request.user, file=uploaded_file)
        return redirect('upload')  # Prevents duplicate uploads on refresh

    all_files = FileUpload.objects.filter(user=request.user).order_by('-uploaded_at')
    files = [f for f in all_files if Path(f.file.path).exists()]
    return render(request, 'filemanager/upload.html', {'files': files})

# ✅ File Download View
@login_required
def download_file(request, file_id):
    file_obj = get_object_or_404(FileUpload, id=file_id)

    if file_obj.user != request.user and not request.user.is_superuser:
        return HttpResponseForbidden("⛔ Not allowed!")

    file_path = Path(file_obj.file.path)
    if not file_path.exists():
        return HttpResponse("❌ File not found.")

    return FileResponse(open(file_path, 'rb'), as_attachment=True)

# ✅ File Delete View
@login_required
def delete_file(request, file_id):
    file_obj = get_object_or_404(FileUpload, id=file_id)

    if file_obj.user != request.user and not request.user.is_superuser:
        return HttpResponseForbidden("⛔ You can't delete this file.")

    # Delete file from disk
    file_path = Path(file_obj.file.path)
    if file_path.exists():
        file_path.unlink()

    # Delete DB entry
    file_obj.delete()
    return redirect('upload')

# ✅ Logout
def logout_view(request):
    logout(request)
    return redirect('login')
