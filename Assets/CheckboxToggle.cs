using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckboxToggle : MonoBehaviour
{
    private bool isOn = false;
    private bool textShowing = false;

    public GameObject textPrefab;
    public Vector3 textOffset;
    private GameObject textPrefabInstance;

    void Update() {
        if (isOn) {
            if (textPrefab != null && !textShowing)
            {
                if (textOffset == null) {
                    textOffset = new Vector3(0, 0, 0);
                }
                textPrefabInstance = Instantiate(textPrefab, transform.position + textOffset, Quaternion.identity);
                // textPrefabInstance.GetComponent<TextMesh>().text = "Press E to interact";
                // textPrefabInstance.GetComponent<TMPro.TextMeshProUGUI>().text = "Press E to interact";
                textShowing = true;
            }
            if(Input.GetKeyDown("e"))
            {
                Debug.Log("pressed E in trigger zone");
            }
        }
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
            if (textPrefabInstance != null) 
            {
                Destroy(textPrefabInstance);
            }
            textShowing = false;
		}
	}
}
