using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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

    public string currentLevel;
    public bool level1;
    public bool level2;

    string path;
    string jsonString;

    public int playerEXP;
    public int playerEXPtoLvlUp;

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
        weaponInventory = new GameObject[2];
        //shotgun = PlayerControl.player.shotgun;
        playerEXPtoLvlUp = 100;
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

        if(playerEXP == playerEXPtoLvlUp)
        {
            Time.timeScale = 0f;
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
        Debug.Log("Game saved.");
        PlayerData data = new PlayerData();

        data.health = health;
        data.maxHealth = maxHealth;
        data.moveSpeed = moveSpeed;
        data.level1 = level1;
        data.pistolMagazineCapacity = pistolMagazineCapacity;
        data.pistolReloadTime = pistolReloadTime;
        data.pistolCurrentAmmo = pistolCurrentAmmo;
        data.shotgunBulletSpread = shotgunBulletSpread;
        data.shotgunMaxSpread = shotgunMaxSpread;
        data.shotgunMagazineCapacity = shotgunMagazineCapacity;
        data.shotgunReloadTime = shotgunReloadTime;
        data.shotgunCurrentAmmo = shotgunCurrentAmmo;
        // Couldn't get weapon loading working right now. The weapon GameObject is saved in JSON-file with InstanceId, but InstanceID
        // is different at every runtime so loading the weapon GameObject from the former InstanceID doesn't work.
        data.weaponInventory[0] = weaponInventory[0];
        data.weaponInventory[1] = weaponInventory[1];


        data.pistol = pistol;
        data.shotgun = shotgun;
        data.activeWeapon = activeWeapon;

        string jsonData = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/playerInfo.json", jsonData);
    }

    public void LoadGame()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.json"))
        {
            Debug.Log("Game loaded");
            path = Application.persistentDataPath + "/playerInfo.json";
            jsonString = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonString);

            health = data.health;
            maxHealth = data.maxHealth;
            moveSpeed = data.moveSpeed;
            level1 = data.level1;
            pistolMagazineCapacity = data.pistolMagazineCapacity;
            pistolReloadTime = data.pistolReloadTime;
            pistolCurrentAmmo = data.pistolCurrentAmmo;
            shotgunBulletSpread = data.shotgunBulletSpread;
            shotgunMaxSpread = data.shotgunMaxSpread;
            shotgunMagazineCapacity = data.shotgunMagazineCapacity;
            shotgunReloadTime = data.shotgunReloadTime;
            shotgunCurrentAmmo = data.shotgunCurrentAmmo;
            weaponInventory[0] = data.weaponInventory[0];
            weaponInventory[1] = data.weaponInventory[1];
            if (data.weaponInventory[1])
            {
                weaponInventory[1] = shotgun;
            }
            pistol = data.pistol;
            shotgun = data.shotgun;
            activeWeapon = data.activeWeapon;

            // Hard coded. I don't like this solution but for now it will do. Might make it smarter later if I have the time and energy.
            if(level1)
            {
                SceneManager.LoadScene("Level2");
            }
            else
            {
                SceneManager.LoadScene("Level1");
            }
        }
    }
}

[Serializable]
class PlayerData
{
    public int health;
    public int maxHealth;
    public float moveSpeed;
    public bool level1;
    public bool level2;
    public int pistolMagazineCapacity;
    public int pistolReloadTime;
    public int pistolCurrentAmmo;
    public float shotgunBulletSpread;
    public float shotgunMaxSpread;
    public int shotgunMagazineCapacity;
    public float shotgunReloadTime;
    public int shotgunCurrentAmmo;
    public GameObject[] weaponInventory = new GameObject[2];
    public GameObject pistol;
    public GameObject shotgun;
    public int activeWeapon;
    public int playerEXP;
}
