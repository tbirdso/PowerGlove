using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;

public class TestAgent : MonoBehaviour
{
    const int NUM_INPUTS = 23;

    // Drag graph model from Resources onto script in the Editor
    public TextAsset graphModel;

    // Persistent TensorFlow graph
    private TFGraph graph;
    private TFSession session;
    private TFSession.Runner runner;

    // Start is called before the first frame update
    void Start()
    {
        TestVariable();
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

        // Get input tensors
        runner = session.GetRunner();
        //TODO how to handle placeholders?
        var placeholder_value1 = graph.Placeholder(TFDataType.Int16);
        var placeholder_value2 = graph.Placeholder(TFDataType.Int16);

        // runner.AddInput(graph["input_placeholder_name"][0], new float[] { placeholder_value1, placeholder_value2 });
        //runner.AddInput(graph["sensor1_input"][0], new float[] { placeholder_value1, placeholder_value2 });
        //TODO need 23 placeholders for input vector

        // Retrieve output
        runner.Fetch(graph["output_placeholder_name"][0]);
        //TODO example is for 2D tensor of floats
        float recurrent_tensor = (float)runner.Run()[0].GetValue();
    }


    #endregion
}
