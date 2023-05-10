from django.contrib import admin


from website.models import Partner, Customer, TrashDetail, TrashList, Voucher, Comment, News, TrashType, CustomerVouchers
# Register your models here.

admin.site.register(Partner)
admin.site.register(Customer)
admin.site.register(TrashDetail)
admin.site.register(TrashList)
admin.site.register(Voucher)
admin.site.register(Comment)
admin.site.register(News)
admin.site.register(TrashType)
admin.site.register(CustomerVouchers)



