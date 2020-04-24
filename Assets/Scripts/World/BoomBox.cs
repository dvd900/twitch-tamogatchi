using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip[] _clips;

    public bool IsOn { get { return _source.isPlaying; } }

    private bool _justToggled;

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

        _justToggled = true;
        StartCoroutine(JustToggledTimer());
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
