using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : StateMachineBehaviour
{
    private float timer;
    private Vector3 StartingPosition;
    private Vector3 roamingPosition;

    // Randomizes enemy patrolling. Creates randomized Vector that is the direction and length of enemy's movement.
    private Vector3 GetRoamingPosition()
    {
        return StartingPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * Random.Range(10f, 70f);
    }
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(0f, 5f);
        StartingPosition = animator.transform.position;
        roamingPosition = GetRoamingPosition();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            animator.SetBool("isPatrolling", false);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
