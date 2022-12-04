using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    #region Player attributes
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    #endregion

    #region Weapons.
    [SerializeField] private int activeWeapon;
    [SerializeField] private GameObject[] weaponInventory;

    [SerializeField] private GameObject pistol;

    [SerializeField] private GameObject shotgun;

    bool reloading = false;
    #endregion


    #region Weapon attributes.
    // Pistol
    [SerializeField] private GameObject pistolBullet;
    [SerializeField] private GameObject pistolBulletSpawn;
    [SerializeField] private float pistolBulletSpeed;
    [SerializeField] private int pistolMagazineCapacity;
    [SerializeField] private float pistolReloadTime;
    private int pistolCurrentAmmo;

    // Shotgun
    [SerializeField] private GameObject shotgunBullet;
    [SerializeField] private GameObject shotgunBulletSpawn;
    [SerializeField] private float shotgunBulletSpeed;
    [SerializeField] private float shotgunMaxSpread;
    [SerializeField] private int shotgunMagazineCapacity;
    [SerializeField] private float shotgunReloadTime;
    private int shotgunCurrentAmmo;
    #endregion
    [SerializeField] private Canvas canvas;

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
        weaponInventory = new GameObject[5];
        weaponInventory[1] = pistol;
        pistolCurrentAmmo = pistolMagazineCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        #region Fire weapons.
        // Fire Pistol
        if (activeWeapon == 0 && Input.GetButtonDown("Fire1"))
        {
            if (pistolCurrentAmmo > 0 && !reloading)
            {
                FirePistol();
                pistolCurrentAmmo--;
            }
            else if (!reloading)
            {
                Debug.Log("Reloading pistol.");
                reloading = true;
                StartCoroutine(ReloadWeapon(pistolReloadTime, activeWeapon));
            }
            else
            {
                Debug.Log("Already reloading.");
            }
        }

        // Fire Shotgun
        if (activeWeapon == 1 && Input.GetButtonDown("Fire1"))
        {
            if(shotgunCurrentAmmo > 0 && !reloading)
            {
                FireShotgun();
                shotgunCurrentAmmo--;
            }
            else if(!reloading)
            {
                Debug.Log("Reloading shotgun.");
                reloading = true;
                StartCoroutine(ReloadWeapon(shotgunReloadTime, activeWeapon));
            }
            else
            {
                Debug.Log("Already realoding.");
            }
        }
        #endregion

        #region Switch to weapon.
        // Switch to pistol.
        if (Input.GetButtonDown("Weapon 1"))
        {
            activeWeapon = 0;
        }

        // Switch to shotgun. Shotgun needs to be in inventory.
        if(Input.GetButtonDown("Weapon 2") && weaponInventory.Contains(shotgun))
        {
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
        #endregion

        // Weapon reload on key press.
        if (Input.GetButtonDown("Reload") && !reloading)
        {
            ManualReload(activeWeapon);
        }

        if(reloading)
        {
            if(activeWeapon == 0)
            {
                canvas.transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().fillAmount += 1 / pistolReloadTime * Time.deltaTime;
            }
            else if(activeWeapon == 1)
            {
                canvas.transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().fillAmount += 1 / shotgunReloadTime * Time.deltaTime;
            }
        }
        else
        {
            canvas.transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().fillAmount = 0;
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
        #region Healthpacks
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
        #endregion

        #region Weapon pickups.
        // Shotgun pickup.
        if (collision.gameObject.CompareTag("ShotgunPickup"))
        {
            Debug.Log("Shotgun picked up.");
            weaponInventory[1] = shotgun;
            Destroy(collision.gameObject);
            shotgunCurrentAmmo = shotgunMagazineCapacity;
        }
        #endregion
    }

    #region Fire weapon functions.
    // Fire Pistol
    private void FirePistol()
    {
        GameObject bulletInstance = Instantiate(pistolBullet, pistolBulletSpawn.transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = pistolBulletSpawn.transform.right * pistolBulletSpeed * transform.localScale.x;
    }

    // Fire shotgun
    private void FireShotgun()
    {
        // Creates a spread of 5 pellets.
        for(int i = 0; i < 5; i++)
        {
            GameObject bulletInstance = Instantiate(shotgunBullet, shotgunBulletSpawn.transform.position, Quaternion.identity);
            Vector3 dir = new Vector3(Random.Range(-shotgunMaxSpread, shotgunMaxSpread), Random.Range(-shotgunMaxSpread, shotgunMaxSpread), 0);
            bulletInstance.GetComponent<Rigidbody2D>().velocity = (shotgunBulletSpawn.transform.right + dir)* shotgunBulletSpeed * transform.localScale.x;
        }
    }

    // Reload functions.
    // Reload weapon when player tries to shoot with empty weapon.
    IEnumerator ReloadWeapon(float reloadTime, int activeWeapon)
    {

        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        Debug.Log("Reload done.");
        
        // Check which weapon is being reloaded.
        if(activeWeapon == 0)
        {
            Debug.Log("Reloading pistol.");
            pistolCurrentAmmo = pistolMagazineCapacity;
        }
        else if(activeWeapon == 1)
        {
            Debug.Log("Reloading shotgun.");
            shotgunCurrentAmmo = shotgunMagazineCapacity;
        }
    }

    // Reload weapon when player presses R-key.
    private void ManualReload(int activeWeapon)
    {
        // Check which weapon is being reloaded.
        if (activeWeapon == 0)
        {
            if(pistolCurrentAmmo != pistolMagazineCapacity)
            {
                reloading = true;
                StartCoroutine(ReloadWeapon(pistolReloadTime, activeWeapon));
            }
        }
        else if(activeWeapon == 1)
        {
            if(shotgunCurrentAmmo != shotgunMagazineCapacity)
            {
                reloading = true;
                StartCoroutine(ReloadWeapon(shotgunReloadTime, activeWeapon));
            }
        }
        else
        {
            Debug.Log("Weapon reload error. Illegal ActiveWeapon.");
        }
    }

    #endregion



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

    #region Getters for UI.
    public float GetCurrentHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public int GetActiveWeapon()
    {
        return activeWeapon;
    }

    public int GetPistolAmmo()
    {
        return pistolCurrentAmmo;
    }

    public int GetShotgunAmmo()
    {
        return shotgunCurrentAmmo;
    }

    public int GetPistolMagazineCapacity()
    {
        return pistolMagazineCapacity;
    }

    public int GetShotgunMagazineCapacity()
    {
        return shotgunMagazineCapacity;
    }

    public float GetPistolReloadTime()
    {
        return pistolReloadTime;
    }

    public float GetShotgunReloadTime()
    {
        return shotgunReloadTime;
    }

    public bool IsReloading()
    {
        return reloading;
    }
    #endregion
}
