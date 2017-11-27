using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class SkeletonArcherAnim : MonoBehaviour
{
    public Animator anim;
    public float smoothness = 10f;
    public bool skipSpawn = false;

    private PlayerController player;
    private UserInput userInput;

    private bool EnableUserInput
    {
        set
        {
            if (userInput != null)
            {
                userInput.enabled = value;
            }
        }
    }

    void Start()
    {
        player = GetComponent<PlayerController>();
        userInput = GetComponent<UserInput>();

        player.moveCallback += OnMove;
        player.jumpCallback += OnJump;

        if (skipSpawn)
        {
            anim.SetTrigger("SkipSpawn");
        }

        EnableUserInput = false;
    }

    void Update()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName("Spawn"))
        {
            EnableUserInput = true;

            float moveSpeed = player.velocity.magnitude;
            anim.SetFloat("MoveSpeed", moveSpeed);

            anim.SetBool("IsGrounded", player.isGrounded);
        }
        else
        {
            EnableUserInput = false;
        }
    }

    void OnMove(float inputH, float inputV)
    {
        Vector3 vel = player.velocity;
        vel.y = 0f;
        if (vel.magnitude > 0)
        {
            Quaternion rotation = anim.transform.rotation;
            rotation = Quaternion.Lerp(rotation, Quaternion.LookRotation(vel), smoothness);
            anim.transform.rotation = rotation;
        }

        anim.SetBool("IsRunning", player.isRunning);
    }

    void OnJump()
    {
        anim.SetTrigger("Jump");
    }
}
