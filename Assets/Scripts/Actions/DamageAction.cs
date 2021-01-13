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

        public DamageAction(Skin skin, DamageType damageType, float stunDuration = 1.0f)
        {
            _skin = skin;
            _damageType = damageType;
            _stunDuration = stunDuration;
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

            yield return new WaitForSeconds(_stunDuration);
        }
    }
}