using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ModuleDataFieldManager : MonoBehaviour
{
    private BaseModule _currentDataField = null;

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
        foreach (ModuleClick moduleClick in FindObjectsOfType<ModuleClick>())
        {
            moduleClick.OnModuleClicked += UpdateDataField;
        }

        ModuleManager.Instance.CircuitUpdated += RefreshDataField;
    }

    private void UpdateDataField(ModuleBox moduleBox)
    {
        var module = moduleBox.PlacedModule;

        if (module is BaseModule && _currentDataField == module) return;
        else if (_currentDataField == null) PlayDataFieldAnimation("Show");
        SetData(module);
        _currentDataField = module;
    }

    private void RefreshDataField()
    {
        if (_currentDataField == null) return;
        SetData(_currentDataField);
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
        _resistanceField.text = module.Resistance.ToString("F1") + " Ω";
        _neighborField.text = module.Amperage.ToString("F2") + " A";
    }

    private void SetNullData()
    {
        _moduleTypeField.text = "No Module";
        _voltageField.text = "0.00 V";
        _resistanceField.text = "0.0 Ω";
        _neighborField.text = "0.00 A";
    }

    private void PlayDataFieldAnimation(string clip) => _animator.Play(clip);
}
