using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckboxToggle : MonoBehaviour
{
    public enum CheckboxType {
        Jump, 
        Minimap, 
        DoubleJump
    }

    private bool isOn = false;
    private bool textShowing = false;
    public bool isChecked = false;

    public KeyCode interactionButton = KeyCode.E;
    public string name = "Press E to Interact";

    public GameObject textPrefab;
    public Vector3 textOffset;
    private GameObject textPrefabInstance;
    public Sprite onSprite;
    public Sprite offSprite;

    [SerializeField]
    public CheckboxType checkboxType;

    void Start() {
        if (isChecked)
        {
            GetComponent<SpriteRenderer>().sprite =  onSprite;
        } else {
            GetComponent<SpriteRenderer>().sprite =  offSprite;
        }
    }

    void Update() {
        if (isOn) {
            if (textPrefab != null && !textShowing)
            {
                if (textOffset == null) {
                    textOffset = new Vector3(0, 0, 0);
                }
                textPrefabInstance = Instantiate(textPrefab, transform.position + textOffset, Quaternion.identity);
                textPrefabInstance.GetComponent<TextMeshPro>().text = name;
                textShowing = true;
            }
            if(Input.GetKeyDown(interactionButton))
            {
                FindObjectOfType<PlayerMovement>().enableJump = true;
                HandleInteraction();
            }
        }
    }

    private void HandleInteraction() {
        isChecked = !isChecked;
        if (isChecked)
        {
            GetComponent<SpriteRenderer>().sprite =  onSprite;
        } else {
            GetComponent<SpriteRenderer>().sprite =  offSprite;
        }

        if (checkboxType == CheckboxType.Jump) {
            FindObjectOfType<PlayerMovement>().canJump = isChecked;
        } else if (checkboxType == CheckboxType.DoubleJump) {
            FindObjectOfType<PlayerMovement>().canDoubleJump = isChecked;
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