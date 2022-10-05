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
        if(enemy.health > enemy.health * 0.25)
        {
            enemy.rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, (Mathf.Atan2(enemy.chaseTarget.transform.position.y - enemy.transform.position.y, enemy.chaseTarget.transform.position.x - enemy.transform.position.x) * Mathf.Rad2Deg) - 90)));
            enemy.rb.MovePosition(Vector2.MoveTowards(enemy.transform.position, enemy.chaseTarget.position, enemy.chaseSpeed * Time.fixedDeltaTime));
        }
        else
        {
            ToFleeState();
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
