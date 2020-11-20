#include <ArduinoJson.h>

 
void setup(){
  Serial.begin(115200); 
}
 
void loop(){
  DynamicJsonBuffer jBuffer;
  JsonObject& root = jBuffer.createObject();

  //If these property names change, so does the c# JsonProperties
  root["a"] = 225; //IndexFlex1
  root["b"] = 255; //IndexFlex2
  root["c"] = 180; //MiddleFlex1
  root["d"] = 255; //MiddleFlex2
  root["e"] = 149; //RingFlex1
  root["f"] = 241; //RingFlex2
  root["g"] = 194; //PinkyFlex1
  root["h"] = 168; //PinkyFlex2
  root["i"] = 149; //ThumbFlex1
  root["j"] = 205; //ThumbFlex2
  root["k"] = 248; //HallEffect1
  root["l"] = 104; //HallEffect2
  root["m"] = 57; //HallEffect3
  root["n"] = 255; //Placeholder1
  root["o"] = 128; //Placeholder2
  root["p"] = 128; //Placeholder3
  root["q"] = 128; //Placeholder4
  root["r"] = 89; //Placeholder5
  root["s"] = 163; //Placeholder6
  root["t"] = 28; //Placeholder7

  root.prettyPrintTo(Serial);
  Serial.println();
  //delay(10);
}
