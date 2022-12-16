using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeStateRanged : IEnemyStateRanged
{
    private StatePatternEnemyRanged enemy;

    public FleeStateRanged(StatePatternEnemyRanged statePatternEnemyRanged)
    {
        this.enemy = statePatternEnemyRanged;
    }

    /*
     * For some reason, flee state doesn't work correctly right now. The enemy enters flee state, but goes to
     * chase state again soon after. Needs bug fixing later.
     */
    public void UpdateState()
    {
        Flee();
        Debug.Log("Enemy fleeing.");
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
