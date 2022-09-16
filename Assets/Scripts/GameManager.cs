using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    private void Awake()
    {
        // Check if manager exists
        if(manager == null)
        {
            // If there's no manager, create a new one.
            // Manager is not destroyed when new scene loads.
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
