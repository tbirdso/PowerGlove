# Filename: tensorflow_model.py
# Written by:   Tom Birdsong
# Course:       ECE 4960
# Purpose:      Train supervised learning model with Keras for Glove project labelling

# Import module dependencies
import csv
import pandas as pd
import numpy as np
from enum import Enum
import joblib
import datetime

# Use sklearn for preprocessing steps
import sklearn.utils
import sklearn.model_selection
import tensorflow as tf
import tensorflow.keras
from tensorflow.keras.models import Sequential
from tensorflow.keras.layers import Input, Dense, Dropout, Activation, Flatten
from tensorflow.python.saved_model import builder as pb_builder

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

# From https://stackoverflow.com/questions/45466020/how-to-export-keras-h5-to-tensorflow-pb
def freeze_session(session, keep_var_names=None, output_names=None, clear_devices=True):
    """
    Freezes the state of a session into a pruned computation graph.

    Creates a new computation graph where variable nodes are replaced by
    constants taking their current value in the session. The new graph will be
    pruned so subgraphs that are not necessary to compute the requested
    outputs are removed.
    @param session The TensorFlow session to be frozen.
    @param keep_var_names A list of variable names that should not be frozen,
                          or None to freeze all the variables in the graph.
    @param output_names Names of the relevant graph outputs.
    @param clear_devices Remove the device directives from the graph for better portability.
    @return The frozen graph definition.
    """
    graph = session.graph
    with graph.as_default():
        freeze_var_names = list(set(v.op.name for v in tf.compat.v1.global_variables()).difference(keep_var_names or []))
        output_names = output_names or []
        output_names += [v.op.name for v in tf.compat.v1.global_variables()]
        input_graph_def = graph.as_graph_def()
        if clear_devices:
            for node in input_graph_def.node:
                node.device = ""
        frozen_graph = tf.compat.v1.graph_util.convert_variables_to_constants(
            session, input_graph_def, output_names, freeze_var_names)
        return frozen_graph

# Start Tensorflow session
sess = tf.compat.v1.Session()
tf.compat.v1.keras.backend.set_session(sess)

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
#print("Test data: " + str(data_test))
#print("Test labels: " + str(labels_test))

# Define model
model = Sequential()
model.add(Input(shape=(4,),name="input1"))
model.add(Dense(10, activation='relu', name="dense1"))
model.add(Dropout(0.5, name="dropout1"))
model.add(Dense(6, activation='softmax', name="output1"))

model.compile(loss='sparse_categorical_crossentropy', optimizer='adam', metrics=['accuracy'])

# Train model
model.fit(data_train, labels_train, batch_size=32, epochs=25, verbose=1)

# Evaluate model
score = model.evaluate(data_test, labels_test, verbose=1)
print(model.metrics_names)
print("Model has score " + str(score))

y = model.predict(x=[[1,0,0,1]], verbose=1)
args_y = np.argmax(y,axis=1)
print("Data [1,0,0,1] returns " + str(y) + " which condenses to " + str(args_y))

# Save model for persistent access
with open("keras_model.json", "w") as fptr:
    fptr.write(model.to_json())
    model.save_weights("keras_model_weights.h5")

pb_folder = '.\pb' + str(int(datetime.datetime.now().timestamp()))
#model.save(pb_folder)

#graph_frozen = freeze_session(session=sess)
#tf.compat.v1.train.write_graph(graph_frozen, pb_folder, "model.pb", as_text=False)

# FIXME check to make sure model can be restored
#new_model = tf.keras.models.load_model(pb_folder)
#new_model.summary()

# From https://leimao.github.io/blog/Save-Load-Inference-From-TF2-Frozen-Graph/
full_model = tf.function(lambda x: model(x))
full_model = full_model.get_concrete_function(
    tf.TensorSpec(model.inputs[0].shape, model.inputs[0].dtype))
# Get frozen ConcreteFunction
frozen_func = tf.python.framework.convert_to_constants.convert_variables_to_constants_v2(full_model)
frozen_func.graph.as_graph_def()

layers = [op.name for op in frozen_func.graph.get_operations()]
print("-" * 50)
print("Frozen model layers: ")
for layer in layers:
    print(layer)

# Save frozen graph from frozen ConcreteFunction to hard drive
tf.io.write_graph(graph_or_graph_def=frozen_func.graph,
                    logdir=pb_folder,
                    name="frozen_graph.pb",
                    as_text=False)