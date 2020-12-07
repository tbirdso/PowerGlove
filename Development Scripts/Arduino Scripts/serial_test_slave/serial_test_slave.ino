/*  Connect 11 -> 12
 *  Connect 12 -> 11
 */

int response = 0;
int one = 1;
int zero = 2;

UART mySerial(digitalPinToPinName(11), digitalPinToPinName(12), NC, NC);  //RX TX

void setup()
{
  // Open serial communications and wait for port to open:
  mySerial.begin(9600);  
  delay(100);
  pinMode(13, OUTPUT);
  
}

void loop()
{
    // Read master message
  if (mySerial.available())
    response += mySerial.read();

  if (response > 255) response = 0;
  analogWrite(13, response);

  
  mySerial.print((char)5);
  delay(500);
  mySerial.write((char)0);
  delay(500);
  mySerial.write(char(255));
  delay(500);
}
