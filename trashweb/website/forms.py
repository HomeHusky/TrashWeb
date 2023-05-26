from django import forms
from django.forms import ModelForm
from .models import Customer, Comment
from django.contrib.auth.models import User
from django.contrib.auth.forms import PasswordChangeForm


# Create a Customer form
class CustomerForm(ModelForm):
    class Meta:
        model = Customer
        fields = ("Name", "Image", "Phone", "Email", "Address")
        labels = {
            "Name": "Enter your Name Here",
            "Image": "Choose your Image Here",
            "Phone": "Enter your Phone Number Here",
            "Email": "Enter your Email Here",
            "Address": "Enter your Address Here",
        }
        widgets = {
            "Name": forms.TextInput(
                attrs={"class": "form-control", "placeholder": "Full Name"}
            ),
            "Phone": forms.TextInput(
                attrs={"class": "form-control", "placeholder": " Phone Number"}
            ),
            "Email": forms.EmailInput(
                attrs={"class": "form-control", "placeholder": " Enter Your Email Here"}
            ),
            "Address": forms.TextInput(
                attrs={
                    "class": "form-control",
                    "placeholder": " Enter Your Address Here",
                }
            ),
        }
        error_messages = {
            "Name": {
                "required": "Name is required",
                "min_length": "Name must be at least 3 characters long",
            },
        }


class UpdateImageForm(forms.ModelForm):
    class Meta:
        model = Customer
        fields = ["Image"]


class PasswordChangingForm(PasswordChangeForm):
    old_password = forms.CharField(
        widget=forms.PasswordInput(
            attrs={
                "class": "form-control",
                "type": "password",
                "placeholder": "Old PassWord",
            }
        )
    )
    new_password1 = forms.CharField(
        max_length=100,
        widget=forms.PasswordInput(
            attrs={
                "class": "form-control",
                "type": "password",
                "placeholder": "New PassWord",
            }
        ),
    )
    new_password2 = forms.CharField(
        max_length=100,
        widget=forms.PasswordInput(
            attrs={
                "class": "form-control",
                "type": "password",
                "placeholder": "Confirm New PassWord",
            }
        ),
    )

    class Meta:
        model = User
        fields = (
            "old_password",
            "new_password1",
            "new_password2",
        )


class CommentForm(forms.ModelForm):
    class Meta:
        model = Comment
        fields = ("body",)

        widgets = {
            "body": forms.Textarea(attrs={"class": "form-control"}),
        }
