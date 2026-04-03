from django.shortcuts import render
from .models import Shop, Product


def shop_list(request):
    """List all active shops"""
    shops = Shop.objects.filter(is_active=True)
    return render(request, 'shop/list.html', {'shops': shops})


def shop_detail(request, shop_id):
    """Show details of a specific shop"""
    shop = Shop.objects.get(id=shop_id)
    products = shop.products.filter(is_sold=False)
    return render(request, 'shop/detail.html', {'shop': shop, 'products': products})
