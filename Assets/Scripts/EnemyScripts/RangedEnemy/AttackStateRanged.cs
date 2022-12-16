using JetBrains.Annotations;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateRanged : IEnemyStateRanged
{
    private StatePatternEnemyRanged enemy;
    public AttackStateRanged(StatePatternEnemyRanged statePatternEnemyRanged)
    {
        this.enemy = statePatternEnemyRanged;
    }

    public void UpdateState()
    {
        enemy.GetComponent<AIPath>().enabled = false;
        enemy.StartCoroutine(Attack());

    }

    // Enemy shoots and goes to chase state.
    IEnumerator Attack()
    {
        enemy.rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(enemy.chaseTarget.transform.position.y - enemy.transform.position.y, enemy.chaseTarget.transform.position.x - enemy.transform.position.x) * Mathf.Rad2Deg) - 90)));
        enemy.animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        ToChaseState();
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

    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
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
