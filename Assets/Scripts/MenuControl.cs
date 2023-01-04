using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MenuControl : MonoBehaviour
{
    GameObject mainMenu;
    GameObject options;
    GameObject[] mainMenuButtons;
    GameObject[] optionsButtons;
    public GameObject musicSlider;
    public GameObject soundEffectSlider;

    private void Start()
    {
        mainMenu = GameObject.FindGameObjectWithTag("MainMenuButtons");
        options = GameObject.FindGameObjectWithTag("OptionsButtons");

        mainMenuButtons = (GameObject.FindGameObjectsWithTag("MainMenuButtons"));
        optionsButtons = (GameObject.FindGameObjectsWithTag("OptionsButtons"));

        foreach (GameObject gameObject in optionsButtons)
        {
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        GameManager.manager.musicVolume  = musicSlider.GetComponent<Slider>().value;
        GameManager.manager.soundEffectsVolume = soundEffectSlider.GetComponent<Slider>().value;
    }

    public void LoadMap()
    {
        SceneManager.LoadScene("Level1");
        GameManager.manager.GetComponent<AudioSource>().clip = GameManager.manager.inGame;
        GameManager.manager.GetComponent<AudioSource>().Play();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        foreach (GameObject gameObject in optionsButtons)
        {
            gameObject.SetActive(true);
        }


        foreach (GameObject gameObject in mainMenuButtons)
        {
            gameObject.SetActive(false);
        }

    }

    public void BackToMenu()
    {
        foreach (GameObject gameObject in mainMenuButtons)
        {
            gameObject.SetActive(true);
        }

        foreach (GameObject gameObject in optionsButtons)
        {
            gameObject.SetActive(false);
        }

    }

    public void SaveGame()
    {
        GameManager.manager.SaveGame();
    }

    public void LoadGame()
    {
        GameManager.manager.LoadGame();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
