#define NUMFLEXSENSORS 2  // number of 
#define CALIBRATE_TIME 5  // in seconds

  // finger_joint_type | i.e. middle_mcp_out
int index_mcp_IN = A0;
int index_pip_IN = A1;
int index_mcp_OUT = 3;   
int index_pip_OUT = 5;

int mag_IN = A7;

int flexV, magVal;
int flexMinMax[NUMFLEXSENSORS*2];  // finger = n, n is mcp, n+1 is pcp, n is even
char text[50];
int i;
int count;

void setup(void) {
  Serial.begin(9600); 

  pinMode(index_mcp_IN, INPUT);
  pinMode(index_pip_IN, INPUT);
  pinMode(mag_IN, INPUT);
  
  pinMode(index_mcp_OUT, OUTPUT);
  pinMode(index_pip_OUT, OUTPUT);
  pinMode(6, OUTPUT);

    // Init for MinMax array
  for (i = 0; i < 2*NUMFLEXSENSORS; i++)
    (i%2 == 0) ? (flexMinMax[i] = 1024) : (flexMinMax[i] = 0);

  count = 0;
}

void loop(void) {

//    // Calibrate for 5 Seconds
//  while(count < CALIBRATE_TIME*1000){
//    findMinMax(index_mcp_IN, 0, 1);
//    findMinMax(index_pip_IN, 2, 3);
//    delay(1);
//    count++;
//  }
////  if (count == 5000) {
////    sprintf(text,"index_mcp = %d | Min = %d | Max = %d", flexV, flexMinMax[0], flexMinMax[1]);
////    Serial.println(text);
////  }
//
//      // Index MCP Joint - 0,1
//  flexV = analogRead(index_mcp_IN);
//  // Min = 441 | Max = 677
//  if (flexV < flexMinMax[0]) analogWrite(index_mcp_OUT, 255);
//  if (flexV > flexMinMax[1]) analogWrite(index_mcp_OUT, 0);
//  else
//    analogWrite(index_mcp_OUT, 255 - map(flexV, flexMinMax[0], flexMinMax[1], 0, 255));
//  
//      // Index PIP Joint - 2,3
//  flexV = analogRead(index_pip_IN);
//  // Min = 468 | Max = 643
//  if (flexV < flexMinMax[2]) analogWrite(index_pip_OUT, 255);
//  if (flexV > flexMinMax[3]) analogWrite(index_pip_OUT, 0);
//  else 
//    analogWrite(index_pip_OUT, 255 - map(flexV, flexMinMax[2], flexMinMax[3], 0, 255));

      // Middle MCP Joint - 4,5
      // Middle PIP Joint - 6,7
  
      // Ring MCP Joint - 8,9
      // Ring PIP Joint - 10,11
      
      // Pinky MCP Joint - 12,13 
      // Pinky PIP Joint - 14,15
    
      // Thumb MCP Joint - 16,17
      // Thumb PIP Joint - 18,19

  flexV = analogRead(index_mcp_IN);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  flexV = analogRead(index_pip_IN);
  sprintf(text,"%d, ", flexV);
  Serial.print(text);

  // Hall Effect Sensor
  magVal = analogRead(mag_IN);
  sprintf(text,"%d", magVal);
  Serial.println(text);
  
  delay(10);
}


void findMinMax(int pin, int minVal, int maxVal){
  int flexV;
  flexV = analogRead(pin);
  if (flexV < flexMinMax[minVal]) flexMinMax[minVal] = flexV;
  if (flexV > flexMinMax[maxVal]) flexMinMax[maxVal] = flexV;
  sprintf(text,"index_mcp = %d | Min = %d | Max = %d", flexV, flexMinMax[0], flexMinMax[1]);
  Serial.println(text);
}

float norm(int val, int minVal, int maxVal){
  return (float)(val-minVal)*(2/(maxVal-minVal));
}
