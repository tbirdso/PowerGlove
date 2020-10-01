# Filename: tensorflow_model.py
# Written by:   Tom Birdsong
# Course:       ECE 4960
# Purpose:      Train supervised learning model with sklearn for Glove project labelling
# Issues: - Currently relies on sklearn instead of TensorFlow/Keras for model
#         - Model persistence with pickling in joblib is not portable to Unity

# Import module dependencies
import csv
import pandas as pd
import numpy as np
from enum import Enum
import joblib

import sklearn
import sklearn.metrics
import sklearn.preprocessing
import sklearn.svm
import sklearn.model_selection

#TODO: Define DNN with Keras
#import tensorflow as tf
#from tensorflow import keras;

# Enumerate useful constants
class signs(Enum):
    ZERO = 0
    ONE = 1
    TWO = 2

    NONACTIVE = 0
    ACTIVE = 1

class colName(Enum):
    label = 'Label'
    f1 = 'Feature1'
    f2 = 'Feature2'
    f3 = 'Feature3'
    f4 = 'Feature4'

# Import CSV data
data_raw = pd.read_csv("..\Power Glove Project\Data\data.csv");

print('Input count is ' + str(len(data_raw)));

# Shuffle data so that it is not time sensitive
data_shuffled = sklearn.utils.shuffle(data_raw)

# Split data into input/labels and train/test
rows = data_shuffled.drop(columns=[colName.label.value])
labels = data_shuffled[colName.label.value]
#print("Labels:\n" + str(labels))
#print("Data: " + str(rows))

#data_scaled = pd.DataFrame(sklearn.preprocessing.scale(rows), columns=[colName.f1.value, colName.f2.value, colName.f3.value, colName.f4.value])
#print("Data scaled: " + str(data_scaled))

data_train, data_test, labels_train, labels_test = sklearn.model_selection.train_test_split(rows, labels, test_size=0.20)
print("Test data: " + str(data_test))
print("Test labels: " + str(labels_test))

# Train model
model = sklearn.svm.SVC(C=3, kernel="linear")
model.fit(data_train, labels_train)

# Evaluate model
labels_pred = model.predict(data_test)
accuracy_score = sklearn.metrics.accuracy_score(labels_test, labels_pred)
print("Model has accuracy " + str(accuracy_score))
#print("Data [1,0,1,0] returns " + str(model.predict([[1,0,1,0]])))

# Save model for Python access
joblib.dump(model, 'trained_model.joblib')