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
    // Start is called before the first frame update
    void Start()
    {
        //explosion.Stop(true);
        StartCoroutine(WaitToExplode(10.0f));
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
        float time = waitTime;
        float endOffsetY = 1.0f;
        float startOffsetY = fuseRend.material.GetTextureOffset("_MainTex").y;
        while(time > 0)
        {
            time -= Time.deltaTime;
            //float t = 1 - time / waitTime;
            //float newOffset = endOffsetY * t + (1 - t) * (startOffsetY);
            //fuseRend.material.SetTextureOffset("_MainTex", new Vector2(0, newOffset));
            yield return null;
        }
        explosion.gameObject.SetActive(true);
        explosion.time = 0;
        explosion.Play();
        aSource.pitch = Random.Range(0.8f, 1.1f);
        aSource.PlayOneShot(aClip, 1f);
        bomba.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
        //StartCoroutine(UnExplode(1f));
    }
}
