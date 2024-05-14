using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class ModuleDataFieldManager : MonoBehaviour
{
    private BaseModule _currentDataField = null;
    private ModuleManager _moduleManager;

    [Header("Data Fields")]
    [SerializeField] private TextMeshProUGUI _moduleTypeField;
    [SerializeField] private TextMeshProUGUI _voltageField;
    [SerializeField] private TextMeshProUGUI _resistanceField;
    [SerializeField] private TextMeshProUGUI _neighborField;

    [Header("Extra")]
    [Space(2)]
    [SerializeField] private Animator _animator;

    void Start()
    {
        _moduleManager = ModuleManager.Instance;

        foreach (ModuleClick moduleClick in FindObjectsOfType<ModuleClick>())
        {
            moduleClick.OnModuleClicked += UpdateDataField;
        }

        _moduleManager.CircuitUpdated += RefreshDataField;
        _moduleManager.CircuitCleared += ClearDataField;
    }

    private void UpdateDataField(ModuleBox moduleBox)
    {
        if (!_moduleManager.CircuitIsPowered) return;

        var module = moduleBox.PlacedModule;

        if (module is BaseModule && _currentDataField == module) return;
        else if (_currentDataField == null) PlayDataFieldAnimation("Show");
        SetData(module);
        _currentDataField = module;
    }

    private void RefreshDataField()
    {
        if (_currentDataField == null) return;
        if (!_moduleManager.CircuitIsPowered) ClearDataField();

        SetData(_currentDataField);
    }

    public void ClearDataField()
    {
        if (_currentDataField == null) return;
        SetNullData();
        _currentDataField = null;
        PlayDataFieldAnimation("Hide");
    }

    private void SetData(BaseModule module)
    {
        if (module == null)
        {
            SetNullData();
            return;
        }
        _moduleTypeField.text = module.Name.ToString();
        _voltageField.text = module.Voltage.ToString("F2") + " V";
        _resistanceField.text = module.Resistance.ToString("F2") + " Ω";
        _neighborField.text = module.Amperage.ToString("F2") + " A";
    }

    private void SetNullData()
    {
        _moduleTypeField.text = "Intet Modul";
        _voltageField.text = "0.00 V";
        _resistanceField.text = "0.0 Ω";
        _neighborField.text = "0.00 A";
    }

    private void PlayDataFieldAnimation(string clip) => _animator.Play(clip);
}
