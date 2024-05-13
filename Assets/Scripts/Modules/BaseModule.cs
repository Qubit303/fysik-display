using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Types;
using Module.Neighbors;
using Module.Formulas;

namespace Module.Types
{
    public enum ModuleType
    {
        Serial,
        ParallelDown,
        ParallelUp
    }

    public enum ModuleNames
    {
        Batteri,
        Diode,
        Modstand,
        Amperemeter,
        Ledning
    }
}

public class BaseModule : ScriptableObject, IModule<ModuleBox>
{
    public int MQTTResistance;
    public Sprite Picture;
    public Color ModuleColor;
    private protected ModuleBox _box;
    public ModuleType Type;
    public ModuleNames Name;

    private bool _powered = false;
    public float Resistance;
    public float Voltage { get; set; }
    public float Amperage { get; set; }
    public float Power { get; set; }

    private protected ModuleManager _moduleManager;

    public virtual void UpdateModule()
    {
        var voltageLoss = 0.0f;

        if (FindMySeries() is List<BaseModule> series)
        {
            var seriesResistance = _moduleManager.GetResistance(series);
            var seriesVoltage = _moduleManager.SeriesVoltage[series];
            Amperage = Formulas.CalculateAmperage(seriesVoltage, seriesResistance);

            voltageLoss = Formulas.CalculateLoss(seriesVoltage, Resistance, seriesResistance);
        }

        TransferPower(voltageLoss);
        _powered = true;
    }

    public virtual void TransferPower(float loss)
    {
        try
        {
            var neighbor = GetNeighbor(NeighborDir.Right);
            neighbor.Voltage = Voltage - loss;
            neighbor.UpdateModule();
        }
        catch
        {
            Debug.LogError("Module update failed! End of circuit reached!");
        }
    }

    public void Init(ModuleBox box)
    {
        _box = box;
        _moduleManager = ModuleManager.Instance;
        Debug.Log($"{this.GetType().Name} initialized!");
    }

    private List<BaseModule> FindMySeries()
    {
        foreach (List<BaseModule> series in _moduleManager.FullCircuit)
        {
            if (series.Contains(this))
            {
                return series;
            }
        }
        return null;
    }

    public float GetResistance()
    {
        return Resistance;
    }

    public BaseModule GetNeighbor(NeighborDir dir)
    {
        if (_box.Neighbors.ContainsKey(dir))
        {
            return _box.Neighbors[dir].PlacedModule;
        }
        return null;
    }

    public void NoPower()
    {
        Debug.Log("No power detected! " + this);
        Voltage = 0;
        Amperage = 0;
        Power = 0;
    }

    public virtual bool IsPowered()
    {
        var powered = _powered;
        _powered = false;
        return powered;
    }
}