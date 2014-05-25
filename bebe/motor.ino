int dir1PinA = 2;
int dir1PinB = 3;
int dir2PinA = 4;
int dir2PinB = 5;
int SpeedPinA = 9;
int SpeedPinB = 10;
unsigned long time;
int speed;
int dir_motorA;
int dir_motorB;
int speedA;
int speedB;

int in_bytes[10];
boolean handshake = false;
int incomingByte = 0;
#define MAX_MILLIS_TO_WAIT 1000  //or whatever
unsigned long starttime;

void setup(){
 Serial.begin(9600);
 pinMode(dir1PinA,OUTPUT); 
 pinMode(dir1PinB,OUTPUT); 
 pinMode(SpeedPinA,OUTPUT);
 pinMode(dir2PinA,OUTPUT); 
 pinMode(dir2PinB,OUTPUT); 
 pinMode(SpeedPinB,OUTPUT);
 time = millis();
 speed = 234;

 /*digitalWrite(dir1PinA,LOW);
 digitalWrite(dir1PinB,HIGH);
 digitalWrite(dir2PinA,LOW);
 digitalWrite(dir2PinB,HIGH);
 analogWrite(SpeedPinA,speed);
 analogWrite(SpeedPinB,speed);*/
//delay(2000);
}
 void loop()
 {
   starttime = millis();
    if(handshake){
        while ( (Serial.available()<8) && ((millis() - starttime) < MAX_MILLIS_TO_WAIT) )
        {      
          // hang in this loop until we either get 9 bytes of data or 1 second
          // has gone by
        }
        if(Serial.available() < 8)
        {
                // the data didn't come in - handle that problem here
           // Serial.println("ERROR - Didn't get 9 bytes of data!");
        }
        else
        {
             for(int n=0; n<8; n++)
               in_bytes[n] = Serial.read(); // Then: Get them. 
             dir_motorA = in_bytes[0];
             dir_motorB = in_bytes[4];
             speedA = in_bytes[1]*100 + in_bytes[2]*10 + in_bytes[3];
             speedB = in_bytes[5]*100 + in_bytes[6]*10 + in_bytes[7];  
             //Serial.println( speedA + speedB );  
             //Serial.write(1);
        
       analogWrite(SpeedPinA,speedA);
       analogWrite(SpeedPinB,speedB);
       if (1 == dir_motorA)
       {
         digitalWrite(dir1PinA,LOW);
         digitalWrite(dir1PinB,HIGH);
       }
       else
       {
         digitalWrite(dir1PinA,HIGH);
         digitalWrite(dir1PinB,LOW);
       }
       if (1 == dir_motorB)
       {
         digitalWrite(dir2PinA,LOW);
         digitalWrite(dir2PinB,HIGH);
       }
       else
       {
         digitalWrite(dir2PinA,HIGH);
         digitalWrite(dir2PinB,LOW);
       }
       delay(200);
      }
    }
    else{
      perform_handshake();
    }  
}

void perform_handshake()
{
  while ( Serial.available()<1);
  int in_bit = 0;
  in_bit = Serial.read();
  if(in_bit == 1)
  { 
    handshake = true;
    Serial.write(1);
    delay(1000);
  }
  else 
    perform_handshake();
}
