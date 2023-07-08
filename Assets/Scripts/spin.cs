using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float speed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().angularVelocity = speed;
    }
}
