#define NUMFLEXSENSORS 2  // number of 
#define CALIBRATE_TIME 5  // in seconds
#define ACC_SENS 0.122    // mg
#define GYRO_SENS 70      // mdps

#include <Arduino_LSM9DS1.h>

UART mySerial(digitalPinToPinName(11), digitalPinToPinName(12), NC, NC);  //RX TX

  // finger_joint_type | i.e. middle_mcp_out
int thumb_mcp = A3;
int thumb_pip = A2;
int thumb_hes = A7;

int index_mcp = A1;
int index_pip = A5;

int middle_mcp = A0;
int middle_pip = A4;

float d_x, d_y, d_z;
float d_pitch, d_roll, d_yaw;
float pitch=0, roll=0, yaw=0;
float xSum, ySum, zSum;
int xAcc, yAcc, zAcc;
int xGyro, yGyro, zGyro;
int val;

unsigned long t = 0;
unsigned long time_elapsed;

void setup(void) {
  Serial.begin(9600); 

  pinMode(thumb_mcp, INPUT);
  pinMode(thumb_pip, INPUT);
  pinMode(thumb_hes, INPUT);
  pinMode(index_mcp, INPUT);
  pinMode(index_pip, INPUT);
  pinMode(middle_mcp, INPUT);
  pinMode(middle_pip, INPUT);
  pinMode(13, OUTPUT);

  mySerial.begin(9600);  
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
  time_elapsed = millis() - t;

  if(IMU.accelerationAvailable()){ //the axis values x, y, and z range from -1 to 1
    IMU.readAcceleration(d_x, d_y, d_z);
    xAcc = map(d_x, -1, 1, 1, 255);      //scaled now at 1 to 255
    yAcc = map(d_y, -1, 1, 1, 255);      //negative can't be sent on mySerial
    zAcc = map(d_z, -1, 1, 1, 255);
  }

  if(IMU.gyroscopeAvailable()){
    IMU.readGyroscope(d_pitch, d_roll, d_yaw);
//    x = x * 1000 * time_elapsed; // in deg/ms
//    y = y * 1000 * time_elapsed;
//    z = z * 1000 * time_elapsed;
//    xSum += x; 
//    ySum += y; 
//    zSum += z; 
  }

  pitch += (d_pitch / GYRO_SENS) * time_elapsed;
  roll += (d_roll / GYRO_SENS) * time_elapsed;
  yaw += (d_yaw / GYRO_SENS) * time_elapsed;

  pitch = 0.98*(pitch) + 0.02*(atan2(d_y, d_z)*180/PI);
  roll = 0.98*(roll) + 0.02*(atan2(d_x, d_z)*180/PI);
  //yaw = 0.98*(yaw) + 0.02*(atan2(d_x, d_y)*180/PI);
  Serial.print(pitch);
  Serial.print(" ");
  Serial.println(roll);
//  Serial.print(" ");
//  Serial.println(yaw);

  
  
//  Serial.print(map(analogRead(thumb_mcp), 630, 820, 1, 255));
//  Serial.print("\t");
//  Serial.print(map(analogRead(thumb_pip), 680, 860, 1, 255));
//  Serial.print("\t");
//  Serial.print(map(analogRead(index_mcp), 630, 860, 1, 255));
//  Serial.print("\t");
//  Serial.print(map(analogRead(index_pip), 620, 830, 1, 255));
//  Serial.print("\t");
//  Serial.print(map(analogRead(middle_mcp), 700, 940, 1, 255));
//  Serial.print("\t");
//  Serial.print(analogRead(thumb_hes));
//  Serial.println("\t");
  
  
//Serial.print(map(analogRead(pinky_pip), 470, 626, 1, 255));
//  Serial.print("\t");
  // If received polling request from master
  if (mySerial.available()){
    mySerial.read();    
    digitalWrite(13, HIGH);

    
    mySerial.write(char(0));
    mySerial.write(char(xAcc));              //Accelerometer data unlabeled
    mySerial.write(char(yAcc));
    mySerial.write(char(zAcc));
    xGyro = map(pitch, -180, 180, 1, 255);      //scaled now at 1 to 255
    yGyro = map(roll, -180, 180, 1, 255);      //negative can't be sent on mySerial
    zGyro = map(yaw, -180, 180, 1, 255); 
    mySerial.write(char(xGyro));              //Accelerometer data unlabeled
    mySerial.write(char(yGyro));
    mySerial.write(char(zGyro));
    

    val = map(analogRead(thumb_mcp), 630, 820, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));

    val = map(analogRead(thumb_pip), 680, 860, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));

    val = map(analogRead(thumb_hes), 160, 720, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));

    val = map(analogRead(index_mcp), 630, 860, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));

    val = map(analogRead(index_pip), 620, 830, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));

    val = map(analogRead(middle_mcp), 700, 940, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));

    val = map(analogRead(middle_pip), 620, 795, 1, 255);
    if (val < 1) val = 1;
    if (val > 255) val = 255;
    mySerial.write(char(val));
  }
  //delay(50);
  t = millis();
  digitalWrite(13, LOW);
}
