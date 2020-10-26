int mag_IN = A6;
int led0=6, led1=7, led2=8, led3=9, led4=10;
int magVal;
char text[200];

void setup() {
  // put your setup code here, to run once:
  pinMode(led0, OUTPUT);
  pinMode(led1, OUTPUT);
  pinMode(led2, OUTPUT);
  pinMode(led3, OUTPUT);
  pinMode(led4, OUTPUT);
  pinMode(mag_IN, INPUT);
  Serial.begin(9600); 
}

void loop() {
  // put your main code here, to run repeatedly:
  magVal = analogRead(mag_IN);
  // Magnet Range: 
  sprintf(text,"\tMag Val = %d", magVal);
  Serial.println(text);
  magVal <= 507 ? digitalWrite(led0, HIGH) : digitalWrite(led0, LOW);
  magVal <= 499 ? digitalWrite(led1, HIGH) : digitalWrite(led1, LOW);
  magVal <= 477 ? digitalWrite(led2, HIGH) : digitalWrite(led2, LOW);
  magVal <= 402 ? digitalWrite(led3, HIGH) : digitalWrite(led3, LOW);
  magVal <= 270 ? digitalWrite(led4, HIGH) : digitalWrite(led4, LOW);
  
  delay(100);
}
