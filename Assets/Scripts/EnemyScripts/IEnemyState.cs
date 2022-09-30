using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    // Interface for melee enemy.
    void UpdateState();
    void OnTriggerEnter2D(Collider2D collision);
    void OnCollisionEnter2D(Collision2D collision);
    void ToPatrolState();
    void ToChaseState();
    void ToFleeState();
    void ToAttackState();
    void ToAlertState();
}
