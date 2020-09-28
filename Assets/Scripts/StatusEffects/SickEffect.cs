
using UnityEngine;

public class SickEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Sick; } }

    public Color _sickColor;
    
    private Color _originalColor;

    protected override void DoStartEffect()
    {
        LeanTween.value(gameObject, 0, 1.5f, 1f).setOnUpdate((float val) => {
            _skin.bodyRend.material.SetFloat("_SickAmt", val);
        });
    }

    protected override void DoUpdateEffect()
    {
        Debug.Log("Setting sick color to: " + _sickColor);
    }
    protected override void DoStopEffect()
    {
        LeanTween.value(gameObject, 1.5f, 0, 1f).setOnUpdate((float val) => {
            _skin.bodyRend.material.SetFloat("_SickAmt", val);
        });
    }

}