using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrondClicked : MonoBehaviour
{
    public event Action OnBackgroundClicked;

    private void Start()
    {
        foreach (var module in FindObjectsOfType<ModuleClick>())
            OnBackgroundClicked += module.AnotherClicked;

        OnBackgroundClicked += FindObjectOfType<ModuleDataFieldManager>().ClearDataField;
    }

    void OnMouseDown()
    {
        OnBackgroundClicked?.Invoke();
    }
}
