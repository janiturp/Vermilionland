using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbJoke : MonoBehaviour
{
    private bool traurig;
    private int bande;
    // Start is called before the first frame update
    void Start()
    {
        traurig = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private String armeeDerTristen()
    {
        if(traurig == true)
        {
            bande++;
            return ("So wie ich!");
        }
        else
        {
            return ("");
        }
    }
}
