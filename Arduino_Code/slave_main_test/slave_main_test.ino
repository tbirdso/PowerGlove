#define NUMFLEXSENSORS 2  // number of 
#define CALIBRATE_TIME 5  // in seconds

#include <Arduino_LSM9DS1.h>



//UART mySerial(digitalPinToPinName(11), digitalPinToPinName(12), NC, NC);  //RX TX

  // finger_joint_type | i.e. middle_mcp_out
int thumb_mcp = A3;
int thumb_pip = A2;
int thumb_hes = A7;

int index_mcp = A1;
int index_pip = A5;

int middle_mcp = A0;
int middle_pip = A4;

float x, y, z;
int xAcc, yAcc, zAcc;
int xGyro, yGyro, zGyro;
int val;

void setup(void) {
  //Serial.begin(9600); 

  pinMode(thumb_mcp, INPUT);
  pinMode(thumb_pip, INPUT);
  pinMode(thumb_hes, INPUT);
  pinMode(index_mcp, INPUT);
  pinMode(index_pip, INPUT);
  pinMode(middle_mcp, INPUT);
  pinMode(middle_pip, INPUT);

  Serial.begin(9600);  
  delay(100);

  if (!IMU.begin()){  //IMU check
    Serial.println("Failed to initialize IMU!");
    exit(1);
  }
}

void loop(void) {

  // Scaling to 8-255 to prevent a sensor from sending an ID number
 
  // Couldn't we just send one ID number start the line and 
  // then use the same order for the data?

  if(IMU.accelerationAvailable()){ //the axis values x, y, and z range from -1 to 1
    IMU.readAcceleration(x, y, z);
    xAcc = map(x, -1, 1, 8, 255);      //scaled now at 8 to 255
    yAcc = map(y, -1, 1, 8, 255);      //negative can't be sent on mySerial
    zAcc = map(z, -1, 1, 8, 255);      
  }

  if(IMU.gyroscopeAvailable()){
    IMU.readGyroscope(x, y, z);
    xGyro = map(x, -1000, 1000, 8, 255);      //scaled now at 8 to 255
    yGyro = map(y, -1000, 1000, 8, 255);      //negative can't be sent on mySerial
    zGyro = map(z, -1000, 1000, 8, 255); 
  }
  

  // If received polling request from master
  if (Serial.available()){
    Serial.read();
    Serial.read();

    
    Serial.println(0);
    Serial.println(xAcc);              //Accelerometer data unlabeled
    Serial.println(yAcc);
    Serial.println(char(zAcc));
    Serial.println(char(xGyro));              //Accelerometer data unlabeled
    Serial.println(char(yGyro));
    Serial.println(char(zGyro));
    

    val = map(analogRead(thumb_mcp), 0, 1023, 8, 255);
    Serial.println(char(1));
    Serial.println(char(val));

    val = map(analogRead(thumb_pip), 0, 1023, 8, 255);
    Serial.println(char(2));
    Serial.println(char(val));

    val = map(analogRead(thumb_hes), 0, 1023, 8, 255);
    Serial.println(char(3));
    Serial.println(char(val));

    val = map(analogRead(index_mcp), 0, 1023, 8, 255);
    Serial.println(char(4));
    Serial.println(char(val));

    val = map(analogRead(index_pip), 0, 1023, 8, 255);
    Serial.println(char(5));
    Serial.println(char(val));

    val = map(analogRead(middle_mcp), 0, 1023, 8, 255);
    Serial.println(char(6));
    Serial.println(char(val));

    val = map(analogRead(middle_pip), 0, 1023, 8, 255);
    Serial.println(char(7));
    Serial.println(char(val));
  }
}
