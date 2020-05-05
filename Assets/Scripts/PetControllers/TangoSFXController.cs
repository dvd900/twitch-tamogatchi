using System;
using UnityEngine;

public class TangoSFXController : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _pickupClip;
    [SerializeField] private AudioClip _eatClip;
    [SerializeField] private AudioClip _hitOnHeadClip;
    [SerializeField] private AudioClip _lifeClip;

    private Skin _skin;

    private void Awake()
    {
        _skin = GetComponent<Skin>();    
    }

    public void PlayPickUp()
    {
        PlayClip(_pickupClip);
    }

    public void PlayEatClip()
    {
        PlayClip(_eatClip);
    }

    public void PlayLifeClip()
    {
        PlayClip(_lifeClip);
    }

    public void PlayHitOnHeadClip()
    {
        PlayClip(_hitOnHeadClip);
    }

    private void PlayClip(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
}
