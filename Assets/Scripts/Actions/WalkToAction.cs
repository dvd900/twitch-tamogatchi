
using UnityEngine;

public class WalkToAction : AIAction {

    public Vector3 dest { get { return _dest; } set { _dest = value; } }
    private Vector3 _dest;

    public WalkToAction(Skin skin) : base(skin) {
        _dest = CoordsUtils.RandomWorldPointOnScreen();
    }

    public override AIAction Generate() {
        return new WalkToAction(_skin);
    }

    public override void Interrupt() {
        _skin.movementController.StopWalking();
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