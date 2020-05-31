using UnityEngine;
using System.Collections;
using GameData;
using System;

public class SpeechController : MonoBehaviour {

    [SerializeField] private DialogueData _dialogueData;

    private Skin _skin;

    private void Start() {
        _skin = GetComponent<Skin>();
        MessengerServer.singleton.SetHandler(NetMsgInds.SpeechMessage, OnSpeechMessage);
    }

    private void OnSpeechMessage(string msg) {
        //SpeechMessage speechMessage = (SpeechMessage)msg;
        //Speak(speechMessage.msg);
    }

    public void Speak(string text) {
        Debug.Log("sweetango: " + text);
    }

    public void SayRandomDialogue() {
        int ind = UnityEngine.Random.Range(0, _dialogueData.IdleDialogues.Length);
        Speak(_dialogueData.IdleDialogues[ind]);
    }
}
