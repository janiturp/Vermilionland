using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    public bool isPaused = false;

    #region Player attributes
    public int health;
    public int maxHealth;
    public float moveSpeed;
    #endregion

    #region Weapons.
    public int activeWeapon;
    public GameObject[] weaponInventory;
    public GameObject pistol;
    public GameObject shotgun;
    #endregion

    [SerializeField] private Canvas canvas;

    #region Weapon attributes.
    // Pistol
    public int pistolMagazineCapacity;
    public int pistolReloadTime;
    public int pistolCurrentAmmo;

    // Shotgun
    public float shotgunBulletSpread;
    public float shotgunMaxSpread;
    public int shotgunMagazineCapacity;
    public float shotgunReloadTime;
    public int shotgunCurrentAmmo;
    #endregion

    #region Music
    public AudioClip mainMenu;
    public AudioClip inGame;
    public AudioClip winScreen;
    #endregion

    private AudioSource audioSource;

    public float musicVolume;
    public float soundEffectsVolume;

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
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = mainMenu;
        audioSource.Play();
        health = 100;
        maxHealth = 100;
        moveSpeed = 10;
        pistolMagazineCapacity = 16;
        pistolCurrentAmmo = 16;
        pistolReloadTime = 1;
        shotgunBulletSpread = 0.5f;
        shotgunMaxSpread = 0.6f;
        shotgunMagazineCapacity = 6;
        shotgunReloadTime = 2;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = musicVolume;

        if(Input.GetKeyUp(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame();
        }
    }

    void PauseGame()
    {
        GameObject pauseScene = GameObject.FindGameObjectWithTag("PauseScene");
        if (isPaused)
        {
            Debug.Log("Game paused.");
            Time.timeScale = 0f;
            SceneManager.LoadScene("PauseScene", LoadSceneMode.Additive);
        }
        else
        {
            Time.timeScale = 1f;
            Destroy(pauseScene);
        }
    }

    public void SaveGame()
    {

    }

    public void LoadGame()
    {

    }
}
