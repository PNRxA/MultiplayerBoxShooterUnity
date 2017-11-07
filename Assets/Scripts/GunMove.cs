using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunMove : NetworkBehaviour
{
    public Vector3 mouse_pos;
    public Transform target; //Assign to the object you want to rotate
    public Vector3 object_pos;
    public float angle;
    Rigidbody rigi;
    Light lit;
    LineRenderer lr;
    PlayerWeapons weps;

    public float damage = 1;
    public float fireRate = 0.5F;
    public float nextFire = 0.0F;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        lit = GetComponent<Light>();
        lr = GetComponent<LineRenderer>();
        weps = GetComponent<PlayerWeapons>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Only allow control to the local player
        if (isLocalPlayer)
        {
            //Move the gun around the player
            Move();
            //If there is ammo in the current gun then shoot it
            if (weps.curAmmo[weps.selectedWeapon] > 0 && !weps.reloading[weps.selectedWeapon])
            {
                //If using mouse, attack on mouse0 down
                if (Input.GetButton("Fire1") && Time.time > nextFire)
                {
                    CmdAttack();
                }
                //If using controller, attack on right trigger down
                if (Input.GetAxisRaw("Fire1") > 0 && Time.time > nextFire)
                {
                    CmdAttack();
                }
            }
        }
    }

    void Move()
    {
        //Check if there is a joystick connected
        string[] joysticks = Input.GetJoystickNames();
        if (joysticks.Length < 1)
        {
            //If there is no joystick, rotate using the mouse
            mouse_pos = Input.mousePosition;
            mouse_pos.z = 5.23f; //The distance between the camera and object
            object_pos = Camera.main.WorldToScreenPoint(target.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.x, mouse_pos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        else if (joysticks[0].Length < 10)
        {
            //If there is no joystick, rotate using the mouse
            mouse_pos = Input.mousePosition;
            mouse_pos.z = 5.23f; //The distance between the camera and object
            object_pos = Camera.main.WorldToScreenPoint(target.position);
            mouse_pos.x = mouse_pos.x - object_pos.x;
            mouse_pos.y = mouse_pos.y - object_pos.y;
            angle = Mathf.Atan2(mouse_pos.x, mouse_pos.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        else
        {
            //If there is a joystick then rotate using the controller
            float rotation = Mathf.Atan2(Input.GetAxis("RightH"), -Input.GetAxis("RightV")) * Mathf.Rad2Deg;

            if (Input.GetAxis("RightH") != 0 || Input.GetAxis("RightV") != 0)
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotation, transform.eulerAngles.z);
            }
        }

    }

    //Call the server command from the client
    [Command]
    void CmdAttack()
    {
        RpcAttack();
    }

    [ClientRpc]
    void RpcAttack()
    {
        //Determine what's in front of the gun
        Vector3 fwd = target.transform.TransformDirection(Vector3.forward);
        //Dubuggin ray to see where the real ray is going
        Debug.DrawRay(target.transform.position, fwd * 50, Color.green);
        Debug.DrawRay(target.transform.position, (Quaternion.Euler(0, -10, 0) * fwd) * 50);
        Debug.DrawRay(target.transform.position, (Quaternion.Euler(0, 10, 0) * fwd) * 50, Color.green);
        RaycastHit hit;


        Vector3[] points = new Vector3[3];
        //Cast ray for shotgun
        if (weps.selectedWeapon == 2)
        {

            if (Physics.Raycast(target.transform.position, fwd, out hit))
            {
                Debug.Log("hit.transform.name");
                //do something if hit object 
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<Enemy>().playerToCredit = gameObject.GetComponent<Player>();
                    hit.transform.gameObject.GetComponent<Enemy>().health -= damage;
                    Debug.Log("Close to enemy");

                }
                points[0] = hit.point;
                if (Physics.Raycast(target.transform.position, (Quaternion.Euler(0, -10, 0) * fwd), out hit))
                {
                    Debug.Log("hit.transform.name");
                    //do something if hit object 
                    if (hit.transform.tag == "Enemy")
                    {
                        hit.transform.gameObject.GetComponent<Enemy>().playerToCredit = gameObject.GetComponent<Player>();
                        hit.transform.gameObject.GetComponent<Enemy>().health -= damage;
                        Debug.Log("Close to enemy");

                    }
                    points[1] = hit.point;
                    if (Physics.Raycast(target.transform.position, (Quaternion.Euler(0, 10, 0) * fwd), out hit))
                    {
                        Debug.Log("hit.transform.name");
                        //do something if hit object 
                        if (hit.transform.tag == "Enemy")
                        {
                            hit.transform.gameObject.GetComponent<Enemy>().playerToCredit = gameObject.GetComponent<Player>();
                            hit.transform.gameObject.GetComponent<Enemy>().health -= damage;
                            Debug.Log("Close to enemy");

                        }
                        points[2] = hit.point;


                        StartCoroutine("DisableLR", points);
                        nextFire = Time.time + fireRate;
                    }
                }
            }
        }
        else
        {
            //Cast ray for normal guns
            if (Physics.Raycast(target.transform.position, fwd, out hit))
            {
                Debug.Log("hit.transform.name");
                //do something if hit object 
                if (hit.transform.tag == "Enemy")
                {
                    hit.transform.gameObject.GetComponent<Enemy>().playerToCredit = gameObject.GetComponent<Player>();
                    hit.transform.gameObject.GetComponent<Enemy>().health -= damage;
                    Debug.Log("Close to enemy");

                }
                points[0] = hit.point;

                StartCoroutine("DisableLR", points);

                nextFire = Time.time + fireRate;
            }
        }
    }

    //enable/disable lr and lit to look like gunfire
    IEnumerator DisableLR(Vector3[] point)
    {

        weps.curAmmo[weps.selectedWeapon] -= 1;
        lit.enabled = true;
        lr.numPositions = 2;
        lr.enabled = true;
        //If shotgun then render three shots at 45 degree angles, otherwise just render the one
        if (weps.selectedWeapon == 2)
        {
            lr.numPositions = 6;

            lr.SetPosition(0, target.position);
            lr.SetPosition(1, point[0]);
            lr.SetPosition(2, target.position);
            lr.SetPosition(3, point[1]);
            lr.SetPosition(4, target.position);
            lr.SetPosition(5, point[2]);
        }
        else
        {
            lr.SetPosition(0, target.position);
            lr.SetPosition(1, point[0]);
        }
        yield return new WaitForSeconds(.05f);
        lr.enabled = false;
        lit.enabled = false;
    }
}