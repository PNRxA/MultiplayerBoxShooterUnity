using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class Enemy : NetworkBehaviour
{
    public Player[] targets;
    NavMeshAgent agent;
    public Player closestPlayer;
    float closestDist = float.MaxValue;
    [SyncVar]
    public int health = 3;

    // Use this for initialization
    void Start()
    {

    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

    //Destroy enemy when there is no more health
    [ClientRpc]
    void RpcDie()
    {
        //Destroy on all clients and the server
        Destroy(gameObject);
    }
}
