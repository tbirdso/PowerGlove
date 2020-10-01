using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.Diagnostics;

public class TestAgent : MonoBehaviour
{
    const int NUM_INPUTS = 23;

    // Drag graph model from Resources onto script in the Editor
    public TextAsset graphModel;

    // Test by setting input vector directly
    public List<float> inputs = new List<float>() { 0f, 0f, 0f, 0f };

    // Persistent TensorFlow graph
    private TFGraph graph;
    private TFSession session;
    private TFSession.Runner runner;

    // Start is called before the first frame update
    void Start()
    {
        LoadTensorFlowGraph();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Private Methods
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

    // Follow tutorial at https://github.com/llSourcell/Unity_ML_Agents/blob/master/docs/Using-TensorFlow-Sharp-in-Unity-(Experimental).md
    private void LoadTensorFlowGraph()
    {
        // Recreate the graph in Unity
        graph = new TFGraph();
        graph.Import(graphModel.bytes);
        session = new TFSession(graph);

        UnityEngine.Debug.Log(graph["sequential/output1/Softmax"].ToString());
        UnityEngine.Debug.Log(graph["sequential/dense1/Relu"].ToString());
        UnityEngine.Debug.Log(graph["x"].ToString());

        //UnityEngine.Debug.Log(graph["input1_input"].ToString());
        //UnityEngine.Debug.Log(graph["dense1"].ToString());
        //UnityEngine.Debug.Log(graph["output1"].ToString());
        //UnityEngine.Debug.Log(graph["output1/Softmax"].ToString());

        // Get input tensors
        runner = session.GetRunner();
        //TODO how to handle placeholders?
        var placeholder_value1 = graph.Placeholder(TFDataType.Int32);
        var placeholder_value2 = graph.Placeholder(TFDataType.Int32);
        var placeholder_value3 = graph.Placeholder(TFDataType.Int32);
        var placeholder_value4 = graph.Placeholder(TFDataType.Int32);

        // runner.AddInput(graph["input_placeholder_name"][0], new float[] { placeholder_value1, placeholder_value2 });
        runner.AddInput(graph["x"][0], new float[,] { { inputs[0], inputs[1], inputs[2], inputs[3] } });
        //TODO need 23 placeholders for input vector

        // Retrieve output
        runner.Fetch(graph["sequential/output1/Softmax"][0]);
        //TODO example is for 2D tensor of floats
        var output = runner.Run();
        var vecResults = output[0].GetValue();
        float[,] results = (float[,])vecResults;

        for (int i = 0; i < 6; i++)
        {
            UnityEngine.Debug.Log("Output " + i + " is " + results[0, i].ToString());
        }

        UnityEngine.Debug.Log("The response for input " + 
            inputs[0].ToString() + inputs[1].ToString() + inputs[2].ToString() + inputs[3].ToString() + 
            " is " + getMaxIndex(results));

    }

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
}
