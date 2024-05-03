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
        if (CheckUpperLaneBox())
        {
            ModuleBox upperBox = Neighbors[NeighborDir.Up];
            _moduleManager.ParallelCircuits.Add(new List<ModuleBox> { upperBox });
            upperBox.Voltage = Voltage;
            upperBox.UpdateModule();
        }

        if (CheckLowerLaneBox())
        {
            ModuleBox lowerBox = Neighbors[NeighborDir.Down];
            _moduleManager.ParallelCircuits.Add(new List<ModuleBox> { lowerBox });
            lowerBox.Voltage = Voltage;
            lowerBox.UpdateModule();
        }
    }

    private bool CheckUpperLaneBox()
    {
        for (int i = 1; i < 5; i++)
        {
            if (_moduleManager.ModuleBoxes[i].PlacedModule == null) continue;
            return true;
        }
        return false;
    }

    private bool CheckLowerLaneBox()
    {
        for (int i = 5; i < 9; i++)
        {
            if (_moduleManager.ModuleBoxes[i].PlacedModule == null) continue;
            return true;
        }
        return false;
    }

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
