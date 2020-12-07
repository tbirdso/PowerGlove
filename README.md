# Power Glove
This is the code repo for our ECE 4960 Power Glove project

# Google Drive
Documents and media: https://drive.google.com/drive/folders/1B_ubtBGzQjS6-t31AJv48zaNYKo7mjQd?usp=sharing

# Code Organization
The project repository is organized as follows:

## Arduino Scripts
Various scripts that we used to design and test Arduino scripts. The "glove_samples" script is used to make an Arduino board emulate the glove, sending fixed glove data samples serially with JSON packaging.

## Arduino Code
Arduino script deliverables to drive functionality in the master and slave Arduino Unos on the power glove. An emulation script is also included where any Arduino can send precollected glove data over the serial line via JSON packaging.

## Machine Learning Code
Scripts used in building and training a neural network for sign identification. Includes the final model as a frozen graph and various training logs viewable with Tensorboard. Also includes script to unify discrete data sets into one master training set.

## Power Glove Project
Final deliverable Unity project. Opens in Unity v2019.4.9f1 LTS. Demonstration scene is under "Assets / Scenes / HandSimScene" and scene for training data collection is under "Assets / Scenes / TrainingScene". Other scenes consist of various tests and intermediate demonstrations over the project lifetime.

## Unity Scripts
C# scripts driving Unity simulation functionality. This folder is a copy of the folder "Power Glove Project / Assets / Scripts". Contains scripts describing hand animations, serial data collection and processing, Leap Motion logic, TensorFlow inference logic, and global properties.

## Development Scripts
Other scripts developed during the project lifetime and not included in the final deliverable, including Arduino scripts, neural network models, and the Milestone 1 Unity project.