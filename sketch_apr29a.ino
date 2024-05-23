#include <Wire.h>
#include <ADXL345.h>

#define address1     0x1D    // Standard address if SDO/ALT ADDRESS is HIGH.
#define address2     0x53    // Alternate address if SDO/ALT ADDRESS is LOW.

ADXL345 accel1(address1);
ADXL345 accel2(address2);

int sensorValue = 0;
unsigned long lastSendTime = 0; // Last send time
const int sendInterval = 500; // Send data every 100ms


void setup() {
  pinMode(A0, INPUT);

  Serial.begin(9600);
  Wire.begin();

  // Data Rate
  // - ADXL345_RATE_3200HZ: 3200 Hz
  // - ADXL345_RATE_1600HZ: 1600 Hz
  // - ADXL345_RATE_800HZ:  800 Hz
  // - ADXL345_RATE_400HZ:  400 Hz
  // - ADXL345_RATE_200HZ:  200 Hz
  // - ADXL345_RATE_100HZ:  100 Hz
  // - ADXL345_RATE_50HZ:   50 Hz
  // - ADXL345_RATE_25HZ:   25 Hz
  // - ...
  if (!accel1.writeRate(ADXL345_RATE_200HZ)) {
    Serial.println("write rate 1: failed");
    while(1) {
      delay(10);
    }
  }
  if (!accel2.writeRate(ADXL345_RATE_400HZ)) {
    Serial.println("write rate 2: failed");
    while(1) {
      delay(10);
    }
  }

  // Data Range
  // - ADXL345_RANGE_2G: +-2 g
  // - ADXL345_RANGE_4G: +-4 g
  // - ADXL345_RANGE_8G: +-8 g
  // - ADXL345_RANGE_16G: +-16 g
  if (!accel1.writeRange(ADXL345_RANGE_16G)) {
    Serial.println("write range 1: failed");
    while(1) {
      delay(10);
    }
  }

  if (!accel1.start()) {
    Serial.println("start 1: failed");
    while(1) {
      delay(10);
    }
  }
  if (!accel2.writeRange(ADXL345_RANGE_16G)) {
    Serial.println("write range 2: failed");
    while(1) {
      delay(100);
    }
  }

  if (!accel2.start()) {
    Serial.println("start 2: failed");
    while(1) {
      delay(10);
    }
  }
}

void loop() {
  if (millis() - lastSendTime >= sendInterval) {
lastSendTime = millis();
sensorValue = analogRead(A0);

  if (accel1.update()) {
    Serial.print(" x1: ");
    Serial.print(accel1.getX());
    Serial.print(",y1: ");
    Serial.print(accel1.getY());
    Serial.print(",z1: ");
    Serial.print(accel1.getZ());
  } else {
    Serial.println("update 1 failed");
    while(1) {
      delay(10);
    }
  }
  if (accel2.update()) {
    Serial.print(",x2: ");
    Serial.print(accel2.getX());
    Serial.print(",y2: ");
    Serial.print(accel2.getY());
    Serial.print(",z2: ");
    Serial.print(accel2.getZ());
  } else {
    Serial.println("update 2 failed");
    while(1) {
      delay(10);
    }
  }
  Serial.print(",");
  Serial.print(sensorValue);
  Serial.println("");
  delay(30);
}
}