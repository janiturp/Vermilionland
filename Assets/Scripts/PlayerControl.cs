using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // Player attributes
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
#pragma warning disable
    private bool noWeapons;

    // Weapons
    private int activeWeapon;
    [SerializeField]
    private bool pistolActive;
    [SerializeField]
    private GameObject pistol;
#pragma warning disable
    private bool pistolInInventory;

    // Weapon attributes
    [SerializeField]
    private float dashForce;
    // Pistol
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletSpawn;
    [SerializeField]
    private float bulletSpeed;

    private Rigidbody2D rb;
    Vector2 direction;
    private float angle;
    private float speed = 60f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        noWeapons = true;
        pistolInInventory = false;
        pistolActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Fire Pistol
        if (pistolActive && Input.GetButtonDown("Fire1"))
        {
            GameObject bulletInstance = Instantiate(bullet, bulletSpawn.transform.position, Quaternion.identity);
            bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletSpawn.transform.right * bulletSpeed * transform.localScale.x;
        }

    }

    private void FixedUpdate()
    {
        // Movement.
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, verticalInput * moveSpeed);

        // Character Rotation.
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Player takes damage if enemy's arm hits the player.
        Collider2D myCollider = collision.GetContact(0).collider;
        if (myCollider.CompareTag("EnemyArm"))
        {
            Debug.Log("Enemy hits the player. Taking damage.");
            TakeDamage(10);
        }
  
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Healthpacks
        if (collision.gameObject.CompareTag("HealthPackSmall"))
        {
            Heal(20);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("HealthPackBig"))
        {
            Heal(60);
            Destroy(collision.gameObject);
        }

        // Pistol pickup.
        if (collision.gameObject.CompareTag("PistolPickup"))
        {
            Debug.Log("Pistol picked up.");
            pistolInInventory = true;
            noWeapons = false;
            pistolActive = true;
            pistol.SetActive(true);
            Destroy(collision.gameObject);
        }
    }

    private void FirePistol()
    {
        if(pistolActive)
        {

        }
    }
    private void ActiveWeapon ()
    {
        switch (activeWeapon)
        {
            case 2:
                print("Pistol");
                pistolActive = true;
                pistol.SetActive(true);
                pistolInInventory = true;
                break;
            case 1:
                print("No weapon");
                noWeapons = true;
                break;
            default: 
                print ("Incorrect weapon.");
                break;
        }
    }

    // Take damage from enemy.
    private void TakeDamage(float amount)
    {
        health -= amount;
    }

    // Heal from healthpacks
    private void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        Debug.Log("Healed " + amount + ". Current health: " + health);
    }
}
