using UnityEngine;
using System.Collections.Generic;
using Module.Neighbors;

[CreateAssetMenu(fileName = "BatteryModule", menuName = "Modules/Battery")]
public class BatteryModule : BaseModule, IModule<ModuleBox>
{
    [field: SerializeField] public float BatteryVoltage;

    public override void UpdateModule()
    {
        Voltage = BatteryVoltage;
        TransferPower();
    }

    private void TransferPower()
    {

        if (GetNeighbor(NeighborDir.Up) is BaseModule upperBox)
        {
            CreateLane(upperBox);
        }

        if (GetNeighbor(NeighborDir.Down) is BaseModule lowerBox)
        {
            CreateLane(lowerBox);
        }
    }

    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Batteri;
    }

    private void AddLaneToCircuit(List<BaseModule> series)
    {
        _moduleManager.FullCircuit.Add(series);
    }

    private void CreateLane(BaseModule baseBox)
    {
        var lane = new List<BaseModule>() { baseBox };
        var currentBox = baseBox;
        while (true)
        {
            currentBox = currentBox.GetNeighbor(NeighborDir.Right);
            if (currentBox != null)
            {
                lane.Add(currentBox);
                continue;
            }
            break;
        }
        string finalLane = string.Join(", ", lane);
        Debug.Log("Lane created: " + finalLane);
        AddLaneToCircuit(lane);

        _moduleManager.SeriesVoltage.Add(lane, Voltage);

        baseBox.Voltage = Voltage;
        baseBox.UpdateModule();
    }

    public override bool IsPowered()
    {
        return true;
    }
}
