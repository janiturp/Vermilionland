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
        Vector3 originalPosition = enemy.arm.transform.position;

        enemy.arm.transform.Translate(new Vector2(0, 0.1f) * stabSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.5f);

        enemy.arm.transform.position = originalPosition;

        yield return new WaitForSeconds(2);
        ToChaseState();


        /* Old system for arm swing. Switched to arm stab.
         * 
        // Arm's original position.
        Vector3 originalPosition = enemy.armRotator.transform.position;
        // How much the enemy's arm is rotated when swung.
        Vector3 rotateAmount = new Vector3(0, 0, -90);
        // Check just in case if arm is not activated before swinging.
        if(!enemy.arm.activeSelf)
        {
            // Activate arm.
            enemy.arm.SetActive(true);
            // Swing the arm.
            enemy.armRotator.GetComponent<RotateArm>().zAngle = Mathf.Atan(rotateAmount.z) * Mathf.Rad2Deg;
            // Wait until swing finishes.
            yield return new WaitUntil(() => enemy.armRotator.GetComponent<RotateArm>().rotating == false);
            // Put the arm in the original position.
            enemy.armRotator.GetComponent<RotateArm>().zAngle = Mathf.Atan(originalPosition.z) * Mathf.Rad2Deg;
            // Disable the arm.
            enemy.arm.SetActive(false);
            // Wait for 1 seconds and go to AlertState again.
            yield return new WaitForSeconds(1);
            ToChaseState();
        }
        */
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
