using UnityEngine;

[CreateAssetMenu(fileName = "DiodeModule", menuName = "Modules/Diode")]
public class DiodeModule : BaseModule, IModule<ModuleBox>
{
    public Color DiodeColor;
    public GameObject DiodeObject;
    private Diode _diode;

    public override void UpdateModule()
    {
        base.UpdateModule();

        _diode.SetLightRadius(0.5f + 2 * Amperage);
    }

    void OnValidate()
    {
        Name = Module.Types.ModuleNames.Diode;
    }

    public override void Init(ModuleBox box)
    {
        base.Init(box);

        _diode = Instantiate(DiodeObject, box.transform.position, Quaternion.identity, box.transform).GetComponent<Diode>();
        _box.SetColor(DiodeColor);
        _diode.GetComponent<SpriteRenderer>().color = new Color(DiodeColor.r, DiodeColor.g, DiodeColor.b, 0.4f);
    }

    public override void NoPower()
    {
        base.NoPower();
        _diode.SetLightRadius(0);
    }

    public override void OnRemove()
    {
        base.OnRemove();

        Destroy(_diode.gameObject);
    }
}
