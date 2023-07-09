using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StopMovingTooltip : MonoBehaviour
{
    public float idleTime;
    public float stoppedThreshold = 0.2f;
    public float xOffset = -1f;
    public float yOffset = 1f;
    private float idleTimer;
    private Rigidbody2D rb;
    public GameObject tooltipObject;
    public string popupText;
    private GameObject popup;
    // Start is called before the first frame update
    void Start()
    {
        idleTimer = idleTime;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.x) >= stoppedThreshold || Mathf.Abs(rb.velocity.y) >= stoppedThreshold * 10f)
        {
            idleTimer = idleTime;
            Destroy(popup);
        } else
        {
            idleTimer -= Time.deltaTime;
        }

        if (idleTimer <= 0)
        {
            if (popup == null)
            {
                popup = Instantiate(tooltipObject, transform.position, Quaternion.identity);
                popup.transform.Translate(Vector2.right * xOffset + Vector2.up * yOffset);
                popup.GetComponent<TextMeshPro>().text = popupText;
            }
        }
    }
}
