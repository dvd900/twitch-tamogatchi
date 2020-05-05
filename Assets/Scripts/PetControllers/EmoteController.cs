using UnityEngine;
using System.Collections;

public enum EmoteType
{
    None,
    Wave,
    Cheer,
    Discomfort
}

public class EmoteController : MonoBehaviour {
    
    public bool IsDoingEmote
    {
        get { return _currentEmote != EmoteType.None; }
    }

    private Skin _skin;
    
    private float _mouthTimer;
    private EmoteType _currentEmote;

    void Awake() {
        _skin = GetComponent<Skin>();
    }

    public void EmoteDone()
    {
        _currentEmote = EmoteType.None;
    }

    public void DiscomfortEmote() {
        _skin.sfxController.PlayHitOnHeadClip();
        //iTween.PunchScale(_skin.renderer.gameObject, iTween.Hash("amount",
        //    new Vector3(0f, 0.2f, 0.2f), "time", _discomfortTime));
        
        //_eyeTimer = _discomfortTime;

        Debug.Log("OW!!!");
    }

    public void Cheer() {
        _currentEmote = EmoteType.Cheer;
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("cheer");
    }

    public void SpawnCheer()
    {
        _currentEmote = EmoteType.Cheer;
        _skin.animator.SetTrigger("spawncheer");
    }

    public void Wave()
    {
        _currentEmote = EmoteType.Wave;
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("wave");
    }

    public void ChewEmote()
    {
        _skin.animator.SetTrigger("chew");
    }

    private void Update() {
    }


}
