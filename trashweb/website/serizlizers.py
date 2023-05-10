from rest_framework import serializers
from .models import Partner

class PartnerSerializer(serializers.ModelSerializer):
    class Meta:
        model = Partner
        fields = ('IdPartner', 'Image', 'CreateAt', 'Name', 'Phone', 'Email')
        