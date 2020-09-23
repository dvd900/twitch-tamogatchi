
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

    public PickupAction(Skin skin, Item item) : base(skin)
    {
        _item = item;
    }

    public override AIAction Generate() {
        return new PickupAction(_skin);
    }

    public override void Interrupt() {
        _skin.movementController.StopWalking();
    }

    public override bool IsFinished() {
        return _item == null || (_skin.itemController.HeldItem == _item && !_skin.itemController.IsPickingUp);
    }

    public override void StartAction() {
        WalkToItem();
    }

    public override void UpdateAction() {
        if(_item == null)
        {
            _skin.movementController.StopWalking();
            return;
        }

        if (!_skin.movementController.IsWalking && !_skin.itemController.IsPickingUp 
            && _skin.itemController.HeldItem != _item) {

            if(_skin.itemController.IsInRange(_item)) {
                if(_turnStarted && !_skin.movementController.IsTurning) {
                    _skin.itemController.Pickup(_item);
                }
                else if (!_turnStarted) {
                    _skin.movementController.FaceTarget(_item.transform);
                    _turnStarted = true;
                }
            } else {
                WalkToItem();
            }

        }
    }

    private void WalkToItem() {
        Vector3 dest = _skin.itemController.GetPickupDest(_item);
        _skin.movementController.WalkToPosition(dest);
    }

    public override float Score(AIWorldData data) {
        if(data.ClosestItem == null) {
            return 0;
        }

        float bestScore = ScoreItem(data.ClosestItem);
        bestItem = data.ClosestItem;

        foreach(Item item in data.ItemsInRange) {
            if(item.value <= data.ClosestItem.value) {
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
        float distMult = .3f + .2f * (1.0f - d / _skin.itemController.PickupRange);

        float heldItemMod = 0.0f;
        if(_skin.itemController.HeldItem != null) {
            heldItemMod += .2f;

            // eventually incorporate modifier for same type item
            //if(_skin.itemController.heldItem == item) {
            //    heldItemMod += .05f;
            //}
        }

        return ((item.value / 100.0f) + .5f) * distMult - heldItemMod;
    }
}
