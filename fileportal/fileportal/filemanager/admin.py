from django.contrib import admin
from .models import User, FileUpload
from django.utils.html import format_html

class FileInline(admin.TabularInline):
    model = FileUpload
    extra = 0

class UserAdmin(admin.ModelAdmin):
    list_display = ['phone']
    inlines = [FileInline]

class FileUploadAdmin(admin.ModelAdmin):
    list_display = ['user', 'uploaded_at', 'file_link']

    def file_link(self, obj):
        return format_html('<a href="{}" target="_blank">{}</a>', obj.file.url, obj.file.name)
    file_link.short_description = 'File'

admin.site.register(User, UserAdmin)
admin.site.register(FileUpload, FileUploadAdmin)