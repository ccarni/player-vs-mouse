using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{
    public GameObject thingToSpawn; 
    private GameObject thing;
    private float cameraZ;
    private float thrust = 20f;

    void Start() 
    {
        thing = Instantiate(thingToSpawn, Vector3.zero, Quaternion.identity);
        cameraZ = Camera.main.transform.position.z;
    }

    void Update()
    {
        thing.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Vector3.forward * cameraZ;
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.name == "Player")
        {
            //If the GameObject's name matches the one you suggest, output this message in the console
            Debug.Log("Do something here");
        }

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Player")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Do something else here");
        }
    }
}