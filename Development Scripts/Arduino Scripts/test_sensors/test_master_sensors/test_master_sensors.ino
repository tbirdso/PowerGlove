

  // finger_joint_type | i.e. middle_mcp_out
int ring_mcp = A0;
int ring_pip = A7;
int ring_hes = A4;

int pinky_mcp = A1;
int pinky_pip = A2;
int pinky_hes = A3;

int index_hes = A5;
//int index_mcp_IN = A6;


//int index_mcp_OUT = 3;   
//int index_pip_OUT = 5;

int flexV;

void setup(void) {
  Serial.begin(9600);
  Serial.setTimeout(50); 

  pinMode(ring_mcp, INPUT);
  pinMode(ring_pip, INPUT);
  pinMode(ring_hes, INPUT);
  pinMode(pinky_mcp, INPUT);
  pinMode(pinky_pip, INPUT);
  pinMode(pinky_hes, INPUT);
  pinMode(index_hes, INPUT);


  // set the data rate for the SoftwareSerial port
  // Software Serial needs to be on the same Baud Rate
  // and on a different Baud rate than the Serial output (but maybe not)
  mySerial.begin(9600);
  
}

void loop(void) {

  flexV = analogRead(ring_mcp);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  flexV = analogRead(ring_pip);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  // Hall Effect Sensor
  magVal = analogRead(ring_hes);
  sprintf(text,"%d, ", magVal);
  Serial.print(text); 

  delay(1);

  flexV = analogRead(pinky_mcp);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  flexV = analogRead(pinky_pip);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  // Hall Effect Sensor
  magVal = analogRead(pinky_hes);
  sprintf(text,"%d, ", magVal);
  Serial.print(text);

  delay(1);

  // Hall Effect Sensor
  magVal = analogRead(index_hes);
  sprintf(text,"%d", magVal);
  Serial.println(text);
  
  delay(10);
}
