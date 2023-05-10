from django.shortcuts import render, redirect, get_object_or_404
from django.views.generic import ListView, DetailView, CreateView
from django.views import generic
from django.contrib.sessions.models import Session
from .thread import runYolov8Thread, runAnotherCam
from django.contrib import messages
from django.contrib.auth.models import User
from django.contrib.auth.views import PasswordChangeView
from django.contrib.auth.forms import PasswordChangeForm
from .models import Customer, Partner, TrashDetail, TrashList, Voucher, Comment, News, CustomerVouchers
from .forms import CustomerForm, PasswordChangingForm, CommentForm
from django.contrib.auth import authenticate,login,logout
from django.http import HttpResponseRedirect, HttpResponse
from django.urls import reverse_lazy, reverse
from django.views.decorators import gzip
from django.http import StreamingHttpResponse
from .camera import VideoCamera, gen_yolo
from django.http import JsonResponse
from django.contrib.sites.shortcuts import get_current_site
from django.template.loader import render_to_string
from django.utils.http import urlsafe_base64_encode, urlsafe_base64_decode
# from django.utils.encoding import force_bytes, force_str, force_text, DjangoUnicodeDecodeError
from .utils import generate_token
import uuid
from mypage import settings
from django.core.mail import send_mail
from qrcode import *

# from .camera import FacialLandMarksPosition, predict_eye_state

# Create your views here.

# def send_action_email(user, request):
#     current_site = get_current_site(request)
#     email_subject = 'Activate your account!'
#     emai_body = render_to_string('website/activate.html',{
#         'user': user,
#         'domain': current_site,
#         'uid': urlsafe_base64_encode(force_bytes(user.pk)),
#         'token': generate_token.make_token(user),
#     })

data = None

class NewsView(ListView):
    model = News
    template_name = 'trashweb/news.html'

class VoucherView(ListView):
    model = Voucher
    template_name = 'trashweb/voucher.html'

def UserView(request):
    trashlists = TrashList.objects.filter(iduser_id = request.user.customer.id)
    customerVouchers = CustomerVouchers.objects.filter(Customer_id_id = request.user.customer.id)
    partnerName = Partner.objects.all()
    context = {
        'trashlists': trashlists,
        'customerVouchers' : customerVouchers,
        'partnerName' : partnerName,
    }
    return render(request, 'trashweb/user.html', context)

class TrashDetailView(DetailView):
    model = TrashDetail
    template_name = 'trashweb/trash_detail.html'

def getValue(request):
    current_customer = request.user.customer #admin
    current_customer_id = current_customer.id #1
    querysetCustomer = Customer.objects.get(id=current_customer_id)   
    current_point = querysetCustomer.point #0.0
    print(current_point)
    return HttpResponse(current_point)

class VoucherDetailView(DetailView):
    model = Voucher
    template_name = 'trashweb/voucher_detail.html'

class PasswordsChangeView(PasswordChangeView):
    form_class = PasswordChangingForm
    template_name = 'trashweb/changepassword.html'
    success_url = reverse_lazy('index')

class AddCommentView(CreateView):
    model = Comment
    form_class = CommentForm
    template_name = 'trashweb/add_comment.html'
    # fields = '__all__'

    success_url = reverse_lazy('voucher')
    
    def form_valid(self, form):
        form.instance.post_id = self.kwargs['pk']
        return super().form_valid(form)


def index(request):
    num_partner = Partner.objects.count()
    num_custommer = Customer.objects.count()
    context = {
        'num_partner': num_partner,
        'num_custommer' : num_custommer,
    }
    # current_customer_id = request.user.customer.id
    # querysetCustomer = Customer.objects.get(id=current_customer_id)
    # print(querysetCustomer.auth_token)

    return render(request, "trashweb/index.html", context)

