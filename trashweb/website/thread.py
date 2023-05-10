import threading
from .yolov8 import genYolov8, genByAnotherCamera

class runYolov8Thread(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
    def run(self):
        try:
            print('Thread execution started!')
            genYolov8()
        except Exception as e:
            print(e)

class runAnotherCam(threading.Thread):
    def __init__(self):
        threading.Thread.__init__(self)
    def run(self):
        try:
            print('The second camera started!')
            genByAnotherCamera()
        except Exception as e:
            print(e)
