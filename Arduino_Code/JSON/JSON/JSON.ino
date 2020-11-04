#include <ArduinoJson.h> // v5.11.2

 
void setup(){
  Serial.begin(115200); 
}
 
void loop(){
  DynamicJsonBuffer jBuffer;
  JsonObject& root = jBuffer.createObject();

  //If these property names change, so does the c# JsonProperties
  root["a"] = 1; // index_mcp
  root["b"] = 2; // index_pip
  root["c"] = 3; // index_mcp
  root["d"] = 4; // middle_pip
  root["e"] = 5; // ring_mcp
  root["f"] = 6; // ring_pip
  root["g"] = 7; // pinky_mcp
  root["h"] = 8; // pinky_pip
  root["i"] = 9; // thumb_mcp
  root["j"] = 10; // thumb_pip
  root["k"] = 11; // thumb_hes
  root["l"] = 12; // index_hes
  root["m"] = 13; // ring_hes
  root["n"] = 14; // pinky_hes
  root["o"] = 15; // x_acc
  root["p"] = 16; // y_acc
  root["q"] = 17; // z_acc
  root["r"] = 18; // pitch
  root["s"] = 19; // roll
  root["t"] = 20; // yaw
  root["u"] = 21; // Placeholder8
  root["v"] = 22; // Placeholder9
  root["w"] = 23; // Placeholder10
  root["x"] = 24; // Placeholder11

  root.prettyPrintTo(Serial);
  Serial.println();
  //delay(10);
}

//public class PowerGlove
//    {
//        [JsonProperty("a")] public int index_mcp { get; set; }
//        [JsonProperty("b")] public int index_pip { get; set; }
//        [JsonProperty("c")] public int middle_mcp { get; set; }
//        [JsonProperty("d")] public int middle_pip { get; set; }
//        [JsonProperty("e")] public int ring_mcp { get; set; }
//        [JsonProperty("f")] public int ring_pip { get; set; }
//        [JsonProperty("g")] public int pinky_mcp { get; set; }
//        [JsonProperty("h")] public int pinky_pip { get; set; }
//        [JsonProperty("i")] public int thumb_mcp { get; set; }
//        [JsonProperty("j")] public int thumb_pip { get; set; }
//        [JsonProperty("k")] public int thumb_hes { get; set; }
//        [JsonProperty("l")] public int index_hes { get; set; }
//        [JsonProperty("m")] public int ring_hes { get; set; }
//        [JsonProperty("n")] public int pinky_hes { get; set; }
//        [JsonProperty("o")] public int x_acc { get; set; }
//        [JsonProperty("p")] public int y_acc { get; set; }
//        [JsonProperty("q")] public int z_acc { get; set; }
//        [JsonProperty("r")] public int pitch { get; set; }
//        [JsonProperty("s")] public int roll { get; set; }
//        [JsonProperty("t")] public int yaw { get; set; }
//        [JsonProperty("u")] public int Placeholder8 { get; set; }
//        [JsonProperty("v")] public int Placeholder9 { get; set; }
//        [JsonProperty("w")] public int Placeholder10 { get; set; }
//        [JsonProperty("x")] public int Placeholder11 { get; set; }
//    }
