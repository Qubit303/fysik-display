using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ModuleBox))]
[RequireComponent(typeof(BoxCollider2D))]
public class ModuleClick : MonoBehaviour
{
    private ModuleBox _myModule;
    public event System.Action<ModuleBox> OnModuleClicked;

    void Awake()
    {
        _myModule = GetComponent<ModuleBox>();
    }

    void OnMouseDown()
    {
        OnModuleClicked?.Invoke(_myModule);
    }
}
