#include <ArduinoJson.h>

 
void setup(){
  Serial.begin(115200); 
}
 
void loop(){
  DynamicJsonBuffer jBuffer;
  JsonObject& root = jBuffer.createObject();

  //If these property names change, so does the c# JsonProperties
  root["a"] = 1; //IndexFlex1
  root["b"] = 2; //IndexFlex2
  root["c"] = 3; //MiddleFlex1
  root["d"] = 4; //MiddleFlex2
  root["e"] = 5; //RingFlex1
  root["f"] = 6; //RingFlex2
  root["g"] = 7; //PinkyFlex1
  root["h"] = 8; //PinkyFlex2
  root["i"] = 9; //ThumbFlex1
  root["j"] = 10; //ThumbFlex2
  root["k"] = 11; //HallEffect1
  root["l"] = 12; //HallEffect2
  root["m"] = 13; //HallEffect3
  root["n"] = 14; //Placeholder1
  root["o"] = 15; //Placeholder2
  root["p"] = 16; //Placeholder3
  root["q"] = 17; //Placeholder4
  root["r"] = 18; //Placeholder5
  root["s"] = 19; //Placeholder6
  root["t"] = 20; //Placeholder7
  root["u"] = 21; //Placeholder8
  root["v"] = 22; //Placeholder9
  root["w"] = 23; //Placeholder10
  root["x"] = 24; //Placeholder11

  root.prettyPrintTo(Serial);
  Serial.println();
  //delay(10);
}
