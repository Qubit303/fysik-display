using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResistanceModule", menuName = "Modules/Resistance")]
public class ResistanceModule : BaseModule, IModule<ModuleBox>
{
    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Modstand;
    }
}
