from django.urls import path
from rest_framework_simplejwt.views import TokenObtainPairView, TokenRefreshView
from .views import *
from .views import HomeView

urlpatterns = [
    path('', HomeView.as_view(), name='home'),

    path('register/', RegisterView.as_view(), name='register'),
    path('token/', TokenObtainPairView.as_view(), name='token_obtain_pair'),
    path('token/refresh/', TokenRefreshView.as_view(), name='token_refresh'),
    path('slots/', SlotCreateView.as_view(), name='create-slot'),
    path('my-slots/', SlotListView.as_view(), name='list-slot'),
    path('appointments/', AppointmentCreateView.as_view(), name='create-appointment'),
    path('my-appointments/', AppointmentListView.as_view(), name='list-appointments'),
]





