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
    public string portName = "COM6";
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
            print(glove.index_mcp);
            hand.fingers[Hand.INDEX].BendJoint(Finger.MCP, ScaleBend(glove.index_mcp));
            hand.fingers[Hand.INDEX].BendJoint(Finger.IP, ScaleBend(glove.index_pip));

            hand.fingers[Hand.MIDDLE].BendJoint(Finger.MCP, ScaleBend(glove.middle_mcp));
            hand.fingers[Hand.MIDDLE].BendJoint(Finger.IP, ScaleBend(glove.middle_pip));

            hand.fingers[Hand.RING].BendJoint(Finger.MCP, ScaleBend(glove.ring_mcp));
            hand.fingers[Hand.RING].BendJoint(Finger.IP, ScaleBend(glove.ring_pip));

            hand.fingers[Hand.PINKY].BendJoint(Finger.MCP, ScaleBend(glove.pinky_mcp));
            hand.fingers[Hand.PINKY].BendJoint(Finger.IP, ScaleBend(glove.pinky_pip));

            hand.thumb.BendJoint(Thumb.MCP, ScaleBend(glove.thumb_mcp));
            hand.thumb.BendJoint(Thumb.IP, ScaleBend(glove.thumb_pip));

            hand.spreadFingers(ScaleSpread(glove.index_hes), ScaleSpread(glove.ring_hes), ScaleSpread(glove.pinky_hes), ScaleSpread(glove.thumb_hes));
            //hand.spreadFingers(100, ScaleSpread(glove.ring_hes), ScaleSpread(glove.pinky_hes), ScaleSpread(glove.thumb_hes));

            //hand.RotateHand(glove.pitch, glove.roll, glove.yaw);
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

    private float ScaleBend(int num)
    {
        //Scale 140 - 220 to 0 - 90
        return (float)((num - 255f) * (90f / (1f - 255f)));
        //return -(float)((num - 140) * (90 / (220 - 140)));  // meme
        //float m = 0.08797f;
        //return m * (float)num;
    }

    private float ScaleSpread(int num)
    {
        //NEED TO DO
        //return an angle between 0 and somewhere around 30
        //Scale 40 - 140 to 0 - 40
        print(((num - 140f) * (40f / (40f - 140))));
        return (float)((num - 140f) * (40f / (40f - 140f)));
    }
}

