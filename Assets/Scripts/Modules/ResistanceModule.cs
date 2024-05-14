using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResistanceModule", menuName = "Modules/Resistance")]
public class ResistanceModule : BaseModule, IModule<ModuleBox>
{
    public GameObject ResistanceSlider;
    private ResistanceSlider _slider = null;

    public override void OnClick()
    {
        if (_slider != null) return;
        _slider = Instantiate(ResistanceSlider, Vector2.zero, Quaternion.identity).GetComponentInChildren<ResistanceSlider>();
        _slider.SetValue(Resistance);
        _slider.OnResistanceChanged += ChangeResistance;
    }

    public override void UnClick()
    {
        if (_slider == null) return;
        Destroy(_slider.transform.root.gameObject);
        _slider = null;
    }

    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Modstand;
    }

    private void ChangeResistance(float resistance)
    {
        Resistance = resistance;
        _moduleManager.SoftUpdate();
    }
}
