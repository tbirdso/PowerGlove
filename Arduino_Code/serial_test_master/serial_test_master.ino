/*  Connect 2 -> 2
 *  Connect 3 -> 3
 */

#include <SoftwareSerial.h>


SoftwareSerial mySerial(2, 3); // RX, TX

int message = 0;

void setup()
{
  // Open serial communications and wait for port to open:
  Serial.begin(19200);
  Serial.setTimeout(50);
  delay(100);
  pinMode(5, OUTPUT);

  // set the data rate for the SoftwareSerial port
  // Software Serial needs to be on the same Baud Rate
  // and on a different Baud rate than the Serial output (but maybe not)
  mySerial.begin(9600);
}

void loop()
{

    // Read slave message and print to Serial Monitor
  if (mySerial.available()){
    message = mySerial.read();
    Serial.println(message);
  }

    // Blink LED on pin 5 using message
  if (message == 1){
    digitalWrite(5, HIGH);
    mySerial.write(25);         // Talk back to slave
  }
    
  if (message == 0)
    digitalWrite(5, LOW);
  
  
}
