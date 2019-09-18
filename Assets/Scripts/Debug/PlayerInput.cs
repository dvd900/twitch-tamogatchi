using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public Skin _skin;
    public Planner _planner;
    public ItemSpawner _itemSpawner;

    private void Update() {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

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

        if(Input.GetButtonDown("Idle")) {
            DoIdle();
        }

        if (Input.GetButtonDown("Fire1")) {
            DoClick(mousePos);
        }

        if(Input.GetButtonDown("Fire2")) {
            DoRightClick(mousePos);
        }

        //_skin.faceController.DoLookAt(CoordsUtils.ScreenToWorldPos(mousePos));
    }

    public void DoIdle() {
        IdleAction action = new IdleAction(_skin);
        action.waitTime = 5.0f;
        _skin.actionController.DoAction(action);
    }

    public void DoRightClick(Vector2 screenPos) {
        Vector3 worldPos = CoordsUtils.ScreenToWorldPos(screenPos);
        _itemSpawner.SpawnItem(3, worldPos);
        //_itemSpawner.SpawnRandomItem(worldPos);
    }

    public void DoClick(Vector2 screenPos) {
        Vector3 worldPoint = CoordsUtils.ScreenToWorldPos(screenPos);
        WalkToAction action = new WalkToAction(_skin);
        action.dest = worldPoint;
        _skin.actionController.DoAction(action);
        //Ray ray = Camera.main.ScreenPointToRay(screenPos);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, 1000f)) {
            //Item item = hit.transform.GetComponent<Item>();
            //if(item != null) {
            //    _skin.actionController.DoAction(new PickupAction(_skin, item));
            //} else {
            //    _skin.actionController.DoAction(new WalkToAction(_skin, hit.point));
            //}
        //}
    }

}
