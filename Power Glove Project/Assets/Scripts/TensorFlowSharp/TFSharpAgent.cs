/* Filename:    TFSharpAgent.cs
 * Written by:  Tom Birdsong
 * Course:      ECE 4960 Fall 2020
 * Purpose:     Load trained TensorFlow graph and run inference on demand
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.Diagnostics;
using System;

public class TFSharpAgent : MonoBehaviour
{
    #region Members
    // Number of features (variables) expected as input to the ML model
    // Leap Motion dataset with boolean variables removed has 40 features
    public int NUM_FEATURES = 40;
    // Number of possible labels/categories expected as ML output
    // Leap Motion dataset maps onto 1-10 and TensorFlow expects 0 starting index, so
    // rather than transform it was easier to map onto 0-10 which is 11 labels
    public int NUM_LABELS = 11;

    // Drag graph model from Resources onto script in the Editor
    public TextAsset graphModel;

    public bool isDebug;

    // Persistent TensorFlow graph
    private TFGraph graph;
    private TFSession session;
    private TFSession.Runner runner;

    #endregion

    #region Public Methods

    /* Returns a label given a vector of inputs */
    public int RunInference(List<float> inputs)
    {
        LoadTensorFlowGraph();

        int inferredLabel;

        // Verify that the input list is the appropriate size
        if(inputs.Count != NUM_FEATURES)
        {
            UnityEngine.Debug.Log("Agent expected " + NUM_FEATURES.ToString() + 
                " elements but got " + inputs.Count.ToString());
            return -1;
        }

        // Pass input into model
        float[,] rowInput = ListTo2DArray(inputs);
        runner.AddInput(graph["x"][0], rowInput);

        // Run inference
        runner.Fetch(graph["sequential/output1/Softmax"][0]);
        var output = runner.Run();

        // Retrieve results
        var vecResults = output[0].GetValue();
        float[,] results = (float[,])vecResults;

        /* for (int i = 0; i < NUM_LABELS; i++)
        {
            UnityEngine.Debug.Log("Output " + i + " is " + results[0, i].ToString());
        } */

        // The softmax output layer returns the probability that the data point corresponds to
        // each possible label, so select the label with the highest probability

        inferredLabel = getMaxIndex(results);
        if (isDebug)
        {
            UnityEngine.Debug.Log("The response for the input vector is " + inferredLabel);
        }

        return getMaxIndex(results);
    }

    #endregion

    #region Private Methods

    // Start is called before the first frame update
    void Start()
    {
        LoadTensorFlowGraph();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Follow tutorial at https://github.com/llSourcell/Unity_ML_Agents/blob/master/docs/Using-TensorFlow-Sharp-in-Unity-(Experimental).md
    private void LoadTensorFlowGraph()
    {
        // Recreate the graph in Unity
        graph = new TFGraph();
        graph.Import(graphModel.bytes);
        session = new TFSession(graph);
        runner = session.GetRunner();

        //UnityEngine.Debug.Log(graph["sequential/output1/Softmax"].ToString());
        //UnityEngine.Debug.Log(graph["sequential/dense1/Relu"].ToString());
        //UnityEngine.Debug.Log(graph["x"].ToString());

        //UnityEngine.Debug.Log(graph["input1_input"].ToString());
        //UnityEngine.Debug.Log(graph["dense1"].ToString());
        //UnityEngine.Debug.Log(graph["output1"].ToString());
        //UnityEngine.Debug.Log(graph["output1/Softmax"].ToString());

        return;
    }

    // Private method to convert input array to format the TensorFlow graph can accept
    private T[,] ListTo2DArray<T>(List<T> listToConvert)
    {
        T[,] arr = new T[1, listToConvert.Count];
        
        for(int index = 0; index < listToConvert.Count; index++) {
            arr[0,index] = listToConvert[index];
        }

        return arr;
    }

    // Private method to get the largest index from stochastic TensorFlow graph output
    private int getMaxIndex(float[,] results)
    {
        int maxIndex = 0;
        for(int i = 0; i < results.Length - 1; i++)
        {
            if(results[0,maxIndex] < results[0,i+1])
            {
                maxIndex = i + 1;
            }
        }
        return maxIndex;
    }
    #endregion

    #region Unused methods
    private void TestPlaceholders()
    {
        graph = new TFGraph();
        session = new TFSession(graph);

        // Get input tensors
        runner = session.GetRunner();
        var var_a = graph.Placeholder(TFDataType.Int16);
        var var_b = graph.Placeholder(TFDataType.Int16);

        var add = graph.Add(var_a, var_b);
        var mul = graph.Mul(var_a, var_b);

        runner.AddInput(var_a, new TFTensor((short)3));
        runner.AddInput(var_b, new TFTensor((short)2));
        UnityEngine.Debug.Log("a+b=" + runner.Run(add).GetValue());
        UnityEngine.Debug.Log("a*b=" + runner.Run(mul).GetValue());
    }

    void TestVariable()
    {
        UnityEngine.Debug.Log("Variables");
        var status = new TFStatus();
        using (var g = new TFGraph())
        {
            var initValue = g.Const(1.5);
            var increment = g.Const(0.5);
            TFOperation init;
            TFOutput value;
            var handle = g.Variable(initValue, out init, out value);

            // Add 0.5 and assign to the variable.
            // Perhaps using op.AssignAddVariable would be better,
            // but demonstrating with Add and Assign for now.
            var update = g.AssignVariableOp(handle, g.Add(value, increment));

            var s = new TFSession(g);
            // Must first initialize all the variables.
            s.GetRunner().AddTarget(init).Run(status);

            // Now print the value, run the update op and repeat
            // Ignore errors.
            for (int i = 0; i < 5; i++)
            {
                // Read and update
                var result = s.GetRunner().Fetch(value).AddTarget(update).Run();

                UnityEngine.Debug.Log("Result of variable read " + i + " -> " + result[0].GetValue());
            }
        }
    }
    #endregion
}
