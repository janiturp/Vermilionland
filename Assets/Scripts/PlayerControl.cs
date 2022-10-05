using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    // Player attributes
    [SerializeField] private int activeWeapon;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;

    // Weapons
    [SerializeField] private bool pistolActive;
    [SerializeField] private GameObject pistol;

    [SerializeField] private bool shotgunActive;
    [SerializeField] private GameObject shotgun;
    private bool shotgunInInventory;


    // Weapon attributes
    // Pistol
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private GameObject pistolBulletSpawn;
    [SerializeField] private float pistolBulletSpeed;

    // Shotgun
    [SerializeField] private GameObject shotgunBullet;
    [SerializeField] private GameObject shotgunBulletSpawn;
    [SerializeField] private float shotgunBulletSpeed;
    [SerializeField] private float shotgunMaxSpread;


    // Stuff for movement.
    private Rigidbody2D rb;
    Vector2 direction;
    private float angle;
    private float speed = 60f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        activeWeapon = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Fire Pistol
        if (activeWeapon == 0 && Input.GetButtonDown("Fire1"))
        {
            FirePistol();
        }

        // Fire Shotgun
        if (activeWeapon == 1 && Input.GetButtonDown("Fire1"))
        {
            FireShotgun();
        }

        // Switch to pistol.
        if(Input.GetButtonDown("Weapon 1"))
        {
            print("Pistol");
            activeWeapon = 0;
        }

        // Switch to shotgun. Shotgun needs to be in inventory.
        if(Input.GetButtonDown("Weapon 2") && shotgunInInventory)
        {
            print("Shotgun");
            activeWeapon = 1;
        }

        // Switch system for active weapon.
        switch (activeWeapon)
        {
            case 0:
                pistol.SetActive(true);
                shotgun.SetActive(false);
                break;
            case 1:
                shotgun.SetActive(true);
                pistol.SetActive(false);
                break;
            default:
                print("Incorrect weapon.");
                break;
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

        // Shotgun pickup.
        if (collision.gameObject.CompareTag("ShotgunPickup"))
        {
            Debug.Log("Shotgun picked up.");
            shotgunInInventory = true;
            Destroy(collision.gameObject);
        }
    }

    // Fire Pistol
    private void FirePistol()
    {
        GameObject bulletInstance = Instantiate(pistolBullet, pistolBulletSpawn.transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = pistolBulletSpawn.transform.right * pistolBulletSpeed * transform.localScale.x;
    }

    // Fire shotgun
    private void FireShotgun()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject bulletInstance = Instantiate(shotgunBullet, shotgunBulletSpawn.transform.position, Quaternion.identity);
            Vector3 dir = new Vector3(Random.Range(-shotgunMaxSpread, shotgunMaxSpread), Random.Range(-shotgunMaxSpread, shotgunMaxSpread), 0);
            bulletInstance.GetComponent<Rigidbody2D>().velocity = (shotgunBulletSpawn.transform.right + dir)* shotgunBulletSpeed * transform.localScale.x;
        }
    }



    // Take damage from enemy.
    private void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Dies.");
        // Later create a system that detects current scene and reloads it.
        //SceneManager.LoadScene("SampleScene");
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
