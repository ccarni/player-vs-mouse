using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public AudioClip soundClip;
    private AudioSource audioSource;
    public void QuitGame() 
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
        if (audioSource != null)
        {
            audioSource.Play();
        }
        Application.Quit();
    }
}

