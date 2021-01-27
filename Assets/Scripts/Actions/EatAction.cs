using UnityEngine;
using System.Collections;

namespace AIActions
{
    public class EatAction : SweeTangoAction, GeneratedAction
    {
        private bool _hasEaten;
        private float _timer;

        public EatAction(Skin skin) : base(skin)
        {
            _timer = 2.0f;
        }

        GeneratedAction GeneratedAction.Generate(AISkin skin)
        {
            return new EatAction((Skin)skin);
        }

        public override void Interrupt()
        {
            _skin.itemController.DoEatDone();
        }

        public override bool IsFinished()
        {
            return _timer <= 0;
            //return !_skin.itemController.IsEating && !_skin.movementController.IsTurning;
        }

        public override void StartAction()
        {
            _skin.movementController.StopWalking();
            _skin.movementController.FaceCamera();
        }

        public override void UpdateAction()
        {
            _timer -= Time.deltaTime;

            if (!_hasEaten && !_skin.movementController.IsTurning)
            {
                _skin.itemController.EatHeldItem();
                _hasEaten = true;
            }
        }

        float GeneratedAction.Score(AISkin skin)
        {
            if (_skin.itemController.HeldItem != null && _skin.itemController.HeldItem is Consumable)
            {
                return .55f;
            }
            else
            {
                return 0.0f;
            }
        }
    }
}
