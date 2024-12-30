using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reload : StateMachineBehaviour
{

    public float reloadTime = 0.5f;
    bool hasReloaded = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasReloaded = false ;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (hasReloaded) 
        {
            return;
        }
        if(stateInfo.normalizedTime >= reloadTime) //retorna o valor normalizado da animacao, sendo: 0  inicio da animacao, 0.5 = meio da animacao, 1 = final da animacao
        {
            animator.GetComponent<Weapon>().Reload();
            hasReloaded = true;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasReloaded = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
