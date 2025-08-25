from django.contrib import admin
from .models import User, FileUpload
from django.utils.html import format_html
from pathlib import Path

class FileInline(admin.TabularInline):
    model = FileUpload
    extra = 0

class UserAdmin(admin.ModelAdmin):
    list_display = ['phone']
    inlines = [FileInline]

class FileUploadAdmin(admin.ModelAdmin):
    list_display = ['user', 'uploaded_at', 'file_link']
    ordering = ['-uploaded_at']

    def get_queryset(self, request):
        queryset = super().get_queryset(request)
        valid_ids = []
        for file_obj in queryset:
            if Path(file_obj.file.path).exists():
                valid_ids.append(file_obj.id)
            else:
                # If the file is missing from disk, delete DB entry
                file_obj.delete()
        return queryset.filter(id__in=valid_ids)

    def file_link(self, obj):
        if Path(obj.file.path).exists():
            return format_html('<a href="{}" target="_blank">📄 {}</a>', obj.file.url, obj.file.name)
        else:
            return format_html('<span style="color:red;">❌ Missing File</span>')

    file_link.short_description = 'Download File'

admin.site.register(User, UserAdmin)
admin.site.register(FileUpload, FileUploadAdmin)
