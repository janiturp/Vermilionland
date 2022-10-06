using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertState : IEnemyState
{
    private StatePatternEnemy enemy;

    public AlertState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Alert();
        enemy.StartCoroutine(EnemyAlerted());
    }

    void Alert()
    {
        // Disables enemy hearing. No need for it anymore.
        enemy.GetComponent<CircleCollider2D>().enabled = false;
        enemy.GetComponent<AIPath>().enabled = false;
        enemy.moveSpot.transform.position = enemy.chaseTarget.transform.position;
    }

    IEnumerator EnemyAlerted()
    {
        // Enemy is alerted and looks at player.
        enemy.rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(enemy.chaseTarget.transform.position.y - enemy.transform.position.y, enemy.chaseTarget.transform.position.x - enemy.transform.position.x) * Mathf.Rad2Deg) - 90)));

        // Wait for 2 seconds. Will later insert a roar here. Enemy goes to ChaseState after roaring.
        yield return new WaitForSeconds(2);
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
        enemy.currentState = enemy.fleeState;
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

}
