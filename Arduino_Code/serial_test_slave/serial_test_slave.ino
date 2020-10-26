/*  Connect 2 -> 2
 *  Connect 3 -> 3
 */

int response = 0;

#include <SoftwareSerial.h>
SoftwareSerial mySerial(3, 2); // RX, TX

void setup()
{
  // Open serial communications and wait for port to open:
  mySerial.begin(9600);  
  delay(100);
  pinMode(5, OUTPUT);
  
}

void loop()
{
    // Read master message
  if (mySerial.available())
    response += mySerial.read();

  if (response > 255) response = 0;
  analogWrite(5, response);

  
  mySerial.write(char(1));
  delay(500);
  mySerial.write(char(0));
  delay(500);
//  mySerial.write(char(255));
//  delay(500);
}
