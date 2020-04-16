using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public Skin _skin;
    public Planner _planner;
    public ItemSpawner _itemSpawner;
    public GameObject _walkDestPrefab;

    private void Update() {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 viewMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);

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

        if(Input.GetButtonDown("Emote")) {
            int emoteNum = Random.Range(0, 3);
            switch(emoteNum) {
                case 0:
                    _skin.emoteController.DiscomfortEmote();
                    break;
                case 1:
                    _skin.emoteController.Wave();
                    break;
                case 2:
                    _skin.emoteController.Cheer();
                    break;
            }
        }

        if(Input.GetButtonDown("Idle")) {
            DoIdle();
        }

        if (Input.GetButtonDown("Fire1")) {
            DoClick(viewMousePos);
        }

        if(Input.GetButtonDown("Fire2")) {
            DoRightClick(viewMousePos);
        }

        //_skin.faceController.DoLookAt(CoordsUtils.ScreenToWorldPos(mousePos));
    }

    public void DoIdle() {
        IdleAction action = new IdleAction(_skin);
        action.waitTime = 5.0f;
        _skin.actionController.DoAction(action);
    }

    public void DoRightClick(Vector2 viewPos) {
        Vector3 worldPos = CoordsUtils.ViewToWorldPos(viewPos);
        //_itemSpawner.SpawnItem(3, worldPos);
        _itemSpawner.SpawnRandomItem(worldPos);
    }

    public void DoClick(Vector2 viewPos) {
        Debug.Log(viewPos);

        Vector3 worldPoint = CoordsUtils.ViewToWorldPos(viewPos);
        var marker = Instantiate<GameObject>(_walkDestPrefab, worldPoint, Quaternion.identity);

        WalkToAction action = new WalkToAction(_skin);
        action.dest = worldPoint;
        action._debugMarker = marker;
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
