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
		aSource.pitch = Random.Range(0.8f, 1.1f);
		bomba.SetActive(false);

		var hits = BombCast(smallRange);
        if (hits.Length > 0)
        {
            var skin = hits[0].GetComponentInParent<Skin>();
            skin.actionController.DoAction(new IdleAction(skin, 3.0f, false));
            skin.emoteController.Bombed();
            skin.statsController.AddHealth(-closeDamage);
			aSource.volume = 0.75f;
			aSource.PlayOneShot(boomCloseHit, 0.75f);
		}
        else
        {
            hits = BombCast(bigRange);
            if (hits.Length > 0)
            {
                var skin = hits[0].GetComponentInParent<Skin>();
                skin.emoteController.DiscomfortEmote();
                skin.statsController.AddHealth(-farDamage);
				aSource.volume = 1f;
				aSource.PlayOneShot(boomFar,1f);
			}
			else
			{
				aSource.volume = 0.7f;
				aSource.PlayOneShot(boomFar,0.7f);
			}
        }

        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
        Destroy(explosion.gameObject);
    }

    private Collider[] BombCast(SphereCollider collider)
    {
        return Physics.OverlapSphere(collider.transform.position,
            collider.radius * collider.transform.lossyScale.x, VBLayerMask.SweeTangoLayerMask);
    }
}
