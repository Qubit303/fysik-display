using System;
using UnityEngine;

[RequireComponent(typeof(ModuleBox))]
[RequireComponent(typeof(CircleCollider2D))]
public class ModuleClick : MonoBehaviour
{
    private ModuleBox _myModule;
    public event Action<ModuleBox> OnModuleClicked;
    public event Action Clicked;

    private BaseModule _moduleClicked;

    void Awake()
    {
        _myModule = GetComponent<ModuleBox>();
    }

    private void Start()
    {
        var moduleManager = ModuleManager.Instance;

        moduleManager.CircuitCleared += AnotherClicked;

        foreach (var module in FindObjectsOfType<ModuleClick>())
        {
            if (module == this) continue;
            Clicked += module.AnotherClicked;
        }
    }

    void OnMouseDown()
    {
        var module = _myModule.PlacedModule;

        if (module is BaseModule)
        {
            _moduleClicked = module;
            _moduleClicked.OnClick();
        }

        OnModuleClicked?.Invoke(_myModule);
        Clicked?.Invoke();
    }

    public void AnotherClicked()
    {
        if (_moduleClicked is BaseModule)
        {
            _moduleClicked.UnClick();
            _moduleClicked = null;
        }
    }
}
