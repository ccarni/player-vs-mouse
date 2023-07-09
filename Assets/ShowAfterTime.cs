using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowAfterTime : MonoBehaviour
{
    public float timeToWait = 4f;
    void Start() {
        StartCoroutine(ShowSelf());
    }

    // Update is called once per frame
    IEnumerator ShowSelf() 
    {
        Debug.Log("SHOWING :");
        yield return new WaitForSeconds(timeToWait);
        gameObject.SetActive(true);
    }
}