def register(request):
    if request.method=='POST':
        uname=request.POST.get('username')
        email=request.POST.get('email')
        pass1=request.POST.get('password1')
        pass2=request.POST.get('password2')

        try:
            if uname=='':
                messages.add_message(request, messages.ERROR, "Vui Lòng Nhập Tên Đăng Nhập!")

            elif email=='':
                messages.add_message(request, messages.ERROR, "Vui Lòng Nhập Emai!")

            elif User.objects.filter(email = email).first():
                messages.add_message(request, messages.ERROR, "Email không chính xác!")

            elif pass1=='':
                messages.add_message(request, messages.ERROR, "Vui Lòng Nhập Mật Khẩu!")

            elif len(pass1) < 8:
                messages.add_message(request, messages.ERROR, "Vui Lòng Nhập Trên 8 Kí Tự!")
            
            elif pass1!=pass2:
                messages.add_message(request, messages.ERROR, "Mật Khẩu Không Trùng Khớp!")
            
            elif User.objects.filter(username=uname).exists():
                messages.add_message(request, messages.ERROR, "Tên Đăng Nhập Đã Được Sử Dụng!")

            else:
                my_user=User.objects.create_user(uname,email,pass1)
                my_user.save()
                
                my_customer = Customer.objects.create(User=my_user,Name=uname, Email=email)
                messages.success(request, 'Sign Up successfully!')
            return redirect('login1')
        except Exception as e:
            print(e)
    return render (request,'website/register.html')

def token(request):
    current_customer_id = request.user.customer.id
    querysetCustomer = Customer.objects.get(id=current_customer_id)
    token = str(uuid.uuid4())
    querysetCustomer.auth_token = token
    querysetCustomer.save()
    email = querysetCustomer.Email
    print(token, email)

    send_mail_verify(email, token)

    return render(request, "website/token.html")

def success(request):
    return render(request, "website/success.html")

def send_mail_verify(email, token):
    subject = "Your account needs to obe verify!"
    message = f'Hi paste the link to verify your account http://127.0.0:1800/trashweb/{token}'
    from_email = settings.EMAIL_HOST
    recipient_list = [email]
    send_mail(subject, message, from_email, recipient_list)

def loginPage(request):
    if request.method=='POST':
        username=request.POST.get('username')
        pass1=request.POST.get('pass')
        user=authenticate(request,username=username,password=pass1)
        if user is not None:
            login(request,user)
            current_user = request.user
            current_id = current_user.id
            print(current_id)
            return redirect('index')
        else:
            messages.success(request, "Tên Đăng Nhập hoặc Mật Khẩu không chính xác!!!")
            return redirect('login1')
    return render (request,'website/login.html')

def logout_user(request):
    logout(request)
    return redirect("login1")


def buy_voucher(request, pk):
    voucher_id = get_object_or_404(Voucher, id=request.POST.get('voucher_id'))
    voucher_point = voucher_id.NeededPoint
    print(voucher_point)
    current_customer = request.user.customer
    current_customer_id = request.user.customer.id
    current_customer_point = request.user.customer.point
    
    print(current_customer_point)
    voucher_selected = Voucher.objects.get(id=voucher_id.id)
    if(voucher_selected.AmountAvailable==0):
        messages.error(request, "Voucher này đã được bán hết! Xin lỗi vì sự thiếu sót này. Chúng tôi sẽ thông báo cho bạn khi có voucher. Cảm ơn vì đã chọn chúng tôi")
    else:
        querycreateCustomerVouchers = CustomerVouchers.objects.create(Customer_id = current_customer, Voucher_id=voucher_id)
        voucher_selected.AmountAvailable = voucher_selected.AmountAvailable - 1
        voucher_selected.save()
        querysetCustomer = Customer.objects.get(id=current_customer_id)
        querysetCustomer.point = current_customer_point - voucher_point
        querysetCustomer.save()
        messages.success(request, "Đổi Voucher Thành Công!")
    return HttpResponseRedirect(reverse('voucher_detail', args=[str(pk)]))

def buy_voucher_fail(request, pk):
    messages.error(request, "Bạn Không Đủ Điểm!!!")
    return HttpResponseRedirect(reverse('voucher_detail', args=[str(pk)]))

def about(request):
    return render(request, "trashweb/about.html")

def service(request):
    return render(request, "trashweb/service.html")

def team(request):
    return render(request, "trashweb/team.html")    

def contact(request):
    if request.method == "POST":
        message_name = request.POST['message-name']
        messages_email = request.POST['message-email']
        message = request.POST['message']

        send_mail(
            message_name, #subject
            message, #message
            messages_email, #from email
            ['vitrannhat1@gmail.com'] #To Email
        )
        
        return render(request, "trashweb/contact.html", {'message_name': message_name})
    return render(request, "trashweb/contact.html")

