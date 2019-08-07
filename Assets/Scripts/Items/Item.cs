using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float value { get { return _value; } }
    [SerializeField] private float _value;

    private float life;
    bool dustOff = false;
    GameObject clone;
    ParticleSystem ps;

    protected Collider _collider;
    protected Rigidbody _rigidbody;

    public bool isHeld { get { return _holder != null; } }

    protected Skin _holder;

    void Start()
    {
        _collider = GetComponentInChildren<Collider>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        if(dustOff == true)
        {
             life -= Time.deltaTime;
            if (life <= 0.0f)
            {
                Destroy(clone.gameObject);
            }
        }
    }

    public void EnableHolding(Skin holder) 
    {
        _collider.enabled = false;
        _rigidbody.isKinematic = true;

        _holder = holder;
    }

    public void DisableHolding() 
    {
        _collider.enabled = true;
        _rigidbody.isKinematic = false;

        _holder = null;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            if (dustOff == false)
            {
                dustOff = true;
                clone = ItemSpawner.singleton.MakeDust();
                ps = clone.GetComponent<ParticleSystem>();
                life = ps.startLifetime/3; //divide by simulation speed
            }
           //ps.Play(); 
        }
        foreach (ContactPoint c in collision.contacts)
        {
            if(c.otherCollider.tag == "Head")
            {
                EmoteController emote = c.otherCollider.gameObject.GetComponentInParent<EmoteController>();

                emote.DiscomfortEmote();
            }
 
        }

    }
}
