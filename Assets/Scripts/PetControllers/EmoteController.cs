using UnityEngine;
using System.Collections;

public class EmoteController : MonoBehaviour {
    
    public static void ClearEyeTriggers(Animator animator)
    {
        animator.ResetTrigger("ouchEyes");
        animator.ResetTrigger("bombedEyes");
    }

    public bool IsDoingEmote
    {
        get { return _isDoingEmote || _startingEmote; }
    }

    private Skin _skin;
    
    private float _mouthTimer;
    private bool _isDoingEmote;
    private bool _startingEmote;

    void Awake() {
        _skin = GetComponent<Skin>();
    }

    public void EmoteStart()
    {
        _startingEmote = false;
        _isDoingEmote = true;
    }

    public void EmoteEnd()
    {
        _startingEmote = false;
        _isDoingEmote = false;
    }

    public void DiscomfortEmote() {
        _startingEmote = true;
        _skin.sfxController.PlayHitOnHeadClip();
        _skin.animator.SetTrigger("ouchEyes");
        //iTween.PunchScale(_skin.rootBone.gameObject, iTween.Hash("amount",
        //    new Vector3(0f, 0.2f, 0.2f), "time", 1.0f));

        //_eyeTimer = _discomfortTime;

        Debug.Log("OW!!!");
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
        _startingEmote = true;
		_skin.animator.SetTrigger("bombed");
        _skin.animator.SetTrigger("bombedEyes");
    }

    public void DieEmote()
    {
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

    private void Update() {
    }


}
