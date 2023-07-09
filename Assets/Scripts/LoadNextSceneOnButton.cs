using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneOnButton : MonoBehaviour
{
    public GameObject interactionChecker;

    // Update is called once per frame
    void Update()
    {
        if (interactionChecker.GetComponent<CheckboxToggle>().isChecked)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
