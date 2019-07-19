using UnityEngine;
using System.Collections;

public class ActionController : MonoBehaviour {

    private AIAction currentAction;

    public void DoAction(AIAction action) {
        if(currentAction != null) {
            currentAction.Interrupt();
        }

        currentAction = action;
    }

    private void Update() {

        if(currentAction != null) {
            currentAction.UpdateAction();

            if(currentAction.IsFinished()) {
                currentAction = null;
            }
        }
    }
}
