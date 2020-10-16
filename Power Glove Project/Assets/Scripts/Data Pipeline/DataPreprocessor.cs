using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to normalize sensor data for training and at runtime
public class DataPreprocessor : MonoBehaviour
{
    #region Members
    const int MIN_INDEX = 0, MAX_INDEX = 1;

    // Data collected during training
    private List<int[]> collectedData = new List<int[]>();
    // Observed extrema for each variable. Null if no records checked yet.
    private int[,] extrema;
    #endregion

    #region Public Methods
    // Perform preprocessing steps on a single record:
    // - Update observed extremes with the given values
    // - Return normalized values
    public float[] PreprocessRecord(int[] row)
    {
        if(ValidateDataPoint(row))
        {
            UpdateExtrema(row);
            return Normalize(row);
        } else
        {
            Defs.Debug("Invalid data point: " + row.ToString());
            return null;
        }
    }

    // Process a 2D array of data, such as a collected training data set
    // that includes labels
    public float[,] PreprocessDataSet(int[,] dataSet)
    {
        if (ValidateDataSet(dataSet))
        {
            UpdateExtrema(dataSet);
            return Normalize(dataSet);
        } else
        {
            Defs.Debug("Attempted to normalize an invalid data set");
            return null;
        }
    }

    // Verify that each data point is the expected size
    // TODO are there any additional verification steps?
    public bool ValidateDataPoint(int[] row)
    {
        return row.Length >= Defs.NUM_FEATURES;
    }

    // Verify that each data point is the expected size
    // TODO are there any additional verification steps?
    public bool ValidateDataSet(int[,] dataSet)
    {
        return dataSet.GetLength(1) >= Defs.NUM_FEATURES;
    }

    // Discard previously observed calibration values
    public void ResetCalibration()
    {
        extrema = new int[2, Defs.NUM_FEATURES];
    }

    #endregion

    #region Private Methods

    // Update local values for variable extremes
    private void UpdateExtrema(int[] row)
    {
        // Initialize if necessary
        if (extrema == null)
        {
            extrema = new int[2, Defs.NUM_FEATURES];
            for (var index = 0; index < Defs.NUM_FEATURES; index++)
            {
                extrema[MIN_INDEX, index] = row[index];
                extrema[MAX_INDEX, index] = row[index];
            }
        }
        else
        {
            for (var index = 0; index < Defs.NUM_FEATURES; index++)
            {
                if (extrema[MIN_INDEX, index] > row[index])
                    extrema[MIN_INDEX, index] = row[index];
                if (extrema[MAX_INDEX, index] < row[index])
                    extrema[MAX_INDEX, index] = row[index];
            }
        }
        return;
    }

    // Overload to update extremes from multiple records
    private void UpdateExtrema(int[,] arr)
    {
        // Initialize if necessary
        if (extrema == null)
        {
            extrema = new int[2, Defs.NUM_FEATURES];
            for (var index = 0; index < Defs.NUM_FEATURES; index++)
            {
                extrema[MIN_INDEX, index] = arr[0,index];
                extrema[MAX_INDEX, index] = arr[0,index];
            }
        }

        for (var row = 0; row < arr.GetLength(0); row++)
        {
            for (var col = 0; col < Defs.NUM_FEATURES; col++)
            {
                if (extrema[MIN_INDEX, col] > arr[row, col])
                    extrema[MIN_INDEX, col] = arr[row, col];
                if (extrema[MAX_INDEX, col] < arr[row, col])
                    extrema[MAX_INDEX, col] = arr[row, col];
            }
        }
    }

    // Reference observed max and min for each variable to transform
    // from the observation range onto the training range defined in
    // the Defs class
    private float[] Normalize(int[] row)
    {
        float[] rowNorm = new float[row.Length];
        // Ignore any label columns beyond feature columns
        for(var index = 0; index < Defs.NUM_FEATURES; index++)
        {
            // Compute each normalized feature according to the formula
            // norm = (x - x_min) * ((new_max - new_min) / (x_max - x_min)) + new_min
            rowNorm[index] = (row[index] - extrema[MIN_INDEX, index]) *
                ((float)Defs.TRAINING_MAX - Defs.TRAINING_MIN) /
                (extrema[MAX_INDEX, index] - extrema[MIN_INDEX, index]) +
                Defs.TRAINING_MIN;
        }
        // Copy any label columns without transforming
        for(var index = Defs.NUM_FEATURES; index < row.Length; index++)
        {
            rowNorm[index] = row[index];
        }

        return rowNorm;
    }

    // Reference observed max and min for each variable to transform
    // from the observation range onto the training range defined in
    // the Defs class
    private float[,] Normalize(int[,] dataSet)
    {
        float[,] normalizedData = new float[dataSet.GetLength(0), dataSet.GetLength(1)];

        for (int row = 0; row < dataSet.GetLength(0); row++)
        {
            // Only normalize feature columns
            for (int col = 0; col < Defs.NUM_FEATURES; col++)
            {
                // Compute each normalized feature according to the formula
                // norm = (x - x_min) * ((new_max - new_min) / (x_max - x_min)) + new_min
                normalizedData[row, col] = (dataSet[row, col] - extrema[MIN_INDEX, col]);
                normalizedData[row, col] *=
                    (float)(Defs.TRAINING_MAX - Defs.TRAINING_MIN) /
                    (extrema[MAX_INDEX, col] - extrema[MIN_INDEX, col]);
                normalizedData[row,col] += Defs.TRAINING_MIN;
            }
            // Copy over label columns without transformation
            for (int col = Defs.NUM_FEATURES; col < dataSet.GetLength(1); col++)
            {
                normalizedData[row, col] = dataSet[row, col];
            }
        }

        return normalizedData;
    }

    #endregion
}
