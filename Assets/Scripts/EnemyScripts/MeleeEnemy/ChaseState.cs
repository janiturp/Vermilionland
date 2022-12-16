using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : IEnemyState
{
    private StatePatternEnemy enemy;

    public ChaseState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        //Look();
        Chase();
    }

    // Enemy looks at player and moves towards them.
    void Chase()
    {
        enemy.animator.SetTrigger("Chase");
        enemy.GetComponent<AIPath>().enabled = true;
        enemy.GetComponent<AIDestinationSetter>().target = enemy.chaseTarget;
        enemy.GetComponent<AIPath>().maxSpeed = enemy.chaseSpeed;
        enemy.GetComponent<AIPath>().endReachedDistance = 2f;

        if (enemy.GetComponent<AIPath>().reachedEndOfPath)
        {
            ToAttackState();
        }
        
    }

    // Raycast for enemy's attackrange. When player is in range, enemy goes to AttackState.
    void Look()
    {
        Debug.DrawRay(enemy.eye.position, enemy.eye.right * enemy.attackRange, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(enemy.eye.position, enemy.eye.right, enemy.attackRange);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.collider.gameObject.transform;
            ToAttackState();
        }
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {

    }

    public void ToFleeState()
    {
        enemy.currentState = enemy.fleeState;
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

}
