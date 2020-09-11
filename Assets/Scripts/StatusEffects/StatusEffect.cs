
using UnityEngine;

public abstract class StatusEffect : MonoBehaviour
{
    public abstract EffectType Type { get; }

    public float _ttl;

    private float _timeRemaining;

    protected virtual void Start()
    {
        _timeRemaining = _ttl;
        Debug.Log("Starting effect: " + Type);
    }

    protected virtual void Update()
    {
        _timeRemaining -= Time.deltaTime;
        if(_timeRemaining < 0)
        {
            Debug.Log("stopping effect: " + Type);
            Destroy(this);
        }
    }
}