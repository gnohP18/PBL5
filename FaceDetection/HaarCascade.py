from io import BytesIO
from imutils.video import VideoStream
import cv2 as cv
import imutils
import tensorflow as tf
import numpy as np
from skimage.transform import resize
import utils.service as service
import joblib
import threading
import requests
from datetime import datetime
import socket 
from PIL import Image
import io
messPast = [""]

class OpenDoorThread(threading.Thread):
    def __init__(self,id,img,checkin_time):
        threading.Thread.__init__(self)
        self.id = id
        self.checkin_time = checkin_time
        self.img = img
        # self.socket = socket.socket()
        # self.socket.connect(("192.168.181.58", 4000)) 
    def run(self): 
        try:  
            if (datetime.now() - self.checkin_time).total_seconds() > 5:
                print((datetime.now() - self.checkin_time).total_seconds())
                print("Run")
                # self.socket.send(b"Unlock")
                # self.socket.close()
                #image_file = BytesIO(self.img)
                self.img = cv.cvtColor(self.img, cv.COLOR_BGR2RGB)
                image = Image.fromarray(self.img)
                image_buffer = io.BytesIO()
                image.save(image_buffer, format='JPEG')
                image_bytes = image_buffer.getvalue()
                files = {'fileData': ('image.jpg', image_bytes, 'image/jpeg')}
                res = requests.post('http://103.253.146.161/api/app/time-sheet/time-sheet-for-employee?employeeCode='+str(self.id),files=files)
                print(res)
        except Exception as e:
            print(e)
            pass
# class SendMessage(threading.Thread):
#     def __init__(self,message,messPast):
#         threading.Thread.__init__(self)
#         self.messPast = messPast
#         self.message = message
#         self.socket = socket.socket()
#     def run(self):
#         if(self.messPast[0] == self.message): return
#         self.messPast[0] = self.message
        
#         try:
#             self.socket.connect(("192.168.181.58", 4000)) 
#             self.socket.send(self.message.encode('utf-8'))
#             self.socket.close()
#         except Exception:
#             pass
class Monitor:
    def __init__(self):
        self.start_time = datetime.now()
        self.recent_time = datetime.now()
        self.checkin_time = datetime.now()

camera = VideoStream(0).start();
face_detect = cv.CascadeClassifier('models/haarcascade_frontalface_alt2.xml')
clf = joblib.load('models/face_recog.pkl')
le = joblib.load('models/label_encoder.pkl')

monitor = {name: Monitor() for name in le.classes_}
print(monitor)

MapName = {
    "102200168":"102200168-Hiep",
    "102200199":"102200199-Tu",
    "102200185":"102200185-Phong",
    "102200179":"102200179-Minh"
}

while cv.waitKey(1) & 0xFF != ord('q'):
    img = camera.read()
    img = imutils.resize(img,width=400)
    cap = img.copy()
    all_faces = face_detect.detectMultiScale(img)

    for index, current_face in enumerate(all_faces):
        try:
            x,y,width,height = current_face
            dis = height / img.shape[0];
            if dis < 0.5 :
                #SendMessage("Closer,Please!!",messPast).start() 
                cv.putText(img, "Closer, Please!!", (100, 200), cv.FONT_HERSHEY_COMPLEX_SMALL,
                                    1, (0, 255, 0), thickness=1, lineType=2)
                continue;
            left_x, left_y = x, y
            right_x, right_y = x+width, y+height
            face_img = img[left_y:right_y,left_x:right_x]
            face_img = resize(face_img,
                                (160, 160), mode='reflect')
            embs = service.calc_embs(face_img[np.newaxis])
            predictions = clf.predict(embs)
            probabilities = clf.predict_proba(embs)
            predicted_label = le.inverse_transform(predictions)[0]
            
            name = predicted_label
            NameDecode = MapName[name]
            proba = probabilities[0][predictions[0]]

            print("Name: {}, Probability: {}".format(name, proba))

            if proba > 0.8:
                delta = (datetime.now() - monitor[name].recent_time).total_seconds();
                monitor[name].recent_time = datetime.now()
                if delta < 1 :
                    count = 3 - int((datetime.now() - monitor[name].start_time).total_seconds());
                else :
                    monitor[name].start_time = datetime.now()
                    count = 3

                if count == 0:
                    thread = OpenDoorThread(name,cap,monitor[name].checkin_time)
                    thread.start()
                    monitor[name].checkin_time = datetime.now()
                elif count < 0:
                    count = "Done!!!"
                
                
                # if(count != 0) : SendMessage(name + "," + str(count)+"," + "Welcome !!!",messPast).start()
                cv.putText(img, str(count), (left_x + 20, int(left_y + right_y/2) ),
                                    cv.FONT_HERSHEY_SIMPLEX,
                                    2, (0, 255, 0), thickness=5, lineType=2)
                cv.rectangle(img,(left_x,left_y),(right_x,right_y),(0,255,0),2)
                cv.putText(img, NameDecode, (left_x, left_y), cv.FONT_HERSHEY_COMPLEX_SMALL,
                                        1, (255, 255, 255), thickness=1, lineType=2)
                cv.putText(img, str(round(proba, 3)), (left_x, left_y + 17),
                                        cv.FONT_HERSHEY_COMPLEX_SMALL,
                                        1, (255, 255, 255), thickness=1, lineType=2)
            else:
                # SendMessage("Face Unknown !!!",messPast).start()
                cv.rectangle(img,(left_x,left_y),(right_x,right_y),(0,0,255),2)
                cv.putText(img, "Unknown", (left_x, left_y), cv.FONT_HERSHEY_COMPLEX_SMALL,
                                        1, (255, 255, 255), thickness=1, lineType=2)
        except Exception as e:
            print(e)
            pass

    cv.imshow("Face detect",img)

