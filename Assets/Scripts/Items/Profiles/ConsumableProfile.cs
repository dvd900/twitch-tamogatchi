
using ItemSystem;
using UnityEngine;

[System.Serializable]
public class ConsumableProfile : ItemProfile {

    [Header("Consumable Properties")]
    public float _healthMod;
    public float _staminaMod;
    public float _hungerMod;
    public float _happinessMod;

    public EffectProfile[] _statusEffects;

    public override void UpdateUniqueProperties(ItemBase itemToChangeTo) {
        base.UpdateUniqueProperties(itemToChangeTo);

        ConsumableProfile item = (ConsumableProfile)itemToChangeTo;

        _healthMod = item._healthMod;
        _staminaMod = item._staminaMod;
        _hungerMod = item._hungerMod;
        _happinessMod = item._happinessMod;
        _statusEffects = item._statusEffects;
    }
}

[System.Serializable]
public class EffectProfile
{
    public EffectType EffectType;
    public float EffectChance;
}
