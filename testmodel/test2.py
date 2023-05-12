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

TCP_IP = '192.168.1.95'
TCP_PORT = 5565

# Connect to the server
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((TCP_IP, TCP_PORT))
start = time.time()
run = True

response = ''
while (run):
    if (time.time() - start)>=6:
        run = False

    # Mã hóa ảnh thành chuỗi base64
    with open('img/blog-1.jpg', 'rb') as f:
        image_data = base64.b64encode(f.read()).decode('utf-8')

    # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
    data = {
        'image': image_data,
        'string_data': '[12, 2, 3]'
    }
    json_data = json.dumps(data)

    # Gửi dữ liệu ảnh đến server
    # s.send(json_data.encode())
    # while(True):
        # Wait for the response from the server

    response = s.recv(1024)
    print(response.decode())
    # if response == b'Start':
    print("sending")
    print(json_data.count)
    s.sendall(json_data.encode())
        
        # elif response == b'Ready to receive data' :
        #      # Send a list of images to the server
        #     image_dir = 'images'
        #     image_files = [os.path.join(image_dir, f) for f in os.listdir(image_dir) if os.path.isfile(os.path.join(image_dir, f))]
        #     s.sendall(str(len(image_files)).encode())
        #     for image_file in image_files:
        #         with open(image_file, 'rb') as f:
        #             # Load the image using Pillow
        #             img = Image.open(io.BytesIO(f.read()))

        #             # Convert the image to byte array
        #             img_byte_arr = io.BytesIO()
        #             img.save(img_byte_arr, format='JPEG')
        #             img_byte_arr = img_byte_arr.getvalue()

        #             # Send the length of the image and the image data to the server
        #             s.sendall(str(len(img_byte_arr)).encode())
        #             s.sendall(img_byte_arr)
        # elif response != b'Stop' :
            # Close the connection
    if response == b'Stop':
        s.close()


