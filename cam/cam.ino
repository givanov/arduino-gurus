// pin definition
int data_p[] = {12,13,2,3,4,5,6,7};
int xclock_p = 8;
int pclock_p = 9;
int href_p = 10;
int vsynch_p = 11;
// not used int siod = 12;
// not used int sioc = 13;
 
int lastPclock = 0;
int pclock;
int href;
int vsynch;
byte pixel;
 long control = 1;
void setup() {
 
  Serial.begin(9600);
  Serial.println("started");
  // configure data pins as input
  for (int i = 0; i < 8; i++) {
    pinMode(data_p[i], INPUT);
  }
  // configure rest input pins
  pinMode(xclock_p, OUTPUT);
  pinMode(pclock_p, INPUT);
  pinMode(href_p, INPUT);
  pinMode(vsynch_p, INPUT);
 control = 0;
}
 

 
void loop() {
  if (control <0) {
    control = 0;
  }
  control++;
  // just take one snapshot (307200 = 600 * 480) {{{
  if (control < 307200) {
    
 
 
  
  digitalWrite(xclock_p, HIGH);
  
  if (control <307200 ){
    
   
   pixel = readByteFromDataInput();
   Serial.println(pixel);
   }
  
  //Serial.println("clock written");

  /*vsynch = digitalRead(vsynch_p);
  href = digitalRead(href_p);
  pclock = digitalRead(pclock_p);
   
  if (vsynch == HIGH) return;
  if (href == LOW) return;
  if (!(lastPclock == HIGH && pclock == LOW)) {
    lastPclock = pclock;
    return;
  } else {
    lastPclock = pclock;
  }
  
  pixel = readByteFromDataInput();
  
  char s[10];
  //sprintf(s, "%d ", pixel);
  
  Serial.println(pixel);
 
  if (control % 640 == 0) {
    Serial.println("");
  }
  
  // just take one snapshot {{{
  //control++;
  // }}}
 
  if (control >= 3) {
    Serial.println("finished");
  }*/
  }
}
 
byte readByteFromDataInput() {
  byte result = 0;
  for (int i = 0; i < 8; i++) {
    result = result << 1 | digitalRead(data_p[i]);
    //Serial.println(result);
  }
  return result;
}
