using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject thingToSpawn; 
    private Vector3 mousePos;
    private Object thing;

    void Start() 
    {
        mousePos = Input.mousePosition;
        thing = Instantiate(thingToSpawn, mousePos, Quaternion.identity);
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        // thing.transform.position = mousePos;
    }
}
