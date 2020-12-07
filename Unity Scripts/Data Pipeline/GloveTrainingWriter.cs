using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GloveTrainingWriter : CSVWriter
{
    public GloveTrainingBuffer buffer;

    // Start is called before the first frame update
    void Start()
    {
        featureNames = Enum.GetNames(typeof(Defs.Sensor)).ToList<string>();
        buffer.LabelledSetReady.AddListener(WriteGloveData);
    }

    void WriteGloveData()
    {
        WriteData(buffer.labelledSet);
    }
}
