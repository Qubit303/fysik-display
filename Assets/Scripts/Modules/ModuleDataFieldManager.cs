using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class ModuleDataFieldManager : MonoBehaviour
{
    private ModuleBox _currentDataField = null;

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
        if (_currentDataField == moduleBox) return;
        else if (_currentDataField == null) PlayDataFieldAnimation("Show");
        SetData(moduleBox);
        _currentDataField = moduleBox;
    }

    private void RefreshDataField()
    {
        if (_currentDataField == null) return;
        SetData(_currentDataField);
    }

    private void SetData(ModuleBox moduleBox)
    {
        BaseModule module = moduleBox.PlacedModule;
        _moduleTypeField.text = module == null ? "Ledning" : module.Name.ToString();
        _voltageField.text = moduleBox.Voltage.ToString();
        _resistanceField.text = moduleBox.Resistance.ToString();

        string allNeighbors = string.Join(", ", moduleBox.Neighbors.Values.Select(x => x.Number));
        _neighborField.text = allNeighbors;
    }

    private void PlayDataFieldAnimation(string clip) => _animator.Play(clip);
}
