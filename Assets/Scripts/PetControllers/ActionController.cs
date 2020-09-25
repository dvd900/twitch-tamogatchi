using UnityEngine;
using System.Collections;
using System;

public class ActionController : MonoBehaviour
{
    public AIAction CurrentAction { get { return _currentAction; } }
    private AIAction _currentAction;

    public AIAction LastAction { get { return _lastAction; } }
    private AIAction _lastAction;

    public bool IsDying { get { return _currentAction is DeathAction; } }

    public void DoAction(AIAction action) {
        if(IsDying && !(action is DeathAction))
        {
            return;
        }

        if(_currentAction != null) {
            _currentAction.Interrupt();
        }

        _lastAction = _currentAction;

        _currentAction = action;
        _currentAction.StartAction();

        Debug.Log("Doing action: " + action);
    }

    private void Update() {
        if(_currentAction != null) {
            _currentAction.UpdateAction();

            if(_currentAction.IsFinished()) {
                _lastAction = _currentAction;
                _currentAction = null;
            }
        }
    }
}
