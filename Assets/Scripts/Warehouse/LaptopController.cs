﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LaptopController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private TextMeshPro _screenText;

    [SerializeField] private AudioSource _aSource;
    [SerializeField] private AudioClip _beep;

    [TextArea]
    [SerializeField] private string _bootText;

    private bool _isOn;

    public void TurnOn()
    {
        _screenText.text = "";
        StartCoroutine(TurnOnRoutine());
    }

    private IEnumerator TurnOnRoutine()
    {
        yield return new WaitForSeconds(3.75f);

        //_animator.SetTrigger("turnOn");
        //_isOn = true;

		yield return new WaitForSeconds(0.5f);
        //_aSource.PlayOneShot(_beep,1f);
        
        yield return new WaitForSeconds(3.0f);
        //TextAnimation.PrintText(_bootText, _screenText, .02f);
    }
}
