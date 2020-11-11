using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HandController : MonoBehaviour
{
    public Transform wrist;
    public string portName = "COM5";
    public int baudRate = 115200;

    public GloveTrainingBuffer buf;
    public TFSharpAgent agent;

    private Hand hand;
    private SerialPort sp;

    void Start() //Called before the first frame
    {
        //Init serial port
        sp = new SerialPort(portName, baudRate);
        sp.Open();
        sp.ReadTimeout = 10000;

        hand = new Hand(wrist);
    }

    void Update() //Called every frame
    {
        //Poll the serial port
        try
        {
            if (!sp.IsOpen)
            {
                sp.Open();
            }

            var JsonString = GetJSONstring();
            if(JsonString == null) //Ignore Json strings that had errors
            {
                return;
            }
            var glove = (PowerGlove)JsonConvert.DeserializeObject(JsonString, typeof(PowerGlove));

            // thumb_mcp	 thumb_pip	 thumb_hes	 index_mcp	 index_pip	 middle_mcp	 middle_pip
            if (buf != null) buf.AddData(glove);

            if (agent != null)
            {
                var result = agent.RunInference(new List<float>() { glove.thumb_mcp, glove.thumb_pip, glove.thumb_hes, glove.index_mcp, glove.index_pip, glove.middle_mcp, glove.middle_pip });
                Defs.Debug(result.ToString());
            }

            //Write data from Json pacakge to the hand
            hand.thumb.BendJoint(Thumb.MCP, ScaleNum(glove.thumb_mcp));
            hand.thumb.BendJoint(Thumb.IP, ScaleNum(glove.thumb_pip));
            hand.fingers[Hand.INDEX].BendJoint(Finger.MCP, ScaleNum(glove.index_mcp));
            hand.fingers[Hand.INDEX].BendJoint(Finger.IP, ScaleNum(glove.index_pip));
            hand.fingers[Hand.MIDDLE].BendJoint(Finger.MCP, ScaleNum(glove.middle_mcp));
            hand.fingers[Hand.MIDDLE].BendJoint(Finger.IP, ScaleNum(glove.middle_pip));
            //hand.fingers[Hand.RING].BendJoint(Finger.MCP, ScaleNum(glove.ring_mcp));
            //hand.fingers[Hand.RING].BendJoint(Finger.IP, ScaleNum(glove.ring_pip));
            //hand.fingers[Hand.PINKY].BendJoint(Finger.MCP, ScaleNum(glove.pinky_mcp));
            //hand.fingers[Hand.PINKY].BendJoint(Finger.IP, ScaleNum(glove.pinky_pip));


            ////Need to write this function, will probably need to have the glove to test as I go
            //hand.spreadFingers(ScaleNum(glove.index_hes), ScaleNum(glove.ring_hes), ScaleNum(glove.pinky_hes), ScaleNum(glove.thumb_hes));

            ////Manually spread a single finger wihtout regard to the other fingers, will be replaced with above function
            //hand.fingers[Hand.INDEX].SpreadFinger(ScaleNum(glove.index_hes));
            //hand.fingers[Hand.RING].SpreadFinger(ScaleNum(glove.ring_hes));
            //hand.fingers[Hand.PINKY].SpreadFinger(ScaleNum(glove.pinky_hes));
            //hand.thumb.SpreadThumb(ScaleNum(glove.thumb_hes));

            ////May need to rewrite this function. I cannot tell if it is working with potentiometers, I need the gyroscope in hand to test
            //hand.RotateHand(ScaleNum(glove.pitch), ScaleNum(glove.yaw), ScaleNum(glove.roll));
        }
        catch (System.Exception ex)
        {
            throw;
        }
    }

    private string GetJSONstring()
    {
        string serialBuffer = "";
        while (!serialBuffer.Equals("{")) //Look for start of Json string
        {
            serialBuffer = sp.ReadLine();
        }

        int count = 1;
        while (true)
        {
            string value = sp.ReadLine();
            serialBuffer += value;
            if (value.Equals("{")) //If another open bracket is found, return null
            {
                return null;
            }

            if (value.Equals("}")) //Break out of the loop and return the Json string
            {
                break;
            }

            count++;
            if(count >= PowerGlove.size) //If the closing bracket is not found when it should be, return null
            {
                return null;
            }
        }

        return serialBuffer;
    }

    private float ScaleNum(int num)
    {
        //Scale 140 - 220 to 0 - 90
        return (float)((num - 220) * (90 / (140 - 220)));
        //return -(float)((num - 140) * (90 / (220 - 140)));  // meme
        //float m = 0.08797f;
        //return m * (float)num;
    }
}

