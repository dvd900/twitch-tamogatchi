using System;
using UnityEngine;

public class AE_Controller : MonoBehaviour
{
    [SerializeField] private Skin _skin;
    [SerializeField] private ParticleSystem _sparkles;
    [SerializeField] private ParticleSystem _cheer;
    [SerializeField] private ParticleSystem _cheer2;
    [SerializeField] private ParticleSystem _eat;

    public void AE_StartSparkles()
    {
        _sparkles.Play();
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
    }

    public void AE_PutItemInHand()
    {
        Debug.Log("AE put item in hadn");
        _skin.itemController.DoPutItemInHand();
    }

    public void AE_PickupDone()
    {
        Debug.Log("AE pickup done");
        _skin.itemController.DoPickupDone();
    }

}
