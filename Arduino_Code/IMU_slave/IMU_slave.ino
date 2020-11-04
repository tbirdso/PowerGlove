/*  Connect 2 -> 2
 *  Connect 3 -> 3
 */

#include <Arduino_LSM9DS1.h>

UART mySerial(digitalPinToPinName(3), digitalPinToPinName(2), NC, NC);  //RX TX

float x, y, z = 0;  //Accelerometer axis values
float xScaled, yScaled, zScaled; 
void setup()
{

  if (!IMU.begin())   //IMU check
  {
    Serial.println("Failed to initialize IMU!");
    exit(1);
  }
  // Open serial communications and wait for port to open:
  mySerial.begin(9600);  
  delay(100);
  
}

void loop()
{
  if(IMU.accelerationAvailable()) //the axis values x, y, and z range from -1 to 1
  {
    IMU.readAcceleration(x, y, z);
    xScaled = (x*90) + 90;              //scaled now at 0 to 180
    yScaled = (y*90) + 90;              //negative can't be sent on mySerial
    zScaled = (z*90) + 90;              //Values will be adjusted in Master code
  }
  
  delay(50);
  mySerial.write(xScaled);              //Accelerometer data unlabeled
  mySerial.write(yScaled);
  mySerial.write(zScaled);

  delay(500);
  
  /* Gyroscope: 
   *  measures change in motion more than orientation
   *  but here it is if you want to try it out
  
  if(IMU.gyroscopeAvailable){
    IMU.readGyroscope(x, y, z);
    
  }
  Values are usually pretty high. Example code used a threshold of 400 */
  
  /* Magnetic Field Sensor:
   * I haven't tried this out but here's the initializations
  if (IMU.magneticFieldAvailable())
  {
    IMU.readMagneticField(x, y, z);

    Serial.println(x);
  }*/

}
