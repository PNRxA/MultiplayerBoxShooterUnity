using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    [SyncVar]
    public int health = 10;
    [SyncVar]
    public int score = 0;
    public bool gameOver = false;

    float scrH, scrW;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            scrH = Screen.height / 9;
            scrW = Screen.width / 16;
            if (health <= 0)
            {
                gameOver = true;
                GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "GAME OVER");
                if (GUI.Button(new Rect(scrW * 7.5f, scrH * 4.5f, scrW, scrH), "Quit"))
                {
                    Application.Quit();
                }
            }
        }
    }
}
