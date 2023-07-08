using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject thingToSpawn; 
    private GameObject thing;
    private Camera cam;
    private Vector3 mousePos;

    void Start() 
    {
        mousePos = Input.mousePosition;
        mousePos.z = 10f;

        cam = Camera.main;

        thing = Instantiate(thingToSpawn, cam.ScreenToWorldPoint(mousePos), Quaternion.identity);
    }

    void Update()
    {
        thing.GetComponent<Transform>().position = cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
