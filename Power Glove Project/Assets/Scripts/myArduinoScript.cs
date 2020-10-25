using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class myArduinoScript : MonoBehaviour
    {
        SerialPort sp = new SerialPort("COM3", 115200);
        public Text m_MyText;
        int count = 0;
        DateTime reference;
        void Start()
        {
            sp.Open();
            sp.ReadTimeout = 10000;
            reference = DateTime.Now;
        }

        void Update()
        {
            try
            {
                this.m_MyText = gameObject.GetComponent<Text>();
                if (!sp.IsOpen)
                    sp.Open();
                var JsonString = GetJSONstring();
                var glove = (PowerGlove)JsonConvert.DeserializeObject(JsonString, typeof(PowerGlove));

                count++;
                if(DateTime.Now - reference > new TimeSpan(0, 0, 1))
                {
                    reference = DateTime.Now;
                    print($"JSON objects sent per second {count}");
                    m_MyText.text = $"JSON_objects/sec {count}";
                    count = 0;
                }
            }
            catch (System.Exception ex)
            {
                throw;                
            }
        }
        string GetJSONstring() 
        {
            var serialBuffer = "";
            while(!serialBuffer.Equals("{"))
            {
                serialBuffer = sp.ReadLine();
            }
            var count = 1;
            while(count != 0)
            {
                var value = sp.ReadLine();
                serialBuffer += value;
                if (value.Equals("{"))
                    count++;
                if (value.Equals("}"))
                    count--;
            }
            return serialBuffer;
        }
    }
    public class PowerGlove
    {
        [JsonProperty("a")] public int IndexFlex1 { get; set; }
        [JsonProperty("b")] public int IndexFlex2 { get; set; }
        [JsonProperty("c")] public int MiddleFlex1 { get; set; }
        [JsonProperty("d")] public int MiddleFlex2 { get; set; }
        [JsonProperty("e")] public int RingFlex1 { get; set; }
        [JsonProperty("f")] public int RingFlex2 { get; set; }
        [JsonProperty("g")] public int PinkyFlex1 { get; set; }
        [JsonProperty("h")] public int PinkyFlex2 { get; set; }
        [JsonProperty("i")] public int ThumbFlex1 { get; set; }
        [JsonProperty("j")] public int ThumbFlex2 { get; set; }
        [JsonProperty("k")] public int HallEffect1 { get; set; }
        [JsonProperty("l")] public int HallEffect2 { get; set; }
        [JsonProperty("m")] public int HallEffect3 { get; set; }
        [JsonProperty("n")] public int Placeholder1 { get; set; }
        [JsonProperty("o")] public int Placeholder2 { get; set; }
        [JsonProperty("p")] public int Placeholder3 { get; set; }
        [JsonProperty("q")] public int Placeholder4 { get; set; }
        [JsonProperty("r")] public int Placeholder5 { get; set; }
        [JsonProperty("s")] public int Placeholder6 { get; set; }
        [JsonProperty("t")] public int Placeholder7 { get; set; }
        [JsonProperty("u")] public int Placeholder8 { get; set; }
        [JsonProperty("v")] public int Placeholder9 { get; set; }
        [JsonProperty("w")] public int Placeholder10 { get; set; }
        [JsonProperty("x")] public int Placeholder11 { get; set; }
    }
}
