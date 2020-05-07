using UnityEngine;
using System.Collections;

public class EmoteController : MonoBehaviour {
    
    public bool IsDoingEmote
    {
        get { return _isDoingEmote; }
    }

    private Skin _skin;
    
    private float _mouthTimer;
    private bool _isDoingEmote;

    void Awake() {
        _skin = GetComponent<Skin>();
    }

    public void EmoteDone()
    {
        _isDoingEmote = false;
    }

    public void DiscomfortEmote() {
        _skin.sfxController.PlayHitOnHeadClip();
        //iTween.PunchScale(_skin.renderer.gameObject, iTween.Hash("amount",
        //    new Vector3(0f, 0.2f, 0.2f), "time", _discomfortTime));
        
        //_eyeTimer = _discomfortTime;

        Debug.Log("OW!!!");
    }

    public void Cheer() {
        _isDoingEmote = true;
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("cheer");
    }

    public void SpawnCheer()
    {
        _isDoingEmote = true;
        _skin.animator.SetTrigger("spawncheer");
    }

    public void Wave()
    {
        _isDoingEmote = true;
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("wave");
    }

    public void Dance()
    {
        _isDoingEmote = true;
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("dance");
    }

    public void ChewEmote()
    {
        _isDoingEmote = true;
        _skin.animator.SetTrigger("chew");
    }

    private void Update() {
    }


}
