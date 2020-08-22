using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

    public static PlayerInput Instance;

    public Skin _skin;
    public Planner _planner;
    public ItemSpawner _itemSpawner;
    public GameObject _walkDestPrefab;

    [SerializeField] private LayerMask _clickLayerMask;

    private int _emoteNum = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Update() {
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        Vector2 viewMousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);

        if (Input.GetButtonDown("Interact")) {
            if (_skin.itemController.HeldItem != null) {
                _skin.actionController.DoAction(new EatAction(_skin));
            } else {
                Item item = _planner.worldData.closestItem;
                if (item != null) {
                    _skin.itemController.Pickup(item);
                }
            }
        }

        if(Input.GetButtonDown("Emote")) {
            DoEmote();
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

    private void DoEmote()
    {
        _skin.actionController.DoAction(new EmoteAction(_skin));
    }

    private void DoIdle() {
        IdleAction action = new IdleAction(_skin);
        action.waitTime = 5.0f;
        _skin.actionController.DoAction(action);
    }

    private void DoRightClick(Vector2 viewPos) {
        Vector3 worldPos = CoordsUtils.ViewToWorldPos(viewPos);
        //_itemSpawner.SpawnItem(3, worldPos);
        _itemSpawner.SpawnRandomItem(worldPos, "local player");
    }

    private void DoClick(Vector2 viewPos) {
        Ray ray = LevelRefs.Instance.WorldCam.ViewportPointToRay(viewPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f, _clickLayerMask))
        {
            if(hit.collider.tag == VBLayerMask.ItemTag)
            {
                var item = hit.collider.GetComponent<Item>();
                PickupAction action = new PickupAction(_skin, item);
                _skin.actionController.DoAction(action);
            }
            else if(hit.collider.tag == VBLayerMask.SweeTangoTag)
            {
                if(_skin.itemController.HeldItem != null)
                {
                    EatAction action = new EatAction(_skin);
                    _skin.actionController.DoAction(action);
                }
                else
                {
                    DoEmote();
                }
            }
            else if(hit.collider.tag == VBLayerMask.GroundTag)
            {
                var marker = Instantiate<GameObject>(_walkDestPrefab, hit.point, Quaternion.identity);

                WalkToAction action = new WalkToAction(_skin);
                action.dest = hit.point;
                action._debugMarker = marker;
                _skin.actionController.DoAction(action);
            }
        }
        else
        {
            Debug.Log("Didn't hit anything");
        }
        Vector3 worldPoint = CoordsUtils.ViewToWorldPos(viewPos);
        
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
