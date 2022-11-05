using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private StatePatternEnemy enemy;
    private float stabSpeed;
    public AttackState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        stabSpeed = 100;
        // Stops the enemy movement. In theory. Not sure if it works properly. Bugfix this later.
        enemy.rb.velocity = Vector2.zero;
        enemy.GetComponent<AIPath>().enabled = false;
        enemy.StartCoroutine(Attack());
    }

    // Enemy enables their arm, tries to stab the player, then moves to ChaseState again.
    IEnumerator Attack()
    {
        enemy.animator.SetTrigger("Attack");

        /*
        Vector3 originalPosition = enemy.arm.transform.position;

        enemy.arm.transform.Translate(new Vector2(0, 0.1f) * stabSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);

        enemy.arm.transform.position = originalPosition;
        */

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
