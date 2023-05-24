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
    def __init__(self):
        self.data = {
            "Carboard": 0,
            "Dangerous": 0,
            "Glass": 0,
            "Metal": 0,
            "Other Garbage": 0,
            "Paper": 0,
            "Plastic": 0,
        }
        # self.model1 = model1
        self.model2 = YOLO(model=r"best.pt")
        self.cam_number = 1
        self.folder_name = ""
        self.imgpath = ""
        self.create_folder = 0
        self.trash_drop = False
        self.stopServer = True

    @staticmethod
    def reload_trash_list():
        return {
            "Carboard": 0,
            "Dangerous": 0,
            "Glass": 0,
            "Metal": 0,
            "Other Garbage": 0,
            "Paper": 0,
            "Plastic": 0,
        }

    def update_trash_list(self, name):
        self.data[name] += 1

    def get3value(self, list_temp):
        valueRecycle = (
            list_temp[0] + list_temp[2] + list_temp[3] + list_temp[5] + list_temp[6]
        )
        valueDangerous = list_temp[1]
        valueOther = list_temp[4]
        newlist = [valueRecycle, valueDangerous, valueOther]
        return newlist

    def get_values(self):
        list_temp = []
        for value in self.data.values():
            list_temp.append(value)
        return list_temp

    def get_type(self):
        type = "Recycle"

    def predict_model_classifier(self, frame):
        pass

    def predict_model_trash(self, frame):
        detect_params = self.model2.predict(
            source=frame, conf=0.7, save=False, verbose=False
        )
        # detect_params = model.predict(source=frame, conf=0.7, save=False)
        # Convert tensor array to numpy
        results = detect_params[0].cpu().numpy()
        boxes = detect_params[0].boxes
        return results, boxes

    # Định nghĩa hàm chạy vô hạn để cập nhật biến sau mỗi giây
    def run_predict(self):
        count = int(0)
        hostHuman = "localhost"
        portHuman = 2345
        detect_time = time.time()
        undetect_time = time.time()
        cap = cv2.VideoCapture(self.cam_number)
        count = 0
        time_out = 10
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
                # Nếu k nhận được dữ liệu quay lại từ đầu!
                continue

            if data.decode() == "False":
                count += 1

            elif data.decode() == "True":
                self.stopServer = False
                self.create_folder += 1
                count = 0

            if count == 3:
                self.create_folder = 0
                self.stopServer = True
                # shutil.rmtree(self.folder_name)
                print("Vượt quá thời gian")
                print("Server Stop: ", self.stopServer)

            if self.stopServer == True:
                print("Không phát hiện người")
                continue

            # Xét biến create_folder = 1 mới tạo folder để chỉ tạo 1 folder trong vòng while true
            if self.create_folder == 1:
                print("Xác nhận đã có người")

                self.stopServer = False
                current_path_name = datetime.datetime.now().strftime(r"%d_%H-%M-%S")
                current_folder_name = os.path.join(Images_path, current_path_name)
                os.mkdir(current_folder_name)
                self.folder_name = current_folder_name
                print("Server Started")

            # Bật camera detect rác realtime
            ret, frame = cap.read()
            results, boxes = self.predict_model_trash(frame)

            len_results = len(results)

            # Terminate run when "Q" pressed
            if cv2.waitKey(1) == ord("q"):
                break

            if len_results == 0:
                end_undetect_time = time.time()
                if end_undetect_time - undetect_time > 2:
                    detect_time = time.time()

            if len_results != 0:
                end_time = time.time()
                # Tạo ảnh trước khi lưu dữ liệu, cụ thể là sau khi detect được 1 giây
                if end_time - detect_time > 1:
                    undetect_time = time.time()
                    imageDataAfter1Second = frame
                # Tạo biến cho phép cập nhật data(cho phép vứt rác)
                if end_time - detect_time > 2:
                    self.trash_drop = True
                # Vẽ khung ảnh
                for i in range(len_results):
                    box = boxes[i]  # returns one box
                    clsID = box.cls.cpu().numpy()[0]
                    conf = box.conf.cpu().numpy()[0]
                    bb = box.xyxy.cpu().numpy()[0]

                    cv2.rectangle(
                        frame,
                        (int(bb[0]), int(bb[1])),
                        (int(bb[2]), int(bb[3])),
                        (255, 255, 0),
                        3,
                    )

                    # Display class name and confidence
                    font = cv2.FONT_HERSHEY_COMPLEX
                    cv2.putText(
                        frame,
                        self.model2.names[int(clsID)] + " " + str(round(conf, 3)) + "%",
                        (int(bb[0]), int(bb[1]) - 10),
                        font,
                        1,
                        (0, 255, 255),
                        2,
                    )
            try:
                # Display the resulting frame
                cv2.imshow(f"check", frame)
            except Exception:
                print("Chưa có server UI để gửi")
                continue
            if len_results == 0 and self.trash_drop == True:
                self.send_server(imageDataAfter1Second, clsID)
                self.trash_drop = False

        cap.release()
        cv2.destroyAllWindows()

    def send_server(self, imageDataAfter1Second, clsID):
        TCP_IP = "192.168.1.247"
        TCP_PORT = 5565
        count = 0

        try:
            # Connect to the server
            s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            s.connect((TCP_IP, TCP_PORT))
            response = ""
            # Value receive from server
            response = s.recv(1024)

        except Exception:
            print("Không thể kết nối đến ServerUI!")

        if not response:
            print("Không nhận được response từ Server.")

        if response == b"Start":
            if self.folder_name != "":
                name = self.model2.names[int(clsID)]
                self.update_trash_list(name)
                count += 1
                imageNameFile = os.path.join(self.folder_name, f"{name}_{count}.jpg")
                cv2.imwrite(imageNameFile, imageDataAfter1Second)
                if (
                    name == "Carboard"
                    or name == "Glass"
                    or name == "Metal"
                    or name == "Paper"
                    or name == "Plastic"
                ):
                    name = "Recycle"

                print("Curren file image: ", imageNameFile)
                with open(imageNameFile, "rb") as f:
                    image_data = base64.b64encode(f.read()).decode("utf-8")

                # self.data = self.get3type()
                list_temp = self.get_values()
                list_send = self.get3value(list_temp)
                # list3type = self.get3value(list_temp)
                self.trash_drop = False
                data = {
                    "image": image_data,
                    "string_data": str(list_send),
                    "type": str(name),
                }
                json_data = json.dumps(data)
                # Tạo file hình ảnh với mỗi vật
                try:
                    s.sendall(json_data.encode())
                except ValueError:
                    print("Lỗi: Bạn phải nhập một số nguyên.")
                except ZeroDivisionError:
                    print("Lỗi: Không thể chia cho 0.")
                else:
                    print("Không có lỗi xảy ra.")
                    print("Đã send list: ", str(list_temp))
                    print("Loại rác phát hiện: ", str(name))

        if response == b"Stop":
            self.data = self.reload_trash_list()
            print("Server stopped!")
            s.close()


MoldelTrash2 = ModelTrash()
MoldelTrash2.run_predict()
