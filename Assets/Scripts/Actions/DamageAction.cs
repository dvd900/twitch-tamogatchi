using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIActions
{
    public enum DamageType
    {
        Discomfort,
        Bombed
    }

    public class DamageAction : CoroutineAction
    {
        private Skin _skin;
        private DamageType _damageType;
        private float _stunDuration;
        private AIAction _actionToResume;

        public DamageAction(Skin skin, DamageType damageType, float stunDuration)
        {
            _skin = skin;
            _damageType = damageType;
            _stunDuration = stunDuration;

            var currentAction = skin.actionController.CurrentAction;
            if(currentAction is WalkToAction || currentAction is PickupAction)
            {
                _actionToResume = currentAction;
            }
            else if(currentAction is SleepAction)
            {
                _stunDuration = Mathf.Max(1.0f, stunDuration);
            }
            else if(currentAction is DamageAction)
            {
                var prevDamageAction = (currentAction as DamageAction);
                _actionToResume = prevDamageAction._actionToResume;
                _stunDuration = Mathf.Max(_stunDuration, prevDamageAction._stunDuration);
            }
        }

        protected override IEnumerator DoAction()
        {
            if (_skin.speechController.IsSpeaking)
            {
                _skin.speechController.StopSpeaking();
            }

            switch(_damageType)
            {
                case DamageType.Discomfort:
                    _skin.emoteController.DiscomfortEmote();
                    break;
                case DamageType.Bombed:
                    _skin.emoteController.Bombed();
                    break;
            }

            while(_stunDuration > 0)
            {
                _stunDuration -= Time.deltaTime;
                yield return null;
            }

            if(_actionToResume != null)
            {
                _skin.actionController.DoAction(_actionToResume);
            }
        }
    }
}