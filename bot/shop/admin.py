from django.contrib import admin
from .models import Shop, Product


@admin.register(Shop)
class ShopAdmin(admin.ModelAdmin):
    list_display = ['shop_name', 'owner_telegram_id', 'is_active', 'created_at', 'active_products_count']
    list_filter = ['is_active', 'created_at']
    search_fields = ['shop_name', 'owner_telegram_id']
    readonly_fields = ['created_at', 'updated_at']
    
    def active_products_count(self, obj):
        return obj.active_products_count
    active_products_count.short_description = 'Active Products'


@admin.register(Product)
class ProductAdmin(admin.ModelAdmin):
    list_display = ['name', 'shop', 'price', 'ram_storage', 'condition', 'is_sold', 'created_at']
    list_filter = ['condition', 'is_sold', 'guarantee', 'created_at']
    search_fields = ['name', 'shop__shop_name', 'description']
    readonly_fields = ['created_at', 'updated_at']
    list_editable = ['is_sold']
    
    fieldsets = (
        ('Basic Information', {
            'fields': ('shop', 'name', 'price', 'ram_storage', 'condition')
        }),
        ('Warranty & Guarantee', {
            'fields': ('warranty', 'guarantee')
        }),
        ('Details', {
            'fields': ('description', 'image', 'is_sold')
        }),
        ('Timestamps', {
            'fields': ('created_at', 'updated_at'),
            'classes': ('collapse',)
        }),
    )
