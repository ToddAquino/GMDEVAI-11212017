using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : NPCBaseFSM
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("FLEE!");
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (opponent != null)
        {
            var direction = NPC.transform.position - opponent.transform.position;
            NPC.transform.rotation = Quaternion.Slerp(NPC.transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            NPC.transform.Translate(0, 0, Time.deltaTime * speed);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}


}
