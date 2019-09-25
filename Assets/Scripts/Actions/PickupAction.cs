
using UnityEngine;

public class PickupAction : AIAction {

    private static Item bestItem;

    private Item _item;
    /// <summary>
    /// Did we start turning to face the item were gona pick up?
    /// </summary>
    private bool _turnStarted;

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
        WalkToItem();
    }

    public override void UpdateAction() {
        if (!_skin.movementController.IsWalking && !_skin.itemController.isPickingUp 
            && _skin.itemController.heldItem != _item) {

            if(_skin.itemController.IsInRange(_item)) {
                if(_turnStarted && !_skin.movementController.IsTurning) {
                    _skin.itemController.Pickup(_item);
                }
                else if (!_turnStarted) {
                    _skin.movementController.LookAtPosition(_item.transform.position);
                    _turnStarted = true;
                }
            } else {
                WalkToItem();
            }

        }
    }

    private void WalkToItem() {
        Debug.Log("WALKING TO ITEM");
        Vector3 dest = _skin.itemController.GetPickupDest(_item);
        _skin.movementController.WalkToPosition(dest);
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

        return ((item.value / 100.0f) + .5f) * distMult - heldItemMod;
    }
}
