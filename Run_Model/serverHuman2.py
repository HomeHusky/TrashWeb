import socket
from ultralytics import YOLO
import cv2
from ultralytics.yolo.utils.plotting import Annotator
import time
import threading
import os
# Take the time when people start taking out the trash
HOST = 'localhost'
PORT = 2345
class ModelPerson():
    def __init__(self, model):
        self.model = model
        self.status = False

    def checkPerson(self, xyxy):
        x1,y1,x2,y2 = xyxy
        if int(y1)<=150:
            if int(x2-x1)>=300 or int(y2-y1)>=240: # width>=300 or height>=240 (of the box)
                return True
        return False

    # Get the box with the largest area
    def get_largest_area_box(self, boxes):
        areas = []
        for box in boxes:
            x1,y1,x2,y2 = box
            areas.append((x2-x1) * (y2-y1)) #calculate the area of the box

        return boxes[areas.index(max(areas))]
    @staticmethod
    def update_global_variable(status):
        # send data to next server
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
            s.connect((HOST, PORT))
            s.sendall(status.encode())

        # time.sleep(0.5)

    def webcam(self):
        
        cap = cv2.VideoCapture(0)
        _, frame = cap.read()
        status_time = time.time()
        goin = 0
        goout = 0
        color = (0,0,255)
        path = os.getcwd()
        folder_path = f'{path}/Images'
        previous_count = len([name for name in os.listdir(folder_path) if os.path.isdir(os.path.join(folder_path, name))])
        previous_folders = []
        over_time = False
        while True:
            _, frame = cap.read()
            frame = cv2.flip(frame, 1)
            img = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
            results = self.model.predict(img, verbose=False, classes = 0) # get prediction
            current_folders = [name for name in os.listdir(folder_path) if os.path.isdir(os.path.join(folder_path, name))]
            # print(current_folders)
            if len(current_folders) > previous_count:
                current_folders = [name for name in os.listdir(folder_path) if os.path.isdir(os.path.join(folder_path, name))]
                cv2.imwrite(f'{folder_path}/{current_folders[-1]}/person_face.jpg', frame)
                previous_count = len(current_folders)
            person_boxes = []
            # processing
            for r in results:
                annotator = Annotator(frame)
                boxes = r.boxes
                for box in boxes:
                    c = box.cls
                    b = box.xyxy[0].tolist()  # get box coordinates in (top, left, bottom, right) format
                    person_boxes.append(b)
                    annotator.box_label(b, self.model.names[int(c)])
            if person_boxes:
                largest_box = self.get_largest_area_box(person_boxes)
                check = self.checkPerson(largest_box)

            # Check for someone standing near the webcam or not
            if check:
                goin+=1
                goout=0
                status_time = time.time()
                
            else:
                goout+=1

            if goin>=10:
                self.status = True
                color = (0,255,0)
            if goout>=10:
                goin=0
                self.status = False
                color = (0,0,255)
            # Put text to show the status
            cv2.putText(frame,str(self.status),(20,20),cv2.FONT_HERSHEY_SIMPLEX,1,color,2,cv2.LINE_AA)
            current_time = time.time()
            if float(current_time - status_time) >= 4:
                over_time = True
            else:
                over_time = False
            message = f'{self.status},{over_time}'
            self.update_global_variable(message)
            # save = int(save_check)
            # if save == 1:
            #     cv2.imwrite(f'{path}_face.jpg', img)
            print(self.status)
            print(goin, goout)
            # Draw box into frame
            frame = annotator.result()  
            # Show frame
            cv2.imshow('YOLO V8 Detection', frame)     
            if cv2.waitKey(1) & 0xFF == ord('q'):
                break

        cap.release()
        cv2.destroyAllWindows()

if __name__ == "__main__":
    model = YOLO('yolov8s.pt')
    modelPerson = ModelPerson(model)
    # modelPerson.webcam()
    # thread1 = threading.Thread(target=modelPerson.webcam())
    # thread2 = threading.Thread(target=modelPerson.update_global_variable())
    while True:
        try:
            modelPerson.webcam()
            # thread1.start()
            # thread2.start()
        except Exception as e:
            print("Đang kết nối với server!")
            
