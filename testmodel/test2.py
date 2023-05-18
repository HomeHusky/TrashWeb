import os
import socket
import socket
import base64
import json
import os
import time
import threading
import io
from PIL import Image
import time

def get_local_ip():
    s = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
    s.connect(("8.8.8.8", 80))
    ip_address = s.getsockname()[0]
    s.close()
    if ip_address.startswith("192.168."):
        return ip_address
    return None


TCP_IP = get_local_ip()
TCP_PORT = 5565



run = True
start = time.time()
response = ''
a = 1
b = 2
c = 3
d = 'recycle'

stringdata = [int(a),int(b),int(c), d]
while (run):
    # Connect to the server
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    response = ''
    # Value receive from server
    response = s.recv(1024)
    print(response.decode())

    end = time.time()
    if (end - start)>20:
        run = False
    
    # Mã hóa ảnh thành chuỗi base64
    with open('img/blog-1.jpg', 'rb') as f:
        image_data = base64.b64encode(f.read()).decode('utf-8')

    if response == b'Start':
        # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
        data = {
            'image': image_data,
            'string_data': str(stringdata),
            'type': str(d)
        }
        json_data = json.dumps(data)
        s.sendall(json_data.encode())
        stringdata[0] = stringdata[0] + 1
        stringdata[1] = stringdata[1] + 1
        stringdata[2] = stringdata[2] + 1

    if response == b'Stop':
        s.close()
        break

    print ("Thoi gian hien tai la:", end-start)
    print ("Bien stop: ", run)

    s.close()

    time.sleep(1)

