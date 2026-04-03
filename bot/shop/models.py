from django.db import models
from django.utils import timezone


class Shop(models.Model):
    CONDITION_CHOICES = [
        ('good', 'Good'),
        ('excellent', 'Excellent'),
        ('average', 'Average'),
    ]

    id = models.AutoField(primary_key=True)
    owner_telegram_id = models.BigIntegerField(unique=True, help_text="Telegram user ID of the shop owner")
    shop_name = models.CharField(max_length=100, default="Mobile Shop")
    is_active = models.BooleanField(default=True)
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    class Meta:
        db_table = 'shops'
        verbose_name = 'Shop'
        verbose_name_plural = 'Shops'

    def __str__(self):
        return f"{self.shop_name} (ID: {self.owner_telegram_id})"

    @property
    def active_products_count(self):
        return self.products.filter(is_sold=False).count()


class Product(models.Model):
    CONDITION_CHOICES = [
        ('good', 'Good'),
        ('excellent', 'Excellent'),
        ('average', 'Average'),
    ]

    id = models.AutoField(primary_key=True)
    shop = models.ForeignKey(Shop, on_delete=models.CASCADE, related_name='products')
    name = models.CharField(max_length=200, help_text="Phone model name")
    price = models.DecimalField(max_digits=10, decimal_places=2, help_text="Price in local currency")
    ram_storage = models.CharField(max_length=100, help_text="RAM and Storage (e.g., '8GB/256GB')")
    condition = models.CharField(max_length=20, choices=CONDITION_CHOICES, default='good')
    warranty = models.TextField(blank=True, help_text="Warranty details")
    guarantee = models.BooleanField(default=False, help_text="Has guarantee")
    description = models.TextField(blank=True, help_text="Product description")
    image = models.CharField(max_length=500, blank=True, help_text="Telegram file_id of the product image")
    is_sold = models.BooleanField(default=False, help_text="Mark if product is sold")
    created_at = models.DateTimeField(auto_now_add=True)
    updated_at = models.DateTimeField(auto_now=True)

    class Meta:
        db_table = 'products'
        verbose_name = 'Product'
        verbose_name_plural = 'Products'
        ordering = ['-created_at']

    def __str__(self):
        return f"{self.name} - {self.shop.shop_name} (${self.price})"

    @property
    def formatted_price(self):
        return f"${self.price:,.2f}"

    @property
    def condition_display(self):
        return dict(self.CONDITION_CHOICES).get(self.condition, self.condition.title())
