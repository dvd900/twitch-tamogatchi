using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private bool _loop;

    public bool IsOn { get { return _source.isPlaying; } }

    private bool _justToggled;
    private int _index;
    private bool _waiting;

    private void Start()
    {
        Toggle();
    }

    private void Toggle()
    {
        if(IsOn)
        {
            _source.Stop();
        }
        else
        {
            _index = Random.Range(0, _clips.Length);
            _source.PlayOneShot(_clips[_index]);
        }

        _justToggled = true;
        StartCoroutine(JustToggledTimer());
    }

    private void Update()
    {
        if(!_source.isPlaying && _loop && !_waiting)
        {
            StartCoroutine(WaitAndPlayNextTrack());
        }
    }

    private IEnumerator WaitAndPlayNextTrack()
    {
        _waiting = true;
        yield return new WaitForSeconds(5.0f);
        _index = (_index + 1) % _clips.Length;
        _source.PlayOneShot(_clips[_index]);
        _waiting = false;
    }

    private IEnumerator JustToggledTimer()
    {
        yield return new WaitForSeconds(1.0f);
        _justToggled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == VBLayerMask.ItemTag && !_justToggled)
        {
            Toggle();
        }
    }
}
