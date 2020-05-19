using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject bomba;
    public AudioSource aSource;
    public AudioClip aClip;
    public Renderer fuseRend;

    public float closeDamage;
    public float farDamage;

    public SphereCollider smallRange;
    public SphereCollider bigRange;
    
    void Start()
    {
        StartCoroutine(WaitAndExplode(10.0f));
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        explosion.gameObject.SetActive(true);
        explosion.time = 0;
        explosion.Play();
        aSource.pitch = Random.Range(0.8f, 1.1f);
        aSource.PlayOneShot(aClip, 1f);
        bomba.SetActive(false);
        
        var hits = BombCast(smallRange);
        if (hits.Length > 0)
        {
            var skin = hits[0].GetComponentInParent<Skin>();
            skin.emoteController.Bombed();
            skin.statsController.AddHealth(-closeDamage);
        }
        else
        {
            hits = BombCast(bigRange);
            if (hits.Length > 0)
            {
                var skin = hits[0].GetComponentInParent<Skin>();
                skin.emoteController.DiscomfortEmote();
                skin.statsController.AddHealth(-farDamage);
            }
        }

        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }

    private Collider[] BombCast(SphereCollider collider)
    {
        return Physics.OverlapSphere(collider.transform.position,
            collider.radius * collider.transform.lossyScale.x, VBLayerMask.SweeTangoLayerMask);
    }
}
