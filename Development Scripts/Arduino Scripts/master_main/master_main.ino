#define NUMFLEXSENSORS 2  // number of 
#define CALIBRATE_TIME 5  // in seconds

#include <SoftwareSerial.h>
#include <ArduinoJson.h> // v5.11.2


SoftwareSerial mySerial(11, 12); // RX, TX


  // finger_joint_type | i.e. middle_mcp_out
int ring_mcp = A0;
int ring_pip = A7;
int ring_hes = A4;

int pinky_mcp = A1;
int pinky_pip = A2;
int pinky_hes = A3;

int index_hes = A5;

int index_mcp_val;
int index_pip_val;
int middle_mcp_val;
int middle_pip_val;
int thumb_mcp_val;
int thumb_pip_val;
int thumb_hes_val;
int x_acc_val;
int y_acc_val;
int z_acc_val;
int x_gyro_val;
int y_gyro_val;
int z_gyro_val;



int flexV, count = 0;
int message = 0;

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

  Serial.println("Starting!\n");
  
}

void loop(void) {
  
  while(mySerial.available())   // Empty Incoming queue
    mySerial.read();

    
  mySerial.write(1);    // request new data from slave
  delay(10);
  count = 0;
  
//  DynamicJsonBuffer jBuffer;
//  JsonObject& root = jBuffer.createObject();

  if (mySerial.available()){
    while (count < 13){
      message = mySerial.read();
//      Serial.println(message);
      switch(message){
        case 0:
//          Serial.println("IMU Read\n");
          x_acc_val = mySerial.read();
          y_acc_val = mySerial.read();
          z_acc_val = mySerial.read();
          x_gyro_val = mySerial.read();
          y_gyro_val = mySerial.read();
          z_gyro_val = mySerial.read();
          count += 6;
          break;
        case 1:
          thumb_mcp_val = mySerial.read();
          count++;
          break;
        case 2:
          thumb_pip_val = mySerial.read();
          count++;
          break;
        case 3:
          thumb_hes_val = mySerial.read();
          count++;
          break;
        case 4:
          index_mcp_val = mySerial.read();
          count++;
          break;
        case 5:
          index_pip_val = mySerial.read();
          count++;
          break;
        case 6:
          middle_mcp_val = mySerial.read();
          count++;
          break;
        case 7:
          middle_pip_val = mySerial.read();
          count++;
          break;
        default:
          break;
      }
    }  
  }

  

//  root["a"] = index_mcp_val; // index_mcp
//  root["b"] = index_pip_val; // index_pip
//  root["c"] = middle_mcp_val; // middle_mcp
//  root["d"] = middle_pip_val; // middle_pip
//  root["e"] = analogRead(ring_mcp); // ring_mcp
//  root["f"] = analogRead(ring_pip); // ring_pip
//  root["g"] = analogRead(pinky_mcp); // pinky_mcp
//  root["h"] = analogRead(pinky_pip); // pinky_pip
//  root["i"] = thumb_mcp_val; // thumb_mcp
//  root["j"] = thumb_pip_val; // thumb_pip
//  root["k"] = thumb_hes_val; // thumb_hes
//  root["l"] = analogRead(index_hes); // index_hes
//  root["m"] = analogRead(ring_hes); // ring_hes
//  root["n"] = analogRead(pinky_hes); // pinky_hes
//  root["o"] = x_acc_val; // x_acc
//  root["p"] = y_acc_val; // y_acc
//  root["q"] = z_acc_val; // z_acc
//  root["r"] = x_gyro_val; // x_gyro
//  root["s"] = y_gyro_val; // y_gyro
//  root["t"] = z_gyro_val; // z_gyro
////  root["u"] = 21; // Placeholder8
////  root["v"] = 22; // Placeholder9
////  root["w"] = 23; // Placeholder10
////  root["x"] = 24; // Placeholder11

  Serial.print("t_mcp\t t_pip\t i_mcp\t i_pip\t m_mcp\t m_pip r_mcp\t r_pip\t p_mcp\t p_pip\t t_hes\t i_hes\t r_hes\t p_hes");
  Serial.println("\t x_acc\t y_acc\t z_acc\t x_gy\t y_gy\t z_gy");
  Serial.print(thumb_mcp_val);
  Serial.print("\t ");
  Serial.print(thumb_pip_val);
  Serial.print("\t ");
  Serial.print(index_mcp_val);
  Serial.print("\t ");
  Serial.print(index_pip_val);
  Serial.print("\t ");
  Serial.print(middle_mcp_val);
  Serial.print("\t ");
  Serial.print(middle_pip_val);
  Serial.print("\t ");
  Serial.print(map(analogRead(ring_mcp), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(map(analogRead(ring_pip), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(map(analogRead(pinky_mcp), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(map(analogRead(pinky_pip), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(thumb_hes_val);
  Serial.print("\t ");
  Serial.print(map(analogRead(index_hes), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(map(analogRead(ring_hes), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(map(analogRead(pinky_hes), 0, 1023, 8, 255));
  Serial.print("\t ");
  Serial.print(x_acc_val);
  Serial.print("\t ");
  Serial.print(y_acc_val);
  Serial.print("\t ");
  Serial.print(z_acc_val);
  Serial.print("\t ");
  Serial.print(x_gyro_val);
  Serial.print("\t ");
  Serial.print(y_gyro_val);
  Serial.print("\t ");
  Serial.print(z_gyro_val);
  Serial.println("\t ");

  delay(500);

//  root.prettyPrintTo(Serial);
//  Serial.println();
}
