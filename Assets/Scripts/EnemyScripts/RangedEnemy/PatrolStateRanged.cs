using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolStateRanged : IEnemyStateRanged
{
    private StatePatternEnemyRanged enemy;

    public PatrolStateRanged(StatePatternEnemyRanged statePatternEnemyRanged)
    {
        this.enemy = statePatternEnemyRanged;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
    }

    // Randomized patrolling logic.
    void Patrol()
    {
        // Not sure if patrolSpeed is used... Check later and remove if not needed.
        Vector2 patrolSpeed = new Vector2(0f, 1f);

        // If enemy gets to the end of path to the moveSpot and wait time is 0, moveSpot is moved to a new random location.
        if (enemy.GetComponent<AIPath>().reachedEndOfPath)
        {
            if (enemy.waitTime <= 0)
            {
                enemy.moveSpot.position = new Vector2(enemy.transform.position.x + (Random.Range(enemy.minX, enemy.maxX)), enemy.transform.position.y + (Random.Range(enemy.minY, enemy.maxY)));
                enemy.waitTime = Random.Range(0f, 5f);
            }
            else
            {
                enemy.waitTime -= Time.deltaTime;
            }
        }
    }

    // Raycast line for enemy's vision.
    void Look()
    {
        Debug.DrawRay(enemy.eye.position, enemy.eye.right * enemy.sightRange, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(enemy.eye.position, enemy.eye.right, enemy.sightRange);

        // If raycast is hit = enemy sees player and goes to AlertState.
        if(hit.collider != null && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.collider.gameObject.transform;
            ToAlertState();
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        // If player enters enemy's CircleCollider = enemy hears player and goes to AlertState.
        if(collision.CompareTag("Player"))
        {
            enemy.chaseTarget = collision.gameObject.transform;
            ToAlertState();
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
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
        // Already in PatrolState
    }

    public void ToAlertState()
    {
        enemy.currentState = enemy.alertState;
    }
}
