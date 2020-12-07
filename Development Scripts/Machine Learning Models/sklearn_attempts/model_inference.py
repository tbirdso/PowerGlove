# Filename: model_inference.py
# Written by:   Tom Birdsong
# Course:       ECE 4960
# Purpose:      Run inference on an input data set
# Issues: - Very slow, takes ~1 second to load the model for each call
#         - No good process to integrate with Unity

import sys
import sklearn
import joblib

EXPECTED_INPUT_LENGTH = 4

# Get the input vector
if len(sys.argv) != EXPECTED_INPUT_LENGTH + 1:
    print("Could not get input argument!")
    exit(-1)

in_vector = list()
try:
    for arg in sys.argv[1:]:
        print("Converting " + str(arg))
        in_vector.append(float(arg))
except:
    print("Could not convert args to floats!")
    exit(-1)

# Load the trained model
model = joblib.load('trained_model.joblib')

# Run inference
label_pred = model.predict([in_vector])

print("Found " + str(label_pred))