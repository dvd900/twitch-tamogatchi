
using UnityEngine;

public class SickEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Sick; } }

    public Color _sickColor;
    
    private Color _originalColor;

    protected override void DoStartEffect()
    {
        _originalColor = _skin.bodyRend.material.color;
        _skin.bodyRend.material.color = _sickColor;

        Debug.Log("Setting sick color to: " + _sickColor);
    }

    protected override void DoUpdateEffect()
    {

    }

    protected override void DoStopEffect()
    {
        _skin.bodyRend.material.color = _originalColor;
    }
}