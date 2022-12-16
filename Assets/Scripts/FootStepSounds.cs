using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSounds : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private AudioSource audioSource;
    [SerializeField] private AudioClip footstep1;
    [SerializeField] private AudioClip footstep2;
    private Rigidbody2D rb;
    int randomFoot;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = GameManager.manager.soundEffectsVolume;

        if (rb.velocity.magnitude > 1f && !audioSource.isPlaying)
        {
            randomFoot = Random.Range(1, 2);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            if(randomFoot == 1)
            {
                audioSource.clip = footstep1;
            }
            else if(randomFoot == 2)
            {
                audioSource.clip = footstep2;
            }
            audioSource.Play();
        }
    }
}
