using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserEnemy : MonoBehaviour
{
    private GameObject player;
    public float speed = 0.1f;
    void Start()
    {
        player = GameObject.Find("Player");
        // player = GameObject.FindWithTag("Player");  
    }

    // Update is called once per frame
    void Update()
    {
        var step = speed * Time.deltaTime;
        Vector3.MoveTowards(transform.position, player.transform.position, step);
        Debug.DrawLine(transform.position, player.transform.position, Color.red);
    }
}
