using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    public string grabTag = "Letter";
    public float grabDistance;
    public RaycastHit2D[] lGrabHit;
    public RaycastHit2D[] rGrabHit;
    public float throwSpeed = 1500f;
    public float throwTorque = 30f;


    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        lGrabHit = Physics2D.RaycastAll(transform.position, Vector2.left, grabDistance);
        rGrabHit = Physics2D.RaycastAll(transform.position, Vector2.right, grabDistance);



        if (!Input.GetButtonDown("F")) return;

        foreach (var hit in lGrabHit)
        {
            if (hit.collider.tag == grabTag)
            { 
                hit.collider.GetComponent<Rigidbody2D>().AddForce(Vector2.up * throwSpeed);
                hit.collider.GetComponent<Rigidbody2D>().AddTorque(throwTorque);
            }
        }

        foreach (var hit in rGrabHit)
        {
            if (hit.collider.tag == grabTag)
            {
                hit.collider.GetComponent<Rigidbody2D>().AddForce(Vector2.up * throwSpeed);
                hit.collider.GetComponent<Rigidbody2D>().AddTorque(-throwTorque);
            }
        }
    }

    void GetInput()
    {
        
    }
}
