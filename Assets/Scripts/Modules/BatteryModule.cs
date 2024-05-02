using UnityEngine;

[CreateAssetMenu(fileName = "BatteryModule", menuName = "Modules/Battery")]
public class BatteryModule : BaseModule, IModule<ModuleBox>
{
    public float Voltage;

    public override void UpdateModule()
    {
        _box.Voltage = Voltage;
    }

    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Batteri;
    }
}
