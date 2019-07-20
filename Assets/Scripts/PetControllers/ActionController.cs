using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

    public AIAction currentAction { get { return _currentAction; } }
    private AIAction _currentAction;

    public void DoAction(AIAction action) {
        if(_currentAction != null) {
            _currentAction.Interrupt();
        }

        _currentAction = action;
        _currentAction.StartAction();

        Debug.Log("Doing action: " + action);
    }

    private void Update() {

        if(_currentAction != null) {
            _currentAction.UpdateAction();

            if(_currentAction.IsFinished()) {
                _currentAction = null;
            }
        }
    }
}
