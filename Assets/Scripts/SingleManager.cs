using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SingleManager : NetworkManager
{
    // Use this for initialization
    void Start()
    {
        maxConnections = -1;
        // Just make sure match making is stopped
        if (matchMaker != null)
            StopMatchMaker();
        // Just make sure no server is running
        if (!IsClientConnected() && !NetworkServer.active && matchMaker == null)
            StartHost();

        
    }

    void Awake()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
