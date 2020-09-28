
using UnityEngine;

public class SickEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Sick; } }
    
    private Color _originalColor;

    protected override void DoStartEffect()
    {
        _skin.emoteController.StartPukeEmote();
        LeanTween.value(gameObject, 0, 1.3f, 1f).setOnUpdate((float val) => {
            _skin.bodyRend.material.SetFloat("_SickAmt", val);
        });
    }

    protected override void DoUpdateEffect()
    {
    }
    protected override void DoStopEffect()
    {
        LeanTween.value(gameObject, 1.3f, 0, 5f).setOnUpdate((float val) => {
            _skin.bodyRend.material.SetFloat("_SickAmt", val);
        });
    }

}