using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public const int INDEX = 0;
    public const int MIDDLE = 1;
    public const int RING = 2;
    public const int PINKY = 3;

    public Transform[] fingerTips = new Transform[4];
    public Finger[] fingers;


    void Start() //Called before the first frame
    {
        //finger
        fingers = new Finger[4];
        for(int i = 0; i < 4; i++)
        {
            fingers[i] = new Finger(fingerTips[i]);
        }

        /*
        //Example of moving the index finger

        fingers[INDEX].SpreadFinger(-40);
        fingers[INDEX].BendJoint(Finger.IP, 60);
        fingers[INDEX].BendJoint(Finger.MCP, 30);
        */

    }

    void OnConnectionEvent(bool success) //Called when attempting to connect to COM port
    {
        if (success)
        {
            print("Connected\n");
        }
        else
        {
            print("Connection Failure\n");
        }
    }

    void OnMessageArrived(string msg) //Called when a line is received on the COM port
    {
        /*
         * ARDUINO CODE USED TO SEND DATA
         
            int IP;
            int MCP;
            int analogIP = A0;
            int analogMCP = A3;
            int threshold = 3;

            void setup() {
              // put your setup code here, to run once:
              pinMode(analogIP, INPUT);
              pinMode(analogMCP, INPUT);
              Serial.begin(9600);
            }

            void loop() {
              //Send IP Joint when change is greater than threshold
              int temp = analogRead(analogIP);
              if(abs(IP - temp) > threshold){
                IP = temp;
                Serial.print("1,");
                Serial.print(IP);
                Serial.println();
              }

              //Send MCP Joint when change is greater than threshold
              temp = analogRead(analogMCP);
              if(abs(MCP - temp) > threshold){
                MCP = temp;
                Serial.print("2,");
                Serial.print(MCP);
                Serial.println();
              }
  
              delay(10);
            }

        */

        //Message is formatted as x,Y where x is the joint and Y is the unscaled value for the joint
        int joint = int.Parse(msg.Split(',')[0]);
        int val = int.Parse(msg.Split(',')[1]);

        print(joint + "     " + val + "\n");

        if(joint == 1)
        {
            fingers[INDEX].BendJoint(Finger.IP, ScaleNum(val));
        }
        else if(joint == 2)
        {
            fingers[INDEX].BendJoint(Finger.MCP, ScaleNum(val));
        }
    }

    private float ScaleNum(int num)
    {
        //Scale 0 - 1023 to 0 - 90
        float m = 0.08797f;
        return m * (float)num;
    }

    void Update() //Called every frame
    {

    }
}
