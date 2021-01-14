
using UnityEngine;

namespace AIActions
{
    public class IdleAction : SweeTangoAction, GeneratedAction
    {
        private const float CHANCE_TO_SPEAK = 0.2f;

        private float _waitTime;
        private float _timer;
        private bool _sayDialogue;

        /// <summary>
        /// Chance to say dialogue. if not, wait ~avgIdleTime, otherwise say dialogue then wait ~avgIdleTime / 2
        /// </summary>
        /// <param name="skin"></param>
        public IdleAction(Skin skin) : base(skin)
        {
            _waitTime = _skin.stats.AvgIdleTime
                + _skin.stats.AvgIdleTime * (Random.value - .5f);

            if(!_skin.speechController.IsSpeaking)
            {
                _sayDialogue = Random.value < CHANCE_TO_SPEAK;
            }

            if (_sayDialogue)
            {
                _waitTime /= 2.0f;
            }
        }

        public IdleAction(Skin skin, float waitTime, bool sayDialogue) : base(skin)
        {
            _waitTime = waitTime;
            _sayDialogue = sayDialogue;
        }

        GeneratedAction GeneratedAction.Generate(AISkin skin)
        {
            return new IdleAction((Skin)skin);
        }

        public override void Interrupt() { }

        public override bool IsFinished()
        {
            return _timer <= 0;
        }

        float GeneratedAction.Score(AISkin skin)
        {
            if(skin.GetActionController().LastAction is DamageAction)
            {
                return 0;
            }
            return .3f;
        }

        public override void StartAction()
        {
            _timer = _waitTime;
            _skin.movementController.StopWalking();
            if (_sayDialogue)
            {
                _skin.movementController.FaceCamera();
                _skin.speechController.PrepareRandomDialogue();
            }
        }

        public override void UpdateAction()
        {
            if (!_sayDialogue && !_skin.speechController.IsSpeaking)
            {
                _timer -= Time.deltaTime;
            }
            else if (_sayDialogue && !_skin.movementController.IsTurning)
            {
                _skin.speechController.PlayPreparedClip();
                _sayDialogue = false;
            }
        }
    }
}
