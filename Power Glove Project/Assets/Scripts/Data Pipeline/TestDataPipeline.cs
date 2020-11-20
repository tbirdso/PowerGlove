using Leap.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class to test functionality of data preprocessing workflow
public class TestDataPipeline : MonoBehaviour
{
    // public GameStateManager manager;
    public bool isTraining;
    public DataBufferInt buffer;
    public DataPreprocessor preprocessor;
    public CSVWriter writer;
    public TFSharpAgent agent;

    private System.Random rand;

    //TODO: where is training data persisted before writing?
    private int sensorID;

    private int[] realtimeData;
    private float[] lastScaledRow;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        buffer.RecordReady.AddListener(OnRecordReady);
        buffer.LabelledSetReady.AddListener(OnLabelledSetReady);
    }

    // Update is called once per frame
    // Generate a random data point for one sensor in each given frame.
    // Assume 
    void Update()
    {
        if (isTraining)
        {
            // Randomly generate and add either a sensor data point or a label
            if (sensorID > Defs.NUM_TRAINING_COLS) sensorID = 0;

            if (sensorID < Defs.NUM_FEATURES)
                buffer.AddData(sensorID, rand.Next(300, 700));
            else if (sensorID < Defs.NUM_TRAINING_COLS)
                buffer.AddData(sensorID, rand.Next(1, 11));

            sensorID++;
        }
        else
        {
            // Randomly generate and add a sensor data point
            if (sensorID > Defs.NUM_FEATURES) sensorID = 0;

            buffer.AddData(sensorID, rand.Next(300, 700));
            sensorID++;
        }
    }

    // Handle event when a full record is available. Do preprocessing
    // for calibration and if allowed run inference
    void OnRecordReady()
    {
        if (!isTraining)
        {
            // Updates max/min for each record and returns scaled row
            lastScaledRow = preprocessor.PreprocessRecord(buffer.record);

            // Run inference
            int? label = agent.RunInference(lastScaledRow.ToList());
            if (label.HasValue)
                Defs.Debug("Got label " + Defs.LABELS[label.Value]);
            else
                Defs.Debug("No label returned from inference.");
        }
    }

    // Handle event when full training data set is ready.
    // Normalize data and write out to CSV
    void OnLabelledSetReady()
    {
        if (isTraining)
        {
            float[,] normalizedData = preprocessor.PreprocessDataSet(buffer.labelledSet);
            // write out data
            if (normalizedData != null)
                writer.WriteData(normalizedData);
        }
    }
}
