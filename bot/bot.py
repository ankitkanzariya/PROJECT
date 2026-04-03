import os
import sys
import django
import asyncio
import logging
from datetime import datetime

from telegram import Update, InlineKeyboardButton, InlineKeyboardMarkup
from telegram.ext import (
    Application,
    CommandHandler,
    MessageHandler,
    filters,
    ConversationHandler,
    CallbackQueryHandler,
    ContextTypes,
)

# Setup Django
os.environ.setdefault('DJANGO_SETTINGS_MODULE', 'telegram_bot.settings')
django.setup()

from django.db import models
from shop.models import Shop, Product
from asgiref.sync import sync_to_async

# Configure logging
logging.basicConfig(
    format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',
    level=logging.INFO
)
logger = logging.getLogger(__name__)

# Conversation states
ADD_PHONE, MODEL, PRICE, RAM_STORAGE, CONDITION, WARRANTY, GUARANTEE, DESCRIPTION, PHOTO = range(9)

# Bot token from environment
BOT_TOKEN = os.environ.get('TELEGRAM_BOT_TOKEN')
if not BOT_TOKEN:
    logger.error("TELEGRAM_BOT_TOKEN environment variable not set!")
    sys.exit(1)


@sync_to_async
def get_or_create_shop(telegram_id: int, first_name: str = "") -> Shop:
    """Get or create a shop for the given telegram ID"""
    shop, created = Shop.objects.get_or_create(
        owner_telegram_id=telegram_id,
        defaults={'shop_name': f"{first_name}'s Mobile Shop"}
    )
    return shop


@sync_to_async
def get_shop(telegram_id: int) -> Shop:
    """Get shop by telegram ID"""
    try:
        return Shop.objects.get(owner_telegram_id=telegram_id)
    except Shop.DoesNotExist:
        return None


@sync_to_async
def get_shop_products(shop: Shop, include_sold: bool = False):
    """Get products for a shop"""
    queryset = shop.products.all()
    if not include_sold:
        queryset = queryset.filter(is_sold=False)
    return list(queryset)


@sync_to_async
def create_product(shop: Shop, **kwargs) -> Product:
    """Create a new product"""
    return Product.objects.create(shop=shop, **kwargs)


@sync_to_async
def get_product(product_id: int) -> Product:
    """Get product by ID"""
    try:
        return Product.objects.get(id=product_id)
    except Product.DoesNotExist:
        return None


@sync_to_async
def delete_product(product_id: int) -> bool:
    """Delete a product"""
    try:
        product = Product.objects.get(id=product_id)
        product.delete()
        return True
    except Product.DoesNotExist:
        return False


@sync_to_async
def mark_product_sold(product_id: int) -> bool:
    """Mark a product as sold"""
    try:
        product = Product.objects.get(id=product_id)
        product.is_sold = True
        product.save()
        return True
    except Product.DoesNotExist:
        return False


