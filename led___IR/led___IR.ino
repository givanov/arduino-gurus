// Arduino 7 segment display example software
// http://www.hacktronics.com/Tutorials/arduino-and-7-segment-led.html
// License: http://www.opensource.org/licenses/mit-license.php (Go crazy)
 
// Define the LED digit patters, from 0 - 9
// Note that these patterns are for common cathode displays
// For common anode displays, change the 1's to 0's and 0's to 1's
// 1 = LED on, 0 = LED off, in this order:
//    Arduino pin: 2,3,4,5,6,7,8
#include <IRremote.h>
#include <IRremoteInt.h>
byte seven_seg_digits[10][7] = { { 1,1,1,1,1,1,0 },  // = 0
                                                           { 0,1,1,0,0,0,0 },  // = 1
                                                           { 1,1,0,1,1,0,1 },  // = 2
                                                           { 1,1,1,1,0,0,1 },  // = 3
                                                           { 0,1,1,0,0,1,1 },  // = 4
                                                           { 1,0,1,1,0,1,1 },  // = 5
                                                           { 1,0,1,1,1,1,1 },  // = 6
                                                           { 1,1,1,0,0,0,0 },  // = 7
                                                           { 1,1,1,1,1,1,1 },  // = 8
                                                           { 1,1,1,0,0,1,1 }   // = 9
                                                           };
int RECV_PIN = 11;

IRrecv irrecv(RECV_PIN);

decode_results results;

void setup() {                
  pinMode(2, OUTPUT);   
  pinMode(3, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(5, OUTPUT);
  pinMode(6, OUTPUT);
  pinMode(7, OUTPUT);
  pinMode(8, OUTPUT);
  pinMode(9, OUTPUT);
  writeDot(0);  // start with the "dot" off
  Serial.begin(9600);
  irrecv.enableIRIn();
}

void writeDot(byte dot) {
  digitalWrite(9, dot);
}
    
void sevenSegWrite(byte digit) {
  byte pin = 2;
  for (byte segCount = 0; segCount < 7; ++segCount) {
    digitalWrite(pin, seven_seg_digits[digit][segCount]);
    ++pin;
  }
}
int dump(decode_results *results) {
   //Serial.print(results->value, HEX);
   //Serial.println("");
 if(results->value == 0xFF6897)
        return 0;
 if(results->value == 0xFF30CF)
        return 1;
 if(results->value == 0xFF18E7)
        return 2;
 if(results->value == 0xFF7A85)
        return 3;
 if(results->value == 0xFF10EF)
        return 4;
 if(results->value == 0xFF38C7)
        return 5;
 if(results->value == 0xFF5AA5)
        return 6;
 if(results->value == 0xFF42BD)
        return 7;
 if(results->value == 0xFF4AB5)
        return 8;
 if(results->value == 0xFF52AD)
        return 9; 
 return 0;      
/*0 - FF6897
1 - FF30CF
2 - FF18E7
3 - FF7A85
4 - FF10EF
5 - FF38C7
6 - FF5AA5
7 - FF42BD
8 - FF4AB5
9 - FF52AD
*/
}
void loop() {
  //for (byte count = 10; count > 0; --count) {
  // delay(1000);
 //  sevenSegWrite(count - 1); 
 // }
 if (irrecv.decode(&results)) {
    Serial.println(results.value, HEX);
    int asd = dump(&results);
    Serial.print(asd);
    delay(1000);
    sevenSegWrite(asd);
    irrecv.resume(); // Receive the next value
  }
  delay(1000);
}

