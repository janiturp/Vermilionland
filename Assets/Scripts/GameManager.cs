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
