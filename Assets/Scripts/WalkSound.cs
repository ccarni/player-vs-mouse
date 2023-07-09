using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public PlayerMovement player;
    void Start() {
        player = GetComponent<PlayerMovement>();
    }
    void Update() 
    {
        if (player.grounded &&  player.velocity.magnitude > 2f && !audioSource.isPlaying )
        {
            Debug.Log("Playing sound");
            audioSource.Play();
        }
    }
}
