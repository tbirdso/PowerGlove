# Filename: tensorflow_model.py
# Written by:   Tom Birdsong
# Course:       ECE 4960
# Purpose:      Train supervised learning model with Keras for Glove project labelling

### Import module dependencies
import csv
import pandas as pd
import numpy as np
from enum import Enum
import datetime
from shutil import copyfile

# Use sklearn for preprocessing steps
import sklearn.utils
import sklearn.model_selection

# Use TensorFlow.Keras for building, training, freezing, and exporting model
# Note that Keras import will display errors if running on a machine not set up
# with GPUs and CUDA, can usually just ignore them if you intend to train
# on your CPU
import tensorflow as tf
import tensorflow.keras
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Input, Dense, Dropout, Activation, Flatten


### Enumerate useful constants and define functions
# Store model results for each run in a unique folder
run_id = str(int(datetime.datetime.now().timestamp()))
OUTPUTFOLDER = ".\model" + run_id

# Data file generated by Unity project
DATAFILEPATH = "..\\Power Glove Project\\Data\\train_digits_noaccrot.csv"

# Definitions for DNN
NUM_FEATURES = 14
NUM_LABEL_ENUMS = 11

# DNN configuration values
NUM_DENSE_NODES = 50
P_DROPOUT = 0.5

# Training values
NUM_EPOCHS = 25

# Feature names and Label column
class colName(Enum):
    label = 'Label'

# Set up TensorBoard logging
tensorboard_callback = tf.keras.callbacks.TensorBoard(log_dir="logs/scalars/" + run_id)
        
# Procedure to export a model from TensorFlow 2.0 as a frozen graph
#  which TensorFlowSharp can interpret.
# From https://leimao.github.io/blog/Save-Load-Inference-From-TF2-Frozen-Graph/
# Credit to Lei Mao 
# Parameters: - model to freeze
#             - Directory and filename to write out frozen graph
#             - verbose operations (listing frozen model layers) off or on
def freeze_and_export_model(model, dir, name, verbose=False):
    full_model = tf.function(lambda x: model(x))
    full_model = full_model.get_concrete_function(
        tf.TensorSpec(model.inputs[0].shape, model.inputs[0].dtype))
    # Get frozen ConcreteFunction
    frozen_func = tf.python.framework.convert_to_constants.convert_variables_to_constants_v2(full_model)
    frozen_func.graph.as_graph_def()

    if verbose:
        layers = [op.name for op in frozen_func.graph.get_operations()]
        print("-" * 50)
        print("Frozen model layers: ")
        for layer in layers:
            print(layer)

    # Save frozen graph from frozen ConcreteFunction to hard drive
    tf.io.write_graph(graph_or_graph_def=frozen_func.graph,
                        logdir=dir,
                        name=name,
                        as_text=False)


### Start Tensorflow 1.x session
# We need this so we can freeze and export the graph later
#sess = tf.compat.v1.Session()
#tf.compat.v1.keras.backend.set_session(sess)


### Import CSV data
data_raw = pd.read_csv(DATAFILEPATH);
print('Imported ' + str(len(data_raw)) + ' records');


### Preprocessing steps
# Shuffle data so that it is not time sensitive
data_shuffled = sklearn.utils.shuffle(data_raw)

# Split data into input and output (label) sets
rows = data_shuffled.drop(columns=[colName.label.value])
labels = data_shuffled[colName.label.value]
#print("Labels:\n" + str(labels))
#print("Data: " + str(rows))

#TODO scale data points on the domain [-1, 1] within the
# output range of each respective sensor

# Split data into train (80%) and test (20%) sets
data_train, data_test, labels_train, labels_test = sklearn.model_selection.train_test_split(rows, labels, test_size=0.20)
#print("Test data: " + str(data_test))
#print("Test labels: " + str(labels_test))


### Define model with Keras
# Create a Deep Neural Network (DNN) with the following configuration:
# - Input layer of N inputs
# - Dense layer of M nodes
# - Dropout layer with P percent chance of dropping weights to mitigate overfitting
# - Output layer of (Q=num distinct labels) nodes with Softmax activation that gives 
#       the probability the input vector maps to any given label
model = Sequential()
model.add(Input(shape=(NUM_FEATURES,),name="input1"))
model.add(Dense(NUM_DENSE_NODES, activation='relu', name="dense1"))
model.add(Dropout(P_DROPOUT, name="dropout1"))
model.add(Dense(NUM_LABEL_ENUMS, activation='softmax', name="output1"))

# TODO: Test categorical one-hots with different loss function
model.compile(loss='sparse_categorical_crossentropy', optimizer='adam', metrics=['accuracy'])


### Train model
# TODO: Experiment with different numbers of epochs for training
#print('Data_train is ' + str(type(data_train)))
#print('Data_train has ' + str(len(list(data_train))) + ' features and ' + str(len(data_train)) + ' rows.')

model.fit(data_train, labels_train, batch_size=32, epochs=NUM_EPOCHS, verbose=1, callbacks=[tensorboard_callback])


### Evaluate model
score = model.evaluate(data_test, labels_test, verbose=1)
print(model.metrics_names)
print('\n***************************************************************')
print("Trained model has " + str(score[0]) + " loss and " + str(score[1])+ " accuracy")
print('\n***************************************************************')


### Save model for persistent access

# Save Protobuf frozen graph weights in .bytes file that 
#  Unity and TensorFlowSharp can read and interpret
freeze_and_export_model(model=model, dir=OUTPUTFOLDER, name="frozen_graph.bytes", verbose=False)

# Also save traditional JSON and h5 versions in case we need to
# import back to Python for some reason
with open(OUTPUTFOLDER + "\\keras_model.json", "w") as fptr:
    fptr.write(model.to_json())
    model.save_weights(OUTPUTFOLDER + "\\keras_model_weights.h5")
