using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DieState : IEnemyState
{
    private StatePatternEnemy enemy;

    public DieState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {

    }
    public void UpdateState()
    {
        Die();
    }

    // The enemy dies. RIP.
    void Die()
    {
        StatePatternEnemy.Destroy(enemy.gameObject, 1);
    }

    public void ToAlertState()
    {

    }

    public void ToAttackState()
    {

    }

    public void ToChaseState()
    {

    }

    public void ToFleeState()
    {

    }

    public void ToPatrolState()
    {

    }

}
