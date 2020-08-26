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

    [SerializeField] private float _indicatorDuration;
    [SerializeField] private Light _indicatorLight;

    [SerializeField] private float _closeDamage;
    [SerializeField] private float _farDamage;

    [SerializeField] private float _closeForce;
    [SerializeField] private float _farForce;

    [Range(0, 90)]
    [SerializeField] private float _closeAngle;

    public SphereCollider smallRange;
    public SphereCollider bigRange;

    private Coroutine _explodeRoutine;
    private bool _didExplode;
    private bool _wasTriggered;
    private float _indicatorIntensity;
    public Renderer bombRend;
    
    void Start()
    {
        bombRend = gameObject.GetComponentInChildren<Renderer>();
        aSource.volume = 0.05f;
		aSource.PlayOneShot(fuse);
        _explodeRoutine = StartCoroutine(WaitAndExplode(10.0f));
        _indicatorIntensity = _indicatorLight.intensity;
        _indicatorLight.intensity = 0;
        _indicatorLight.enabled = false;
    }

    private IEnumerator IndicatorRoutine(float duration)
    {
        _indicatorLight.enabled = true;
        bool oneSecondLeft = false;
        float t = 0;
        while(t < duration)
        {
            _indicatorLight.intensity = Mathf.Lerp(0, _indicatorIntensity, t / duration);
            t += Time.deltaTime;
            if (t > duration - 0.1f && oneSecondLeft == false)
            {
                oneSecondLeft = true;
                LeanTween.scale(gameObject, 4.5f * Vector3.one, .1f).setEaseInSine();
                bombRend.material.SetColor("_EmissionColor", Color.white);
                bombRend.material.EnableKeyword("_EMISSION");
            }
            yield return null;
        }

        _indicatorLight.enabled = false;
    }

    private IEnumerator WaitAndExplode(float waitTime)
    {
        if(waitTime < _indicatorDuration)
        {
            yield return new WaitForSeconds(waitTime);
        }
        else
        {
            yield return new WaitForSeconds(waitTime - _indicatorDuration);
            yield return IndicatorRoutine(_indicatorDuration);
        }

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
                BombItem(hit.gameObject, true);
            }
            else if(!hitSweetClose)
            {
                hitSweetClose = true;
                var skin = hit.GetComponentInParent<Skin>();
                skin.actionController.DoAction(new IdleAction(skin, 3.0f, false));
                skin.movementController.StopWalking();
                skin.movementController.FaceCamera();
                skin.emoteController.Bombed();
                skin.statsController.AddHealth(-_closeDamage);
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
                BombItem(hit.gameObject, false);
            }
            else if(!hitSweetFar)
            {
                hitSweetFar = true;
                var skin = hit.GetComponentInParent<Skin>();
                skin.emoteController.DiscomfortEmote();
                skin.statsController.AddHealth(-_farDamage);
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

        _didExplode = true;

        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
        Destroy(explosion.gameObject);
    }

    private void BombItem(GameObject hitItem, bool closeHit)
    {
        var bomb = hitItem.GetComponent<Bomb>();
        if(bomb != null)
        {
            if(!bomb._didExplode)
            {
                LaunchItem(hitItem, closeHit);
                if(!bomb._wasTriggered)
                {
                    bomb.StopCoroutine(bomb._explodeRoutine);
                    float waitTime = (closeHit) ? .2f : .4f;
                    bomb._explodeRoutine = bomb.StartCoroutine(bomb.WaitAndExplode(waitTime));
                    bomb._wasTriggered = true;
                }
            }
        }
        else
        {
            LaunchItem(hitItem, closeHit);
        }
    }

    private void LaunchItem(GameObject hitItem, bool closeHit)
    {
        var item = hitItem.GetComponentInParent<Item>();
        float angle = (closeHit) ? _closeAngle : 0;
        var launchVec = GetLaunchVector(item.transform.position, _closeForce, angle);
        item.Launch(launchVec);
    }

    private Vector3 GetLaunchVector(Vector3 itemPos, float force, float angle)
    {
        Vector3 d = itemPos - transform.position;
        d.y = 0;
        d.Normalize();
        return force * Vector3.RotateTowards(d, Vector3.up, (Mathf.PI / 180) * angle, 0);
    }

    private Collider[] BombCast(SphereCollider collider)
    {
        return Physics.OverlapSphere(collider.transform.position,
            collider.radius * collider.transform.lossyScale.x, VBLayerMask.SweeTangoAndItemLayerMask);
    }
}
