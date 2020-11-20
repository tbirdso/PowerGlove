#include <ArduinoJson.h>

const int NUM_LABELS = 11;
const int NUM_FEATURES = 20;
int samples[NUM_LABELS][NUM_FEATURES] = {{1,11,1,44,-5,35,-49,65,125,43,237,189,255,255,128,128,128,98,155,251},
    {186,253,99,1,34,52,43,89,138,173,249,255,255,255,128,128,128,103,151,3},
    {133,250,75,38,60,45,97,71,248,204,245,255,255,255,128,128,128,99,159,8},
    {215,248,193,255,65,27,51,102,250,214,244,254,255,255,128,128,128,100,159,13},
    {184,253,197,227,182,243,242,250,5,205,250,203,115,240,128,128,128,107,159,19},
    {219,255,222,253,238,238,259,252,193,201,246,229,228,244,128,128,128,101,149,25},
    {222,255,180,255,149,241,194,168,149,205,248,104,57,255,128,128,128,89,163,28},
    {216,255,172,247,118,76,224,260,121,216,250,156,255,128,128,128,128,110,164,35},
    {175,247,150,31,115,206,247,227,142,214,248,255,255,228,128,128,128,95,168,37},
    {153,8,174,236,164,245,242,249,209,202,247,255,247,236,128,128,128,72,189,43},
    {25,72,21,104,16,68,154,242,205,211,248,161,255,255,255,128,128,92,178,54}};

int index = 0;
 
void setup(){
  Serial.begin(115200);
}
 
void loop(){
  DynamicJsonBuffer jBuffer;
  JsonObject& root = jBuffer.createObject();

  for(int i = 0; i < 30; i++) {
    //If these property names change, so does the c# JsonProperties
    root["a"] = samples[index][0]; //IndexFlex1
    root["b"] = samples[index][1]; //IndexFlex2
    root["c"] = samples[index][2]; //MiddleFlex1
    root["d"] = samples[index][3]; //MiddleFlex2
    root["e"] = samples[index][4]; //RingFlex1
    root["f"] = samples[index][5]; //RingFlex2
    root["g"] = samples[index][6]; //PinkyFlex1
    root["h"] = samples[index][7]; //PinkyFlex2
    root["i"] = samples[index][8]; //ThumbFlex1
    root["j"] = samples[index][9]; //ThumbFlex2
    root["k"] = samples[index][10]; //HallEffect1
    root["l"] = samples[index][11]; //HallEffect2
    root["m"] = samples[index][12]; //HallEffect3
    root["n"] = samples[index][13]; //Placeholder1
    root["o"] = samples[index][14]; //Placeholder2
    root["p"] = samples[index][15]; //Placeholder3
    root["q"] = samples[index][16]; //Placeholder4
    root["r"] = samples[index][17]; //Placeholder5
    root["s"] = samples[index][18]; //Placeholder6
    root["t"] = samples[index][19]; //Placeholder7
  
    root.prettyPrintTo(Serial);
    Serial.println();
    delay(1);
  }
  index = (index + 1) % (NUM_LABELS);
}
