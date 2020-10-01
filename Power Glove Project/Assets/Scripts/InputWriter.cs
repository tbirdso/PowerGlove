using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class InputWriter : MonoBehaviour
{
    #region Members
    public const int MAX_RECORDS = 1000;
    public const int NUM_FEATURES = 4;
    public const char delim = ',';

    // Buffer for records with appended labels
    public int[,] data;
    // Destination data file
    public string filename = "data.csv";
    // Feature names
    public List<string> featureNames;

    // Rover for inserting records
    private int index;
    // Label header for CSV file
    private string LABEL_HEADER = "Label";
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        index = 0;
        data = new int[MAX_RECORDS, NUM_FEATURES + 1];
    }

    // Update is called once per frame
    void Update()
    {
        TestParseLabel();

        if (index > 0 && index < MAX_RECORDS)
        {
            UnityEngine.Debug.Log("index: " + index);
            UnityEngine.Debug.Log("Last label: " + data[index-1, NUM_FEATURES]);
        }
        // If we overflow the buffer then write out data and clear the buffer
        if(index >= MAX_RECORDS)
        {
            WriteData();
            index = 0;
        }
    }

    #region Private Methods
    // Dummy method to generate test features representing the binary mapping for each label.
    // This should be replaced with a method coupling serial data
    // with a label in the actual project.
    void TestParseLabel()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            data[index, 0] = 0;
            data[index, 1] = 0;
            data[index, 2] = 0;
            data[index, 3] = 1;
            data[index, NUM_FEATURES] = 1;
            index++;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            data[index, 0] = 0;
            data[index, 1] = 0;
            data[index, 2] = 1;
            data[index, 3] = 0;
            data[index, NUM_FEATURES] = 2;
            index++;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            data[index, 0] = 0;
            data[index, 1] = 0;
            data[index, 2] = 1;
            data[index, 3] = 1;
            data[index, NUM_FEATURES] = 3;
            index++;
        }
    }

    // Write out data to a CSV file
    void WriteData()
    {
        StreamWriter outStream;
        StringBuilder builder = new StringBuilder();
        int i, feature_index;

        using(outStream = System.IO.File.CreateText(filename))
        {
            // Write column headers
            foreach(string feature_name in featureNames)
            {
                builder.Append(feature_name + delim);
            }
            builder.Append(LABEL_HEADER);
            outStream.WriteLine(builder.ToString());

            // Write data
            for(i = 0; i < index; i++)
            {
                builder.Clear();
                for(feature_index = 0; feature_index < NUM_FEATURES + 1; feature_index++)
                {
                    builder.Append(data[i, feature_index].ToString() + delim);
                }
                builder.Remove(builder.Length - 1, 1);

                UnityEngine.Debug.Log("Writing this line: " + builder.ToString());
                outStream.WriteLine(builder.ToString());
            }
        }
    }
    #endregion
}
