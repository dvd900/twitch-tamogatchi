using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float value { get { return _profile._value; } }
    
    /// <summary>
    /// Should it drop in when spawned?
    /// </summary>
    [SerializeField] private bool _dropsIn = true;

    protected ItemProfile _profile;

    private float life;
    bool dustOff = false;
    GameObject clone;
    ParticleSystem ps;

    protected Rigidbody _rigidbody;
    protected Collider _collider;

    public bool isHeld { get { return _holder != null; } }
    public bool dropsIn { get { return _dropsIn; } }

    protected virtual bool IsPickupabble { get { return false; } }

    protected Skin _holder;

    protected virtual void OnPickup() { }
    protected virtual void OnDrop() { }

    private void Awake() {
        var profileContainer = GetComponent<ItemContainer>();
        ItemBase item = profileContainer.item;
        Debug.Log("Spawned item: " + item.itemName);
        _profile = (ItemProfile)ItemSystemUtility.GetItemCopy(item.itemID, item.itemType);
        _collider = GetComponentInChildren<Collider>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        AIWorldData.Instance.AllItems.Add(this);
    }

    private void OnDestroy()
    {
        AIWorldData.Instance.AllItems.Remove(this);
    }

    void Update()
    {
        if (dustOff == true)
        {
             life -= Time.deltaTime;
            if (life <= 0.0f)
            {
                Destroy(clone.gameObject);
            }
        }
    }

    public bool CanBePickedUp()
    {
        return IsPickupabble && !isHeld && _rigidbody.velocity.magnitude < 3;
    }

    public void EnableHolding(Skin holder) 
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;

        _holder = holder;

        OnPickup();
    }

    public void DisableHolding() 
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;

        _holder = null;

        OnDrop();
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == VBLayerMask.GroundTag)
        {
            if (dustOff == false)
            {
                dustOff = true;
                clone = ItemSpawner.Instance.MakeDust();
                
                clone.transform.position = new Vector3(transform.position.x, transform.position.y-2, transform.position.z);
                ps = clone.GetComponent<ParticleSystem>();
                life = ps.startLifetime/3; //divide by simulation speed
            }
           //ps.Play(); 
        }
        foreach (ContactPoint c in collision.contacts)
        {
            if(c.otherCollider.tag == VBLayerMask.SweeTangoTag)
            {
                EmoteController emote = c.otherCollider.gameObject.GetComponentInParent<EmoteController>();

                emote.DiscomfortEmote();
            }
 
        }

    }
}
