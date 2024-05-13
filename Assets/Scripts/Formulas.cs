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
            if (resistance < Mathf.Pow(10, -6))
                return 0;

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

        public static float CalculateLoss(float seriesVoltage, float resistance, float seriesResistance)
        {
            return seriesVoltage * (resistance / seriesResistance);
        }
    }
}
