using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Leap_Motion
{
    public class UI_Manager : MonoBehaviour
    {
        public Text currentText;
        public Text nextText;
        public Text scoreText;
        private int Score = 0;
        private List<string> HandSigns = new List<string>();
        private string NeedToMake;
        void Start()
        {
            UpdateNeedToMake();
        }

        void Update()
        {

        }
        public UI_Manager()
        {
            HandSigns.Add("0");
            HandSigns.Add("1");
            HandSigns.Add("2");
            HandSigns.Add("3");
            HandSigns.Add("4");
            HandSigns.Add("5");
            HandSigns.Add("6");
            HandSigns.Add("7");
            HandSigns.Add("8");
            HandSigns.Add("9");
            
            UpdateNeedToMake();
            
        }

        public void UpdateHandValue(string value)
        {
            currentText.text = value;
            if (value.Equals(NeedToMake))
            {
                UpdateScore();
                UpdateNeedToMake();
            }
        }
        public void UpdateNeedToMake()
        {
            var random = new System.Random();
            var next = NeedToMake;
            while (next == null || next.Equals(NeedToMake))
                next = HandSigns[random.Next(HandSigns.Count)];

            NeedToMake = next;

            nextText.text = $"Need to make: {NeedToMake}";
        }
        public void UpdateScore()
        {
            Score++;
            scoreText.text = $"Score: {Score}";
        }
    }
}
