using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Class to test functionality of data preprocessing workflow
public class TestPreprocessor : MonoBehaviour
{
    public GameStateManager manager;
    public DataPreprocessor preprocessor;
    public CSVWriter writer;
    public TFSharpAgent agent;

    private System.Random rand;

    //TODO: where is training data persisted before writing?
    private int[,] trainingData;
    private int recIndex;

    private int[] realtimeData;
    private float[] lastScaledRow;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        trainingData = new int[Defs.NUM_TRAINING_RECORDS, Defs.NUM_TRAINING_COLS];
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.IsTraining)
        {
            if (recIndex > Defs.NUM_TRAINING_RECORDS) recIndex = 0;

            for (int index = 0; index < Defs.NUM_FEATURES; index++)
            {
                trainingData[recIndex, index] = rand.Next(300, 700);
            }
            for (int index = Defs.NUM_FEATURES; index < Defs.NUM_TRAINING_COLS; index++)
            {
                trainingData[recIndex, index] = rand.Next(1, 11);
            }
            recIndex++;

            if (recIndex >= Defs.NUM_TRAINING_RECORDS)
            {
                float[,] normalizedData = preprocessor.PreprocessDataSet(trainingData);
                // write out data
                if (normalizedData != null)
                    writer.WriteData(normalizedData);

                recIndex = 0;
            }
        }
        else
        {
            realtimeData = new int[Defs.NUM_FEATURES];

            for (int index = 0; index < Defs.NUM_FEATURES; index++)
            {
                realtimeData[index] = rand.Next(300, 700);
            }

            // Updates max/min for each record and returns scaled row
            lastScaledRow = preprocessor.PreprocessRecord(realtimeData);

            // Run inference if done with calibration,
            // otherwise discard results
            if(!manager.IsCalibrating)
            {
                int? label = agent.RunInference(lastScaledRow.ToList());
                if (label.HasValue)
                    Defs.Debug("Got label " + Defs.LABELS[label.Value]);
                else
                    Defs.Debug("No label returned from inference.");
            }
        }
    }
}
