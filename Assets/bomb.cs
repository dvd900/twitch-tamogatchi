using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public ParticleSystem explosion;
    public GameObject bomba;
    public AudioSource aSource;
    public AudioClip aClip;
    // Start is called before the first frame update
    void Start()
    {
        explosion.Stop(true);
        StartCoroutine(UnExplode(1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator UnExplode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        bomba.transform.localScale = new Vector3(0, 0, 0);
        explosion.Stop(true);
        bomba.SetActive(true);
        iTween.ScaleTo(bomba, new Vector3(1f, 1f, 1f), 1);
        StartCoroutine(WaitToExplode(3f));
    }
    private IEnumerator WaitToExplode(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        explosion.time = 0;
        explosion.Play();
        aSource.pitch = Random.Range(0.8f, 1.1f);
        aSource.PlayOneShot(aClip, 1f);
        bomba.SetActive(false);
        StartCoroutine(UnExplode(1f));
    }
}
