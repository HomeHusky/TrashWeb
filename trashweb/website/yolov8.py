from ultralytics import YOLO
from ultralytics.yolo.v8.detect.predict import DetectionPredictor

def genYolov8():
    model = YOLO("yolov8m.pt")

    results = model.predict(source="0", show=True, conf=0.6, classes='0') 

def genByAnotherCamera():
    model2 = YOLO("best.pt")
    results2 = model2.predict(source="1", show=True, conf=0.6)
