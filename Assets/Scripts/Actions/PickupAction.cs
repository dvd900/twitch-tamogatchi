
public class PickupAction : AIAction {

    private Item item;

    public PickupAction(Skin skin, Item item) : base(skin) {
        this.item = item;
    }

    public override void Interrupt() {
        skin.movementController.StopWalking();
    }

    public override bool IsFinished() {
        return skin.pickupController.heldItem == item;
    }

    public override void StartAction() {
        skin.movementController.WalkToPosition(item.transform.position);
    }

    public override void UpdateAction() {
        if(!skin.movementController.isWalking) {
            skin.pickupController.Pickup(item);
        }
    }
}
