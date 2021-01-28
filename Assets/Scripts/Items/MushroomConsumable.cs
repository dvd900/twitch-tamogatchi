using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomConsumable : Consumable
{
    [SerializeField] private float _animSpeed;
    [SerializeField] private Renderer _rend;

    protected override bool IsPickupabble { get { return _isDoneSpawning; } }

    private bool _isDoneSpawning;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(RampFadeRoutine());
    }

    private IEnumerator RampFadeRoutine()
    {
        float _rampOffset = 0;
        while (_rampOffset < 1)
        {
            _rend.material.SetFloat(VBShaderUtils.P_RAMP_OFFSET, _rampOffset);
            _rampOffset += _animSpeed * Time.deltaTime;

            yield return null;
        }

        _isDoneSpawning = true;
    }
}
