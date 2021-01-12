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
    [SerializeField] private ParticleSystem _dizzy;
    [SerializeField] private Animator _crumbAnimator;
    [SerializeField] private GameObject _ghostPrefab;
	public AudioSource aSource;

    private void Update()
    {
        //_crumbs.Emit(3);
    }

    public void AE_StartSparkles()
    {
        _sparkles.Play();
      //  if(!_skin.speechController.IsSpeaking)
      //  {
		    //aSource.PlayOneShot(aHello, 1f);
      //  }
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

    public void AE_EatDone()
    {
        _skin.itemController.DoEatDone();
    }

    public void AE_PutItemInHand()
    {
        _skin.itemController.DoPutItemInHand();
        _skin.sfxController.PlayPickUp();
    }

    public void AE_PickupDone()
    {
        _skin.itemController.DoPickupDone();
    }

    public void AE_SpawnGhost()
    {
        GameObject.Instantiate(_ghostPrefab, transform.position, transform.rotation);
    }
    public void AE_Dizzy()
    {
        _dizzy.Emit(1);
    }

    public void AE_StepSFX()
	{
		_skin.sfxController.PlayStepClip();
	}
	public void AE_FallSFX()
	{
		_skin.sfxController.PlayFallClip();
	}
	public void AE_HurtSFX()
	{
		_skin.sfxController.PlayHurtClip();
	}
	public void AE_SquishSFX()
	{
		_skin.sfxController.PlayPickUp();
	}
	public void AE_JumpSFX()
	{
		_skin.sfxController.PlayJumpClip();
	}
    public void AE_DeathSFX()
	{
		_skin.sfxController.PlayDeathClip();
	}

}
