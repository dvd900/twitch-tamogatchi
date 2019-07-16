using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public GameObject[] consumptionStates;

    GameObject character;
    EyeTrackBlink characterEyes;
    public GameObject spawnObject, head;
    public float dust_y, life;
    public Vector3 dust_size;
    bool dustOff = false;
    GameObject clone;
    ParticleSystem ps;

    private Collider m_collider;
    private Rigidbody m_rigidbody;

    private bool m_isHeld;
    private int m_consumptionIndex;

    void Start()
    {
        character = GameObject.FindWithTag("Character");
        characterEyes = character.GetComponent<EyeTrackBlink>();

        m_collider = GetComponent<Collider>();
        m_rigidbody = GetComponent<Rigidbody>();
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

    public void Eat() 
    {
        if(++m_consumptionIndex >= consumptionStates.Length) 
        {
            Destroy(gameObject);
            return;
        }

        consumptionStates[m_consumptionIndex - 1].SetActive(false);
        consumptionStates[m_consumptionIndex].SetActive(true);
    }

    public void EnableHolding() 
    {
        m_collider.enabled = false;
        m_rigidbody.isKinematic = true;

        m_isHeld = true;
    }

    public void DisableHolding() 
    {
        m_collider.enabled = true;
        m_rigidbody.isKinematic = false;

        m_isHeld = false;
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
            if(c.otherCollider.name == "Head")
            {
                iTween.PunchScale(c.otherCollider.gameObject, iTween.Hash("amount", new Vector3(0f, 0.2f, 0.2f), "time", 4.0f));
                characterEyes.EmoteDiscomfort(1f);
            }
 
        }

    }
}
