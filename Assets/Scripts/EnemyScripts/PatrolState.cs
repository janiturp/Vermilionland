using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : IEnemyState
{
    private StatePatternEnemy enemy;

    public PatrolState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
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
        // Logic for enemy to turn towards the moveSpot
        //enemy.rb.MoveRotation(Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(enemy.moveSpot.transform.position.y - enemy.transform.position.y, enemy.moveSpot.transform.position.x - enemy.transform.position.x) * Mathf.Rad2Deg)));
        //enemy.rb.MovePosition(Vector2.MoveTowards(enemy.transform.position, enemy.moveSpot.position, enemy.moveSpeed * Time.fixedDeltaTime));

        // If enemy gets to the spot and wait time is 0, moveSpot is moved to a new random location.
        if (Vector2.Distance(enemy.transform.position, enemy.moveSpot.position) < 0.2f)
        {
            if (enemy.waitTime <= 0)
            {
                //enemy.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(enemy.moveSpot.transform.position.y - enemy.transform.position.y, enemy.moveSpot.transform.position.x - enemy.transform.position.x) * Mathf.Rad2Deg));
                enemy.moveSpot.position = new Vector2(Random.Range(enemy.minX, enemy.maxX), Random.Range(enemy.minY, enemy.maxY));
                enemy.waitTime = Random.Range(0f, 5f);
                Debug.Log("New movespot randomized.");
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

        // Useless code for now.
        /*
        if (collision.collider.GetType() == typeof(BoxCollider2D))
        {
            // Enemy stops when they hit a wall.
            if (collision.gameObject.CompareTag("Wall"))
            {
                Debug.Log("Enemy hit wall.");
                enemy.moveSpot.position = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
            }
        }
        */
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
