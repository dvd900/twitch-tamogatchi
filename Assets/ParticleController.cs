using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    public Animator ac_crumbs;
    public ParticleSystem Crumbs, psSurprise, psStars, psSparkles;
    public GameObject AnimeCrumbs;
    void Start()
    {
        psSparkles.Stop();
        Crumbs.GetComponent<ParticleSystem>().emissionRate = 0;
        AnimeCrumbs.SetActive(false);
    }

    void Update()
    {
    }
    public void Crunch()
    {
        Crumbs.Emit(3);
        AnimeCrumbs.SetActive(true);
        ac_crumbs.Play("animeCrumbs", 0, 0.23f);

    }
    public void Surprise()
    {
        psSurprise.Emit(5);
        psStars.Emit(3);
    }
    public void Sparkles()
    {
        psSparkles.Play();
    }
    public void SparklesStop()
    {
        psSparkles.Stop();
    }
    public void EndEat()
    {
        AnimeCrumbs.SetActive(false);
    }
}
