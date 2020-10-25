using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform wrist;
    public Hand hand;

    /* Moving Fingers
     hand.fingers[FINGER OPTION].BendJoint(JOINT OPTION, ANGLE);
     hand.fingers[FINGER OPTION].SpreadFinger(ANGLE);

     FINGER 0PTION = Hand.INDEX, Hand.MIDDLE, Hand.RING, or Hand.PINKY
     JOINT OPTION = Finger.IP or Finger.MCP
     ANGLE = 0 to 90 degrees (you can put any angle, but outside of this range is pretty much physically impossible for a real hand)
     */

    /* Moving Thumb 
    hand.thumb.BendJoint(JOINT OPTION, ANGLE);
    hand.thumb.OpposeThumb(ANGLE);

    JOINT OPTION = Thumb.IP or Thumb.MCP
    ANGLE = 0 to 90 degrees
     */

    /* Moving Hand 
    hand.RotateHand(AXIS, ANGLE);

    AXIS = Vector3.right for x-axis in world coords or Vector3.forward for z-axis in world coords
    ANGLE = Angle in degrees
     */

    void Start() //Called before the first frame
    {
        hand = new Hand(wrist);
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
        /* ARDUINO CODE USED TO SEND DATA
         
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

        if (joint == 1)
        {
            hand.fingers[Hand.INDEX].BendJoint(Finger.IP, ScaleNum(val));
            hand.fingers[Hand.MIDDLE].BendJoint(Finger.IP, ScaleNum(val));
            hand.fingers[Hand.RING].BendJoint(Finger.IP, ScaleNum(val));
            hand.fingers[Hand.PINKY].BendJoint(Finger.IP, ScaleNum(val));
            hand.thumb.BendJoint(Thumb.IP, ScaleNum(val));
        }
        else if(joint == 2)
        { 
            hand.fingers[Hand.INDEX].BendJoint(Finger.MCP, ScaleNum(val));
            hand.fingers[Hand.MIDDLE].BendJoint(Finger.MCP, ScaleNum(val));
            hand.fingers[Hand.RING].BendJoint(Finger.MCP, ScaleNum(val));
            hand.fingers[Hand.PINKY].BendJoint(Finger.MCP, ScaleNum(val));
            hand.thumb.BendJoint(Thumb.MCP, ScaleNum(val));
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
