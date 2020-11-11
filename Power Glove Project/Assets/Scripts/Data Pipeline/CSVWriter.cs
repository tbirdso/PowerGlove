using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

// Class to handle writing data out to CSV file
public class CSVWriter : MonoBehaviour
{
    #region Members
    public const char delim = ',';
    public bool isDebug;

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
        data = new int[Defs.NUM_TRAINING_RECORDS, Defs.NUM_TRAINING_COLS];
    }

    // Write out data to a CSV file
    public void WriteData<T>(T[,] data)
    {
        StreamWriter outStream;
        StringBuilder builder = new StringBuilder();
        int i, feature_index;

        using (outStream = System.IO.File.CreateText(filename))
        {
            // Write column headers
            foreach (string feature_name in featureNames)
            {
                builder.Append(feature_name + delim);
            }
            builder.Append(LABEL_HEADER);
            outStream.WriteLine(builder.ToString());

            // Write data
            for (i = 0; i < data.GetLength(0); i++)
            {
                builder.Clear();
                for (feature_index = 0; feature_index < Defs.NUM_TRAINING_COLS; feature_index++)
                {
                    builder.Append(data[i, feature_index].ToString() + delim);
                }
                builder.Remove(builder.Length - 1, 1);

                outStream.WriteLine(builder.ToString());
            }
        }

        if(isDebug)
        {
            Defs.Debug("Data was written to " + filename);
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
            data[index, Defs.NUM_FEATURES] = 1;
            index++;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            data[index, 0] = 0;
            data[index, 1] = 0;
            data[index, 2] = 1;
            data[index, 3] = 0;
            data[index, Defs.NUM_FEATURES] = 2;
            index++;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            data[index, 0] = 0;
            data[index, 1] = 0;
            data[index, 2] = 1;
            data[index, 3] = 1;
            data[index, Defs.NUM_FEATURES] = 3;
            index++;
        }
    }


    #endregion
}
