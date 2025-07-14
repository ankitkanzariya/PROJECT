from django.urls import path
from . import views

urlpatterns = [
    path('', views.login_view, name='login'),
    path('upload/', views.upload_file, name='upload'),
    path('logout/', views.logout_view, name='logout'),
    path('download/<int:file_id>/', views.download_file, name='download_file'),
    path('delete/<int:file_id>/', views.delete_file, name='delete_file'),
]

