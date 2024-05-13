using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Neighbors;
using System.Runtime.InteropServices;

public class BatteryBox : ModuleBox
{
    private protected override void Start()
    {
        _moduleManager = ModuleManager.Instance;
        FindNeighbors();
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
