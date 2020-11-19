using System;
using System.Collections;
using System.Collections.Generic;
using AIActions;
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

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _fusePS;

    public Renderer bombRend;

    public SphereCollider smallRange;
    public SphereCollider bigRange;

    private Coroutine _explodeRoutine;
    private bool _didExplode;
    private bool _wasTriggered;
    private float _indicatorIntensity;
    private bool _isDefused;
    
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

    public void Douse()
    {
        Debug.Log("DOUSING!!!");
        StopCoroutine(_explodeRoutine);
        _animator.enabled = false;
        _fusePS.SetActive(false);
        //GameObject.Destroy(gameObject);
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

        _indicatorLight.enabled = false;

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

            IBombable bombable = hit.gameObject.GetComponent<IBombable>();

            Debug.Log("Is bombable: " + bombable + " obj: " + hit.gameObject);
            if(bombable != null)
            {

                bombable.Bomb(true, GetLaunchDirection(hit.transform.position, true));
            }
            else if (hit.gameObject.layer == VBLayerMask.ItemLayer)
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

            IBombable bombable = hit.gameObject.GetComponent<IBombable>();
            if (bombable != null)
            {
                bombable.Bomb(false, GetLaunchDirection(hit.transform.position, false));
            }
            else if (hit.gameObject.layer == VBLayerMask.ItemLayer)
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
        if(bomb != null && closeHit)
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
        var rigidbody = hitItem.GetComponentInParent<Rigidbody>();
        float force = (closeHit) ? _closeForce : _farForce;
        var launchVec = GetLaunchVector(hitItem.transform.position, force, closeHit);
        
        rigidbody.AddForce(launchVec);
        rigidbody.AddTorque(force * UnityEngine.Random.onUnitSphere);
    }

    private float GetLaunchAngle(bool closeHit)
    {
        return (closeHit) ? _closeAngle : 0;
    }

    private Vector3 GetLaunchDirection(Vector3 pos, bool closeHit)
    {
        Vector3 d = pos - transform.position;
        d.y = 0;
        d.Normalize();
        return Vector3.RotateTowards(d, Vector3.up, (Mathf.PI / 180) * GetLaunchAngle(closeHit), 0);
    }

    private Vector3 GetLaunchVector(Vector3 itemPos, float force, bool closeHit)
    {
        return force * GetLaunchDirection(itemPos, closeHit);
    }

    private Collider[] BombCast(SphereCollider collider)
    {
        return Physics.OverlapSphere(collider.transform.position,
            collider.radius * collider.transform.lossyScale.x, VBLayerMask.SweeTangoItemBombableMask);
    }
}
