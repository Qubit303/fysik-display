using System;

public class CircuitArgs : EventArgs
{
    public float Voltage;
    public float Amperage;
    public float Resistance;
    public float Power;

    public CircuitArgs(float voltage, float resistance, float amperage, float power)
    {
        Voltage = voltage;
        Amperage = amperage;
        Resistance = resistance;
        Power = power;
    }
}
