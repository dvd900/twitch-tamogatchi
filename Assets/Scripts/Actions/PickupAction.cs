
using UnityEngine;

public class PickupAction : AIAction {

    private static Item bestItem;

    private Item _item;

    public PickupAction(Skin skin) : base(skin) {
        _item = bestItem;
    }

    public override AIAction Generate() {
        return new PickupAction(_skin);
    }

    public override void Interrupt() {
        _skin.movementController.StopWalking();
    }

    public override bool IsFinished() {
        return _skin.itemController.heldItem == _item && !_skin.itemController.isPickingUp;
    }

    public override void StartAction() {
        _skin.movementController.WalkToPosition(_item.transform.position);
    }

    public override void UpdateAction() {
        if (!_skin.movementController.isWalking && !_skin.itemController.isPickingUp 
            && _skin.itemController.heldItem != _item) {

            _skin.itemController.Pickup(_item);
        }
    }

    public override float Score(AIWorldData data) {
        if(data.closestItem == null) {
            return 0;
        }

        float bestScore = ScoreItem(data.closestItem);
        bestItem = data.closestItem;

        foreach(Item item in data.itemsInRange) {
            if(item.value <= data.closestItem.value) {
                continue;
            }

            float score = ScoreItem(item);
            if(score > bestScore) {
                bestScore = score;
                bestItem = item;
            }
        }
        Debug.Log(bestScore);
        return bestScore;
    }

    private float ScoreItem(Item item) {
        float d = (item.transform.position - _skin.transform.position).magnitude;
        float distMult = .3f + .2f * (1.0f - d / _skin.itemController.pickupRange);

        float heldItemMod = 0.0f;
        if(_skin.itemController.heldItem != null) {
            heldItemMod += .2f;

            // eventually incorporate modifier for same type item
            //if(_skin.itemController.heldItem == item) {
            //    heldItemMod += .05f;
            //}
        }

        return (item.value + .5f) * distMult - heldItemMod;
    }
}
