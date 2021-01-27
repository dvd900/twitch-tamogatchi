
using AIActions;
using UnityEngine;

public class SickEffect : StatusEffect
{
    public override EffectType Type { get { return EffectType.Sick; } }
    
    [SerializeField] private Color _sickColor;
    [SerializeField] private float _pukeInterval;
    [SerializeField] private float _healthDamage;
    [SerializeField] private float _hungerDamage;

    private Color _originalColor;
    private float _pukeTimer;

    protected override void DoStartEffect()
    {
        _originalColor = _skin.bodyRend.material.color;
        LeanTween.color(_skin.bodyRend.gameObject, _sickColor, 1.0f);

        DoPuke();

        //LeanTween.value(gameObject, 0, 1.3f, 1f).setOnUpdate((float val) => {
            //_skin.bodyRend.material.SetFloat("_SickAmt", val);
        //});
    }

    protected override void DoUpdateEffect()
    {
        _pukeTimer -= Time.deltaTime;
        if (_pukeTimer <= 0)
        {
            DoPuke();
        }
    }

    private void DoPuke()
    {
        _skin.actionController.DoAction(new IdleAction(_skin, 5.0f, false));
        _skin.emoteController.StartPukeEmote();
        _skin.statsController.AddHunger(-_hungerDamage);
        _skin.statsController.AddHealth(-_healthDamage);
        _pukeTimer = _pukeInterval;
    }

    protected override void DoStopEffect()
    {
        LeanTween.color(_skin.bodyRend.gameObject, _originalColor, 1.0f);

        //LeanTween.value(gameObject, 1.3f, 0, 5f).setOnUpdate((float val) => {
            //_skin.bodyRend.material.SetFloat("_SickAmt", val);
        //});
    }

}