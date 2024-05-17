  int raw = 0;
  int Vin = 5;
  float Vout = 0;
  float R1 = 1000;
  float R2 = 0;
  float buffer = 0;
  void setup(){
  Serial.begin(9600);
  Serial.print("Code recieved");
  }

  void loop(){
    for (int i = 0; i<=8; i++){
      CheckRes(i);
    }
    /*CheckRes(0);
    CheckRes(1);
    CheckRes(2);
    CheckRes(3);
    CheckRes(4);
    CheckRes(5);
    CheckRes(6);
    CheckRes(7);*/
    delay(1000);
  }

  void CheckRes(int pin){
    raw = 0;
    raw = analogRead(pin);
    if(raw){
      buffer = raw * Vin;
      Vout = (buffer)/1024.0;
      buffer = (Vin/Vout) - 1;
      R2= R1 * buffer;
      if (R2  > 20 && R2 < 150000){
      Serial.print("p");
      Serial.print(pin);
      Serial.print(",");
      Serial.print(R2);
      Serial.println("x");
      }
    }
  }
