//first simple version 
const int clock = 6;
const int data = 7;
byte numbers[10];
void setup()
{
  numbers[0] = B10111011;
  numbers[1] = B00001010;
  numbers[2] = B01110011;
  numbers[3] = B01011011;
  numbers[4] = B11001010;
  numbers[5] = B11011001;
  numbers[6] = B11111001;
  numbers[7] = B00001010;
  numbers[8] = B11111011;
  numbers[9] = B11011011;
  
  pinMode(clock, OUTPUT); 
  pinMode(data , OUTPUT); 
  digitalWrite(clock, LOW);
  digitalWrite(data,LOW);
  shiftOut(data, clock, LSBFIRST, B00001011);
}
void loop(){
  for(int i=0;i<10;i++){
  digitalWrite(clock, LOW);
  digitalWrite(data,LOW);
  shiftOut(data, clock, LSBFIRST, numbers[i]);
  delay(1000);
  }
} 
