using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to store labelled data retrieved from the Power Glove
public class GloveTrainingBuffer : DataBuffer<int>
{
    private void Update()
    {
        if (base.isDebug)
        {
            int? label = getKeyLabel();
            Defs.Debug("Label: " + label.ToString());


            PowerGlove testGlove = new PowerGlove();
            testGlove.index_pip = 200;
            testGlove.index_hes = 255;
            testGlove.thumb_hes = 5;
            testGlove.roll = 90;
            AddData(testGlove);
        }

        // If the user presses the "enter" key during training then
        // make whatever data is in the buffer available as a
        // "complete" data set
        if(Input.GetKeyDown("return"))
        {
            makeBufferAvailable();
        }
    }

    // Append a label to a single glove serial record and
    // add the combined record to a data set buffer, excluding
    // IMU data from the glove
    public void AddData(PowerGlove glove, bool includeIMU = false)
    {
        List<int> data = (includeIMU ? glove.ToList() : glove.FingersToList());
        int? label = getKeyLabel();

        if(label.HasValue)
        {
            data.Add(label.Value);
            record = data.ToArray();

            for (int index = 0; index < record.Length; index++)
                labelSetBuf[labelSetIndex, index] = record[index];
            labelSetIndex++;
        }

        // If a complete set of training records is recorded in the set buffer
        // then push it to the on-demand member and create a new buffer
        if (isSetBufferFull())
        {
            makeBufferAvailable();
        }
    }

    // Translate current key press to label.
    // Spacebar is reserved for the label "None".
    // If no key is pressed then return null
    private int? getKeyLabel()
    {
        for(int index = 0; index < Defs.LABELS.Count - 1; index++)
        {
            if(Input.GetKey(Defs.LABELS[index]))
            {
                return index;
            }
        }

        if(Input.GetKey("space"))
        {
            return Defs.LABELS.Count - 1;
        } else
        {
            return null;
        }
    }
}
