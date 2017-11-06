using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerMove))]
[RequireComponent(typeof(GunMove))]
public class PlayerWeapons : MonoBehaviour
{
    GunMove gun;
    public int selectedWeapon;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        gun = GetComponent<GunMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //Switch weapon up with e and down with q
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWeapon(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon(false);
        }
    }

    //Switch weapon up(true) or down(false) based on bool
    void SwitchWeapon(bool up)
    {
        if (selectedWeapon < 2 && up)
            selectedWeapon++;
        else if (selectedWeapon > 0 && !up)
            selectedWeapon--;
        else if (selectedWeapon == 2 && up)
            selectedWeapon = 0;
        else if (selectedWeapon == 0 && !up)
            selectedWeapon = 2;

        switch (selectedWeapon)
        {
            case 0:
                Debug.Log("0");
				gun.fireRate = .5f;
				gun.damage = 1;
                break;
            case 1:
                Debug.Log("1");
				gun.fireRate = .1f;
				gun.damage = .01f;
                break;
            case 2:
                Debug.Log("2");
				gun.fireRate = 3;
				gun.damage = 3;
                break;
            default:
                Debug.Log("Default case");
                break;
        }
    }
}
