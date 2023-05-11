import socket
import base64
import json
import os
import time
import threading

send_str = ""
send_list =""

# Địa chỉ IP và cổng của server C#
TCP_IP = '192.168.100.29'
TCP_PORT = 5565

# Tạo socket và kết nối đến server C#
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((TCP_IP, TCP_PORT))

print("Bắt đầu....")
print("Đang nhận tín hiệu từ server...")

def sendstr():

    
    # Mã hóa ảnh thành chuỗi base64
    with open('img/team-31.jpg', 'rb') as f:
        image_data = base64.b64encode(f.read()).decode('utf-8')

    # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
    data = {
        'image': image_data,
        'string_data': '[1, 2, 3]'
    }
    json_data = json.dumps(data)

    # Gửi dữ liệu ảnh đến server
    # s.send(json_data.encode())
    string_data = '[1, 2, 3]'
    s.sendall(json_data.encode())
    print(json_data.encode())
    
def sendlist(response):
    if response == "sendlist":
        # Lấy danh sách tên file ảnh trong thư mục
        image_folder = 'img'
        image_files = [f for f in os.listdir(image_folder) if f.endswith('.jpg') or f.endswith('.png')]

        # Gửi danh sách tên file ảnh đến server
        message = '\n'.join(image_files)
        s.sendall(message.encode())
    else: pass

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
        

thread1 = threading.Thread(target=runClient())
thread1.start()

