using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;

    public bool IsOn { get { return _source.isPlaying; } }

    private void Toggle()
    {
        if(IsOn)
        {
            _source.Stop();
        }
        else
        {
            int index = Random.Range(0, _clips.Length);
            _source.PlayOneShot(_clips[index]);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == VBLayerMask.ItemTag)
        {
            Toggle();
        }
    }
}
