
using UnityEngine;

public class WalkToAction : AIAction {

    private Vector3 dest;

    public WalkToAction(Skin skin, Vector3 dest) : base(skin) {
        this.dest = dest;
    }

    public override void Interrupt() {
        skin.movementController.StopWalking();
    }

    public override bool IsFinished() {
        return !skin.movementController.isWalking;
    }

    public override void StartAction() {
        skin.movementController.WalkToPosition(dest);
    }

    public override void UpdateAction() {

    }
}