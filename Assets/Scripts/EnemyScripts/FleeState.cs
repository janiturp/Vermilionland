using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : IEnemyState
{
    private StatePatternEnemy enemy;

    public FleeState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Flee();
    }

    // Enemy flees when low on health.
    void Flee()
    {
        // Draws a Vector to the player which is then inverted. Enemy flees away from the player.
        Vector2 fleeDirection = enemy.transform.position - enemy.chaseTarget.position;
        enemy.rb.MoveRotation(Quaternion.LookRotation(fleeDirection));
        enemy.rb.MovePosition(Vector2.MoveTowards(enemy.transform.position, fleeDirection, enemy.chaseSpeed * Time.fixedDeltaTime));
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
        enemy.currentState = enemy.chaseState;
    }

    public void ToFleeState()
    {

    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

}
