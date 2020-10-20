using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Serializable implementation of generic class
public class DataBufferInt : DataBuffer<int> { }

// Store asynchronous serial data and signal to the pipeline
// when a data point or labelled data set is available
public class DataBuffer<T> : MonoBehaviour
{
    #region Members
    [Tooltip("Most recent complete serial data point")]
    public T[] record = new T[Defs.NUM_FEATURES];
    [Tooltip("Most recent complete labelled serial data set")]
    public T[,] labelledSet = 
        new T[Defs.NUM_TRAINING_RECORDS, Defs.NUM_TRAINING_COLS];

    public bool isDebug = false;

    [Tooltip("Fires when a new complete record is available to be read.")]
    public UnityEvent RecordReady = new UnityEvent();
    [Tooltip("Fires when a new complete dataset is available to be read.")]
    public UnityEvent LabelledSetReady = new UnityEvent();

    // Serial buffer to store data point, may be incomplete
    private SortedList<int,T> recordBuf = new SortedList<int,T>();
    // Buffer to store labelled records, may not be full
    private T[,] labelSetBuf =
        new T[Defs.NUM_TRAINING_RECORDS, Defs.NUM_TRAINING_COLS];
    private int labelSetIndex = 0;

    #endregion

    #region Public Methods
    // Synchronous method for data receiving object to call to
    // populate buffer values as serial data becomes available.
    // Not thread-safe; use synchronous calls from a single
    // data receiving object.
    public void AddData(int sensorID, T data)
    {
        // Validate input
        // TODO define IDs to identify each sensor
        if(sensorID < 0 || sensorID >= Defs.NUM_TRAINING_COLS)
        {
            Defs.Debug("Invalid sensor ID: " + sensorID);
            return;
        }

        if (isDebug)
            Defs.Debug("Adding sensor " + sensorID + " value " + data);

        // Update the buffer for the current record, replacing
        // any previous value recorded for the given sensor 
        if (recordBuf.ContainsKey(sensorID))
            recordBuf.Remove(sensorID);
        recordBuf.Add(sensorID, data);

        // If the buffer has received at least one data point
        // from every sensor since the last time it was pushed
        // then push the buffer to the "record" object for
        // other objects to access on demand
        if(isRecBufferFull())
        {
            record = recordBuf.Values.ToArray();

            for (int index = 0; index < record.Length; index++)
                labelSetBuf[labelSetIndex, index] = record[index];
            labelSetIndex++;

            recordBuf.Clear();
            if (RecordReady != null)
                RecordReady.Invoke();
        }

        // If a complete set of training records is recorded in the set buffer
        // then push it to the on-demand member and create a new buffer
        if(isSetBufferFull())
        {
            labelledSet = labelSetBuf;
            labelSetBuf = new T[Defs.NUM_TRAINING_RECORDS, Defs.NUM_TRAINING_COLS];

            if (LabelledSetReady != null)
                LabelledSetReady.Invoke();
        }
    }

    #endregion

    #region Private Methods
    // Determine whether the sorted list has at least one
    // element for every sensor
    bool isRecBufferFull()
    {
        // TODO handle label column
        return (recordBuf.Count == Defs.NUM_FEATURES);
    }

    // Determine whether all rows in the set buffer are filled
    bool isSetBufferFull()
    {
        return (labelSetIndex == Defs.NUM_TRAINING_RECORDS);
    }

    #endregion

}
