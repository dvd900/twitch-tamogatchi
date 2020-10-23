
using UnityEngine;

namespace AIActions
{
    public class WalkToAction : SweeTangoAction, GeneratedAction
    {
        private static GameObject _debugMarker;

        private Vector3 _dest;

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