def feature(request):
    return render(request, "trashweb/feature.html")

def testimonial(request):
    return render(request, "trashweb/testimonial.html")

def ajax(request):
    return render(request, "website/ajax.html")

def gen_Yolov8(request):
    runYolov8Thread().start()
    return render(request, "trashweb/detecting.html")

def run_another_cam(request):
    runAnotherCam().start()
    return render(request, "trashweb/index.html")

def update_profile(request):
    current_customer = request.user.customer #admin
    current_customer_id = current_customer.id #1
    querysetCustomer = Customer.objects.get(id=current_customer_id)
    form = CustomerForm(request.POST or None, instance=querysetCustomer)
    if form.is_valid():
        form.save()
        return redirect('user')
    context = {'form': form,
    'customer': querysetCustomer}
    return render(request, "trashweb/update.html", context)

def password_success(request):
    return render(request, "website/password_success.html")

def add_profile(request):
    submitted = False
    if request.method == "POST":
        form = CustomerForm(request.POST, request.FILES)
        if form.is_valid():
            form.save()
            return HttpResponseRedirect('/add_profile?submitted=True')
    else:
        form = CustomerForm
        if 'submitted' in request.GET:
            submitted=True
    #CustomerForm
    current_customer = request.user.customer #admin
    current_customer_id = current_customer.id #1
    context = {"form":form,
    'submitted':submitted}
    return render(request, "website/editprofile.html", context)


def user(request):

    #UserDetail
    current_user = request.user #admin
    current_user_id = current_user.id #1
    current_customer = request.user.customer #admin
    current_name = current_customer.Name #Tran Nhat Vi
    current_customer_id = current_customer.id #1

    #CustomerDetail
    querysetCustomer = Customer.objects.get(id=current_customer_id)
    current_email = querysetCustomer.Email #vitrannhat1@gmail.com
    current_phone = querysetCustomer.Phone #0919562182
    current_image = querysetCustomer.Image.url    
    current_point = querysetCustomer.point #0.0
    current_address = querysetCustomer.Address #An khe, Thanh Khe, Da Nang
    current_facebook = querysetCustomer.Facebook
    current_insta = querysetCustomer.Instagram
    current_twitter = querysetCustomer.Twitter

    #TrashDetail
    querysetTrash = TrashList.objects.filter(id=current_customer_id)
    current_score = querysetTrash.values('TotalScore')
    count = 0
    for item in current_score:
        count += item["TotalScore"]
        # print(type(item["TotalScore"])) #float
    print(count) #110.0

    #CurrentUserList
    current_list = querysetTrash.all()
    list_time = current_list.values_list()
    print(list_time) # CreateAt/NumOfTrash/TotalScore/Description
    #SetCurrentUserPoint
    current_point = count
    querysetCustomer.save()
    print("Nguoi dung hien tai la: ", current_name)
    print(current_point)
    

    
    context = {"current_user": current_user, 
    "current_name": current_name, 
    "current_email": current_email,
    "current_phone": current_phone,
    "current_image": current_image,
    "current_point": current_point,
    "current_address": current_address,
    "current_list": current_list,
    "list_time": list_time[:][1:5],
    "current_facebook": current_facebook,
    "current_insta": current_insta,
    "current_twitter": current_twitter,
    }
    return render(request, "trashweb/user.html", context)

@gzip.gzip_page
def camera(request):
    try:
        cam = VideoCamera()
        return StreamingHttpResponse(gen_yolo(cam), content_type="multipart/x-mixed-replace;boundary=frame")
    except:  # This is bad! replace it with proper handling
        pass

def detecting(request):
    return render(request, "trashweb/detecting.html")

def getdata(request):
    current_user = request.user #admin
    current_user_id = current_user.id #1
    current_customer = request.user.customer #admin
    current_name = current_customer.Name #Tran Nhat Vi
    current_customer_id = current_customer.id #1
    queryset = Customer.objects.get(id=current_customer_id)
    queryget = Customer.objects.all()
    context = {
        "queryset": queryset,
    }
    return JsonResponse({"vars": list(queryget.values())})

def qr(request):
    global data
    if request.method == "POST":
        data = request.POST['data']

        img = make(data)
        img.save('website/static/images/test.png')
    else:
        pass
    return render(request, 'trashweb/qr.html', {"data": data})