﻿using UnityEngine;
using System.Collections;

public class EmoteController : MonoBehaviour
{
    
    public static void ClearEyeTriggers(Animator animator)
    {
        animator.ResetTrigger("ouchEyes");
        animator.ResetTrigger("bombedEyes");
    }

    public bool IsDoingEmote
    {
        get { return _isDoingEmote || _startingEmote; }
    }


    [SerializeField] private GameObject _snot;

    private Skin _skin;
    
    private float _mouthTimer;
    private bool _isDoingEmote;
    private bool _startingEmote;

    void Awake() {
        _skin = GetComponent<Skin>();
    }

    public void OnEmoteStart()
    {
        _startingEmote = false;
        _isDoingEmote = true;
    }

    public void OnEmoteEnd()
    {
        _startingEmote = false;
        _isDoingEmote = false;
    }

    public void DiscomfortEmote() {
        _startingEmote = true;
        if (_skin.speechController.IsSpeaking)
        {
            _skin.speechController.StopSpeaking();
        }
        _skin.sfxController.PlayHitOnHeadClip();
        _skin.animator.SetTrigger("ouchEyes");

        iTween.PunchScale(_skin.rootBone.gameObject, iTween.Hash("amount",
            new Vector3(0f, 0.2f, 0.2f), "time", 1.0f));

        //_eyeTimer = _discomfortTime;
    }

    public void Cheer()
    {
        _startingEmote = true;
        _skin.animator.SetTrigger("cheer");
        _skin.animator.SetTrigger("cheerEyes");
    }

    public void SpawnCheer()
    {
        _startingEmote = true;
        _skin.animator.SetTrigger("spawncheer");
        _skin.animator.SetTrigger("cheerEyes");
    }

    public void Wave()
    {
        _startingEmote = true;
        _skin.animator.SetTrigger("wave");
    }

    public void Dance()
    {
        _startingEmote = true;
        _skin.animator.SetTrigger("dance");
        _skin.animator.SetTrigger("danceEyes");
    }

    public void Bombed()
    {
        //if (_skin.speechController.IsSpeaking)
        //{
        //    _skin.speechController.StopSpeaking();
        //}
        _startingEmote = true;
		_skin.animator.SetTrigger("bombed");
        _skin.animator.SetTrigger("bombedEyes");
    }

    public void StartSleep()
    {
        _startingEmote = true;
        _skin.animator.SetTrigger("startSleep");
        _skin.animator.SetBool("isSleeping", true);
    
        //LeanTween.delayedCall(gameObject,4.2f,()=>{
        //    _snot.SetActive(true);
        //    LeanTween.scale(_snot, new Vector3(0.24f, 0.24f, 0.24f), 3f).setEase(LeanTweenType.easeOutBack);
        //});

    }

    public void EnableSleepSnot()
    {
        _snot.SetActive(true);
        LeanTween.scale(_snot, new Vector3(0.24f, 0.24f, 0.24f), 3f).setEase(LeanTweenType.easeOutBack);
    }

    public void StopSleep(bool interrupted)
    {
        Debug.Log("Stopping sleep");
        if (interrupted)
        {
            _skin.animator.SetTrigger("animInterrupted");
        }
        else
        {
            _skin.animator.SetTrigger("stopSleep");
        }
        _skin.animator.SetBool("isSleeping", false);
        _snot.SetActive(false);
    }

    public void DieEmote()
    {
        if (_skin.speechController.IsSpeaking)
        {
            _skin.speechController.StopSpeaking();
        }
        _startingEmote = true;
        _skin.animator.SetTrigger("death");
        _skin.animator.SetTrigger("deathEyes");
    }

    public void ChewEmote()
    {
        _skin.animator.SetTrigger("chew");
        _skin.animator.SetTrigger("chewMouth");
    }

    public void StartSpeakEmote()
    {
        _skin.animator.SetBool("speakMouth", true);
    }

    public void StopSpeakEmote()
    {
        _skin.animator.SetBool("speakMouth", false);
    }
    public void StartPukeEmote()
    {
        _startingEmote = true;
        _skin.animator.SetTrigger("puke");
        _skin.animator.SetTrigger("pukeEyes");
        _skin.animator.SetTrigger("pukeMouth");
    }

    private void Update() {
    }


}
