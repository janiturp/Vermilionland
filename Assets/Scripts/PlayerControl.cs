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
    public static PlayerControl player;
    #region Player attributes
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float moveSpeed;
    #endregion

    #region Weapons.
    [SerializeField] private int activeWeapon;
    [SerializeField] private GameObject[] weaponInventory;

    [SerializeField] private GameObject pistol;

    [SerializeField] public GameObject shotgun;

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

    #region Sound Effects.
    // Weapons
    [SerializeField] private AudioClip pistolSound;
    [SerializeField] private AudioClip shotgunSound;
    [SerializeField] private AudioClip pistolReloadSound;
    [SerializeField] private AudioClip shotgunReloadSound;
    [SerializeField] private AudioClip dryShot;
    [SerializeField] private AudioClip weaponSwitch;
    [SerializeField] private AudioClip shotgunPickUp;

    // HealthPacks
    [SerializeField] private AudioClip healthPackSound;

    // Damage and death
    [SerializeField] private AudioClip damageSoundStab;
    [SerializeField] private AudioClip damageSoundFire;
    [SerializeField] private AudioClip deathSound;

    // Footsteps
    [SerializeField] private AudioClip footstep1;
    [SerializeField] private AudioClip footstep2;
    #endregion

    [SerializeField] private Canvas canvas;
    private int currentSceneIndex;

    private AudioSource audioSource;

    // Stuff for movement.
    private Rigidbody2D rb;
    Vector2 direction;
    private float angle;
    private float speed = 60f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GameManager.manager.pistol = pistol;
        GameManager.manager.shotgun = shotgun;
        rb = GetComponent<Rigidbody2D>();
        GameManager.manager.activeWeapon = 0;
        GameManager.manager.weaponInventory = new GameObject[2];
        GameManager.manager.weaponInventory[0] = pistol;
        //GameManager.manager.pistolCurrentAmmo = GameManager.manager.pistolMagazineCapacity;
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameManager.manager.soundEffectsVolume;

        #region Fire weapons.
        // Fire Pistol
        if (GameManager.manager.activeWeapon == 0 && Input.GetButtonDown("Fire1"))
        {
            if (GameManager.manager.pistolCurrentAmmo > 0 && !reloading)
            {
                FirePistol();
                GameManager.manager.pistolCurrentAmmo--;
            }
            else if (!reloading)
            {
                audioSource.PlayOneShot(pistolReloadSound);
                reloading = true;
                StartCoroutine(ReloadWeapon(GameManager.manager.pistolReloadTime, GameManager.manager.activeWeapon));
            }
            else
            {
                audioSource.PlayOneShot(dryShot);
            }
        }

        // Fire Shotgun
        if (GameManager.manager.activeWeapon == 1 && Input.GetButtonDown("Fire1"))
        {
            if(GameManager.manager.shotgunCurrentAmmo > 0 && !reloading)
            {
                FireShotgun();
                GameManager.manager.shotgunCurrentAmmo--;
            }
            else if(!reloading)
            {
                audioSource.PlayOneShot(shotgunReloadSound);
                reloading = true;
                StartCoroutine(ReloadWeapon(GameManager.manager.shotgunReloadTime, GameManager.manager.activeWeapon));
            }
            else
            {
                audioSource.PlayOneShot(dryShot);
            }
        }
        #endregion

        #region Switch to weapon.
        // Switch to pistol.
        if (Input.GetButtonDown("Weapon 1"))
        {
            audioSource.PlayOneShot(weaponSwitch);
            GameManager.manager.activeWeapon = 0;
        }

        // Switch to shotgun. Shotgun needs to be in inventory.
        if(Input.GetButtonDown("Weapon 2") && GameManager.manager.weaponInventory.Contains(shotgun))
        {
            audioSource.PlayOneShot(weaponSwitch);
            GameManager.manager.activeWeapon = 1;
        }

        // Switch system for active weapon.
        switch (GameManager.manager.activeWeapon)
        {
            case 0:
                GameManager.manager.pistol.SetActive(true);
                GameManager.manager.shotgun.SetActive(false);
                break;
            case 1:
                GameManager.manager.shotgun.SetActive(true);
                GameManager.manager.pistol.SetActive(false);
                break;
            default:
                print("Incorrect weapon.");
                break;
        }
        #endregion

        // Weapon reload on key press.
        if (Input.GetButtonDown("Reload") && !reloading)
        {
            ManualReload(GameManager.manager.activeWeapon);
        }

        if(reloading)
        {
            if(GameManager.manager.activeWeapon == 0)
            {
                canvas.transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().fillAmount += 1 / GameManager.manager.pistolReloadTime * Time.deltaTime;
            }
            else if(GameManager.manager.activeWeapon == 1)
            {
                canvas.transform.GetChild(1).transform.GetChild(3).GetComponent<Image>().fillAmount += 1 / GameManager.manager.shotgunReloadTime * Time.deltaTime;
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
        rb.velocity = new Vector2(horizontalInput * GameManager.manager.moveSpeed, verticalInput * GameManager.manager.moveSpeed);

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
            audioSource.PlayOneShot(damageSoundStab);
            TakeDamage(10);
        }

        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            audioSource.PlayOneShot(damageSoundFire);
            TakeDamage(5);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        #region Healthpacks
        if (collision.gameObject.CompareTag("HealthPackSmall"))
        {
            audioSource.PlayOneShot(healthPackSound);
            Heal(20);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("HealthPackBig"))
        {
            audioSource.PlayOneShot(healthPackSound);
            Heal(60);
            Destroy(collision.gameObject);
        }
        #endregion

        #region Weapon pickups.
        // Shotgun pickup.
        if (collision.gameObject.CompareTag("ShotgunPickup"))
        {
            audioSource.PlayOneShot(shotgunPickUp);
            GameManager.manager.weaponInventory[1] = shotgun;
            Destroy(collision.gameObject);
            GameManager.manager.shotgunCurrentAmmo = GameManager.manager.shotgunMagazineCapacity;
        }
        #endregion
    }

    #region Fire weapon functions.
    // Fire Pistol
    private void FirePistol()
    {
        audioSource.PlayOneShot(pistolSound);
        GameObject bulletInstance = Instantiate(pistolBullet, pistolBulletSpawn.transform.position, Quaternion.identity);
        bulletInstance.GetComponent<Rigidbody2D>().velocity = pistolBulletSpawn.transform.right * pistolBulletSpeed * transform.localScale.x;
    }

    // Fire shotgun
    private void FireShotgun()
    {
        audioSource.PlayOneShot(shotgunSound);
        // Creates a spread of 5 pellets.
        for(int i = 0; i < 5; i++)
        {
            GameObject bulletInstance = Instantiate(shotgunBullet, shotgunBulletSpawn.transform.position, Quaternion.identity);
            Vector3 dir = new Vector3(Random.Range(-GameManager.manager.shotgunMaxSpread, GameManager.manager.shotgunMaxSpread), Random.Range(-GameManager.manager.shotgunMaxSpread, GameManager.manager.shotgunMaxSpread), 0);
            bulletInstance.GetComponent<Rigidbody2D>().velocity = (shotgunBulletSpawn.transform.right + dir)* shotgunBulletSpeed * transform.localScale.x;
        }
    }

    // Reload functions.
    // Reload weapon when player tries to shoot with empty weapon.
    IEnumerator ReloadWeapon(float reloadTime, int activeWeapon)
    {

        yield return new WaitForSeconds(reloadTime);
        reloading = false;
        
        // Check which weapon is being reloaded.
        if(activeWeapon == 0)
        {
            GameManager.manager.pistolCurrentAmmo = GameManager.manager.pistolMagazineCapacity;
        }
        else if(activeWeapon == 1)
        {
            GameManager.manager.shotgunCurrentAmmo = GameManager.manager.shotgunMagazineCapacity;
        }
    }

    // Reload weapon when player presses R-key.
    private void ManualReload(int activeWeapon)
    {
        // Check which weapon is being reloaded.
        if (activeWeapon == 0)
        {
            if(GameManager.manager.pistolCurrentAmmo != GameManager.manager.pistolMagazineCapacity)
            {
                reloading = true;
                audioSource.PlayOneShot(pistolReloadSound);
                StartCoroutine(ReloadWeapon(GameManager.manager.pistolReloadTime, activeWeapon));
            }
        }
        else if(activeWeapon == 1)
        {
            if(GameManager.manager.shotgunCurrentAmmo != GameManager.manager.shotgunMagazineCapacity)
            {
                reloading = true;
                audioSource.PlayOneShot(shotgunReloadSound);
                StartCoroutine(ReloadWeapon(GameManager.manager.shotgunReloadTime, activeWeapon));
            }
        }
        else
        {
            Debug.Log("Weapon reload error. Illegal ActiveWeapon.");
        }
    }

    #endregion



    // Take damage from enemy.
    private void TakeDamage(int amount)
    {
        GameManager.manager.health -= amount;
        if(GameManager.manager.health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        SceneManager.LoadScene(currentSceneIndex);
        GameManager.manager.health = GameManager.manager.maxHealth;
    }

    // Heal from healthpacks
    private void Heal(int amount)
    {
        GameManager.manager.health += amount;
        if (GameManager.manager.health > GameManager.manager.maxHealth)
        {
            GameManager.manager.health = GameManager.manager.maxHealth;
        }
    }

    #region Getters for UI.
    public float GetCurrentHealth()
    {
        return GameManager.manager.health;
    }

    public float GetMaxHealth()
    {
        return GameManager.manager.maxHealth;
    }

    public int GetActiveWeapon()
    {
        return GameManager.manager.activeWeapon;
    }

    public int GetPistolAmmo()
    {
        return GameManager.manager.pistolCurrentAmmo;
    }

    public int GetShotgunAmmo()
    {
        return GameManager.manager.shotgunCurrentAmmo;
    }

    public int GetPistolMagazineCapacity()
    {
        return GameManager.manager.pistolMagazineCapacity;
    }

    public int GetShotgunMagazineCapacity()
    {
        return GameManager.manager.shotgunMagazineCapacity;
    }

    public float GetPistolReloadTime()
    {
        return GameManager.manager.pistolReloadTime;
    }

    public float GetShotgunReloadTime()
    {
        return GameManager.manager.shotgunReloadTime;
    }

    public bool IsReloading()
    {
        return reloading;
    }
    #endregion
}
