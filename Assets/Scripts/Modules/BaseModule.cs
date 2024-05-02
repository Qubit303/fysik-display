using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Types;

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
        Amperemeter
    }
}

public class BaseModule : ScriptableObject, IModule<ModuleBox>
{
    public int Resistance;
    public Sprite Picture;
    public Color ModuleColor;
    private protected ModuleBox _box;
    public ModuleType Type;
    public ModuleNames Name;

    public virtual void UpdateModule()
    {
        Debug.Log($"{this.GetType().Name}[{_box.Number}] updated!");
    }

    public void Init(ModuleBox box)
    {
        _box = box;
        Debug.Log($"{this.GetType().Name} initialized!");
    }
}