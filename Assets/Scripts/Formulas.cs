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

        public static float CaluclateCircuitAmperage(List<List<BaseModule>> fullCircuit)
        {
            var amperage = 0.0f;

            foreach (List<BaseModule> series in fullCircuit)
            {
                var seriesResistance = CalculateSeriesResistance(series);
                var seriesVoltage = ModuleManager.Instance.SeriesVoltage[series];
                amperage += CalculateAmperage(seriesVoltage, seriesResistance);
            }

            return amperage;
        }

        public static float CalculateCircuitResistance(List<List<BaseModule>> fullCircuit)
        {
            float sumOfSeriesRes = 0;

            foreach (List<BaseModule> seriesCircuit in fullCircuit)
            {
                var series = CalculateSeriesResistance(seriesCircuit);
                sumOfSeriesRes += 1 / series;
            }

            return 1 / sumOfSeriesRes;
        }

        public static float CalculateSeriesResistance(List<BaseModule> seriesCircuit)
        {
            float resistance = 0;
            foreach (BaseModule module in seriesCircuit)
            {
                resistance += module.GetResistance();
            }
            return resistance;
        }

        public static float CalculateCircuitPower(float circuitVoltage, float circuitAmperage)
        {
            return circuitVoltage * circuitAmperage;
        }
    }
}
