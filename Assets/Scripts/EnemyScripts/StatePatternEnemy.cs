using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternEnemy : MonoBehaviour
{
    public float aggroRange;
    public float health;

    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public ChaseState chaseState;
    [HideInInspector] public FleeState fleeState;
    [HideInInspector] public AttackState attackState;

    private void Awake()
    {
        patrolState = new PatrolState(this);
        chaseState = new ChaseState(this);
        fleeState = new FleeState(this);
        attackState = new AttackState(this);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
