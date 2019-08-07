﻿using UnityEngine;
using System.Collections;

public class EmoteController : MonoBehaviour {

    public Material baseRMat, baseLMat, ouchRMat, ouchLMat;

    [SerializeField] private float _discomfortTime;
    [SerializeField] private float _chewTime;

    private Skin _skin;

    private float _eyeTimer;
    private float _mouthTimer;

    void Start() {
        _skin = GetComponent<Skin>();
    }

    public void DiscomfortEmote() {
        iTween.PunchScale(_skin.renderer.gameObject, iTween.Hash("amount",
            new Vector3(0f, 0.2f, 0.2f), "time", _discomfortTime));

        _skin.faceController.DoDiscomfortEyes();
        _eyeTimer = _discomfortTime;

        Debug.Log("OW!!!");
    }

    public void ChewEmote() {
        _skin.faceController.DoClosedEyes();
        _skin.faceController.DoChewMouth();
        _eyeTimer = _chewTime;
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
            if(_mouthTimer < 0) {
                _skin.faceController.DoNormalMouth();
            }
        }
    }


}
