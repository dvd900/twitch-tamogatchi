using UnityEngine;
using System.Collections;

public class EatAction : AIAction {

    private bool _hasEaten;

    public EatAction(Skin skin) : base(skin)
    {

    }

    public override AIAction Generate()
    {
        return new EatAction(_skin);
    }

    public override void Interrupt()
    {
        _skin.itemController.DoEatDone();
    }

    public override bool IsFinished()
    {
        return !_skin.itemController.IsEating && !_skin.movementController.IsTurning;
    }

    public override void StartAction()
    {
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
    }

    public override void UpdateAction()
    {
        if (!_hasEaten && !_skin.movementController.IsTurning)
        {
            _skin.itemController.EatHeldItem();
            _hasEaten = true;
        }
    }

    public override float Score(AIWorldData data)
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
