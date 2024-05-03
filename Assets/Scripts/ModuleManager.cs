using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using Module.Types;
using TMPro;

public class ModuleManager : MonoBehaviour
{
    public static ModuleManager Instance;

    [SerializeField] private BaseModule[] _moduleObjects;

    public Dictionary<int, BaseModule> Modules { get; private set; } = new();
    public Dictionary<int, ModuleBox> ModuleBoxes { get; private set; } = new();
    public Dictionary<ModuleBox, BaseModule> ModulesInScene { get; private set; } = new();

    public List<List<ModuleBox>> ParallelCircuits { get; private set; } = new();

    public event Action CircuitUpdated;

    [SerializeField] ColorData _colorData;

    //Display Text
    [Header("Display Text")]
    [Space(2)]
    [SerializeField] private TextMeshProUGUI _insertBatteryText;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        foreach (BaseModule module in _moduleObjects)
        {
            Modules.Add(module.MQTTResistance, module);
        }

        foreach (ModuleBox moduleBox in FindObjectsOfType<ModuleBox>())
        {
            ModuleBoxes.Add(moduleBox.Number, moduleBox);
        }
    }

    private void CreateModule(int pos, int resistance)
    {
        Debug.Log($"Creating module: [{pos}, {resistance}]");
        try
        {
            int closestResistance = 0;
            foreach (KeyValuePair<int, BaseModule> module in Modules)
            {
                if (Math.Abs(module.Key - resistance) < Math.Abs(closestResistance - resistance))
                {
                    closestResistance = module.Key;
                }
            }

            if (!IsBoxEmpty(pos) && IsModuleSame(closestResistance, ModuleBoxes[pos].PlacedModule.MQTTResistance))
            {
                Debug.LogError("Module creation failed! Module already exists at this position!");
                ModuleBoxes[pos].ReceivedUpdate = true;
                return;
            }

            if (closestResistance - resistance > 300)
            {
                Debug.LogError("Module creation failed! No module with resistance close enough found!");
                return;
            }

            ModuleBoxes[pos].PlaceModule(Modules[closestResistance]);
            ModulesInScene.Add(ModuleBoxes[pos], Modules[closestResistance]);
            Debug.Log($"<color=green>Module created successfully at position [<b>{pos}</b>] with resistance [<b>{closestResistance}</b>]!</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Module creation failed! " + e.Message);
        }
    }

    private void RemoveModule(int pos)
    {
        Debug.Log($"Removing module at position: {pos}");
        try
        {
            ModuleBoxes[pos].RemoveModule();
            ModulesInScene.Remove(ModuleBoxes[pos]);
            Debug.Log($"<color=red>Module removed successfully at position [<b>{pos}</b>]!</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Module removal failed! " + e.Message);
        }
    }

    private bool IsModuleSame(int newResistance, int oldResistance)
    {
        return newResistance == oldResistance;
    }

    private bool IsBoxEmpty(int pos)
    {
        return ModuleBoxes[pos].PlacedModule == null;
    }

    private void CheckForUpdates()
    {
        foreach (ModuleBox moduleBox in ModulesInScene.Keys)
        {
            if (!moduleBox.ReceivedUpdate)
            {
                moduleBox.RemoveModule();
                continue;
            }
            moduleBox.ReceivedUpdate = false;
        }
    }

    public void UpdateCiruit(string[] messages)
    {
        UpdateModules(messages);
        CheckForUpdates();
        UpdatePower();
        CircuitUpdated?.Invoke();
    }

    private void UpdateModules(string[] messages)
    {
        foreach (string message in messages)
        {
            string[] digits = Regex.Split(message, @"\D+");

            if (digits[0] == "")
            {
                continue;
            }

            if (int.Parse(digits[1]) > 0)
            {
                CreateModule(int.Parse(digits[0]), int.Parse(digits[1]));
                continue;
            }
            RemoveModule(int.Parse(digits[0]));
        }
    }

    private void UpdatePower()
    {
        ModuleBox firstModule = ModuleBoxes[0];
        if (firstModule.PlacedModule == null || firstModule.PlacedModule.Name != ModuleNames.Batteri)
        {
            _insertBatteryText.gameObject.SetActive(true);
            Debug.LogError("Power update failed! No battery found at the beginning of the circuit!");
            return;
        }
        firstModule.UpdateModule();
        _insertBatteryText.gameObject.SetActive(false);
    }

    public void ClearCircuit()
    {
        ModulesInScene.Clear();
    }

    public ModuleBox GetBox(int i)
    {
        return ModuleBoxes[i];
    }

    public float GetResistance(List<List<ModuleBox>> parallelCircuit)
    {
        float resistance = 0;
        foreach (List<ModuleBox> circuit in parallelCircuit)
        {
            foreach (ModuleBox moduleBox in circuit)
            {
                resistance += moduleBox.PlacedModule.Resistance;
            }
        }
        return resistance;
    }

    public float GetResistance(List<ModuleBox> seriesCircuit)
    {
        float resistance = 0;
        foreach (ModuleBox moduleBox in seriesCircuit)
        {
            resistance += moduleBox.PlacedModule.Resistance;
        }
        return resistance;
    }

    public float GetResistance(ModuleBox moduleBox)
    {
        return moduleBox.PlacedModule.Resistance;
    }
}
