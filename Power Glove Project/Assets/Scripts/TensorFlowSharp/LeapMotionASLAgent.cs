/* Filename:    TestTFSharpAgent.cs
 * Written by:  Tom Birdsong
 * Course:      ECE 4960 Fall 2020
 * Purpose:     Test realtime inference with agent executing TensorFlow graph
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.Diagnostics;
using System.IO;
using Leap;
using Leap.Unity;
using static Leap.Finger;

public class LeapMotionASLAgent : TFSharpAgent
{
    [Tooltip("Leap Motion hand to get values for labelling.")]
    public GameObject trackedHand;

    // Reference to Leap.Hand object for tracking
    private Leap.Hand leapHand;

    // Start is called before the first frame update
    void Start()
    {
        if (trackedHand == null)
        {
            UnityEngine.Debug.Log("Could not get tracked hand!");
        } else
        {
            leapHand = trackedHand.GetComponentInChildren<RigidHand>().GetLeapHand();
        }
    }

    void Update()
    {
        if(isDebug)
        {
            int label = RunInference();
            UnityEngine.Debug.Log("Label is " + label.ToString());
        }
    }

    #region Public Methods

    public int RunInference()
    {
        List<float> inputs = new List<float>();

        // Fingers must be passed in the same default order
        // as used for training: thumb, index, middle, ring, pinky
        foreach(FingerType ftype in Enum.GetValues(typeof(FingerType)))
        {
            Leap.Finger finger = leapHand.Fingers.Find(x => x.Type.Equals(ftype));

            inputs.Add(finger.TipPosition.x);
            inputs.Add(finger.TipPosition.y);
            inputs.Add(finger.TipPosition.z);
            inputs.Add(finger.Direction.x);
            inputs.Add(finger.Direction.y);
            inputs.Add(finger.Direction.z);
            inputs.Add(finger.Width);
            inputs.Add(finger.Length);
        }

        return base.RunInference(inputs);
    }

    #endregion
}
