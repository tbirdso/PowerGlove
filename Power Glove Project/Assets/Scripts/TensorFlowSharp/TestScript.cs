using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.Diagnostics;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Example from https://github.com/migueldeicaza/TensorFlowSharp
        using (var session = new TFSession())
        {
            var graph = session.Graph;

            var a = graph.Const(2);
            var b = graph.Const(3);
            Console.WriteLine("a=" + a + " b=" + b);

            // Add two constants
            var addingResults = session.GetRunner().Run(graph.Add(a, b));
            var addingResultValue = addingResults.GetValue();
            UnityEngine.Debug.Log("a+b="+ addingResultValue);

            // Multiply two constants
            var multiplyResults = session.GetRunner().Run(graph.Mul(a, b));
            var multiplyResultValue = multiplyResults.GetValue();
            UnityEngine.Debug.Log("a*b="+ multiplyResultValue);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
