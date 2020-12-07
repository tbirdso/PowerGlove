using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finger
{
    public const int IP = 0;
    public const int DIP = 0;
    public const int PIP = 1;
    public const int MCP = 2;
    public const int boneChainLength = 3;

    private readonly Transform[] joints;

    public Finger()
    {
        this.joints = null;
    }

    public Finger(Transform fingerTip)
    {
        this.joints = null;
        if (fingerTip != null) { 

            //Initialize joint transform references
            this.joints = new Transform[boneChainLength];
            Transform currentJoint = fingerTip.parent;

            for (int i = 0; i < boneChainLength && currentJoint != null; i++)
            {
                this.joints[i] = currentJoint.transform;
                currentJoint = currentJoint.parent;
            }
        }
    }

    public void BendJoint(int jointNum, float angle)
    {
        if (this.joints != null)
        {
            if (jointNum == MCP)
            {
                //Preserve the finger spread when bending joints
                float yAngle = this.joints[MCP].transform.localEulerAngles.y;
                this.joints[MCP].transform.localEulerAngles = new Vector3(0.0f, yAngle, -angle);
            }
            else
            {
                //Bend both IP joints
                this.joints[DIP].transform.localEulerAngles = new Vector3(0.0f, 0.0f, -angle);
                this.joints[PIP].transform.localEulerAngles = new Vector3(0.0f, 0.0f, -angle);
            }
        }
    }

    public void SpreadFinger(float angle)
    {
        if(this.joints != null)
        {
            //Preserve joint bend when spreading
            float zAngle = this.joints[MCP].transform.localEulerAngles.z;
            this.joints[MCP].transform.localEulerAngles = new Vector3(0.0f, angle, zAngle);
        }
    }

    public void SpreadFinger(Finger otherFinger)
    {
        //Place holder for creating anlges between two fingers
    }

    public override string ToString()
    {
        if (this.joints != null)
        {
            float dipAngle = this.joints[DIP].transform.localEulerAngles.z;
            float pipAngle = this.joints[PIP].transform.localEulerAngles.z;
            float mcpAngle = this.joints[MCP].transform.localEulerAngles.z;
            float spreadAngle = this.joints[MCP].transform.localEulerAngles.y;
            return "DIP:" + dipAngle + " PIP:" + pipAngle + " MCP:" + mcpAngle + " Spread:" + spreadAngle + "\n";
        }
        return "No finger\n";
    }
}
