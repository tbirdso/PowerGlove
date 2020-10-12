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
        public Text m_MyText;
        public LeapMotionASLAgent agent;
        public bool UseML = true;

        void Start()
        {
            //Text sets your text to say this message
            this.m_MyText = gameObject.GetComponent<Text>();
            m_MyText.text = "Use Leap Motion Camera to display hand.";
        }

        void Update()
        {
            if (UseML)
            {
                this.m_MyText = gameObject.GetComponent<Text>();
                m_MyText.text = agent.RunInference().ToString();
            }
            else
            {
                //Press the space key to change the Text message
                if (true || Input.GetKey(KeyCode.Space))
                {
                    this.m_MyText = gameObject.GetComponent<Text>();
                    var handModel = GameObject.Find("RigidRoundHand_R");
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
                        this.m_MyText.text = "1";
                    }
                    //2
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "2";
                    }
                    //3
                    else if (fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "3";
                    }
                    //4
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "4";
                    }
                    //5
                    else if (fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "5";
                    }
                    //6
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "6";
                    }
                    //7
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "7";
                    }
                    //8
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "8";
                    }
                    //9
                    else if (!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "9";
                    }
                    //10
                    else if (fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_INDEX))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_MIDDLE))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_RING))).First().IsExtended &&
                       !fingers.Where(x => (x.Type.Equals(FingerType.TYPE_PINKY))).First().IsExtended)
                    {
                        this.m_MyText.text = "10";
                    }
                    else //unknown
                    {
                        this.m_MyText.text = "UNKNOWN";
                    }
                }
            }
        }
    }
}




