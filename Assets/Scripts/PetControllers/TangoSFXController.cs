using System;
using UnityEngine;

public class TangoSFXController : MonoBehaviour
{
    [SerializeField] private AudioSource _source;

    [SerializeField] private AudioClip _pickupClip;
    [SerializeField] private AudioClip _eatClip;
    [SerializeField] private AudioClip _hitOnHeadClip;
    [SerializeField] private AudioClip _lifeClip;
	[SerializeField] private AudioClip _stepClip;

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
    public void PlayStepClip()
	{
		PlayClip(_stepClip, new Vector2(0.15f,0.25f), new Vector2(0.85f,1.2f));
	}

    private void PlayClip(AudioClip clip)
    {
        _source.PlayOneShot(clip);
    }
    private void PlayClip(AudioClip clip, float volume)
	{
		_source.PlayOneShot(clip, volume);
	}
	private void PlayClip(AudioClip clip, Vector2 volumeRange, Vector2 pitchRange)
	{
		_source.pitch = UnityEngine.Random.Range(pitchRange.x, pitchRange.y);
		_source.PlayOneShot(clip, UnityEngine.Random.Range(volumeRange.x, volumeRange.y));
	}

}
