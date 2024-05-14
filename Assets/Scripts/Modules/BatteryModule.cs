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

        if (GetNeighbor(Directions.Up) is BaseModule upperBox)
        {
            CreateLane(upperBox);
            ActivateWire(Directions.Up);
        }
        else RemoveWire(Directions.Up);

        if (GetNeighbor(Directions.Down) is BaseModule lowerBox)
        {
            CreateLane(lowerBox);
            ActivateWire(Directions.Down);
        }
        else RemoveWire(Directions.Down);
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
            currentBox = currentBox.GetNeighbor(Directions.Right);
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
