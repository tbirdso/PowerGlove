﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global constants for the Power Glove project
/// </summary>
public static class Defs
{
    // Number of sensor variables collected
    public const int NUM_FEATURES = 20;
    // Number of label variables we are mapping to
    public const int NUM_LABELS = 1;
    // Expected number of columns in training data
    public const int NUM_TRAINING_COLS = NUM_FEATURES + NUM_LABELS;
    // Expected number of rows in training data
    public const int NUM_TRAINING_RECORDS = 13000;

    // Domain for normalized data to train NN
    public const float TRAINING_MIN = -1;
    public const float TRAINING_MAX = 1;

    // Possible ordered mappings for label outputs
    // ex. output integer value 1 -> LABELS[1] -> "1"
    public static List<string> LABELS = new List<string>()
    {
        "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "None"
    };

    public enum Sensor
    {
        INDEX_MCP,
        INDEX_PIP,
        MIDDLE_MCP,
        MIDDLE_PIP,
        RING_MCP,
        RING_PIP,
        PINKY_MCP,
        PINKY_PIP,
        THUMB_MCP,
        THUMB_PIP,
        THUMB_HES,
        INDEX_HES,
        RING_HES,
        PINKY_HES,
        X_ACC,
        Y_ACC,
        Z_ACC,
        PITCH,
        ROLL,
        YAW
    }

    // Test for whether a label is in the possible set
    public static bool IsValidLabel(string label)
    {
        return LABELS.Contains(label);
    }

    // Quick reference to debug print
    public static void Debug(string str)
    {
        UnityEngine.Debug.Log(str);
    }
}