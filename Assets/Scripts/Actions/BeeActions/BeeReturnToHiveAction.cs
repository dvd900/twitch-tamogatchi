using System;
using System.Collections;
using UnityEngine;

namespace AIActions
{
    public class BeeReturnToHiveAction : CoroutineAction
    {
        private BeeController _bee;

        public BeeReturnToHiveAction(BeeController bee)
        {
            _bee = bee;
        }

        protected override IEnumerator DoAction()
        {
            Vector3 target = _bee.Hive.transform.position;
            target.y += 6;
            Vector3 d = target - _bee.transform.position;
            float t = d.magnitude / (10 * _bee.Speed);

            _bee.transform.LookAt(target);
            var moveTween = LeanTween.move(_bee.gameObject, target, t).setEaseInOutQuad();

            while(LeanTween.isTweening(moveTween.id))
            {
                yield return null;
            }
        }
    }
}
