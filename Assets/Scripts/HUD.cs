using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(GunMove))]
[RequireComponent(typeof(PlayerWeapons))]
public class HUD : MonoBehaviour
{
    float scrW, scrH;
    GunMove gun;
    PlayerWeapons weps;
    Player player;
    Player[] players;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        players = FindObjectsOfType<Player>();
    }

    void Awake()
    {
        player = GetComponent<Player>();
        weps = GetComponent<PlayerWeapons>();
        gun = GetComponent<GunMove>();
    }

    void OnGUI()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;

        //Show each player's health bar
        for (int i = 1; i < players.Length + 1; i++)
        {
            if (players[i - 1] != null)
            {
                GUI.Box(new Rect(scrW, scrH * i, players[i - 1].health * scrW, scrH * .5f), "Player" + (i) + " Health");
            }
        }
        //Show each player's score
        for (int i = 1; i < players.Length + 1; i++)
        {
            if (players[i - 1] != null)
            {
                GUI.Box(new Rect(scrW, (scrH * i) + (scrH * .5f), scrW * 10, scrH * .5f), "Player" + (i) + " Score: " + players[i - 1].score);
            }
        }
    }
}
