using UnityEngine;
using System.Collections;
using System;

public class ItemController : MonoBehaviour {
    public float PickupRange { get { return _pickupRange; } }
    /// <summary>
    /// Maximum distance that you can see items to go pick em up
    /// </summary>
    [SerializeField] private float _pickupRange;
    /// <summary>
    /// How close you have to be to the item to grab it
    /// </summary>
    [SerializeField] private float _grabRange;

    public Item HeldItem { get { return _heldItem; } }
    private Item _heldItem;

    public bool IsPickingUp { get { return _isPickingUp; } }
    private bool _isPickingUp;

    public bool IsEating { get { return _isEating; } }
    private bool _isEating;

    private Skin _skin;

    private Item _itemToPickup;

    private float _defaultArmStiffness;
    private float _defaultArmDamping;
    private Quaternion _heldItemRot;

    private void Start() {
        _skin = GetComponent<Skin>();

//        _defaultArmStiffness = _skin.lArmBone.m_Stiffness;
//        _defaultArmDamping = _skin.lArmBone.m_Damping;
    }

    private void LateUpdate() {
        if (_heldItem != null) {
            UpdateHeldItemPos();
        }
    }

    private void UpdateHeldItemPos() {
        _heldItem.transform.position = _skin.lHandTransform.position;
        _heldItem.transform.rotation = _skin.lHandTransform.rotation * _heldItemRot;
    }

    public void Pickup(Item item) {
        if (_heldItem != null) {
            DropHeldItem();
        }

        _skin.animator.SetTrigger("pickup");

        _itemToPickup = item;
        _isPickingUp = true;
    }



    public void DropHeldItem() {
        if (_heldItem == null) {
            Debug.LogError("Trying to drop a null item!");
            return;
        }

        //_skin.lArmBone.m_Damping = _defaultArmDamping;
        //_skin.lArmBone.m_Stiffness = _defaultArmStiffness;
        //_skin.lArmBone.UpdateParameters();

        //_skin.rArmBone.m_Damping = _defaultArmDamping;
        //_skin.rArmBone.m_Stiffness = _defaultArmStiffness;
        //_skin.rArmBone.UpdateParameters();

        _heldItem.DisableHolding();
        _heldItem.transform.parent = null;

        _isPickingUp = false;

        _heldItem = null;
    }

    public bool IsInRange(Item item) {
        Vector3 d = item.transform.position - transform.position;
        d.y = 0;
        return d.magnitude < _grabRange + _skin.movementController.WPRange + CoordsUtils.EPSILON;
    }

    public Vector3 GetPickupDest(Item item) {
        Vector3 d = transform.position - item.transform.position;
        d.y = 0;
        return item.transform.position + _grabRange * d.normalized;
    }

    public void DoBiteItem()
    {
        ((Consumable)_heldItem).DoEat();
    }

    public void DoPutItemInHand()
    {
        _heldItem = _itemToPickup;
        _itemToPickup = null;
        //_heldItemRot = Quaternion.AngleAxis(90, Vector3.right);
        _heldItem.EnableHolding(_skin);

        _heldItemRot = Quaternion.Inverse(_skin.lHandTransform.rotation) * _heldItem.transform.rotation;

        UpdateHeldItemPos();
    }

    public void DoPickupDone()
    {
        _isPickingUp = false;

        if(_heldItem is Cosmetic)
        {
            _skin.cosmeticController.EquipItem((Cosmetic)_heldItem);
            _heldItem = null;
        }
    }

    public void DoEatDone()
    {
        _isEating = false;
    }

    public void EatHeldItem() {
        if (_heldItem == null) {
            Debug.LogError("Trying to eat a null item!");
            return;
        }

        _skin.emoteController.ChewEmote();
        _isEating = true;
    }

}
