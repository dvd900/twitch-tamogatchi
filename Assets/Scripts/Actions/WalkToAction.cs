
using UnityEngine;

public class WalkToAction : AIAction {

    private Vector3 _dest;

    public WalkToAction(Skin skin, Vector3 dest) : base(skin) {
        _dest = dest;
    }

    public override AIAction Clone() {
        return new WalkToAction(_skin, _dest);
    }

    public override void Interrupt() {
        _skin.movementController.StopWalking();
    }

    public override bool IsFinished() {
        return !_skin.movementController.isWalking;
    }

    public override void StartAction() {
        _skin.movementController.WalkToPosition(_dest);
    }

    public override void UpdateAction() {

    }

    public override float Score(AIWorldData data) {
        _dest = CoordsUtils.RandomWorldPointOnScreen();
        return .25f;
    }
}