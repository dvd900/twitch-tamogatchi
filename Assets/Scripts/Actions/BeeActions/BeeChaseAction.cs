
using System.Collections;
using UnityEngine;

namespace AIActions
{
    public class BeeChaseAction : CoroutineAction
    {
        private BeeController _bee;
        private Animator _beeAnimator;
        private Skin _tango;
        private Transform _targetTransform;

        public BeeChaseAction(BeeController bee, Animator beeAnimator, Skin tango, Transform targetTransform)
        {
            _bee = bee;
            _beeAnimator = beeAnimator;
            _tango = tango;
            _targetTransform = targetTransform;
        }

        protected override IEnumerator DoAction()
        {

            UpdateTarget();
            Vector3 d = _targetTransform.position - _bee.transform.position;
            float dmag = d.magnitude;

            float t = dmag / (25 * _bee.Speed);
            var moveTween = LeanTween.move(_bee.gameObject, _targetTransform, t).setEaseInBack();
            _bee.transform.LookAt(_targetTransform);

            _beeAnimator.SetBool("Angry", true);

            while (LeanTween.isTweening(moveTween.id))
            {
                yield return null;

                if(_tango == null)
                {
                    yield break;
                }

                UpdateTarget();
            }
            _bee.Sting();
            _tango.movementController.StopWalking();
            _tango.movementController.FaceCamera();
            _tango.emoteController.DiscomfortEmote();
            _tango.statsController.AddHealth(-3);

            yield return new WaitForSeconds(.1f);

            _bee.Die();
        }

        private void UpdateTarget()
        {
            Vector3 target = _tango.headCollider.ClosestPoint(_bee.transform.position);
            Vector3 headCenter = _tango.headCollider.transform.TransformPoint(_tango.headCollider.center);
            Vector3 normal = target - headCenter;
            normal.Normalize();
            _targetTransform.position = target + 1.5f * normal;
        }
    }
}
