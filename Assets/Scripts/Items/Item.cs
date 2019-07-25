using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject[] consumptionStates;

    public float value;

    public GameObject spawnObject;
    public float dust_y, life;
    public Vector3 dust_size;
    bool dustOff = false;
    GameObject clone;
    ParticleSystem ps;

    private Collider _collider;
    private Rigidbody _rigidbody;

    private int _biteInd;

    public bool isHeld { get { return _holder != null; } }

    private Skin _holder;

    void Start()
    {
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
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

    public void DoEat() 
    {
        Debug.Log("DOEAT");
        if(gameObject == null) {
            Debug.LogError("GO null");
            Debug.LogError("current action: " + _holder.actionController.currentAction);
        }

        if (++_biteInd >= consumptionStates.Length) 
        {
            Destroy(gameObject);
            return;
        }

        consumptionStates[_biteInd - 1].SetActive(false);
        consumptionStates[_biteInd].SetActive(true);
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
                clone = Instantiate(spawnObject, new Vector3(transform.position.x, transform.position.y + dust_y, transform.position.z), Quaternion.Euler(new Vector3(90, 0, 0)), null) as GameObject;
                clone.transform.localScale = dust_size;
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
