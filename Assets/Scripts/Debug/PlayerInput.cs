using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public Skin _skin;
    public Planner _planner;

    void Update() {
        if (Input.GetButtonDown("Interact")) {
            if (_skin.itemController.heldItem != null) {
                _skin.actionController.DoAction(new EatAction(_skin));
            } else {
                Item item = _planner.worldData.closestItem;
                if (item != null) {
                    _skin.itemController.Pickup(item);
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
            //if(item != null) {
            //    _skin.actionController.DoAction(new PickupAction(_skin, item));
            //} else {
            //    _skin.actionController.DoAction(new WalkToAction(_skin, hit.point));
            //}
        }
    }

}
