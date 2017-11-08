using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPoints;

    int spawnPoint = 0;
    float spawnRate = 1F;
    float nextSpawn = 0.0F;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isServer)
        {
            if (Time.time > nextSpawn)
            {
                Spawn();
            }
        }
    }

    // [Command]
    // void CmdSpawn()
    // {
    //     RpcSpawn();
    // }

    void Spawn()
    {
        if (spawnPoint < enemySpawnPoints.Length - 1)
            spawnPoint++;
        else
            spawnPoint = 0;
        GameObject enemy = (GameObject)Instantiate(enemyPrefab, enemySpawnPoints[spawnPoint].position, enemySpawnPoints[spawnPoint].rotation);
        NetworkServer.Spawn(enemy);
        //Network.Instantiate(enemyPrefab, enemySpawnPoints[spawnPoint].position, enemySpawnPoints[spawnPoint].rotation, 0);
        nextSpawn = Time.time + spawnRate;
    }
}
