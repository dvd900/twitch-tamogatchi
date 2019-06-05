using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    GameObject character;
    EyeTrackBlink characterEyes;
    public GameObject spawnObject, head;
    public float dust_y, life;
    public Vector3 dust_size;
    bool dustOff = false;
    GameObject clone;
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        character = GameObject.FindWithTag("Character");
        characterEyes = character.GetComponent<EyeTrackBlink>();
    }

    // Update is called once per frame
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
