using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System;

public class Enemy : NetworkBehaviour
{
    public Player[] targets;
    NavMeshAgent agent;
    public Player closestPlayer;
    float closestDist = float.MaxValue;
    [SyncVar]
    public float health = 3;
    float scrW, scrH;
    public Player playerToCredit;
    public float newDmg, oldDmg;

    public bool ShowDamageBox;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        newDmg = health;
        oldDmg = health;
    }

    void Update()
    {
        if (health <= 0)
        {
            RpcDie();
        }

        // If agent has reached the player
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    if (closestPlayer != null)
                    {
                        closestPlayer.health--;
                        RpcDie();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        UpdateTarget();
    }

    void UpdateTarget()
    {
        targets = FindObjectsOfType<Player>();
        //Check which is the closest player and set that as the target to move towards
        foreach (var player in targets)
        {
            //Determine closest player to follow
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distToPlayer < closestDist)
            {
                closestDist = distToPlayer;
                closestPlayer = player;
            }
        }
        //Move towards target
        agent.destination = closestPlayer.transform.position;
    }

    void OnGUI()
    {
        scrH = Screen.height / 9;
        scrW = Screen.width / 16;
        Vector3 wantedPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        GUI.Box(new Rect(wantedPos.x + (scrW * -.75f), Screen.height - wantedPos.y - (scrH * .3f), health * (scrW * .5f), scrH * .2f), "");

        if (oldDmg != health)
        {
            newDmg = oldDmg - health;
            oldDmg = health;
            StartCoroutine("ShowDamage");
        }
        if (ShowDamageBox)
        {
            GUI.Box(new Rect(wantedPos.x + (scrW * -.75f), Screen.height - wantedPos.y - (scrH * .3f), scrW * .5f, scrH * .5f), Math.Round(newDmg, 1).ToString());
        }
    }

    IEnumerator ShowDamage()
    {
        ShowDamageBox = true;
        yield return new WaitForSeconds(.08f);
        ShowDamageBox = false;
    }

    //Destroy enemy when there is no more health
    [ClientRpc]
    void RpcDie()
    {
        if (playerToCredit != null)
        {
            playerToCredit.score++;
        }
        //Destroy on all clients and the server
        Destroy(gameObject);
        NetworkServer.UnSpawn(gameObject);
    }
}
