from django.shortcuts import render, redirect, get_object_or_404
from .models import Product

def product_list(request):
    products = Product.objects.all()
    return render(request, 'store/product_list.html', {'products': products})

def product_create(request):
    if request.method == 'POST':
        Product.objects.create(
            name=request.POST['name'],
            price=request.POST['price'],
            description=request.POST['description']
        )
        return redirect('product_list')
    return render(request, 'store/product_form.html')

def product_update(request, pk):
    product = get_object_or_404(Product, pk=pk)
    if request.method == 'POST':
        product.name = request.POST['name']
        product.price = request.POST['price']
        product.description = request.POST['description']
        product.save()
        return redirect('product_list')
    return render(request, 'store/product_form.html', {'product': product})

def product_delete(request, pk):
    product = get_object_or_404(Product, pk=pk)
    product.delete()
    return redirect('product_list')