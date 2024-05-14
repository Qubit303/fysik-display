using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CircuitDataField : MonoBehaviour
{
    private bool _shown = false;

    [Header("Data Fields")]
    [SerializeField] private TextMeshProUGUI _voltageField;
    [SerializeField] private TextMeshProUGUI _resistanceField;
    [SerializeField] private TextMeshProUGUI _amperageField;
    [SerializeField] private TextMeshProUGUI _powerField;

    [Header("Extra")]
    [Space(2)]
    [SerializeField] private Animator _animator;

    private ModuleManager _moduleManager;

    private void Start()
    {
        _moduleManager = ModuleManager.Instance;

        _moduleManager.CircuitChanged += UpdateDataField;
        _moduleManager.CircuitCleared += ClearDataField;

    }

    private void UpdateDataField(CircuitArgs args)
    {
        if (!_shown && _moduleManager.CircuitIsPowered)
        {
            PlayDataFieldAnimation("Show");
            _shown = true;
        }
        if (!_moduleManager.CircuitIsPowered) ClearDataField();

        _voltageField.text = args.Voltage.ToString("F2") + " V";
        _resistanceField.text = args.Resistance.ToString("F2") + " Ω";
        _amperageField.text = args.Amperage.ToString("F2") + " A";
        _powerField.text = args.Power.ToString("F2") + " W";
    }

    private void ClearDataField()
    {
        if (_shown)
        {
            PlayDataFieldAnimation("Hide");
            _shown = false;
        }

        _voltageField.text = "0.00 V";
        _resistanceField.text = "0.0 Ω";
        _amperageField.text = "0.00 A";
        _powerField.text = "0.00 W";
    }

    private void PlayDataFieldAnimation(string clip) => _animator.Play(clip);
}
