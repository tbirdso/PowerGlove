using Assets.Scripts.Leap_Motion;
using Leap;
using Leap.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using static Leap.Finger;

namespace Assets.Plugins.LeapMotion.Core.Scripts
{
    public class myHandScript : MonoBehaviour
    {
        public Text currentText;
        public Text nextText;
        public Text scoreText;
        public LeapMotionASLAgent agent;
        public GameObject RightHandModel;
        public bool UseML = true;
        public UI_Manager UI;

        void Start()
        {
            this.currentText.text = "Use Leap Motion Camera to display hand.";
        }

        void Update()
        {
            if (UseML)
            {
                //Make sure hand is rendered
                var handModel = GameObject.Find("RigidRoundHand_R");
                if (handModel == null)
                {
                    this.currentText.text = "Use Leap Motion Camera to display hand.";
                    return;
                }
                var script = handModel.GetComponent<RigidHand>();
                var hand = script.GetLeapHand();

                var value = agent.RunInference(hand).ToString();
                UI.UpdateHandValue(value);

            }
            else
            {
                //Press the space key to change the Text message
                if (true || Input.GetKey(KeyCode.Space))
                {
                    //var text = GameObject.Find("Current Hand Sign").GetComponent<Text>();
                    var handModel = GameObject.Find("RigidRoundHand_R");
                    if (handModel == null)
                    {
                        this.currentText.text = "Use Leap Motion Camera to display hand.";
                        return;
                    }
                    var script = handModel.GetComponent<RigidHand>();
                    var hand = script.GetLeapHand();
                    var fingers = hand.Fingers;

                    //1
                    if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "1";
                    }
                    //2
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "2";
                    }
                    //3
                    else if (fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "3";
                    }
                    //4
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "4";
                    }
                    //5
                    else if (fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "5";
                    }
                    //6
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "6";
                    }
                    //7
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "7";
                    }
                    //8
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "8";
                    }
                    //9
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "9";
                    }
                    //10
                    else if (fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.currentText.text = "10";
                    }
                    else //unknown
                    {
                        this.currentText.text = "UNKNOWN";
                    }
                    UI.UpdateHandValue(this.currentText.text);
                }
            }
        }
    }
}




