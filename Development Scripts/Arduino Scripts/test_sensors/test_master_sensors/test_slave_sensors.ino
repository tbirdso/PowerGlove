
  // finger_joint_type | i.e. middle_mcp_out
int thumb_mcp = A0;
int thumb_pip = A7;
int thumb_hes = A4;

int index_mcp = A1;
int index_pip = A2;

int middle_mcp = A3;
int middle_pip = A5;


int flexV;

void setup(void) {
  Serial.begin(9600);
  Serial.setTimeout(50); 

  pinMode(thumb_mcp, INPUT);
  pinMode(thumb_pip, INPUT);
  pinMode(thumb_hes, INPUT);
  pinMode(index_mcp, INPUT);
  pinMode(index_pip, INPUT);
  pinMode(middle_mcp, INPUT);
  pinMode(middle_pip, INPUT);


  // set the data rate for the SoftwareSerial port
  // Software Serial needs to be on the same Baud Rate
  // and on a different Baud rate than the Serial output (but maybe not)
  mySerial.begin(9600);
  
}

void loop(void) {

  flexV = analogRead(thumb_mcp);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  flexV = analogRead(thumb_pip);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  // Hall Effect Sensor
  magVal = analogRead(thumb_hes);
  sprintf(text,"%d, ", magVal);
  Serial.print(text);

  delay(1);

  flexV = analogRead(index_mcp);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  flexV = analogRead(index_pip);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  delay(1);

  // Hall Effect Sensor
  magVal = analogRead(middle_mcp);
  sprintf(text,"%d, ", magVal);
  Serial.print(text);

  delay(1);

  // Hall Effect Sensor
  magVal = analogRead(middle_pip);
  sprintf(text,"%d", magVal);
  Serial.println(text);
  
  delay(10);
}
