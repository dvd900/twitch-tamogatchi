﻿
using UnityEngine;

public class WalkToAction : AIAction
{
    private GameObject _debugMarker;
    private Vector3 _dest;

    public WalkToAction(Skin skin) : base(skin) {
        _dest = CoordsUtils.RandomWorldPointOnScreen();
    }

    public WalkToAction(Skin skin, Vector3 dest, GameObject debugMarker) : base(skin)
    {
        _debugMarker = debugMarker;
        _dest = dest;
    }

    public override AIAction Generate() {
        return new WalkToAction(_skin);
    }

    public override void Interrupt() {
        _skin.movementController.StopWalking();
        if(_debugMarker != null)
        {
            GameObject.Destroy(_debugMarker);
        }
    }

    public override bool IsFinished() {
        return !_skin.movementController.IsWalking;
    }

    public override void StartAction() {
        _skin.movementController.WalkToPosition(_dest);
    }

    public override void UpdateAction() {

    }

    public override float Score(AIWorldData data) {
        return .3f;
    }
}