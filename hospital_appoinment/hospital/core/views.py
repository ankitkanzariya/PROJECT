from django.shortcuts import render
from rest_framework.permissions import AllowAny

from rest_framework import generics, permissions, serializers
from .models import Slot, Appointment
from .serializers import UserSerializer, SlotSerializer, AppointmentSerializer
from django.contrib.auth import get_user_model

# try this

from django.http import JsonResponse
from django.views import View

class HomeView(View):
    def get(self, request):
        return JsonResponse({"message": "Welcome to Hospital API"})
 
# ovr

# Create your views here.
class RegisterView(generics.CreateAPIView):
    queryset = get_user_model().objects.all()
    serializer_class = UserSerializer
    permission_classes = [AllowAny]

class SlotCreateView(generics.CreateAPIView):
    queryset = Slot.objects.all()
    serializer_class = SlotSerializer
    permission_classes = [permissions.IsAuthenticated]

    def perform_create(self, serializer):
        serializer.save(doctor=self.request.user)

class SlotListView(generics.ListAPIView):
    serializer_class = SlotSerializer

    def get_queryset(self):
        return Slot.objects.filter(doctor=self.request.user)

class AppointmentCreateView(generics.CreateAPIView):
    queryset = Appointment.objects.all()
    serializer_class = AppointmentSerializer
    permission_classes = [permissions.IsAuthenticated]

    def perform_create(self, serializer):
        slot = serializer.validated_data['slot']
        if slot.is_booked:
            raise serializers.ValidationError("Slot is already booked.")
        slot.is_booked = True
        slot.save()
        serializer.save(patient=self.request.user)

class AppointmentListView(generics.ListAPIView):
    serializer_class = AppointmentSerializer

    def get_queryset(self):
        user = self.request.user
        if user.role == 'patient':
            return Appointment.objects.filter(patient=user)
        elif user.role == 'doctor':
            return Appointment.objects.filter(slot__doctor=user)


