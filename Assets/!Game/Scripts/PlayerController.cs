using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 20f;
    public float runSpeed = 30f;
    public float jumpHeight = 10f;
    public float jumpTime = 1f;
    public AnimationCurve jumpCurve = new AnimationCurve(new Keyframe[3]{
        new Keyframe(0f  ,0f),
        new Keyframe(0.5f,1f),
        new Keyframe(1f  ,0f)
    });

    public bool isRunning = false;
    public bool isJumping = false;

    [HideInInspector] public Vector3 velocity;

    public bool isGrounded
    {
        get { return controller.isGrounded; }
    }

    public delegate void JumpCallback();
    public JumpCallback jumpCallback;

    public delegate void MoveCallback(float inputH, float inputV);
    public MoveCallback moveCallback;
    
    private CharacterController controller;
    private bool jump = false;
    
    private Vector3 gravity;
    private float time = 0f;
    Vector3 Gravity
    {

        get
        {
            if(isGrounded)
            {
                if(isJumping && time <= 0)
                {
                    isJumping = false;
                    time = jumpTime;
                }
                gravity = Vector3.zero;
            }
            else
            {
                gravity += Physics.gravity * Time.deltaTime;
            }

            //Jump time
            if (time > 0f)
            {

                float height = jumpCurve.Evaluate((jumpTime - time) / jumpTime);
                time -= Time.deltaTime;
                return (Vector3.up
                        * (jumpCurve.Evaluate((jumpTime - time) / jumpTime) - height)
                        * jumpHeight);

            }

            return gravity * Time.deltaTime;

        }

    }

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Update()
    {
        velocity += Gravity;

        controller.Move(velocity);
    }

    public void Jump()
    {
        velocity *= Time.deltaTime;
        
        if (isGrounded && !isJumping)
        {
            isJumping = true;
            jumpCallback.Invoke();
        }
    }

    public void Move(float inputH, float inputV)
    {
        Vector3 inputDir = new Vector3(inputH, 0, inputV);
        velocity = transform.TransformDirection(inputDir);
        
        if (isRunning)
        {
            velocity *= runSpeed * Time.deltaTime;
        }
        else
        {
            velocity *= walkSpeed * Time.deltaTime;
        }

        moveCallback.Invoke(inputH, inputV);
    }
}
