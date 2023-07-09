using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckboxToggle : MonoBehaviour
{
    public GameObject minimapCanvas;
    public GameObject scroller;
    public float scrollerOffset;
    public enum CheckboxType {
        Jump, 
        Minimap, 
        DoubleJump,
        Scroller,
        Generic
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
    public CheckboxType checkboxType = CheckboxType.Generic;

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
                HandleInteraction();
            }
        }
    }

    private void HandleInteraction() {
        isChecked = !isChecked;
        if (isChecked && !(checkboxType == CheckboxType.Scroller))
        {
            GetComponent<SpriteRenderer>().sprite =  onSprite;
        } else {
            GetComponent<SpriteRenderer>().sprite =  offSprite;
        }

        if (checkboxType == CheckboxType.Jump) {
            FindObjectOfType<PlayerMovement>().enableJump = isChecked;
        } else if (checkboxType == CheckboxType.DoubleJump) {
            FindObjectOfType<PlayerMovement>().enableDoubleJump = isChecked;
        }

        else if (checkboxType == CheckboxType.Minimap)
        {
            minimapCanvas.SetActive(isChecked);
        }
        else if (checkboxType == CheckboxType.Scroller)
        {
            scroller.transform.position = transform.position + Vector3.up * scrollerOffset;
            scroller.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, scroller.GetComponent<Scroller1>().speed);
            FindObjectOfType<PlayerMovement>().transform.Translate(Vector3.up);
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
