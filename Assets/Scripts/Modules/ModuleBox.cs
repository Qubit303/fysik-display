using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Module.Neighbors;
using System;
using System.Linq;

namespace Module.Neighbors
{
    public enum NeighborDir
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum Lane
    {
        Upper,
        Lower,
        Beginning
    }
}

public class ModuleBox : MonoBehaviour
{
    [field: SerializeField] public int Number { get; private set; }
    [field: SerializeField] public Lane ModuleLane { get; private set; }
    public Dictionary<NeighborDir, ModuleBox> Neighbors { get; private set; } = new();
    public BaseModule PlacedModule { get; set; } = null;
    public bool ReceivedUpdate { get; set; } = false;
    public float Voltage { get; set; } = 0;
    public float Resistance { get; set; } = 0;

    private ModuleManager _moduleManager;

    void Start()
    {
        _moduleManager = ModuleManager.Instance;
        if (ModuleLane != Lane.Beginning) FindNeighbors();
    }

    public void UpdateModule()
    {
        if (PlacedModule != null)
        {
            PlacedModule.UpdateModule();
            return;
        }

        try
        {
            Neighbors[NeighborDir.Right].Voltage = Voltage;
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError("Module update failed! No module found! " + e.Message);
        }
    }

    public void TransferPower()
    {

    }

    public void PlaceModule(BaseModule module)
    {
        ReceivedUpdate = true;

        PlacedModule = module;
        PlacedModule.Init(this);
        SetColor(module.ModuleColor);
    }

    public void RemoveModule()
    {
        if (PlacedModule == null)
        {
            Debug.Log("No module to remove!");
            return;
        }

        PlacedModule = null;
        SetColor(Color.white);
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    private void FindNeighbors()
    {
        try
        {
            ModuleBox left = _moduleManager.GetBox(Number - 1).ModuleLane == ModuleLane ? _moduleManager.GetBox(Number - 1) : null;
            if (left != null) Neighbors.Add(NeighborDir.Left, left);
        }
        catch (Exception e)
        {
            Debug.LogError("No module found to the left! " + e.Message);
        }

        try
        {
            ModuleBox right = _moduleManager.GetBox(Number + 1).ModuleLane == ModuleLane ? _moduleManager.GetBox(Number + 1) : null;
            if (right != null) Neighbors.Add(NeighborDir.Right, right);
        }
        catch (Exception e)
        {
            Debug.LogError("No module found to the right! " + e.Message);
        }

        if (ModuleLane == Lane.Upper)
        {
            try
            {
                ModuleBox down = _moduleManager.GetBox(Number + 4);
                Neighbors.Add(NeighborDir.Down, down);
            }
            catch (Exception e)
            {
                Debug.LogError("No module found below! " + e.Message);
            }
        }
        else
        {
            try
            {
                ModuleBox up = _moduleManager.GetBox(Number - 4);
                Neighbors.Add(NeighborDir.Up, up);
            }
            catch (Exception e)
            {
                Debug.LogError("No module found above! " + e.Message);
            }
        }

        string allNeighbors = string.Join(", ", Neighbors.Values.Select(x => x.Number));
        Debug.Log($"All Neighbors [{Number}]: " + allNeighbors);
    }
}
