from ultralytics import YOLO

model = YOLO('yolov8m.pt')
model.predict(0, show=True, conf=0.7)