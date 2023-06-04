import socket
import time
import numpy as np
from keras.models import load_model
from ultralytics import YOLO
import cv2
import threading
import os 
import datetime
import shutil
import pickle
import struct
import json
import base64
from PIL import Image

TCP_IP = '172.20.46.128'
TCP_PORT = 5565

class ModelTrash:
    def __init__(self, model1, model2):
        self.data = {
                    'Carboard': 0,
                    'Dangerous': 0,
                    'Glass': 0,
                    'Metal': 0,
                    'Other Garbage': 0,
                    'Paper': 0,
                    'Plastic': 0
                }
        self.model1 = model1
        self.model2 = model2
        self.cam_number = 1
        self.folder_name = ''
        self.imgpath = ''
        self.create_folder = 0
        self.trash_drop = False
        self.stopServer = True
        self.number_trash = 0

    @staticmethod
    def reload_trash_list():
        return {
                'Carboard': 0,
                'Dangerous': 0,
                'Glass': 0,
                'Metal': 0,
                'Other Garbage': 0,
                'Paper': 0,
                'Plastic': 0
                }

    def update_trash_list(self, name): 
        self.data[name] += 1

    def get3value(self, list_temp):
        valueRecycle = list_temp[0] + list_temp[2] + list_temp[3] + list_temp[5] + list_temp[6]
        valueDangerous = list_temp[1]
        valueOther = list_temp[4]
        newlist = [valueRecycle, valueDangerous, valueOther]
        return newlist

    def get_values(self):
        list_temp = []
        for value in self.data.values():
            list_temp.append(value)
        return list_temp

    def predict_model_classifier(self, frame):
        # Make the image a numpy array and reshape it to the models input shape.
        image = cv2.resize(frame, (224, 224), interpolation=cv2.INTER_AREA)
        image = np.asarray(image, dtype=np.float32).reshape(1, 224, 224, 3)

        # Normalize the image array
        image = (image / 127.5) - 1

        # Predicts the model
        prediction = self.model1.predict(image, verbose=False)
        index = np.argmax(prediction)
        # class_name = self.class_names[index]
        # confidence_score = prediction[0][index]
        return index
    # @staticmethod
    # def detect_2s(frame, time):

    def predict_model_trash(self, frame):
        detect_params = self.model2.predict(source=frame, conf=0.7, save=False, verbose=False)
            # detect_params = model.predict(source=frame, conf=0.7, save=False)
        # Convert tensor array to numpy
        results = detect_params[0].cpu().numpy()
        boxes = detect_params[0].boxes
        return results, boxes
    
    # Định nghĩa hàm chạy vô hạn để cập nhật biến sau mỗi giây
    def run_predict(self):
        hostHuman = 'localhost'
        portHuman = 2345
        detect_time = time.time()
        previous_frame = None
        previous_time = time.time()
        # cap = cv2.VideoCapture(self.cam_number)
        cap = cv2.VideoCapture(2)

        Images_path = os.path.join(os.getcwd(), "Images")
        print("Đang nhận tín hiệu từ client Human...")
        while True:
            try:
                s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                s.bind((hostHuman, portHuman))
                s.listen()
                conn, addr = s.accept()
                data = conn.recv(1024)
            except Exception:
                print("No connection Human Client!")
                time.sleep(1)
                # Nếu k nhận được dữ liệu quay lại từ đầu!
                continue
            respone = data.decode()
            respone = respone.split(',')                
                    
            if respone[0] == 'True':
                self.stopServer = False
                self.create_folder += 1

            if respone[1] == 'True':
                self.create_folder = 0
                self.stopServer = True
                # shutil.rmtree(self.folder_name)
                print("Vượt quá thời gian, cập nhập danh sách rác!")
                print("Server Stop: ", self.stopServer)

            if self.stopServer == True:
                print("Không phát hiện người")
                continue
            
            # Xét biến create_folder = 1 mới tạo folder để chỉ tạo 1 folder trong vòng while true
            if self.create_folder == 1:
                print("Xác nhận đã có người")
                self.data = self.reload_trash_list()
                list_temp = self.get_values()
                list_send = self.get3value(list_temp)
                self.stopServer = False
                current_path_name = datetime.datetime.now().strftime(r"%d_%m-%H-%M-%S")
                current_folder_name = os.path.join(Images_path, current_path_name)
                try:
                    os.mkdir(current_folder_name)
                except:
                    pass
                self.folder_name = current_folder_name
                print("Bắt đầu")
                print(list_send)

            # Bật camera detect rác realtime
            ret, frame = cap.read()
            index = self.predict_model_classifier(frame)
            # index = 1
            # Có rác 
            if index == 1:
                print("Phát hiện vật")
                results, boxes = self.predict_model_trash(frame)
                len_results = len(results)
                        
                # Model 2 nhận diện được
                if len_results != 0:
                    end_time = time.time()
                    if end_time - detect_time > 3:
                        self.trash_drop = True

                    for i in range(len_results):
                        box = boxes[i]  # returns one box
                        clsID = box.cls.cpu().numpy()[0]
                        conf = box.conf.cpu().numpy()[0]
                        bb = box.xyxy.cpu().numpy()[0]
                        
                        cv2.rectangle(
                            frame,
                            (int(bb[0]), int(bb[1])),
                            (int(bb[2]), int(bb[3])),
                            (255,255,0),
                            3,
                        )
                        name = self.model2.names[int(clsID)]
                        # Display class name and confidence
                        font = cv2.FONT_HERSHEY_COMPLEX
                        cv2.putText(
                            frame,
                            name
                            + " "
                            + str(round(conf, 3))
                            + "%",
                            (int(bb[0]), int(bb[1]) - 10),
                            font,
                            1,
                            (0, 255, 255),
                            2,
                        )
                    # Có rác nhưng ko nhận diện đc bởi model 2
                else:
                    end_undetect_time = time.time()
                    if end_undetect_time - detect_time > 3:
                        name = "Other Garbage"
                        self.trash_drop = True
                current_time = time.time()
                if current_time - previous_time >= 3:
                    namepre = name
                    previous_frame = frame
                    previous_time = current_time

            elif index == 0 and self.trash_drop == True: 
                print("Không phát hiện thấy rác")
                if previous_frame is not None:
                    self.send_server(previous_frame, namepre)
                    self.trash_drop = False
                else:
                    continue
            else:
                detect_time = time.time()
                print("Không phát hiện thấy rác")
            try:
                # Display the resulting frame
                cv2.imshow(f"Detecting", frame)
                if cv2.waitKey(1) == ord('q'):
                    break
            except Exception:
                print("Chưa có server UI để gửi")
            # try:
            #     # Display the resulting frame
            #     cv2.imshow(f"check", new)
            # except Exception:
            #     print("Chưa có server UI để gửi")
            
        cap.release()
        cv2.destroyAllWindows()
            


    def send_server(self, imageDataAfter1Second, name):
        count = 0

        try:

            # Connect to the server
            s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            s.connect((TCP_IP, TCP_PORT))
            response = ''
            # Value receive from server
            response = s.recv(1024)
        
        except Exception:
            print("Không thể kết nối đến ServerUI!")
        
        if not response:
            print("Không nhận được response từ Server.")

        if response == b'Start':
            count = self.number_trash
            if self.folder_name != '':
                self.update_trash_list(name)
                self.number_trash += 1
                imageNameFile = os.path.join(self.folder_name, f'{count}_{name}.jpg')
                cv2.imwrite(imageNameFile, imageDataAfter1Second)
                recycle_trash = ['Cardboard', 'Glass', 'Metal', 'Paper', 'Plastic']
                if name in recycle_trash:
                    name = 'Recycle'
                print("Curren file image: ", imageNameFile)
                with open(imageNameFile, 'rb') as f:
                    image_data = base64.b64encode(f.read()).decode('utf-8')
                list_temp = self.get_values()
                list_send = self.get3value(list_temp)
                data = {
                    'image': image_data,
                    'string_data': str(list_send),
                    'type': str(name)
                        }
                json_data = json.dumps(data)
                print("Curren file image: ", imageNameFile)

                # list3type = self.get3value(list_temp)
                # self.trash_drop = False
                # Tạo file hình ảnh với mỗi vật
                try:
                    
                    s.sendall(json_data.encode())
                except ValueError:
                    print("Lỗi: Bạn phải nhập một số nguyên.")
                except ZeroDivisionError:
                    print("Lỗi: Không thể chia cho 0.")
                else:
                    print("Không có lỗi xảy ra.")
                    print("Đã send list: ", str(list_send))
                    print("Loại rác phát hiện: ", str(name))
        if response == b'Stop':
            self.data = self.reload_trash_list()
            s.close()


    

if __name__ == '__main__':
    # Load the model 1
    model1 = load_model("keras_Model.h5", compile=False)

    model2 = YOLO(model=r"best.pt")

    MoldelTrash2 = ModelTrash(model1, model2) # model yolo and classifier
    MoldelTrash2.run_predict()
    # Tạo ra các luồng xử lý
    # thread1 = threading.Thread(target=MoldelTrash2.run_predict)
    # thread2 = threading.Thread(target=MoldelTrash2.run_predict)

    # Khởi động các luồng xử lý
    # thread1.start()
    # thread2.start()
