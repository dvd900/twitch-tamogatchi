using UnityEngine;
using System.Collections;
using System;

public class ActionController : MonoBehaviour {

    public float avgIdleTime { get { return _avgIdleTime; } }
    [SerializeField] private float _avgIdleTime;

    public float actionRandomness { get { return _actionRandomness; } }
    [SerializeField] private float _actionRandomness;

    public AIAction currentAction { get { return _currentAction; } }
    private AIAction _currentAction;

    private Skin _skin;

    private void Start() {
        _skin = GetComponent<Skin>();

        MessengerServer.singleton.SetHandler(NetMsgInds.IdleMessage, OnIdleMsg);
    }


    public void DoAction(AIAction action) {
        if(_currentAction != null) {
            _currentAction.Interrupt();
            _currentAction.FinishAction();
        }

        _currentAction = action;
        _currentAction.StartAction();

        Debug.Log("Doing action: " + action);
    }

    private void Update() {
        if(_currentAction != null) {
            _currentAction.UpdateAction();

            if(_currentAction.IsFinished()) {
                _currentAction.FinishAction();
                _currentAction = null;
            }
        }
    }

    private void OnIdleMsg(NetMsg msg) {
        DoAction(new IdleAction(_skin));
    }
}
