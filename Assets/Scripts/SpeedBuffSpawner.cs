using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpeedBuffSpawner : MonoBehaviour
{
    [SerializeField] int spawnerWidth;
    [SerializeField] int spawnerHeight;
    int spawnChance;
    int randomNumber;
    [SerializeField] GameObject speedBuff;
    [SerializeField] float timer;
    [SerializeField] float timerTrigger;

    // Start is called before the first frame update
    void Start()
    {
        spawnChance = 25;
        timerTrigger = 5;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timerTrigger <= timer)
        {
            Debug.Log("Calculating speedbuff spawn.");
            randomNumber = Random.Range(0, 100);
            if (randomNumber <= spawnChance)
            {
                Debug.Log("Spawning speedbuff");
                Instantiate(speedBuff, transform.position, Quaternion.identity);

                transform.Translate(Random.Range(0f, 20f), Random.Range(0f, 20f), 0);
            }
            timerTrigger += 5;
        }
    }
}
