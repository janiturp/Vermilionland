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
    public void LoadMap()
    {
        SceneManager.LoadScene("SampleScene");
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

    }

    public void LoadGame()
    {

    }
}
