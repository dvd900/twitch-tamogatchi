using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ActionController : MonoBehaviour
{
    public AIAction CurrentAction { get { return _currentAction; } }
    private AIAction _currentAction;

    public AIAction LastAction { get { return _lastAction; } }
    private AIAction _lastAction;

    public bool IsDying { get { return _currentAction is DeathAction; } }

    private LinkedList<AIAction> _actionList;

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

    public void DoActionSequence(params AIAction[] actionSequence)
    {
        _actionList = new LinkedList<AIAction>(actionSequence);
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
