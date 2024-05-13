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

    public List<List<BaseModule>> FullCircuit { get; private set; } = new();
    public Dictionary<List<BaseModule>, float> SeriesVoltage { get; private set; } = new();

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

            var placedModule = Instantiate(Modules[closestResistance]);
            ModuleBoxes[pos].PlaceModule(placedModule);
            ModulesInScene.Add(ModuleBoxes[pos], placedModule);
            Debug.Log($"<color=green>Module created successfully at position [<b>{pos}</b>] with resistance [<b>{closestResistance}</b>]!</color>");
        }
        catch (Exception e)
        {
            Debug.LogError("Module creation failed! " + e.Message);
        }
    }

    private void RemoveModule(ModuleBox box)
    {
        Debug.Log($"Removing module at position: {box}");
        try
        {
            box.RemoveModule();
            ModulesInScene.Remove(box);
            Debug.Log($"<color=red>Module removed successfully at position [<b>{box}</b>]!</color>");
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
        Debug.Log("Checking for updates...");
        string moduleKeys = string.Join(", ", ModulesInScene.Keys);
        Debug.Log($"Modules in scene: {moduleKeys}");
        foreach (ModuleBox moduleBox in ModulesInScene.Keys)
        {
            Debug.Log(moduleBox.ReceivedUpdate);
            if (!moduleBox.ReceivedUpdate)
            {
                RemoveModule(moduleBox);
                continue;
            }
            moduleBox.ReceivedUpdate = false;
        }
    }

    public void UpdateCiruit(string[] messages)
    {
        FullCircuit.Clear();
        SeriesVoltage.Clear();

        UpdateModules(messages);
        CheckForUpdates();
        UpdatePower();
        CheckPower();
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
            }
        }
    }

    private void UpdatePower()
    {
        if (ModuleBoxes[0].PlacedModule is BatteryModule firstModule)
        {
            firstModule.UpdateModule();
            _insertBatteryText.gameObject.SetActive(false);
            return;
        }
        _insertBatteryText.gameObject.SetActive(true);
        Debug.LogError("Power update failed! No battery found at the beginning of the circuit!");
    }

    private void CheckPower()
    {
        foreach (BaseModule module in ModulesInScene.Values)
        {
            if (module.IsPowered() == false)
            {
                module.NoPower();
            }
        }
    }

    public void ClearCircuit()
    {
        ModulesInScene.Clear();
    }

    public ModuleBox GetBox(int i)
    {
        return ModuleBoxes[i];
    }

    public float GetResistance(List<List<BaseModule>> parallelCircuit)
    {
        float resistance = 0;
        foreach (List<BaseModule> circuit in parallelCircuit)
        {
            resistance += GetResistance(circuit);
        }
        return resistance;
    }

    public float GetResistance(List<BaseModule> seriesCircuit)
    {
        float resistance = 0;
        foreach (BaseModule module in seriesCircuit)
        {
            resistance += module.GetResistance();
        }
        return resistance;
    }

    public float GetResistance(BaseModule module)
    {
        return module.GetResistance();
    }
}
