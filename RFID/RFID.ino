/*
PINOUT:
===========RFID1=================
RFID MODULE 1   MEGA
SDA             D9
SCK             D52
MOSI            D51
MISO            D50
IRQ             N/A
GND             GND
RST             D8
3.3V            3.3V

===========RFID2=================
RFID MODULE 2   MEGA
SDA             D7
SCK             D52
MOSI            D51
MISO            D50
IRQ             N/A
GND             GND
RST             D6
3.3V            3.3V

======RELAY MODULE===============
RELAY MODEL     MEGA
VCC             5V
GND             GND
IN1             2
IN2             3

*/
/* Include the standard Arduino SPI library */
#include <SPI.h>
/* Include the RFID library */
#include <RFID.h>

/* Define the DIO used for the SDA (SS) and RST (reset) pins. */
#define SDA_DIO 9
#define RESET_DIO 8

#define SDA_DIO2 7
#define RESET_DIO2 6

#define RELAY_PIN 2

/* Create an instance of the RFID library */
RFID RC522(SDA_DIO2, RESET_DIO2); 

RFID RC522_2(SDA_DIO, RESET_DIO); 

void setup()
{ 
  Serial.begin(9600);
  /* Enable the SPI interface */
  SPI.begin(); 
  /* Initialise the RFID reader */
  RC522.init();
  RC522_2.init();

  pinMode(RELAY_PIN, OUTPUT);
  digitalWrite(RELAY_PIN, HIGH);
}

void loop()
{
  /* Detect card */
  if (Serial.available() > 0) {
    // read the incoming string:
    String incomingString = Serial.readString();

    // prints the received data
    Serial.print("I received: ");
    Serial.println(incomingString);
    if(incomingString == "true\n" || incomingString == "true"){
      //Serial.println("OPEN LOCK");
      // Code to open lock
      digitalWrite(RELAY_PIN, LOW);

      delay(5000);
    }
    digitalWrite(RELAY_PIN, HIGH);
  }


  /* Has a card been detected? */
  if (RC522.isCard())
  {
    /* If so then get its serial number */
    RC522.readCardSerial();
    Serial.print("{'RFIDNumber':'");
    for(int i=0;i<5;i++)
    {
    Serial.print(RC522.serNum[i],DEC);
    //Serial.print(RC522.serNum[i],HEX); //to print card detail in Hexa Decimal format
    }
    Serial.println("','isInside':'true'}");

    delay(1000);
  }

  if (RC522_2.isCard())
  {
    /* If so then get its serial number */
    RC522_2.readCardSerial();
    Serial.print("{'RFIDNumber':'");
    for(int i=0;i<5;i++)
    {
    Serial.print(RC522.serNum[i],DEC);
    //Serial.print(RC522.serNum[i],HEX); //to print card detail in Hexa Decimal format
    }
    Serial.println("','isInside':'false'}");

    delay(1000);
  }
  delay(1000);
}