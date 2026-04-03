from django.test import TestCase
from .models import Shop, Product


class ShopModelTests(TestCase):
    def setUp(self):
        self.shop = Shop.objects.create(
            owner_telegram_id=123456789,
            shop_name="Test Shop"
        )
    
    def test_shop_creation(self):
        """Test shop model creation"""
        self.assertEqual(self.shop.owner_telegram_id, 123456789)
        self.assertEqual(self.shop.shop_name, "Test Shop")
        self.assertTrue(self.shop.is_active)
    
    def test_shop_str_representation(self):
        """Test shop string representation"""
        expected = "Test Shop (ID: 123456789)"
        self.assertEqual(str(self.shop), expected)
    
    def test_active_products_count(self):
        """Test active products count property"""
        # Create some products
        Product.objects.create(
            shop=self.shop,
            name="iPhone 12",
            price=599.99,
            ram_storage="6GB/128GB",
            condition="good"
        )
        Product.objects.create(
            shop=self.shop,
            name="iPhone 13",
            price=699.99,
            ram_storage="6GB/256GB",
            condition="excellent",
            is_sold=True
        )
        
        # Should only count non-sold products
        self.assertEqual(self.shop.active_products_count, 1)


class ProductModelTests(TestCase):
    def setUp(self):
        self.shop = Shop.objects.create(
            owner_telegram_id=123456789,
            shop_name="Test Shop"
        )
        self.product = Product.objects.create(
            shop=self.shop,
            name="iPhone 12",
            price=599.99,
            ram_storage="6GB/128GB",
            condition="good"
        )
    
    def test_product_creation(self):
        """Test product model creation"""
        self.assertEqual(self.product.name, "iPhone 12")
        self.assertEqual(self.product.price, 599.99)
        self.assertEqual(self.product.shop, self.shop)
        self.assertFalse(self.product.is_sold)
    
    def test_product_str_representation(self):
        """Test product string representation"""
        expected = "iPhone 12 - Test Shop ($599.99)"
        self.assertEqual(str(self.product), expected)
    
    def test_formatted_price(self):
        """Test formatted price property"""
        expected = "$599.99"
        self.assertEqual(self.product.formatted_price, expected)
    
    def test_condition_display(self):
        """Test condition display property"""
        self.assertEqual(self.product.condition_display, "Good")
