using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using AIActions;

public class ActionController : MonoBehaviour
{
    public AIAction CurrentAction { get { return _currentAction; } }
    private AIAction _currentAction;

    public AIAction LastAction { get { return _lastAction; } }
    private AIAction _lastAction;

    public bool IsDying { get { return _currentAction is DeathAction; } }

    [SerializeField] private bool _logActions;

    private Queue<AIAction> _actionQueue;

    public void DoAction(AIAction action) {
        if(IsDying && !(action is DeathAction))
        {
            return;
        }

        if(_currentAction != null) {
            _currentAction.Interrupt();
        }

        _actionQueue = null;

        StartNewAction(action);
    }

    public void DoActionSequence(params AIAction[] actionSequence)
    {
        _actionQueue = new Queue<AIAction>(actionSequence);

        StartNewAction(_actionQueue.Dequeue());

        if(_actionQueue.Count == 0)
        {
            _actionQueue = null;
        }
    }

    private void StartNewAction(AIAction action)
    {
        _lastAction = _currentAction;
        _currentAction = action;
        _currentAction.StartAction();

        if(_logActions)
        {
            Debug.Log(gameObject + " doing action: " + action);
        }
    }

    private void Update() {
        if(_currentAction != null) {
            _currentAction.UpdateAction();

            if(_currentAction.IsFinished()) {
                _lastAction = _currentAction;

                if(_actionQueue != null)
                {
                    StartNewAction(_actionQueue.Dequeue());

                    if(_actionQueue.Count == 0)
                    {
                        _actionQueue = null;
                    }
                }
                else
                {
                    _currentAction = null;
                }
            }
        }
    }
}
