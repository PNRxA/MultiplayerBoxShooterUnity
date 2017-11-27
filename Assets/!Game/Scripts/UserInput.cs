using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class UserInput : MonoBehaviour
{
    private PlayerController player;
    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        player.isRunning = Input.GetKey(KeyCode.LeftShift);

        float inputH = Input.GetAxis("Horizontal");
        float inputV = Input.GetAxis("Vertical");
        player.Move(inputH, inputV);
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }
    }
}
