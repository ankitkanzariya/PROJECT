from django.urls import path
from . import views

urlpatterns = [
    path('', views.shop_list, name='shop_list'),
    path('shop/<int:shop_id>/', views.shop_detail, name='shop_detail'),
]
