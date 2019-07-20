using UnityEngine;
using System.Collections;
using System;

public class ItemController : MonoBehaviour {
    public float pickupRange { get { return _pickupRange; } }
    [SerializeField] private float _pickupRange;

    public Item heldItem { get { return _heldItem; } }
    private Item _heldItem;

    public bool isPickingUp { get { return _isPickingUp; } }
    private bool _isPickingUp;

    public bool isEating { get { return _isEating; } }
    private bool _isEating;

    private Skin _skin;

    private Item _itemToPickup;

    private float _defaultArmStiffness;
    private float _defaultArmDamping;

    private void Start() {
        _skin = GetComponent<Skin>();

        _defaultArmStiffness = _skin.lArmBone.m_Stiffness;
        _defaultArmDamping = _skin.lArmBone.m_Damping;
    }

    private void LateUpdate() {
        if (_heldItem != null) {
            UpdateHeldItemPos();
        }
    }

    private void UpdateHeldItemPos() {
        _heldItem.transform.position = _skin.lHandTransform.position;
        _heldItem.transform.rotation = _skin.lHandTransform.rotation;
    }

    public void Pickup(Item item) {
        if (_heldItem != null) {
            Drop();
        }

        _skin.animator.SetTrigger("pickup");

        _itemToPickup = item;
        _isPickingUp = true;
    }

    public void AE_PutItemInHand() {
        Debug.Log("AE put item in hadn");
        _skin.lArmBone.m_Damping = 1f;
        _skin.lArmBone.m_Stiffness = 1f;
        _skin.lArmBone.UpdateParameters();

        _skin.rArmBone.m_Damping = 1f;
        _skin.rArmBone.m_Stiffness = 1f;
        _skin.rArmBone.UpdateParameters();

        _heldItem = _itemToPickup;
        _itemToPickup = null;
        _heldItem.EnableHolding(_skin);

        UpdateHeldItemPos();
    }

    public void AE_PickupDone() {
        Debug.Log("AE pickup done");
        _isPickingUp = false;
    }

    public void Drop() {
        if (_heldItem == null) {
            Debug.LogError("Trying to drop a null item!");
            return;
        }

        _skin.lArmBone.m_Damping = _defaultArmDamping;
        _skin.lArmBone.m_Stiffness = _defaultArmStiffness;
        _skin.lArmBone.UpdateParameters();

        _skin.rArmBone.m_Damping = _defaultArmDamping;
        _skin.rArmBone.m_Stiffness = _defaultArmStiffness;
        _skin.rArmBone.UpdateParameters();

        _heldItem.DisableHolding();
        _heldItem.transform.parent = null;

        _isPickingUp = false;

        _heldItem = null;
    }

    public void AE_TakeBite() {
        Debug.Log("AE take pite");
        _heldItem.DoEat();
    }

    public void AE_EatDone() {
        Debug.Log("ADE eat done");
        _isEating = false;
    }

    public void EatHeldItem() {
        if (_heldItem == null) {
            Debug.LogError("Trying to eat a null item!");
            return;
        }

        _skin.animator.SetTrigger("eat");
        _isEating = true;
    }

}
