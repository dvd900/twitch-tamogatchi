
using UnityEngine;

namespace AIActions
{
    public class WalkToAction : SweeTangoAction, GeneratedAction
    {
        private const float CHANCE_TO_SPEAK = .1f;

        private static GameObject _debugMarker;

        private Vector3 _dest;
        private bool _sayDialogue;

        public WalkToAction(Skin skin) : base(skin)
        {
            _dest = CoordsUtils.RandomWorldPointOnScreen();
        }

        public WalkToAction(Skin skin, Vector3 dest, GameObject debugMarker) : base(skin)
        {
            if (_debugMarker != null)
            {
                GameObject.Destroy(_debugMarker);
            }

            _debugMarker = debugMarker;
            _dest = dest;
        }

        GeneratedAction GeneratedAction.Generate(AISkin skin)
        {
            return new WalkToAction((Skin)skin);
        }

        public override void Interrupt()
        {
            _skin.movementController.StopWalking();
        }

        public override bool IsFinished()
        {
            return !_skin.movementController.IsWalking;
        }

        public override void StartAction()
        {
            _skin.movementController.WalkToPosition(_dest);

            if(!_skin.speechController.IsSpeaking && Random.value < CHANCE_TO_SPEAK)
            {
                _skin.speechController.PrepareRandomDialogue();
                _skin.speechController.PlayPreparedClip();
            }
        }

        public override void UpdateAction()
        {
        }

        float GeneratedAction.Score(AISkin data)
        {
            return .3f;
        }
    }
}