/* Filename:    TestTFSharpAgent.cs
 * Written by:  Tom Birdsong
 * Course:      ECE 4960 Fall 2020
 * Purpose:     Test realtime inference with agent executing TensorFlow graph
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.Diagnostics;
using System.IO;

public class TestTFSharpAgent : MonoBehaviour
{
    // Data file to load in for testing
    public string dataFile = "Data/Leap/compiled_leap_noext.csv";
    // Reference to the agent
    public TFSharpAgent agent;
    // Toggle testing and output on/off
    public bool doInference = false;

    public bool indexFixed = false;
    public int recordIndex = 1;

    // Data read in from CSV when the program starts
    private List<string[]> data;

    // Start is called before the first frame update
    void Start()
    {
        data = ReadCSV(dataFile);
        UnityEngine.Debug.Log("Test data contains " + data.Count + " records with headers " + String.Join(",", data[0]));
    }

    // Update is called once per frame
    void Update()
    {
        // Test by selecting a random record, running inference, and comparing results
        // with the expected label
        if(doInference && data.Count > 1)
        {
            List<float> inputVector = new List<float>();
            // randomly select a data point to test
            if (!indexFixed)
            {
                recordIndex = (int)(UnityEngine.Random.Range(0f, data.Count - 1) + 1);
            }

            // label is the last column
            int expectedLabel = int.Parse(data[recordIndex][data[recordIndex].Length - 1]);
            for(int dataIndex = 0; dataIndex < data[recordIndex].Length - 1; dataIndex++)
            {
                inputVector.Add(float.Parse(data[recordIndex][dataIndex]));
            }
            int? inferredLabel = agent.RunInference(inputVector);
            if (inferredLabel.HasValue)
                UnityEngine.Debug.Log("Row " + recordIndex.ToString() +
                    " expected label " + expectedLabel.ToString() + " and got " + inferredLabel.ToString());
            else
                Defs.Debug("Could not get a label from inference.");
        }
    }

    #region Public Methods
    // Read in data from a CSV file for testing
    public List<string[]> ReadCSV(string file)
    {
        string csvRaw;
        string[] csvLines;
        List<string[]> csvData = new List<string[]>();

        csvRaw = System.IO.File.ReadAllText(file);
        csvLines = csvRaw.Split('\n');
        for (int index = 0; index < csvLines.Length; index++) {
            csvData.Add(csvLines[index].Split(','));
        }

        return csvData;
    }
    #endregion
}
