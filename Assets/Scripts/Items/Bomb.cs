using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject bomba;
    public AudioSource aSource;
    public AudioClip boomCloseHit, boomFar, fuse;
    public Renderer fuseRend;

    public float closeDamage;
    public float farDamage;

    public SphereCollider smallRange;
    public SphereCollider bigRange;
    
    void Start()
    {
		aSource.volume = 0.05f;
		aSource.PlayOneShot(fuse);
        StartCoroutine(WaitAndExplode(10.0f));
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        explosion.transform.SetParent(null);
        explosion.transform.rotation = Quaternion.identity;
        explosion.gameObject.SetActive(true);
        explosion.time = 0;
        explosion.Play();
		aSource.Stop();
		aSource.pitch = UnityEngine.Random.Range(0.8f, 1.1f);
		bomba.SetActive(false);

		var closeHits = BombCast(smallRange);
        bool hitSweetClose = false;
        foreach(var hit in closeHits)
        {
            if (hit.gameObject == gameObject)
            {
                continue;
            }

            if (hit.gameObject.layer == VBLayerMask.ItemLayer)
            {

            }
            else
            {
                hitSweetClose = true;
                var skin = hit.GetComponentInParent<Skin>();
                skin.actionController.DoAction(new IdleAction(skin, 3.0f, false));
                skin.movementController.StopWalking();
                skin.movementController.FaceCamera();
                skin.emoteController.Bombed();
                skin.statsController.AddHealth(-closeDamage);
            }
        }

        var farHits = BombCast(bigRange);
        bool hitSweetFar = false;
        foreach(var hit in farHits)
        {
            if(hit.gameObject == gameObject || Array.IndexOf(closeHits, hit) != -1)
            {
                continue;
            }

            if (hit.gameObject.layer == VBLayerMask.ItemLayer)
            {

            }
            else
            {
                hitSweetFar = true;
                var skin = hit.GetComponentInParent<Skin>();
                skin.emoteController.DiscomfortEmote();
                skin.statsController.AddHealth(-farDamage);
            }
        }

        if(hitSweetClose)
        {
            aSource.volume = 0.75f;
            aSource.PlayOneShot(boomCloseHit, 0.75f);
        }
        else if(hitSweetFar)
        {
            aSource.volume = 1f;
            aSource.PlayOneShot(boomFar, 1f);
        }
        else
        {
            aSource.volume = 0.7f;
            aSource.PlayOneShot(boomFar, 0.7f);
        }

        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
        Destroy(explosion.gameObject);
    }

    private Collider[] BombCast(SphereCollider collider)
    {
        return Physics.OverlapSphere(collider.transform.position,
            collider.radius * collider.transform.lossyScale.x, VBLayerMask.SweeTangoAndItemLayerMask);
    }
}
