
using System.Collections.Generic;
using UnityEngine;

public class EffectFactory : MonoBehaviour
{
    public static EffectFactory Instance { get { return _instance; } }
    private static EffectFactory _instance;

    [SerializeField] private StatusEffect[] _effectPrefabs;

    private Dictionary<EffectType, StatusEffect> _effectDictionary;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        _effectDictionary = new Dictionary<EffectType, StatusEffect>();
        foreach(var effect in _effectPrefabs)
        {
            _effectDictionary.Add(effect.Type, effect);
        }
    }

    public StatusEffect CreateNewEffectOnObject(EffectType type, GameObject target)
    {
        Debug.Log("Adding effect: " + type + " to: " + target);
        var effect = _effectDictionary[type];
        if(effect == null)
        {
            Debug.LogError("Effect prefab not found for type: " + type);
            return null;
        }

        return UnityUtils.CopyComponent<StatusEffect>(effect, target);
    }
}