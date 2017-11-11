using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(GunMove))]
[RequireComponent(typeof(PlayerWeapons))]
public class HUD : NetworkBehaviour
{
    float scrW, scrH;
    GunMove gun;
    PlayerWeapons weps;
    Player player;
    Player[] players;
    MainMenu menu;

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
        menu = FindObjectOfType<MainMenu>();
    }

    void OnGUI()
    {
        if (menu == null)
        {
            return;
        }
        if (!menu.showOptionsMenu)
        {
            scrH = Screen.height / 9;
            scrW = Screen.width / 16;
            if (isLocalPlayer)
            {
                //Show each player's health bar
                if (players != null)
                {
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

                GUI.Box(new Rect(scrW * 2, scrH * 6.5f, scrW, scrH * .5f), "Next/Prev");
                GUI.Box(new Rect(scrW * 1.5f, scrH * 6.5f, scrW * .5f, scrH * .5f), "Q");
                GUI.Box(new Rect(scrW * 3, scrH * 6.5f, scrW * .5f, scrH * .5f), "E");
                GUI.Box(new Rect(scrW * 2, scrH * 7, scrW, scrH), "");
                GUI.Box(new Rect(scrW * (weps.selectedWeapon), scrH * 7, scrW, scrH), "ShotGun");
                GUI.Box(new Rect(scrW * (weps.selectedWeapon + 1), scrH * 7, scrW, scrH), "Rifle");
                GUI.Box(new Rect(scrW * (weps.selectedWeapon + 2), scrH * 7, scrW, scrH), "Pistol");
                if (weps.curAmmo != null && weps.maxAmmo != null)
                {
                    GUI.Box(new Rect(scrW * (weps.selectedWeapon), scrH * 7.8f, scrW * ((float)weps.curAmmo[2] / (float)weps.maxAmmo[2]), scrH * 0.2f), "");
                    GUI.Box(new Rect(scrW * (weps.selectedWeapon + 1), scrH * 7.8f, scrW * ((float)weps.curAmmo[1] / (float)weps.maxAmmo[1]), scrH * 0.2f), "");
                    GUI.Box(new Rect(scrW * (weps.selectedWeapon + 2), scrH * 7.8f, scrW * ((float)weps.curAmmo[0] / (float)weps.maxAmmo[0]), scrH * 0.2f), "");
                }
            }

        }
    }


}