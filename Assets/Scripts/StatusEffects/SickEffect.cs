
using UnityEngine;

public class SickEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Sick; } }

    public Color _sickColor;

    private Skin _tangoSkin;
    private Color _originalColor;
    
    protected override void Start()
    {
        base.Start();
        _tangoSkin = gameObject.GetComponent<Skin>();
        _originalColor = _tangoSkin.bodyRend.material.color;
        _tangoSkin.bodyRend.material.color = _sickColor;

        Debug.Log("Setting sick color to: " + _sickColor);
    }

    private void OnDestroy()
    {
        _tangoSkin.bodyRend.material.color = _originalColor;
    }
}