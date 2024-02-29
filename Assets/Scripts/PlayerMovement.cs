using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Input Reference")]
    public InputActionAsset playerInputs;
    private InputAction movementInput;
    private InputAction sprintInput;
    private InputAction crouchInput;

    [Header ("speeds")]
    public float speed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;

    [Header("Drag")]
    private float drag;
    public float walkDrag;
    public float sprintDrag;
    public float crouchDrag;

    public bool isCrouching;

    [Header("Sprinting")]
    private bool canSprint;
    private bool isSprinting = false;
    public float timeUntilRecharge;
    private bool isRecharging = false;
    private float sprintAmount;
    public float sprintMax;

    public CinemachineVirtualCamera playerCam;
    public Transform crouchPoint;
    public Transform standPoint;

    private Vector2 axis;
    private Vector3 moveDir;

    //private CharacterController playerController;

    private Rigidbody playerRb;


    private void Start()
    {
        sprintAmount = sprintMax;
        // playerController = gameObject.GetComponent<CharacterController>();
        playerRb = gameObject.GetComponent<Rigidbody>();

        playerInputs.FindActionMap("Player").Enable();
        movementInput = playerInputs.FindActionMap("Player").FindAction("Movement");
        sprintInput = playerInputs.FindActionMap("Player").FindAction("Sprint");
        crouchInput = playerInputs.FindActionMap("Player").FindAction("Crouch"); 
    }

    private void Update()
    {
        if (sprintAmount <= 0f)
        {
            canSprint = false;
        }
        Input();
        SpeedControl();
        if (isRecharging && !isSprinting)
        {
            sprintAmount += 1f;
            sprintAmount = Mathf.Clamp(sprintAmount, 0f, sprintMax);
            if (sprintAmount/sprintMax * 100f > 75f)
            {
                canSprint = true;
            }
            if (sprintAmount >= sprintMax)
            {
                isRecharging = false;
            }
        }
        Debug.Log(sprintAmount);


        
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Input()
    {
        axis = movementInput.ReadValue<Vector2>();
        

        if (sprintInput.IsInProgress() && canSprint)
        {
            
            if (movementInput.ReadValue<Vector2>().magnitude > 0)
            {
                isSprinting = true;
                isCrouching = false;
                sprintAmount -= 1f;
            }
            isRecharging = false;
            speed = sprintSpeed;
            drag = sprintDrag;
            sprintAmount = Mathf.Clamp(sprintAmount, 0f, sprintMax);
            
        }else
        {
            isSprinting = false;
            if (sprintAmount <= sprintMax)
            {
                Invoke(nameof(StartRecharge), timeUntilRecharge);
            }
            speed = walkSpeed;
            drag = walkDrag;

        }
        if (crouchInput.IsInProgress())
        {
            Crouch();
        }else
        {
            StandUp();
        }
        if (movementInput.ReadValue<Vector2>().magnitude == 0)
        {
            drag = 5f;
        }
        playerRb.drag = drag;
    }

    private void Crouch()
    {
        isCrouching = true;
        speed = crouchSpeed;
        drag = crouchDrag;

        playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, new Vector3(playerCam.transform.position.x, crouchPoint.position.y, playerCam.transform.position.z), Time.deltaTime * 10f);// Replace with animator

    }
    private void StandUp()
    {
        isCrouching = false;

        playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, new Vector3(playerCam.transform.position.x, standPoint.position.y, playerCam.transform.position.z), Time.deltaTime * 10f);//Replace with animator

    }
    private void MovePlayer()
    {
        moveDir = transform.forward * axis.y + transform.right * axis.x;

        playerRb.AddForce(moveDir.normalized * speed, ForceMode.Force);
    }


    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        //limit velocity

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            playerRb.velocity = new Vector3(limitedVel.x, playerRb.velocity.y, limitedVel.z);
        }
    }

    private void StartRecharge()
    {
        isRecharging = true;
    }

}
