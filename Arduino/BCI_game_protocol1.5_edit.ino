//#include <SoftwareSerial.h>
//SoftwareSerial mySerial(0, 1); // RX, TX
#include <Mindwave.h> 

#define BAUDRATE 57600
#define DEBUGOUTPUT 0
#define LED 13 
// constants won't change. They're used here to set pin numbers:
String evalue[] = {"",""};
// checksum variables
byte generatedChecksum = 0;
byte checksum = 0; 
int payloadLength = 0;
byte payloadData[64] = {0};
byte poorQuality = 0;
byte attention = 0;
byte meditation = 0;
// system variables
long lastReceivedPacket = 0;
boolean bigPacket = false;
// variables will change:

void setup() {
  //Serial.begin(57600);
  //while (!Serial) ;
  //mySerial.begin(57600);
  pinMode(LED, OUTPUT);
  Serial.begin(BAUDRATE);  
}
////////////////////////////////
// Read data from Serial UART //
////////////////////////////////
byte ReadOneByte()
{
  int ByteRead;
  while(!Serial.available());
  ByteRead = Serial.read();

#if DEBUGOUTPUT  
  Serial.print((char)ByteRead);   // echo the same byte out the USB serial (for debug purposes)
#endif
  return ByteRead;
}


void loop() {
  //if (mySerial.available())
    //Serial.write(mySerial.read());
  //if (Serial.available())
    //mySerial.write(Serial.read());
    
  if(ReadOneByte() == 170) {
    if(ReadOneByte() == 170) {
      payloadLength = ReadOneByte();
      if(payloadLength > 169)                      //Payload length can not be greater than 169
          return;
      generatedChecksum = 0;        
      for(int i = 0; i < payloadLength; i++) {  
        payloadData[i] = ReadOneByte();            //Read payload into memory
        generatedChecksum += payloadData[i];     
      }  
      checksum = ReadOneByte();                      //Read checksum byte from stream      
      generatedChecksum = 255 - generatedChecksum;   //Take one's compliment of generated checksum
        if(checksum == generatedChecksum) {    
        poorQuality = 200;
        attention = 0;
        meditation = 0;
        for(int i = 0; i < payloadLength; i++) {    // Parse the payload
          switch (payloadData[i]) {
         
 case 2:
            i++;            
            poorQuality = payloadData[i];
            bigPacket = true;            
            break;
          case 4:
            i++;
            attention = payloadData[i];                        
            break;
          case 5:
            i++;
            meditation = payloadData[i];
            break;
          case 0x80:
            i = i + 3;    
            //i++; 
            break;
          case 0x83:
            i = i + 25;
          break;
          default:
          break;
          } // switch
          } // for loop
#if !DEBUGOUTPUT
        // *** Add your code here ***
        if(bigPacket) {
          if(poorQuality == 0)
            {digitalWrite(LED, HIGH);}
          else
            {digitalWrite(LED, LOW);}
          evalue[0] = attention;
          evalue[1] = meditation;
          //Serial.println("start");
          //Serial.flush();
          //Serial.print("#");
          //Serial.print(",");
          //Serial.print(evalue[0]);
          //Serial.print(",");
          //Serial.print(evalue[1]);
          Serial.print(poorQuality);
          Serial.print(",");
          Serial.print(attention);
          Serial.print(",");          
          Serial.println(meditation);
#endif        
        bigPacket = false;        
      }
      else {
        // Checksum Error
      }  // end if else for checksum
    } // end if read 0xAA byte
  } // end if read 0xAA byte
}
 }
