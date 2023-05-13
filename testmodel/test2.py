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

TCP_IP = '192.168.1.26'
TCP_PORT = 5565

run = True
start = time.time()
response = ''
while (run):
    # Connect to the server
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    
    # Value receive from server
    response = s.recv(1024)
    print(response.decode())

    end = time.time()
    print (end)
    if (end - start)>15:
        run = False
    
    # Mã hóa ảnh thành chuỗi base64
    with open('img/blog-1.jpg', 'rb') as f:
        image_data = base64.b64encode(f.read()).decode('utf-8')

    a = 1
    b = 2
    c = 3
    
    z = 0
    stringdata = [int(a),int(b),int(c)]

    count = 5
    while (z<=count):
        stringdata[0] = stringdata[0] + 1
        stringdata[1] = stringdata[1] + 1
        stringdata[2] = stringdata[2] + 1
        z = z + 1

    if response == b'Start':
        # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
        data = {
            'image': image_data,
            'string_data': str(stringdata)
        }
        json_data = json.dumps(data)
        s.sendall(json_data.encode())

    if response == b'Stop':
        s.close()

    print ("Thoi gian hien tai la:", end-start)
    print ("Bien stop: ", run)

    s.close()

    time.sleep(1)

