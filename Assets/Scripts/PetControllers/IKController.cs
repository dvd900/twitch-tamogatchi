using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    [SerializeField] private bool _ikActive;

    [SerializeField] private Transform _lookObj;
    [SerializeField] private Transform _rightHandObj;
    [SerializeField] private Transform _leftHandObj;

    private Skin _skin;
    private Animator _animator;

    private void Start() {
        _skin = GetComponent<Skin>();
        _animator = _skin.animator;
    }

    public void SetLookAt(Transform lookObj) {
        _lookObj = lookObj;
    }

    public void ClearLookAt() {
        _lookObj = null;
    }

    public void SetRHandTarget(Transform rHandObj) {
        _rightHandObj = rHandObj;
    }

    public void ClearRHandTarget() {
        _rightHandObj = null;
    }

    public void SetLHandTarget(Transform lHandObj) {
        _leftHandObj = lHandObj;
    }

    public void ClearLHandTarget() {
        _leftHandObj = null;
    }

    private void OnAnimatorIK() {
        if (_ikActive) {

            if (_lookObj != null) {
                _animator.SetLookAtWeight(1);
                _animator.SetLookAtPosition(_lookObj.position);
            } else {
                _animator.SetLookAtWeight(0);
            }

            if (_rightHandObj != null) {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.RightHand, _rightHandObj.position);
                _animator.SetIKRotation(AvatarIKGoal.RightHand, _rightHandObj.rotation);
            } else {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }

            if (_leftHandObj != null) {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                _animator.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandObj.position);
                _animator.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandObj.rotation);
            } else {

                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        } else {
            _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            _animator.SetLookAtWeight(0);
        }
        
    }
}
