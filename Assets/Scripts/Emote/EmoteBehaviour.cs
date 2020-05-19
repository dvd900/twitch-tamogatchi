using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteBehaviour : StateMachineBehaviour
{// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Emote done!" + animator.transform.parent);
        animator.GetComponentInParent<EmoteController>().EmoteStart();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Emote done!" + animator.transform.parent);
        animator.GetComponentInParent<EmoteController>().EmoteEnd();
    }
}
