using UnityEngine;
using System.Collections;
using System;

public class PickupController : MonoBehaviour 
{
    [SerializeField] private float pickupRange;

    private Skin skin;

    public Item heldItem { get { return m_heldItem; } }
    private Item m_heldItem;

    private float defaultArmStiffness;
    private float defaultArmDamping;

    private Collider[] closestItems;

    private void Start()
    {
        skin = GetComponent<Skin>();

        defaultArmStiffness = skin.lArmBone.m_Stiffness;
        defaultArmDamping = skin.lArmBone.m_Damping;

        closestItems = new Collider[10];
    }

    public Item FindPickupCandidate() 
    {
        Collider closestItem = null;

        int numHits = Physics.OverlapSphereNonAlloc(skin.feetTransform.position, 
            pickupRange, closestItems, VBLayerMask.Item);

        float minD = float.MaxValue;
        for(int i = 0; i < numHits; i++) 
        {
            Vector3 d = closestItems[i].transform.position - skin.feetTransform.position;
            float dMag = d.sqrMagnitude;
            if(dMag < minD) 
            {
                minD = dMag;
                closestItem = closestItems[i];
            }
        }

        if(closestItem != null) 
        {
            return closestItem.GetComponent<Item>();
        } 
        else 
        {
            return null;
        }
    }

    public void Pickup(Item item)
    {
        if(m_heldItem != null) 
        {
            Drop();
        }

        skin.lArmBone.m_Damping = 1f;
        skin.lArmBone.m_Stiffness = 1f;
        skin.lArmBone.UpdateParameters();

        skin.rArmBone.m_Damping = 1f;
        skin.rArmBone.m_Stiffness = 1f;
        skin.rArmBone.UpdateParameters();

        skin.animator.SetInteger("action", 1);

        item.EnableHolding();
        item.transform.localRotation = Quaternion.identity;
        item.transform.parent = skin.lHandTransform;
        item.transform.localPosition = Vector3.zero;

        m_heldItem = item;
    }

    public void Drop()
    {
        if(m_heldItem == null)
        {
            Debug.LogError("Trying to drop a null item!");
            return;
        }

        skin.lArmBone.m_Damping = defaultArmDamping;
        skin.lArmBone.m_Stiffness = defaultArmStiffness;
        skin.lArmBone.UpdateParameters();

        skin.rArmBone.m_Damping = defaultArmDamping;
        skin.rArmBone.m_Stiffness = defaultArmStiffness;
        skin.rArmBone.UpdateParameters();

        skin.animator.SetInteger("action", 0);

        m_heldItem.DisableHolding();
        m_heldItem.transform.parent = null;

        m_heldItem = null;
    }
}
