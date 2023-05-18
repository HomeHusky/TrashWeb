import socket
from ultralytics import YOLO
import cv2
from ultralytics.yolo.utils.plotting import Annotator

# Take the time when people start taking out the trash
def checkPerson(xyxy):
    x1,y1,x2,y2 = xyxy
    if int(y1)<=150:
        if int(x2-x1)>=300 or int(y2-y1)>=240: # width>=300 or height>=240 (of the box)
            return True
    return False

# Get the box with the largest area
def get_largest_area_box(boxes):
    areas = []
    for box in boxes:
        x1,y1,x2,y2 = box
        areas.append((x2-x1) * (y2-y1)) #calculate the area of the box

    return boxes[areas.index(max(areas))]

def update_global_variable(status):
    # send data to next server
    host = 'localhost'
    port = 2345
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((host, port))
        s.sendall(str(status).encode())

def webcam():
    
    model = YOLO('yolov8s.pt')  # load model

    cap = cv2.VideoCapture(0)
    _, frame = cap.read()

    goin = 0
    goout = 0
    status = False
    color = (0,0,255)

    while True:
        _, frame = cap.read()
        frame = cv2.flip(frame, 1)
        img = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        results = model.predict(img, verbose=False) # get prediction

        person_boxes = []
        check = False
        # processing
        for r in results:
            annotator = Annotator(frame)
            boxes = r.boxes
            for box in boxes:
                if box.cls==0:
                    c = box.cls
                    b = box.xyxy[0].tolist()  # get box coordinates in (top, left, bottom, right) format
                    person_boxes.append(b)
                    annotator.box_label(b, model.names[int(c)])
        if person_boxes:
            largest_box = get_largest_area_box(person_boxes)
            check = checkPerson(largest_box)

        # Check for someone standing near the webcam or not
        if check:
            goin+=1
            goout=0
            
        else:
            goout+=1

        if goin>=3:
            status = True
            color = (0,255,0)
        if goout>=3:
            goin=0
            status = False
            color = (0,0,255)

        # Put text to show the status
        cv2.putText(frame,str(status),(20,20),cv2.FONT_HERSHEY_SIMPLEX,1,color,2,cv2.LINE_AA)
        try:
            update_global_variable(status)
        except Exception:
            print("No connection Trash Server!")
        print(status)
        print(goin, goout)
        # Draw box into frame
        frame = annotator.result()  
        # Show frame
        cv2.imshow('YOLO V8 Detection', frame)     
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break

    cap.release()
    cv2.destroyAllWindows()

webcam()
