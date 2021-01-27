
using System.Collections.Generic;
using UnityEngine;

public class EffectController: MonoBehaviour
{
    private Skin _skin;
    private List<StatusEffect> _effects;

    private void Start()
    {
        _skin = GetComponent<Skin>();
        _effects = new List<StatusEffect>();
    }

    public void AddEffect(EffectType type)
    {
        Debug.Log("ADDING: " + type);
        foreach(var effect in _effects)
        {
            if(effect.Type == type)
            {
                effect.StackEffect();
                return;
            }
        }

        StatusEffect newEffect = EffectFactory.Instance.CreateNewEffectOnObject(type, gameObject);
        newEffect.StartEffect(_skin);
        _effects.Add(newEffect);
    }

    public void RemoveEffect(EffectType type)
    {
        for(int i = 0; i < _effects.Count; i++)
        {
            if(_effects[i].Type == type)
            {
                _effects[i].StopEffect();
                _effects.RemoveAt(i);
                break;
            }
        }
    }

    private void Update()
    {
        for(int i = 0; i < _effects.Count; i++)
        {
            _effects[i].UpdateEffect();
            if(_effects[i].IsFinished)
            {
                _effects[i].StopEffect();
                _effects.RemoveAt(i);
                i--;
            }
        }
    }
}