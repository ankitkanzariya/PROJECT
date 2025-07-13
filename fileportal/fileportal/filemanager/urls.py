from django.urls import path
from . import views

urlpatterns = [
    path('', views.login_view, name='login'),
    path('upload/', views.upload_file, name='upload'),
    path('logout/', views.logout_view, name='logout'),
]

