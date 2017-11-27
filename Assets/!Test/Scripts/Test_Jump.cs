using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Jump : Test
{
    [Header("Test Parameters")]
    public float minHeight = 1f;

    private PlayerController player;
    private float originalY;
    private float jumpApex;

    // Use this for initialization
    void Start()
    {
        player = GetComponent<PlayerController>();
        originalY = transform.position.y;
    }

    protected override void Simulate()
    {
        // Simulate Jump
        player.Jump();
    }

    protected override void Check()
    {
		// Get the current Y pos
		float playerY = transform.position.y;
		float height = playerY - originalY;
		
		// Is the height over the jumpApex?
		if (height > jumpApex)
		{
			jumpApex = height;
		}

		// Is the jumpApex higher than minHeight
		if (jumpApex > minHeight)
		{
			IntegrationTest.Pass(gameObject);
		}
    }
}
