
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

int val;


int flexV, count = 0;
int message = 0;

void setup(void) {
  Serial.begin(115200);
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
  delay(25);
  count = 0;
  
  DynamicJsonBuffer jBuffer;
  JsonObject& root = jBuffer.createObject();

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
          thumb_mcp_val = mySerial.read();
          thumb_pip_val = mySerial.read();
          thumb_hes_val = mySerial.read();
          index_mcp_val = mySerial.read();
          index_pip_val = mySerial.read();
          middle_mcp_val = mySerial.read();
          middle_pip_val = mySerial.read();
          count = 13;
          break;
        default:
          break;
      }
    }  
  }

  

  root["a"] = index_mcp_val; // index_mcp
  root["b"] = index_pip_val; // index_pip
  root["c"] = middle_mcp_val; // middle_mcp
  root["d"] = middle_pip_val; // middle_pip
  root["e"] = map(analogRead(ring_mcp), 380, 615, 1, 255); // ring_mcp
  root["f"] = map(analogRead(ring_pip), 430, 614, 1, 255); // ring_pip
  root["g"] = map(analogRead(pinky_mcp), 490, 650, 1, 255); // pinky_mcp
  root["h"] = map(analogRead(pinky_pip), 450, 626, 1, 255); // pinky_pip
  root["i"] = thumb_mcp_val; // thumb_mcp
  root["j"] = thumb_pip_val; // thumb_pip
  root["k"] = thumb_hes_val; // thumb_hes
  val = map(analogRead(index_hes), 516, 905, 255, 1); 
  if (val < 1) val = 1;
  if (val > 255) val = 255;
  root["l"] = val; // index_hes
  val = map(analogRead(ring_hes), 533, 915, 255, 1);
  if (val < 1) val = 1;
  if (val > 255) val = 255;
  root["m"] = val; // ring_hes
  val = map(analogRead(pinky_hes), 518, 919, 255, 1);
  if (val < 1) val = 1;
  if (val > 255) val = 255;
  root["n"] = val ; // pinky_hes
  root["o"] = x_acc_val; // x_acc
  root["p"] = y_acc_val; // y_acc
  root["q"] = z_acc_val; // z_acc
  root["r"] = x_gyro_val; // x_gyro
  root["s"] = y_gyro_val; // y_gyro
  root["t"] = z_gyro_val; // z_gyro
//  root["u"] = 21; // Placeholder8
//  root["v"] = 22; // Placeholder9
//  root["w"] = 23; // Placeholder10
//  root["x"] = 24; // Placeholder11

//  Serial.print("t_mcp\t t_pip\t i_mcp\t i_pip\t m_mcp\t m_pip r_mcp\t r_pip\t p_mcp\t p_pip\t t_hes\t i_hes\t r_hes\t p_hes");
//  Serial.println("\t x_acc\t y_acc\t z_acc\t x_gy\t y_gy\t z_gy");
//  Serial.print(thumb_mcp_val);
//  Serial.print("\t ");
//  Serial.print(thumb_pip_val);
//  Serial.print("\t ");
//  Serial.print(index_mcp_val);
//  Serial.print("\t ");
//  Serial.print(index_pip_val);
//  Serial.print("\t ");
//  Serial.print(middle_mcp_val);
//  Serial.print("\t ");
//  Serial.print(middle_pip_val);
//  Serial.print("\t ");
//  Serial.print(map(analogRead(ring_mcp), 350, 620, 1, 255));
//  Serial.print("\t ");
//  Serial.print(map(analogRead(ring_pip), 449, 614, 1, 255));
//  Serial.print("\t ");
//  Serial.print(map(analogRead(pinky_mcp), 490, 673, 1, 255));
//  Serial.print("\t ");
//  Serial.print(map(analogRead(pinky_pip), 470, 626, 1, 255));
//  Serial.print("\t ");
//  Serial.print(thumb_hes_val);
//  Serial.print("\t ");
//  Serial.print(map(analogRead(index_hes), 516, 905, 255, 1));
//  Serial.print("\t ");
//  Serial.print(map(analogRead(ring_hes), 533, 915, 255, 1));
//  Serial.print("\t ");
//  Serial.print(map(analogRead(pinky_hes), 518, 919, 255, 1));
//  Serial.print("\t ");
//  Serial.print(x_acc_val);
//  Serial.print("\t ");
//  Serial.print(y_acc_val);
//  Serial.print("\t ");
//  Serial.print(z_acc_val);
//  Serial.print("\t ");
//  Serial.print(x_gyro_val);
//  Serial.print("\t ");
//  Serial.print(y_gyro_val);
//  Serial.print("\t ");
//  Serial.print(z_gyro_val);
//  Serial.println("\t ");

//  delay(50);

  
  root.prettyPrintTo(Serial);
  Serial.println();
}
