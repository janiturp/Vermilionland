using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]
    private float health;

    [SerializeField]
    private GameObject bloodSplatter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bloodsplatter particle effect. If I have time I will make it so that the particles fly towards the direction
        // Where the bullet came from.
        if(collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(10);
            Instantiate(bloodSplatter, transform.position, Quaternion.identity);
        }
    }

    private void TakeDamage(float amount)
    {
        health -= amount;

        if(health <= 0)
        {
            Debug.Log("Enemy killed.");
            Destroy(gameObject, 1);
        }
    }
}
