using System.Collections;
using System.Collections.Generic;
using DitzelGames.FastIK;
using UnityEngine;

public class IKController : MonoBehaviour
{
    [SerializeField] private bool _ikActive;
    [SerializeField] private FastIKFabric _lHandIK;

    private Coroutine _lHandRoutine;

    private void Start() {
    }

    //public void SetRHandTarget(Transform rHandObj, float transitionTime) {

    //}

    //public void ClearRHandTarget(float transitionTime) {

    //}

    public void SetLHandTarget(Transform lHandObj, float transitionTime) {
        if(_lHandRoutine != null) {
            StopCoroutine(_lHandRoutine);
        }
        _lHandRoutine = StartCoroutine(BlendIK(_lHandIK, lHandObj, transitionTime));
    }

    public void ClearLHandTarget(float transitionTime) {
        if (_lHandRoutine != null) {
            StopCoroutine(_lHandRoutine);
        }
        _lHandRoutine = StartCoroutine(BlendIK(_lHandIK, null, transitionTime));
    }

    private IEnumerator BlendIK(FastIKFabric ikController, Transform target, float duration) {
        float targetBlend = 0;
        if(target != null) {
            ikController.Target = target;
            targetBlend = 1;
        }

        float time = 0;
        float startBlend = ikController.BlendAmt;
        while(time < duration) {
            float f = time / duration;
            ikController.BlendAmt = startBlend * (1 - f) + targetBlend * f;
            time += Time.deltaTime;
            yield return null;
        }

        ikController.BlendAmt = targetBlend;
        if(target == null) {
            ikController.Target = null;
        }
    }

    //private void OnAnimatorIK() {
    //if (_ikActive) {

    //    if (_lookObj != null) {
    //        _animator.SetLookAtWeight(1);
    //        _animator.SetLookAtPosition(_lookObj.position);
    //    } else {
    //        _animator.SetLookAtWeight(0);
    //    }

    //    if (_rightHandObj != null) {
    //        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
    //        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    //        _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandObj.position);
    //        _animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandObj.rotation);
    //    } else {
    //        _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
    //        _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    //    }

    //    if (_leftHandObj != null) {
    //        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    //        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    //        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandObj.position);
    //        _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandObj.rotation);
    //    } else {

    //        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
    //        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
    //    }
    //} else {
    //    _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
    //    _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
    //    _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
    //    _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
    //    _animator.SetLookAtWeight(0);
    //}

    //}
}
