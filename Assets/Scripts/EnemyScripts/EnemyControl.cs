using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float waitTime;

    [SerializeField]
    private Transform moveSpot;
    private float minX = -5;
    private float maxX = 5;
    private float minY = -5;
    private float maxY = 5;

    [SerializeField]
    private GameObject bloodSplatter;

    // Start is called before the first frame update
    void Start()
    {

        // Start randomized patrolling.
        waitTime = Random.Range(0f, 5f);
        moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    // Update is called once per frame
    void Update()
    {

        // Randomized patrolling logic.
        transform.position = Vector2.MoveTowards(transform.position, moveSpot.position, moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if(waitTime <= 0)
            {
                moveSpot.position = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = Random.Range(0f, 5f);
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
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

        // Enemy stops when they hit a wall.
        if(collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Enemy hit wall.");
            moveSpot.position = new Vector2(transform.position.x, transform.position.y);
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
