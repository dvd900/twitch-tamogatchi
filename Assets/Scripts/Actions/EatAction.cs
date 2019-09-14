using UnityEngine;
using System.Collections;

public class EatAction : AIAction {

    public EatAction(Skin skin) : base(skin) {

    }

    public override AIAction Generate() {
        return new EatAction(_skin);
    }

    public override void Interrupt() {

    }

    public override bool IsFinished() {
        return !_skin.itemController.isEating;
    }

    public override void StartAction() {
        _skin.itemController.EatHeldItem();
    }

    public override void UpdateAction() {
    }

    public override float Score(AIWorldData data) {
        if(_skin.itemController.heldItem != null && _skin.itemController.heldItem is Consumable) {
            return .65f;
        } else {
            return 0.0f;
        }
    }
}
