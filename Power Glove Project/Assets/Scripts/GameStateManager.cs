using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to manage scene state behavior among three states:
// - Train: Scene is collecting and exporting labelled data
//              so that an ML model can be trained later
// - Calibrate: Scene is collecting data to set preprocessing
//              parameters before it can generate labels
// - Infer: Scene is running inference with the preprocessing
//          parameters and ML model to generate labels
public class GameStateManager : MonoBehaviour
{
    #region Members
    [Tooltip("ML agent to run inference.")]
    public TFSharpAgent agent;

    [Tooltip("Time interval for calibration in seconds.")]
    public float calibrateInterval = 5.0f;

    [Tooltip("Check if data is being collected for training.")]
    public bool isTraining = false;

    [Tooltip("True if data is currently calibrating.")]
    public bool isCalibrating = false;

    // Total time spent calibrating so far
    private float calibTime = 0;
    #endregion

    #region Private Methods
    private void Start()
    {
        // If the agent is not set up then collect training data
        isTraining = !agent.CanInfer;
        // If the agent is set up then calibrate before inference
        isCalibrating = agent.CanInfer;

        if (isTraining) Defs.Debug("Training...");
        if (isCalibrating) Defs.Debug("Calibrating...");
    }

    private void Update()
    {
        // Update time in calibration mode and try to exit
        // calibration on every loop
        if(isCalibrating)
        {
            calibTime += Time.deltaTime;
            isCalibrating = (calibTime < calibrateInterval);

            if (!isCalibrating) Defs.Debug("Calibration complete.");
        }
    }
    #endregion
}
