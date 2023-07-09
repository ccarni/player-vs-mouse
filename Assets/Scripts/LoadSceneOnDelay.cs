using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnDelay : MonoBehaviour
{
    public float timeToWait = 9f;
    void Start() {
        StartCoroutine(NextScene());
    }

    // Update is called once per frame
    IEnumerator NextScene() 
    {
        yield return new WaitForSeconds(timeToWait);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
