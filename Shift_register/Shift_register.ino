//first simple version 
const int clock = 6;
const int data = 7;
void setup()
{
  pinMode(clock, OUTPUT); 
  pinMode(data , OUTPUT); 
  digitalWrite(clock, LOW);
  digitalWrite(data,LOW);
shiftOut(data, clock, LSBFIRST, B10000000);
  delay(500);
shiftOut(data, clock, LSBFIRST, B01000000);
  delay(500);
shiftOut(data, clock, LSBFIRST, B00100000);
  delay(500);
shiftOut(data, clock, LSBFIRST, B00010000);
  delay(500);
shiftOut(data, clock, LSBFIRST, B00001000);
  delay(500);
shiftOut(data, clock, LSBFIRST, B00000100);
  delay(500);
shiftOut(data, clock, LSBFIRST, B00000010);
  delay(500);  
shiftOut(data, clock, LSBFIRST, B00000001);
delay(500);
shiftOut(data, clock, LSBFIRST, B10101010);
delay(500);
}
void loop(){

} 
