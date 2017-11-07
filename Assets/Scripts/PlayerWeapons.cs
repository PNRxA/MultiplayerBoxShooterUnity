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
    public int[] maxAmmo = new int[3];
    public int[] curAmmo = new int[3];
    public bool[] reloading = new bool[3];

    public float reloadRate = 0.5F;
    public float nextReload = 0.0F;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        maxAmmo[0] = 10;
        maxAmmo[1] = 30;
        maxAmmo[2] = 5;

        //Set current ammo to max ammo and reloading status for all weapons to false
        for (int i = 0; i < curAmmo.Length; i++)
        {
            curAmmo[i] = maxAmmo[i];
            reloading[i] = false;
        }

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

    void FixedUpdate()
    {
        for (int x = 0; x < curAmmo.Length; x++)
        {
            if (curAmmo[x] <= 0)
            {
                reloading[x] = true;
            }
        }

        for (int i = 0; i < reloading.Length; i++)
        {
            if (reloading[i] && Time.time > nextReload)
            {
                Reload(i);
            }
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
                gun.damage = .1f;
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

    void Reload(int weapon)
    {
        if (curAmmo[weapon] < maxAmmo[weapon])
        {
            curAmmo[weapon]++;
        }
        else
        {
            reloading[weapon] = false;
        }
        nextReload = Time.time + reloadRate;
    }
}
