using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushController : MonoBehaviour {

    [SerializeField] private float _animSpeed;
    [SerializeField] private Renderer _rend;

    IEnumerator Start() {
        yield return new WaitForSeconds(3.0f);
        StartCoroutine(RampFadeRoutine());
    }
    
    void Update() {

    }

    private IEnumerator RampFadeRoutine() {

        float _rampOffset = 0;
        while(true) {
            _rampOffset += _animSpeed * Time.deltaTime;
            if (_rampOffset > 1) {
                yield break;
            }

            _rend.material.SetFloat(VBShaderUtils.P_RAMP_OFFSET, _rampOffset);

            yield return null;
        }
    }
}
