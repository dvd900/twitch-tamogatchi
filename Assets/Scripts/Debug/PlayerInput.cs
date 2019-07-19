using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public Skin skin;

    void Update() {
        if (Input.GetButtonDown("Interact")) {
            if (skin.pickupController.heldItem != null) {
                skin.pickupController.heldItem.Eat();
            } else {
                Item item = skin.pickupController.FindPickupCandidate();
                if (item != null) {
                    skin.pickupController.Pickup(item);
                }
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            DoClick(mousePos);
        }
    }

    public void DoClick(Vector2 screenPos) {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f)) {
            Item item = hit.transform.GetComponent<Item>();
            if(item != null) {
                skin.actionController.DoAction(new PickupAction(skin, item));
            } else {
                skin.actionController.DoAction(new WalkToAction(skin, hit.point));
            }
        }
    }

}
