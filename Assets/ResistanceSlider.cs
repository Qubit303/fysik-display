using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ResistanceSlider : MonoBehaviour
{
    public event Action<float> OnResistanceChanged;
    [SerializeField] private TextMeshProUGUI _resistanceText;

    public void ChangeResistance(float resistance)
    {
        OnResistanceChanged?.Invoke(resistance);
        _resistanceText.text = resistance.ToString() + " Ω";
    }

    public void SetValue(float resistance)
    {
        GetComponent<Slider>().value = resistance;
    }
}
