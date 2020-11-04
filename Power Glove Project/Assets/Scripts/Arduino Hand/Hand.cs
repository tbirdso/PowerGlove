
using UnityEngine;

public class Hand
{
    //Hand hierarchy has to be in this order: Index, Middle, Ring, Pinky, Thumb
    public const int INDEX = 0;
    public const int MIDDLE = 1;
    public const int RING = 2;
    public const int PINKY = 3;
    public const int THUMB = 4;

    private readonly Transform wrist;
    public Transform Wrist => this.wrist;
    public Finger[] fingers;
    public Thumb thumb;

    private float xAngle;
    private float yAngle;
    private float zAngle;


    public Hand()
    {
        this.wrist = null;
        this.fingers = new Finger[4];
        this.thumb = null;

        this.xAngle = 0;
        this.yAngle = 0;
        this.zAngle = 0;
    }

    public Hand(Transform wristNode)
    {
        this.wrist = null;
        this.fingers = new Finger[4];
        this.thumb = null;

        this.xAngle = 0;
        this.yAngle = 0;
        this.zAngle = 0;

        if (wristNode != null)
        {
            //Starting from wrist, recursively traverse through the hand to find fingertips
            this.wrist = wristNode;
            InitFingers(this.wrist, 0);


            //Initilize hand at the origin
            this.wrist.localEulerAngles = Vector3.zero;
            this.wrist.position = Vector3.zero;
        }
    }

    private int InitFingers(Transform node, int fingerCount)
    {
        //Find leaf nodes in hand structure and initilize fingers
        //Assumes the hierarchy is in this order: Index, Middle, Ring, Pinky, Thumb
        if (node.childCount == 0 || node.GetChild(0) == null)
        {
            if(fingerCount < THUMB)
            {
                this.fingers[fingerCount] = new Finger(node, this, fingerCount);
            }
            else if(fingerCount == THUMB)
            {
                this.thumb = new Thumb(node, this);
            }

            return fingerCount + 1;
        }

        for (int i = 0; i < node.childCount; i++)
        {
            fingerCount = InitFingers(node.GetChild(i), fingerCount);
        }

        //Returns how many leaf nodes are found in hierarchy
        return fingerCount;
    }

    public void RotateHand(float pitch, float yaw, float roll)
    {
        if(this.wrist != null)
        {
            //Rotate the hand at the wrist

            this.wrist.Rotate(this.xAngle - roll, 0f, 0f, Space.Self);
            this.xAngle = roll;

            this.wrist.Rotate(0f, this.yAngle - yaw, 0f, Space.Self);
            this.yAngle = yaw;
            
            this.wrist.Rotate(0f, 0f, this.zAngle - pitch, Space.Self);
            this.zAngle = pitch;
        }
    }

    public void spreadFingers(float indexLoc, float ringLoc, float pinkyLoc, float thumbLoc)
    {
        //Handle finger spreading here because the hand object has access to all fingers
        //Will need to think about this and maybe write the function with the glove in hand for testing
    }

    public override string ToString()
    {
        string handData = "Index: " + this.fingers[INDEX].ToString();
        handData += "Middle: " + this.fingers[MIDDLE].ToString();
        handData += "Ring: " + this.fingers[RING].ToString();
        handData += "Pinky: " + this.fingers[PINKY].ToString();
        handData += "Thumb: " + this.thumb.ToString();
        handData += "Wrist rotation: " + (new Vector3(this.xAngle, this.yAngle, this.zAngle)).ToString() + "\n";
        return handData;
    }
}

