using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Tooltip("Check if data is being collected for training.")]
    public bool isTraining = false;

    [Tooltip("True if data is currently calibrating.")]
    public bool isCalibrating = false;
}
