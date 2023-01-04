using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelTrigger : MonoBehaviour
{
    int nextSceneIndex;

    public bool cleared;
    public string levelToLoad;

    // Start is called before the first frame update
    void Start()
    {
        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
            {
                // Hard coded. I don't like this solution but for now it will do. Might make it smarter later if I have the time and energy.
                GameManager.manager.level1 = true;
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}
