import socket
import time
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



class ModelTrash:
    def __init__(self, model2 ,host, port):
        self.data = {
                    'Carboard': 0,
                    'Dangerous': 0,
                    'Glass': 0,
                    'Metal': 0,
                    'Other Garbage': 0,
                    'Paper': 0,
                    'Plastic': 0
                }
        # self.model1 = model1
        self.model2 = model2
        self.check_update_avaialbe = True
        self.check_person = False
        self.cam_number = 1
        self.check_cam_availabel = True
        self.host = host
        self.port = port
        self.folder_name = ''
        self.save_images = 0
        self.create_folder = 0
        self.trash_drop = False

        # Mã hóa ảnh thành chuỗi base64
        with open('img/recycle-symbol.png', 'rb') as f:
            image_data = base64.b64encode(f.read()).decode('utf-8')
        
        # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
        list_temp = self.get_values()
        data = {
            'image': image_data,
            'string_data': list_temp
        }
        self.json_data = json.dumps(data)

    @staticmethod
    def reload_trash_list():
        """
        The function returns a dictionary with keys representing different types of trash and values
        initialized to 0.
        :return: A dictionary with keys representing different types of trash and values initialized to
        0.
        """
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
        """
        This function updates the count of a specific item in a trash list.
        
        :param name: The name of the item that needs to be updated in the trash list
        """
        self.data[name] += 1

    def get_values(self):
        """
        This function returns a list of all the values in a dictionary.
        :return: The function `get_values` is returning a list of all the values in the dictionary
        `self.data`.
        """
        list_temp = []
        for value in self.data.values():
            list_temp.append(value)
        return list_temp

    def predict_model_classifier(self, frame):
        
        pass
    def predict_model_trash(self, frame):
        """
        This function takes a frame as input, uses a pre-trained model to predict object detection
        parameters with a confidence threshold of 0.7, converts the results to a numpy array, and
        returns the results and corresponding bounding boxes.
        
        :param frame: The input frame or image on which the object detection model will make predictions
        :return: two values: `results` and `boxes`. `results` is a numpy array containing the detection
        results, and `boxes` is a tensor array containing the bounding boxes of the detected objects.
        """
        detect_params = self.model2.predict(source=frame, conf=0.7, save=False, verbose=False)
            # detect_params = model.predict(source=frame, conf=0.7, save=False)
        # Convert tensor array to numpy
        results = detect_params[0].cpu().numpy()
        boxes = detect_params[0].boxes
        return results, boxes

    def run_predict(self):
        """
        This function captures frames from a video stream, performs object detection on each frame, and
        displays the results in a window while also saving images of detected objects if specified.
        """
        detect_time = time.time()
        cap = cv2.VideoCapture(self.cam_number)
        count = 0
        while True:
            # Capture frame-by-frame
            ret, frame = cap.read()
            # Predict on image
            # if id == 0:
            results, boxes = self.predict_model_trash(frame)
            # else:
            #     results, boxes = predict_model2(frame)
            len_results = len(results)
            if len_results != 0:
                if self.trash_drop == False:
                    detect_time = time.time()
                # check = "Đúng"
                for i in range(len_results):
                    box = boxes[i]  # returns one box
                    clsID = box.cls.cpu().numpy()[0]
                    conf = box.conf.cpu().numpy()[0]
                    bb = box.xyxy.cpu().numpy()[0]
                    
                    #     check = False 
                    # else:
                    #     check = True
                    cv2.rectangle(
                        frame,
                        (int(bb[0]), int(bb[1])),
                        (int(bb[2]), int(bb[3])),
                        (255,255,0),
                        3,
                    )

                    # Display class name and confidence
                    font = cv2.FONT_HERSHEY_COMPLEX
                    cv2.putText(
                        frame,
                        self.model2.names[int(clsID)]
                        + " "
                        + str(round(conf, 3))
                        + "%",
                        (int(bb[0]), int(bb[1]) - 10),
                        font,
                        1,
                        (0, 255, 255),
                        2,
                    )
                    if self.check_update_avaialbe == False:
                        self.data = self.reload_trash_list()
                    else:
                        if (time.time() - detect_time > 0.75) and (self.check_person == True):
                            # if self.check_person == True: 
                            name = self.model2.names[int(clsID)]
                        
                            if self.folder_name != '':
                                if self.save_images == 0 and self.trash_drop == True:
                                    self.update_trash_list(name)
                                    count += 1
                                    # Tạo file hình ảnh với mỗi vật
                                    cv2.imwrite(os.path.join(self.folder_name, f'{name}_{count}.jpg'), frame)
                                    # if lenself.save_images = 1
                                    self.trash_drop = False
                                # elif self.save_images == 1:
                                #     # Chuyển đổi frame thành định dạng bytes
                                #     frame_data = pickle.dumps(frame)
                                #     frame_size = struct.pack("I", len(frame_data))
                                #     print(frame_data, frame_size)
                                #     self.data_2_server(frame_size + frame_data)
            else:                
                if (time.time() - detect_time) > 0.75:
                    self.trash_drop = True
            # Display the resulting frame
            cv2.imshow(f"check", frame)
            # Terminate run when "Q" pressed
            if cv2.waitKey(1) == ord('q'):
                self.check_cam_availabel = False
                break
        cap.release()
        cv2.destroyAllWindows()

    # Định nghĩa hàm chạy vô hạn để cập nhật biến sau mỗi giây
    def start_server(self):
        count = int(0)
        folder_path = os.path.join(os.getcwd(), "Images")
        print("Bắt đầu....")
        print("Đang nhận tín hiệu từ client...")
        while True:
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                if self.check_cam_availabel != False:
                    s.bind((self.host, self.port))
                    s.listen()
                    conn, addr = s.accept()
                    while True:
                        data = conn.recv(1024)
                        if not data:
                            break
                        self.check_person = True if data.decode() == 'True' else False
                        
                        if self.check_person == False:
                            count += 1
                            if count == 3:
                                self.create_folder = 0
                                self.check_update_avaialbe = False 
                                # shutil.rmtree(self.folder_name)
                                print("Vượt quá thời gian")
                            
                        else:
                            self.create_folder += 1
                            if self.create_folder == 1:
                                self.folder_name = datetime.datetime.now().strftime(r"%d_%H-%M-%S")
                                folder_name = os.path.join(folder_path, self.folder_name)
                                os.mkdir(folder_name)
                                self.folder_name = folder_name
                                
                            count = 0 
                            self.check_update_avaialbe = True
                        
                        print("=======================================")
                        print("Person available: ", self.check_person)
                        print("Time person unavaiable:", count)
                        print("Update Data available: ",self.check_update_avaialbe)
                        print("Check Trash Droped:", self.trash_drop)
                        print(self.data)
                        print("Folder: ",self.folder_name)
                        if self.folder_name != '':
                            # Mã hóa ảnh thành chuỗi base64
                            with open(folder_name, 'rb') as f:
                                image_data = base64.b64encode(f.read()).decode('utf-8')
                            
                            # Đóng gói ảnh và chuỗi string vào một đối tượng JSON
                            list_temp = self.get_values()
                            data = {
                                'image': image_data,
                                'string_data': list_temp
                            }
                            self.json_data = json.dumps(data)
                        print(self.json_data)
                        #list_temp = self.get_values()
                        self.data_2_server(self.json_data)
                        time.sleep(1)
                else:
                    break

    def data_2_server(self, data):
        # send data to next server
        host2 = '192.168.1.19'
        port2 = 5565
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((host2, port2))
            s.sendall(self.json_data.encode())
            data_recv = s.recv(1024)
            if len(data_recv):
                response = data_recv.decode()
                if response == "Report":
                    self.save_images = 1
                elif response == "Ok":
                    self.save_images = 2
                elif response == "Delete":    
                    shutil.rmtree(self.folder_name)
                    self.create_folder = 0
                    self.data = self.reload_trash_list()
                else:
                    self.save_images = 0
                print(data_recv, "\n", self.save_images,"\n=========\n")
     

# if __name__ == "_main_":

host = 'localhost'
port = 2345
check_update_avaialbe = False

model = YOLO(model=r"best.pt")
MoldelTrash2 = ModelTrash(model, host, port) # model yolo and classifier

# Tạo ra các luồng xử lý
thread1 = threading.Thread(target=MoldelTrash2.run_predict)
thread2 = threading.Thread(target=MoldelTrash2.start_server)

# Khởi động các luồng xử lý
thread1.start()
thread2.start()