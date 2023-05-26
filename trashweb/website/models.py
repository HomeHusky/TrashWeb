from django.db import models
from django.conf import settings
from django.contrib.auth.models import User, AbstractUser
from django.db.models.fields import IntegerField
import uuid
import random


class Partner(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    Image = models.ImageField(default="default.jpg", upload_to="voucher_pics/")
    CreateAt = models.DateTimeField(auto_now_add=True, null=True)
    Name = models.CharField(max_length=200, null=True)
    Phone = models.CharField(max_length=200, null=True)
    Email = models.CharField(max_length=200, null=True)

    def __str__(self):
        return self.Name

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(Partner, self).save(*args, **kwargs)


class News(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    title = models.CharField(max_length=255)
    Create = models.DateTimeField(auto_now_add=True, null=True)
    body = models.TextField()

    def __str__(self):
        return self.title

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(News, self).save(*args, **kwargs)


class Voucher(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    title = models.CharField(max_length=255)
    AmountAvailable = models.IntegerField(null=True)
    Create = models.DateTimeField(auto_now_add=True, null=True)
    author = models.ForeignKey(Partner, on_delete=models.CASCADE)
    body = models.TextField()
    NeededPoint = models.FloatField(max_length=10, default="0")

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(Voucher, self).save(*args, **kwargs)

    def Needed_Point(self):
        return self.NeededPoint

    def __str__(self):
        return self.title + " | " + str(self.author)


class Customer(models.Model):
    User = models.OneToOneField(User, null=True, on_delete=models.CASCADE)
    id = models.CharField(max_length=9, primary_key=True, unique=True, editable=False)
    Image = models.ImageField(
        default="default.jpg", upload_to="avatars/", null=True, blank=True
    )
    Name = models.CharField(max_length=200, null=True)
    Phone = models.CharField(max_length=200, null=True)
    Email = models.CharField(max_length=200, null=True)
    is_email_verified = models.BooleanField(default=False)
    Twitter = models.URLField(max_length=200, null=True)
    Instagram = models.URLField(max_length=200, null=True)
    Facebook = models.URLField(max_length=200, null=True)
    point = models.FloatField(max_length=200, default=0)
    Address = models.CharField(max_length=200, null=True)
    DataCreated = models.DateTimeField(auto_now_add=True, null=True)
    auth_token = models.IntegerField(null=True, editable=False)

    def __str__(self):
        return str(self.Name)

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(1000, 9999)
            self.id = str(random_digits)
        super(Customer, self).save(*args, **kwargs)


class CustomerVouchers(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    Customer_id = models.ForeignKey(Customer, on_delete=models.CASCADE)
    Voucher_id = models.ForeignKey(Voucher, on_delete=models.CASCADE)
    Buy_time = models.DateTimeField(auto_now_add=True, null=True)
    GenerateCode = models.CharField(
        max_length=9, unique=True, editable=False, null=True
    )

    def __str__(self):
        return str(self.Customer_id) + "|" + str(self.Voucher_id.title)

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
            random_code = random.randint(1000000, 9999999)
            self.GenerateCode = self.Voucher_id.author.Name[:2].upper() + str(
                random_code
            )
        super(CustomerVouchers, self).save(*args, **kwargs)


class TrashDetail(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    recycle = models.IntegerField(null=True)
    dangerous = models.IntegerField(null=True)
    othergarbage = models.IntegerField(null=True)
    description = models.CharField(max_length=300, null=True)
    iduser = models.OneToOneField(Customer, null=True, on_delete=models.CASCADE)

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(TrashDetail, self).save(*args, **kwargs)

    def __str__(self):
        return str(self.id)


class TrashList(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    createat = models.DateTimeField(auto_now_add=True, null=True)
    numoftrash = models.IntegerField(
        "NumTrash", null=True
    )  # Number of types of garbage thrown
    totalscore = models.FloatField("Total Score", max_length=200, null=True)
    description = models.TextField(blank=True)
    iduser = models.ForeignKey(
        Customer, blank=True, null=True, on_delete=models.CASCADE
    )
    trash_detail = models.OneToOneField(
        TrashDetail, null=True, on_delete=models.CASCADE
    )

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(TrashList, self).save(*args, **kwargs)

    def __str__(self):
        return str(self.iduser.Name)

    def getTotalScore(self):
        return self.TotalScore


class TrashType(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    Name = models.CharField(max_length=10, null=True)
    Point = models.IntegerField(null=True)

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(TrashType, self).save(*args, **kwargs)

    def __str__(self):
        return str(self.Name)


class Comment(models.Model):
    id = IntegerField(primary_key=True, unique=True, editable=False)
    post = models.ForeignKey(Voucher, related_name="comments", on_delete=models.CASCADE)
    name = models.ForeignKey(
        Customer, max_length=255, null=True, blank=True, on_delete=models.CASCADE
    )
    body = models.TextField(null=False, blank=False)
    data_added = models.DateTimeField(auto_now_add=True)

    def save(self, *args, **kwargs):
        if not self.id:
            # generate user id with first two letters of username and 7 random digits
            random_digits = random.randint(0, 9999)
            self.id = str(random_digits)
        super(Comment, self).save(*args, **kwargs)

    def __str__(self):
        return "%s - %s" % (self.post.title, self.name)
