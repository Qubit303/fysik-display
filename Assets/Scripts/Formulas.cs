using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Formulas
{
    public class Formulas
    {
        public static float CalculateVoltage(float amperage, float resistance)
        {
            return amperage * resistance;
        }

        public static float CalculateAmperage(float voltage, float resistance)
        {
            return voltage / resistance;
        }

        public static float CalculateResistance(float voltage, float amperage)
        {
            return voltage / amperage;
        }

        public static float CalculatePower(float voltage, float amperage)
        {
            return voltage * amperage;
        }
    }
}
