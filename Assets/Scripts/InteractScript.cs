using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InteractScript : MonoBehaviour
{
    public enum InteractionType {  WinGame, QuitGame };

    public AudioClip soundClip;
    private AudioSource audioSource;
    public VideoPlayer videoPlayer;
    public InteractionType interactionType;
    public VideoClip video;
    private bool isChecked = false;
    private bool isOn = false;

    public KeyCode interactionButton = KeyCode.E;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundClip;
    }

    public void WinGameFn()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        GameObject camera = GameObject.Find("Main Camera");

        if (videoPlayer != null)
        {
            Debug.Log("doing stuff");
            videoPlayer.playOnAwake = false;
            videoPlayer.clip = video;
            videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
            videoPlayer.loopPointReached += EndReached;
            videoPlayer.SetDirectAudioVolume(0, 0.04f);
            videoPlayer.Play();
        }
    }

    void Update() {
        if (isOn && Input.GetKeyDown(interactionButton)) {

            if (interactionType == InteractionType.WinGame) {
                WinGameFn();
            } else if (interactionType == InteractionType.QuitGame) {
                Application.Quit();
            }
        }
    }

    private void EndReached(VideoPlayer vp)
    {
        Application.Quit();
    }

   void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isOn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			isOn = false;
		}
	}
}

