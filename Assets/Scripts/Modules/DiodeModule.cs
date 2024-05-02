using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DiodeModule", menuName = "Modules/Diode")]
public class DiodeModule : BaseModule, IModule<ModuleBox>
{
    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Diode;
    }
}