async def start(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Handle /start command"""
    user = update.effective_user
    telegram_id = user.id
    
    # Check if user is a shop owner (for demo, we'll treat everyone as shop owner)
    # In production, you might have a list of authorized shop owner IDs
    shop = await get_or_create_shop(telegram_id, user.first_name or "Shop Owner")
    
    if shop:
        # Shop owner menu
        keyboard = [
            [InlineKeyboardButton("➕ Add Phone", callback_data="add_phone")],
            [InlineKeyboardButton("📱 View Phones", callback_data="view_phones")],
            [InlineKeyboardButton("❌ Delete Phone", callback_data="delete_phone")],
            [InlineKeyboardButton("✅ Mark as Sold", callback_data="mark_sold")],
        ]
        reply_markup = InlineKeyboardMarkup(keyboard)
        
        await update.message.reply_text(
            f"🏪 Welcome to {shop.shop_name}!\n\n"
            "Choose an action:",
            reply_markup=reply_markup
        )
    else:
        # Customer view - show all available phones
        await show_customer_phones(update, context)


async def show_customer_phones(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Show all available phones to customers"""
    # For demo, we'll show products from the first shop
    # In production, you might have multiple shops or a shop selection mechanism
    shops = await sync_to_async(list)(Shop.objects.filter(is_active=True))
    
    if not shops:
        await update.message.reply_text("📱 No shops available at the moment.")
        return
    
    all_products = []
    for shop in shops:
        products = await get_shop_products(shop)
        for product in products:
            all_products.append((shop, product))
    
    if not all_products:
        await update.message.reply_text("📱 No phones available for sale at the moment.")
        return
    
    # Create message with all available phones
    message = "📱 **Available Phones**\n\n"
    
    for shop, product in all_products:
        message += f"🏪 *{shop.shop_name}*\n"
        message += f"📱 *{product.name}*\n"
        message += f"💰 Price: {product.formatted_price}\n"
        message += f"💾 Storage: {product.ram_storage}\n"
        message += f"📊 Condition: {product.condition_display}\n"
        
        if product.warranty:
            message += f"🛡️ Warranty: {product.warranty}\n"
        
        if product.guarantee:
            message += f"✅ Guarantee Available\n"
        
        if product.description:
            message += f"📝 {product.description}\n"
        
        message += "\n" + "="*30 + "\n\n"
    
    # Add contact button
    keyboard = [[InlineKeyboardButton("📞 Contact Shop Owner", callback_data="contact_owner")]]
    reply_markup = InlineKeyboardMarkup(keyboard)
    
    await update.message.reply_text(
        message,
        reply_markup=reply_markup,
        parse_mode='Markdown'
    )


async def button_callback(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Handle button callbacks"""
    query = update.callback_query
    await query.answer()
    
    data = query.data
    
    if data == "add_phone":
        return await start_add_phone(update, context)
    elif data == "view_phones":
        return await show_shop_phones(update, context)
    elif data == "delete_phone":
        return await start_delete_phone(update, context)
    elif data == "mark_sold":
        return await start_mark_sold(update, context)
    elif data == "contact_owner":
        await query.edit_message_text(
            "📞 Please contact the shop owner directly for more information.\n"
            "You can find their contact details in the shop information."
        )
    elif data.startswith("delete_"):
        product_id = int(data.split("_")[1])
        success = await delete_product(product_id)
        if success:
            await query.edit_message_text("✅ Phone deleted successfully!")
        else:
            await query.edit_message_text("❌ Error deleting phone.")
    elif data.startswith("sold_"):
        product_id = int(data.split("_")[1])
        success = await mark_product_sold(product_id)
        if success:
            await query.edit_message_text("✅ Phone marked as sold!")
        else:
            await query.edit_message_text("❌ Error marking phone as sold.")
    
    return ConversationHandler.END


async def start_add_phone(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Start the add phone conversation"""
    await update.callback_query.edit_message_text(
        "📱 Let's add a new phone!\n\n"
        "First, please enter the **phone model name**:"
    )
    return MODEL


async def get_model(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get phone model name"""
    context.user_data['model'] = update.message.text
    await update.message.reply_text(
        f"✅ Phone model: {update.message.text}\n\n"
        "Now enter the **price** (e.g., 299.99):"
    )
    return PRICE


async def get_price(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get phone price"""
    try:
        price = float(update.message.text)
        context.user_data['price'] = price
        await update.message.reply_text(
            f"✅ Price: ${price:.2f}\n\n"
            "Now enter the **RAM and Storage** (e.g., '8GB/256GB'):"
        )
        return RAM_STORAGE
    except ValueError:
        await update.message.reply_text(
            "❌ Invalid price format. Please enter a valid number (e.g., 299.99):"
        )
        return PRICE


async def get_ram_storage(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get RAM and storage"""
    context.user_data['ram_storage'] = update.message.text
    keyboard = [
        [InlineKeyboardButton("Excellent", callback_data="condition_excellent")],
        [InlineKeyboardButton("Good", callback_data="condition_good")],
        [InlineKeyboardButton("Average", callback_data="condition_average")],
    ]
    reply_markup = InlineKeyboardMarkup(keyboard)
    
    await update.message.reply_text(
        f"✅ Storage: {update.message.text}\n\n"
        "Now select the **condition**:",
        reply_markup=reply_markup
    )
    return CONDITION


async def get_condition(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get phone condition"""
    query = update.callback_query
    await query.answer()
    
    condition = query.data.split("_")[1]
    context.user_data['condition'] = condition
    
    await query.edit_message_text(
        f"✅ Condition: {condition.title()}\n\n"
        "Does the phone have **warranty**? Please provide details (or send 'No'):"
    )
    return WARRANTY


async def get_warranty(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get warranty information"""
    warranty = update.message.text
    if warranty.lower() in ['no', 'none', 'n']:
        warranty = ""
    
    context.user_data['warranty'] = warranty
    
    keyboard = [
        [InlineKeyboardButton("Yes", callback_data="guarantee_yes")],
        [InlineKeyboardButton("No", callback_data="guarantee_no")],
    ]
    reply_markup = InlineKeyboardMarkup(keyboard)
    
    await update.message.reply_text(
        f"✅ Warranty: {warranty or 'No warranty'}\n\n"
        "Does the phone have **guarantee**?",
        reply_markup=reply_markup
    )
    return GUARANTEE


async def get_guarantee(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get guarantee information"""
    query = update.callback_query
    await query.answer()
    
    guarantee = query.data == "guarantee_yes"
    context.user_data['guarantee'] = guarantee
    
    await query.edit_message_text(
        f"✅ Guarantee: {'Yes' if guarantee else 'No'}\n\n"
        "Now enter a **description** for the phone (or send 'Skip'):"
    )
    return DESCRIPTION


async def get_description(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get phone description"""
    description = update.message.text
    if description.lower() in ['skip', 'none']:
        description = ""
    
    context.user_data['description'] = description
    
    await update.message.reply_text(
        f"✅ Description: {description or 'No description'}\n\n"
        "Finally, please upload a **photo** of the phone (or send 'Skip'):"
    )
    return PHOTO


async def get_photo(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Get phone photo and save the product"""
    telegram_id = update.effective_user.id
    shop = await get_shop(telegram_id)
    
    if not shop:
        await update.message.reply_text("❌ Shop not found. Please start again with /start")
        return ConversationHandler.END
    
    # Handle photo
    image_file_id = ""
    if update.message.photo:
        # Get the highest quality photo
        image_file_id = update.message.photo[-1].file_id
    elif update.message.text and update.message.text.lower() in ['skip', 'none']:
        image_file_id = ""
    else:
        await update.message.reply_text("Please upload a photo or send 'Skip':")
        return PHOTO
    
    # Create the product
    product = await create_product(
        shop=shop,
        name=context.user_data['model'],
        price=context.user_data['price'],
        ram_storage=context.user_data['ram_storage'],
        condition=context.user_data['condition'],
        warranty=context.user_data['warranty'],
        guarantee=context.user_data['guarantee'],
        description=context.user_data['description'],
        image=image_file_id
    )
    
    # Send confirmation
    message = f"✅ **Phone added successfully!**\n\n"
    message += f"📱 {product.name}\n"
    message += f"💰 {product.formatted_price}\n"
    message += f"💾 {product.ram_storage}\n"
    message += f"📊 {product.condition_display}\n"
    
    if product.warranty:
        message += f"🛡️ {product.warranty}\n"
    
    if product.guarantee:
        message += f"✅ Guarantee Available\n"
    
    if product.description:
        message += f"📝 {product.description}\n"
    
    if product.image:
        await update.message.reply_photo(
            photo=product.image,
            caption=message,
            parse_mode='Markdown'
        )
    else:
        await update.message.reply_text(message, parse_mode='Markdown')
    
    # Clear user data
    context.user_data.clear()
    
    return ConversationHandler.END


async def show_shop_phones(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Show phones for the shop owner"""
    telegram_id = update.effective_user.id
    shop = await get_shop(telegram_id)
    
    if not shop:
        await update.callback_query.edit_message_text("❌ Shop not found. Please start with /start")
        return
    
    products = await get_shop_products(shop)
    
    if not products:
        await update.callback_query.edit_message_text(
            f"📱 No phones available in {shop.shop_name}.\n"
            "Click '➕ Add Phone' to add your first phone!"
        )
        return
    
    message = f"📱 **{shop.shop_name} - Available Phones**\n\n"
    
    for product in products:
        message += f"📱 *{product.name}*\n"
        message += f"💰 {product.formatted_price}\n"
        message += f"💾 {product.ram_storage}\n"
        message += f"📊 {product.condition_display}\n"
        message += f"🆔 ID: {product.id}\n\n"
    
    await update.callback_query.edit_message_text(
        message,
        parse_mode='Markdown'
    )


async def start_delete_phone(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Start delete phone process"""
    telegram_id = update.effective_user.id
    shop = await get_shop(telegram_id)
    
    if not shop:
        await update.callback_query.edit_message_text("❌ Shop not found. Please start with /start")
        return
    
    products = await get_shop_products(shop)
    
    if not products:
        await update.callback_query.edit_message_text(
            "📱 No phones available to delete."
        )
        return
    
    keyboard = []
    for product in products:
        keyboard.append([
            InlineKeyboardButton(
                f"📱 {product.name} - {product.formatted_price}",
                callback_data=f"delete_{product.id}"
            )
        ])
    
    reply_markup = InlineKeyboardMarkup(keyboard)
    
    await update.callback_query.edit_message_text(
        "🗑️ **Select a phone to delete:**",
        reply_markup=reply_markup
    )


async def start_mark_sold(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Start mark as sold process"""
    telegram_id = update.effective_user.id
    shop = await get_shop(telegram_id)
    
    if not shop:
        await update.callback_query.edit_message_text("❌ Shop not found. Please start with /start")
        return
    
    products = await get_shop_products(shop)
    
    if not products:
        await update.callback_query.edit_message_text(
            "📱 No phones available to mark as sold."
        )
        return
    
    keyboard = []
    for product in products:
        keyboard.append([
            InlineKeyboardButton(
                f"📱 {product.name} - {product.formatted_price}",
                callback_data=f"sold_{product.id}"
            )
        ])
    
    reply_markup = InlineKeyboardMarkup(keyboard)
    
    await update.callback_query.edit_message_text(
        "✅ **Select a phone to mark as sold:**",
        reply_markup=reply_markup
    )


async def cancel(update: Update, context: ContextTypes.DEFAULT_TYPE):
    """Cancel the conversation"""
    await update.message.reply_text("❌ Operation cancelled.")
    context.user_data.clear()
    return ConversationHandler.END


def main():
    """Start the bot"""
    # Create the Application
    application = Application.builder().token(BOT_TOKEN).build()
    
    # Add conversation handler for adding phones
    add_phone_conv = ConversationHandler(
        entry_points=[CallbackQueryHandler(start_add_phone, pattern="^add_phone$")],
        states={
            MODEL: [MessageHandler(filters.TEXT & ~filters.COMMAND, get_model)],
            PRICE: [MessageHandler(filters.TEXT & ~filters.COMMAND, get_price)],
            RAM_STORAGE: [MessageHandler(filters.TEXT & ~filters.COMMAND, get_ram_storage)],
            CONDITION: [CallbackQueryHandler(get_condition, pattern="^condition_")],
            WARRANTY: [MessageHandler(filters.TEXT & ~filters.COMMAND, get_warranty)],
            GUARANTEE: [CallbackQueryHandler(get_guarantee, pattern="^guarantee_")],
            DESCRIPTION: [MessageHandler(filters.TEXT & ~filters.COMMAND, get_description)],
            PHOTO: [MessageHandler(filters.PHOTO | filters.TEXT & ~filters.COMMAND, get_photo)],
        },
        fallbacks=[CommandHandler('cancel', cancel)],
    )
    
    # Add handlers
    application.add_handler(CommandHandler('start', start))
    application.add_handler(add_phone_conv)
    application.add_handler(CallbackQueryHandler(button_callback))
    
    # Start the bot
    logger.info("Starting Telegram bot...")
    application.run_polling(allowed_updates=Update.ALL_TYPES)


if __name__ == '__main__':
    main()
