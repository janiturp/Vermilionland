using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeState : MonoBehaviour
{
    private StatePatternEnemy enemy;

    public FleeState(StatePatternEnemy statePatternEnemy)
    {
        this.enemy = statePatternEnemy;
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
