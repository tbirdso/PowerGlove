using Leap;
using Leap.Unity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private string csvThumbPath = $@"D:\ECE Clemson\Senior Design 2\Leap Sample Data\{DateTime.Now.Month}_{DateTime.Now.Day}_thumb.csv";
        private string csvIndexPath = $@"D:\ECE Clemson\Senior Design 2\Leap Sample Data\{DateTime.Now.Month}_{DateTime.Now.Day}_index.csv";
        private string csvMiddlePath = $@"D:\ECE Clemson\Senior Design 2\Leap Sample Data\{DateTime.Now.Month}_{DateTime.Now.Day}_middle.csv";
        private string csvRingPath = $@"D:\ECE Clemson\Senior Design 2\Leap Sample Data\{DateTime.Now.Month}_{DateTime.Now.Day}_ring.csv";
        private string csvPinkyPath = $@"D:\ECE Clemson\Senior Design 2\Leap Sample Data\{DateTime.Now.Month}_{DateTime.Now.Day}_pinky.csv";

        private bool SamplingDebug = false;

        void Start()
        {
            //Text sets your text to say this message
            this.m_MyText = gameObject.GetComponent<Text>();
            m_MyText.text = "Use Leap Motion Camera to display hand.";

        }

        void Update()
        {
            var handModel = GameObject.Find("RigidRoundHand_R");
            var script = handModel.GetComponent<RigidHand>();
            var hand = script.GetLeapHand();
            var fingers = hand.Fingers;

            if (SamplingDebug)
            {
                var csvDataThumb = ToCsv<Leap.Finger>(",", fingers.Where(x => x.Type.Equals(FingerType.TYPE_THUMB)), !File.Exists(csvThumbPath));
                var csvDataIndex = ToCsv<Leap.Finger>(",", fingers.Where(x => x.Type.Equals(FingerType.TYPE_INDEX)), !File.Exists(csvIndexPath));
                var csvDataMiddle = ToCsv<Leap.Finger>(",", fingers.Where(x => x.Type.Equals(FingerType.TYPE_MIDDLE)), !File.Exists(csvMiddlePath));
                var csvDataRing = ToCsv<Leap.Finger>(",", fingers.Where(x => x.Type.Equals(FingerType.TYPE_RING)), !File.Exists(csvRingPath));
                var csvDataPinky = ToCsv<Leap.Finger>(",", fingers.Where(x => x.Type.Equals(FingerType.TYPE_PINKY)), !File.Exists(csvPinkyPath));


                if (File.Exists(csvThumbPath))
                {
                    File.AppendAllText(csvThumbPath, csvDataThumb);
                }
                else
                {
                    File.WriteAllText(csvThumbPath, csvDataThumb);
                }

                if (File.Exists(csvIndexPath))
                {
                    File.AppendAllText(csvIndexPath, csvDataIndex);
                }
                else
                {
                    File.WriteAllText(csvIndexPath, csvDataIndex);
                }

                if (File.Exists(csvMiddlePath))
                {
                    File.AppendAllText(csvMiddlePath, csvDataMiddle);
                }
                else
                {
                    File.WriteAllText(csvMiddlePath, csvDataMiddle);
                }

                if (File.Exists(csvRingPath))
                {
                    File.AppendAllText(csvRingPath, csvDataRing);
                }
                else
                {
                    File.WriteAllText(csvRingPath, csvDataRing);
                }

                if (File.Exists(csvPinkyPath))
                {
                    File.AppendAllText(csvPinkyPath, csvDataPinky);
                }
                else
                {
                    File.WriteAllText(csvPinkyPath, csvDataPinky);
                }
            }
            //Press the space key to change the Text message
            if (true || Input.GetKey(KeyCode.Space))
            {
                this.m_MyText = gameObject.GetComponent<Text>();

                //1
                if(!fingers.Where(x => (x.Type.Equals(FingerType.TYPE_THUMB))).First().IsExtended &&
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
        public string ToCsv<T>(string separator, IEnumerable<T> objectlist, bool includeHeaders)
        {
            StringBuilder csvdata = new StringBuilder();
            
            Type t = typeof(T);
            FieldInfo[] fields = t.GetFields();

            if (includeHeaders)
            {
                string header = String.Join(separator, fields.Select(f => f.Name).ToArray());
                csvdata.AppendLine(header);
            }
            foreach (var o in objectlist)
                csvdata.AppendLine(ToCsvFields(separator, fields, o));

            return csvdata.ToString();
        }

        public string ToCsvFields(string separator, FieldInfo[] fields, object o)
        {
            StringBuilder linie = new StringBuilder();

            foreach (var f in fields)
            {
                if (linie.Length > 0)
                    linie.Append(separator);

                var x = f.GetValue(o);

                if (x != null)
                    linie.Append(x.ToString());
            }

            return linie.ToString();
        }
    }
}




