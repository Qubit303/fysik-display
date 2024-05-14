using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WireModule", menuName = "Modules/Wire")]
public class WireModule : BaseModule
{
    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Ledning;
    }
}
