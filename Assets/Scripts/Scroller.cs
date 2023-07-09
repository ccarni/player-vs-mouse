using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollbar : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, Random.Range(-1, 1) * speed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
