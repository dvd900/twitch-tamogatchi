
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public abstract EffectType Type { get; }

    public float _ttl;

    protected Skin _skin;

    protected bool _isInfinite;
    protected float _timeRemaining;

    protected abstract void DoStartEffect();
    protected abstract void DoUpdateEffect();
    protected abstract void DoStopEffect();

    public virtual bool IsFinished { get { return _timeRemaining < 0; } }

    public void StartEffect(Skin skin)
    {
        _skin = skin;

        if (_ttl < 0)
        {
            _isInfinite = true;
        }
        else
        {
            _timeRemaining = _ttl;
        }

        DoStartEffect();
    }

    public void UpdateEffect()
    {
        DoUpdateEffect();

        if(_isInfinite)
        {
            return;
        }

        _timeRemaining -= Time.deltaTime;
    }

    public void StopEffect()
    {
        DoStopEffect();
        Destroy(this);
    }

    public virtual void StackEffect()
    {
        if(!_isInfinite)
        {
            _timeRemaining = _ttl;
        }
    }
}