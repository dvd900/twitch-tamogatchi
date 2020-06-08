using System;
using UnityEngine;

public class AE_Controller : MonoBehaviour
{
    [SerializeField] private Skin _skin;
    [SerializeField] private ParticleSystem _sparkles;
    [SerializeField] private ParticleSystem _cheer;
    [SerializeField] private ParticleSystem _cheer2;
    [SerializeField] private ParticleSystem _eat;
    [SerializeField] private ParticleSystem _crumbs;
    [SerializeField] private Animator _crumbAnimator;
	public AudioSource aSource;
	public AudioClip aHello;

    private void Update()
    {
        //_crumbs.Emit(3);
    }

    public void AE_StartSparkles()
    {
        _sparkles.Play();
		aSource.PlayOneShot(aHello, 1f);
    }

    public void AE_StopSparkles()
    {
        _sparkles.Stop();
    }

    public void AE_Cheer()
    {
        _cheer.Emit(5);
        _cheer2.Emit(3);
    }

    public void AE_TakeBite()
    {
        _skin.itemController.DoBiteItem();
        _crumbs.Emit(3);
        _crumbAnimator.SetTrigger("crumbs");
        _skin.sfxController.PlayEatClip();
        //AnimeCrumbs.SetActive(true);
        //ac_crumbs.Play("animeCrumbs", 0, 0.23f);
    }

    public void AE_PutItemInHand()
    {
        Debug.Log("AE put item in hadn");
        _skin.itemController.DoPutItemInHand();
        _skin.sfxController.PlayPickUp();
    }

    public void AE_PickupDone()
    {
        Debug.Log("AE pickup done");
        _skin.itemController.DoPickupDone();
    }

    public void AE_StepSFX()
	{
		_skin.sfxController.PlayStepClip();
	}

}
