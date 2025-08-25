from django.contrib import admin
from .models import User, Slot, Appointment

#doctor-s
@admin.register(User)
class UserAdmin(admin.ModelAdmin):
    list_display = ('username', 'email', 'role')
    list_filter = ('role',)
    search_fields = ('username', 'email')

# Show Slot inffo 
@admin.register(Slot)
class SlotAdmin(admin.ModelAdmin):
    list_display = ('doctor', 'start_time', 'end_time', 'is_booked')
    list_filter = ('doctor', 'is_booked')
    search_fields = ('doctor__username',)

#  Appointment  informacion
@admin.register(Appointment)
class AppointmentAdmin(admin.ModelAdmin):
    list_display = ('get_doctor', 'patient', 'slot', 'status')
    list_filter = ('status',)
    search_fields = ('patient__username', 'slot__doctor__username')

    def get_doctor(self, obj):
        return obj.slot.doctor.username
    get_doctor.short_description = 'Doctor'
