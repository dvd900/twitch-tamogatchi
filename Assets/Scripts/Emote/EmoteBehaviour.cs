using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Emote start!" + animator.transform.parent);
        animator.GetComponentInParent<EmoteController>().OnEmoteStart();
    }
    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Emote done!" + animator.transform.parent);
        
        animator.GetComponentInParent<EmoteController>().OnEmoteEnd();
    }
}
