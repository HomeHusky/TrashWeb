import socket
import base64
import json
import os
import time
import threading
import io
from PIL import Image


send_str = ""
send_list =""

# Địa chỉ IP và cổng của server C#
TCP_IP = '192.168.1.19'
TCP_PORT = 5565

# Tạo socket và kết nối đến server C#
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((TCP_IP, TCP_PORT))

print("Bắt đầu....")
print("Đang nhận tín hiệu từ server...")

def sendstr():

    
    # Mã hóa ảnh thành chuỗi base64
    with open('img/recycle-symbol.png', 'rb') as f:
        image_data = base64.b64encode(f.read()).decode('utf-8')

    # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
    data = {
        'image': image_data,
        'string_data': '[1, 2, 3]'
    }
    json_data = json.dumps(data)

    # Gửi dữ liệu ảnh đến server
    # s.send(json_data.encode())
    string_data = [1, 2, 3]
    s.sendall(json_data.encode())
    print(json_data.encode())
    
def sendlist():
    print("sending")
    # Lấy danh sách tên file ảnh trong thư mục
    IMAGE_DIR = 'images'
    for file in os.listdir(IMAGE_DIR):
        if file.endswith(".jpg") or file.endswith(".png"):
            with open(os.path.join(IMAGE_DIR, file), "rb") as f:
                image_bytes = base64.b64encode(f.read())
                print(image_bytes)
                s.sendall(image_bytes)
    
def runClient():
    while True:
        data_recv = s.recv(1024)
        #print(data_recv)
        if len(data_recv):
            response = data_recv.decode()
            print('==================')
            print(response)
            if response=="sendstring":
                thread2 = threading.Thread(target=sendstr())
                thread2.start()
    
        time.thread_time
        

sendlist()
# thread1 = threading.Thread(target=sendlist())
# thread1.start()

