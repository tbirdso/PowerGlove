/*  Connect 2 -> 2
 *  Connect 3 -> 3
 */

#include <SoftwareSerial.h>
SoftwareSerial mySerial(2, 3); // RX, TX

int BLE;
int count = 0;

void setup()
{
  // Open serial communications and wait for port to open:
  Serial.begin(19200);
  Serial.setTimeout(50);
  delay(100);

  
  //pinMode(5, OUTPUT);   //for LEDs or pin outputs

  // set the data rate for the SoftwareSerial port
  // Software Serial needs to be on the same Baud Rate
  // and on a different Baud rate than the Serial output (but maybe not)
  mySerial.begin(9600);
}

void loop()
{

    // Read slave message and print to Serial Monitor
  if (mySerial.available()){
    BLE = mySerial.read()-90;     //0 to 180 --> -90 to 90
    count++;
    
    switch (count){   //labeling
      case 1:
        Serial.print("x = ");
        break;
      case 2:
        Serial.print("y = ");
        break;
      case 3:
        Serial.print("z = ");
        count = 0;
        break;
    }
    
    Serial.println(BLE);
    
    
    //analogWrite(5, BLE);  //using slave data for LED control
  }

  
  
}

    
  
  
