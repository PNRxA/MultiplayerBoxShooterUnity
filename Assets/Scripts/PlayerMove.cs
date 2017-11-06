using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerMove : NetworkBehaviour
{
    //Higher the value, the slower the player will move
    float speed = 15;

    Rigidbody rigi;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Only move if player is locally connected
        if (isLocalPlayer)
        {
            Move();
        }
    }

    void Move()
    {
        //Move transform based on axis
        transform.position += new Vector3(Input.GetAxisRaw("Horizontal") / speed, 0, Input.GetAxisRaw("Vertical") / speed);
    }
}
