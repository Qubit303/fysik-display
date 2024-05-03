using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Neighbors;

public class BatteryBox : ModuleBox
{
    private protected override void Start()
    {
        _moduleManager = ModuleManager.Instance;
        FindNeighbors();
    }

    public override void TransferPower()
    {
        /*List<ModuleBox> transferToBoxes = new List<ModuleBox>();
        transferToBoxes.Add(FindClosestUpperLaneBox());
        transferToBoxes.Add(FindClosestLowerLaneBox());*/

        foreach (ModuleBox box in Neighbors.Values)
        {
            if (box == null) continue;
            box.Voltage = Voltage;
            box.UpdateModule();
        }
    }

    /*private ModuleBox FindClosestUpperLaneBox()
    {
        for (int i = 1; i < 5; i++)
        {
            if (_moduleManager.ModuleBoxes[i].PlacedModule == null) continue;
            return _moduleManager.ModuleBoxes[i];
        }
        return null;
    }

    private ModuleBox FindClosestLowerLaneBox()
    {
        for (int i = 5; i < 9; i++)
        {
            if (_moduleManager.ModuleBoxes[i].PlacedModule == null) continue;
            return _moduleManager.ModuleBoxes[i];
        }
        return null;
    }*/

    private protected override void FindNeighbors()
    {
        Neighbors.Add(NeighborDir.Up, _moduleManager.ModuleBoxes[1]);
        Neighbors.Add(NeighborDir.Down, _moduleManager.ModuleBoxes[5]);
    }

    private void OnValidate()
    {
        ModuleLane = Lane.Beginning;
        Number = 0;
    }
}
