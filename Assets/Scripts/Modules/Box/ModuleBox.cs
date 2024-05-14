using System.Collections.Generic;
using UnityEngine;
using Module.Neighbors;
using System;
using System.Linq;

namespace Module.Neighbors
{
    public enum Directions
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
    [field: SerializeField] public int Number { get; private protected set; }
    [field: SerializeField] public Lane ModuleLane { get; private protected set; }
    public Dictionary<Directions, ModuleBox> Neighbors { get; private set; } = new();
    public BaseModule PlacedModule { get; set; } = null;
    public bool ReceivedUpdate { get; set; } = false;

    private protected ModuleManager _moduleManager;
    private protected GameObject _spot;

    [Header("Wires")]
    [Space(2)]
    public Wire[] Wires;
    public Dictionary<Directions, GameObject> WireDictionary { get; private set; } = new();

    private protected virtual void Start()
    {
        _moduleManager = ModuleManager.Instance;
        FindNeighbors();

        foreach (Wire wire in Wires)
        {
            WireDictionary.Add(wire.Direction, wire.WireObject);
        }

        _spot = transform.GetChild(0).gameObject;
    }

    public void PlaceModule(BaseModule module)
    {
        _spot.SetActive(false);
        ReceivedUpdate = true;

        PlacedModule = module;
        PlacedModule.Init(this);
        SetSprite(module.Picture);
        //cool
    }

    public void RemoveModule()
    {
        if (PlacedModule == null)
        {
            Debug.Log("No module to remove!");
            return;
        }

        PlacedModule.OnRemove();
        PlacedModule = null;

        _spot.SetActive(true);
        GetComponent<SpriteRenderer>().sprite = null;
    }

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }

    private protected virtual void FindNeighbors()
    {
        try
        {
            ModuleBox left = _moduleManager.GetBox(Number - 1).ModuleLane == ModuleLane ? _moduleManager.GetBox(Number - 1) : null;
            if (left != null) Neighbors.Add(Directions.Left, left);
        }
        catch (Exception e)
        {
            Debug.LogError("No module found to the left! " + e.Message);
        }

        try
        {
            ModuleBox right = _moduleManager.GetBox(Number + 1).ModuleLane == ModuleLane ? _moduleManager.GetBox(Number + 1) : null;
            if (right != null) Neighbors.Add(Directions.Right, right);
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
                Neighbors.Add(Directions.Down, down);
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
                Neighbors.Add(Directions.Up, up);
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
