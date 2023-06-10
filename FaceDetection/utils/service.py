import numpy as np
import cv2
from imageio import imread
from skimage.transform import resize
from keras.models import load_model


facenet = load_model('./models/facenet_keras.h5')
cascade_path = './models/haarcascade_frontalface_default.xml'
cascade = cv2.CascadeClassifier(cascade_path)
image_size = 160

def prewhiten(x):
    if x.ndim == 4:
        axis = (1, 2, 3)
        size = x[0].size
    elif x.ndim == 3:
        axis = (0, 1, 2)
        size = x.size
    else:
        raise ValueError('Dimension should be 3 or 4')

    mean = np.mean(x, axis=axis, keepdims=True)
    std = np.std(x, axis=axis, keepdims=True)
    std_adj = np.maximum(std, 1.0/np.sqrt(size))
    y = (x - mean) / std_adj
    return y

def l2_normalize(x, axis=-1, epsilon=1e-10):
    output = x / np.sqrt(np.maximum(np.sum(np.square(x), axis=axis, keepdims=True), epsilon))
    return output

def load_and_align_images(filepaths, margin = 10):
    aligned_images = []
    for filepath in filepaths:
        try:
            img = imread(filepath)
            print(filepath)
            faces = cascade.detectMultiScale(img,
                                            scaleFactor=1.1,
                                            minNeighbors=3)
            (x, y, w, h) = faces[0]
            cropped = img[y-margin//2:y+h+margin//2,
                        x-margin//2:x+w+margin//2, :]
            aligned = resize(cropped, (image_size, image_size), mode='reflect')
            aligned_images.append(aligned)
        except Exception:
            pass
            
    return np.array(aligned_images)

def calc_embs(imgs, batch_size = 1):
    aligned_images = prewhiten(imgs)
    pd = []
    for start in range(0, len(aligned_images), batch_size):
        pd.append(facenet.predict_on_batch(aligned_images[start:start+batch_size]))
    embs = l2_normalize(np.concatenate(pd))

    return embs