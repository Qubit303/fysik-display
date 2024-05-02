using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Color Data", menuName = "ColorData")]
public class ColorData : ScriptableObject
{
    [Header("Display Colors")]
    public Color BackgroundColor;
    public Color PopUpColor;

    [Header("Display Text Colors")]
    [Space(2)]
    public Color WarningColor;
    public Color WarningOutlineColor;
    [Space(1)]
    public Color PopUpTextColor;

    [Header("Module Colors")]
    [Space(2)]
    public Color DefaultColor;
    public Color DiodeColor;
    public Color ResistorColor;
    public Color BatteryColor;
}
