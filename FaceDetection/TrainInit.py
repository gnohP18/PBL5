import numpy as np
import os
from sklearn.svm import SVC
from sklearn.preprocessing import LabelEncoder
import utils.service as service
from keras.preprocessing.image import ImageDataGenerator
import joblib
image_dir_basepath = 'Dataset/images/'
names = next(os.walk(image_dir_basepath))[1]

gen = ImageDataGenerator(
  rotation_range=20,
  width_shift_range=0.1,
  height_shift_range=0.1,
  shear_range=0.1,
  zoom_range=0.2,
  horizontal_flip=True,
  vertical_flip=True
)
def train(dir_basepath, names):
    labels = []
    embs = []
    for name in names:
        dirpath = os.path.abspath(dir_basepath + name)
        filepaths = [os.path.join(dirpath, f) for f in os.listdir(dirpath)]
        aligned_images = service.load_and_align_images(filepaths)
        embs_ = service.calc_embs(aligned_images,10)    
        labels.extend([name] * len(embs_))
        embs.append(embs_)
        
    embs = np.concatenate(embs)
    le = LabelEncoder().fit(labels)
    y = le.transform(labels)
    clf = SVC(kernel='linear', probability=True).fit(embs, y)
    return le, clf

le, clf = train(image_dir_basepath, names)
joblib.dump(clf, 'models/face_recog.pkl')
joblib.dump(le, 'models/label_encoder.pkl')
