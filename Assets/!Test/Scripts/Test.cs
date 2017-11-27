using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Test : MonoBehaviour
{
    public float checkDelay = 0;

    private float checkTimer = 0f;

    protected virtual void Simulate() { }
    protected virtual void Debug() { }
    protected abstract void Check();

    // Update is called once per frame
    void Update()
    {
        // Simulate the script
        Simulate();

        // Check on an interval (0 if once per frame)
        checkTimer += Time.deltaTime;
        if (checkTimer >= checkDelay)
        {
            Check();
        }

        // Perform debugging
        Debug();
    }
}
