
public class PickupAction : AIAction {

    private Item _item;

    public PickupAction(Skin skin, Item item) : base(skin) {
        _item = item;
    }

    public override AIAction Clone() {
        return new PickupAction(_skin, _item);
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
        if (!_skin.movementController.isWalking && _skin.itemController.heldItem == null) {
            _skin.itemController.Pickup(_item);
        }
    }

    public override float Score(AIWorldData data) {
        if(data.closestItem == null) {
            return 0;
        }

        float bestScore = ScoreItem(data.closestItem);
        Item bestItem = data.closestItem;

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

        _item = bestItem;
        return bestScore;
    }

    private float ScoreItem(Item item) {
        float d = (item.transform.position - _skin.transform.position).magnitude;
        float distMult = .8f + .2f * (1 - d / _skin.itemController.pickupRange);

        return item.value * distMult;
    }
}
