using UnityEngine;
using System.Collections;

public class EmoteController : MonoBehaviour {

    public Material baseRMat, baseLMat, ouchRMat, ouchLMat;

    [SerializeField] private float _discomfortTime;
    [SerializeField] private float _chewTime;
    [SerializeField] private float _cheerTime;
    [SerializeField] private float _chewDelay;

    private Skin _skin;

    private float _eyeTimer;
    private float _mouthTimer;

    void Awake() {
        _skin = GetComponent<Skin>();
    }

    public void DiscomfortEmote() {
        iTween.PunchScale(_skin.renderer.gameObject, iTween.Hash("amount",
            new Vector3(0f, 0.2f, 0.2f), "time", _discomfortTime));

        _skin.faceController.DoDiscomfortEyes();
        _eyeTimer = _discomfortTime;

        Debug.Log("OW!!!");
    }

    public void Cheer() {
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("cheer");

        _skin.faceController.DoHappyClosedEyes();
        _eyeTimer = _cheerTime;
    }

    public void Wave()
    {
        _skin.movementController.StopWalking();
        _skin.movementController.FaceCamera();
        _skin.animator.SetTrigger("wave");
    }

    public void ChewEmote()
    {
        _skin.animator.SetTrigger("chew");
        //_skin.faceController.DoClosedEyes();
        //_eyeTimer = _chewTime;
        _mouthTimer = _chewTime;
    }

    private void Update() {
        if(_eyeTimer > 0) {
            _eyeTimer -= Time.deltaTime;
            if(_eyeTimer < 0) {
                _skin.faceController.DoNormalEyes();
            }
        }

        if(_mouthTimer > 0) {
            _mouthTimer -= Time.deltaTime;
            // slight delay before chew starts
            if (_mouthTimer < _chewTime - _chewDelay && !_skin.faceController.IsChewing) {
                _skin.faceController.DoChewMouth();
            }

            if (_mouthTimer < 0) {
                _skin.faceController.DoNormalMouth();
            }
        }
    }


}
