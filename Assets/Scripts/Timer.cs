using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public float TimeLeft;
    public bool TimerOn = false;
    // public TMPRo.TextMeshPro TimerText;

    void Start()
    {
        TimerOn = true;        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerOn){
            if(TimeLeft > 0)
            {
                TimeLeft -= Time.deltaTime;
                UpdateTimer(TimeLeft);
            } else 
            {
                Debug.Log("Time is up");
                TimeLeft = 0;
                TimerOn = false;
            }
        } 
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        // TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
