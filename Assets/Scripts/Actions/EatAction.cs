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
        return !_skin.itemController.IsEating;
    }

    public override void StartAction() {
        _skin.itemController.EatHeldItem();
    }

    public override void UpdateAction() {
    }

    public override float Score(AIWorldData data) {
        if(_skin.itemController.HeldItem != null && _skin.itemController.HeldItem is Consumable) {
            return .55f;
        } else {
            return 0.0f;
        }
    }
}
