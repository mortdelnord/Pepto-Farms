using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour, IDataPersistence
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
    private Coroutine recharge;
    public float chargeRate;

    public Image sprintBar;
    public float sprintCost;
    private bool canSprint;
    public bool isSprinting = false;
    public float timeUntilRecharge;  
    private float sprintAmount;
    public float sprintMax;
    public bool isMoving = false;
    public CinemachineVirtualCamera playerCam;

    private CinemachineTransposer playerTransposer;
    public Transform crouchPoint;
    public Transform standPoint;

    private Vector2 axis;
    private Vector3 moveDir;

   

    private Rigidbody playerRb;


    private void Start()
    {
        playerTransposer = playerCam.GetCinemachineComponent<CinemachineTransposer>();
        sprintAmount = sprintMax;
        
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
        
        
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void Input()
    {
        axis = movementInput.ReadValue<Vector2>();
        if (movementInput.ReadValue<Vector2>() != Vector2.zero)
        {
            isMoving = true;
        }else
        {
            isMoving = false;
        }
        

        Sprint();

        if (crouchInput.IsInProgress())
        {
            Crouch();
        }else
        {
            StandUp();
        }
        if (movementInput.ReadValue<Vector2>() != Vector2.zero)
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

        //playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, new Vector3(playerCam.transform.position.x, crouchPoint.position.y, playerCam.transform.position.z), Time.deltaTime * 10f);// Replace with animator
        playerTransposer. m_FollowOffset.y = Mathf.Lerp(playerTransposer.m_FollowOffset.y, 0.8f, Time.deltaTime * 10f);
    }
    private void StandUp()
    {
        isCrouching = false;

        //playerCam.transform.position = Vector3.Lerp(playerCam.transform.position, new Vector3(playerCam.transform.position.x, standPoint.position.y, playerCam.transform.position.z), Time.deltaTime * 10f);//Replace with animator

        playerTransposer. m_FollowOffset.y = Mathf.Lerp(playerTransposer.m_FollowOffset.y, 1.5f, Time.deltaTime * 10f);
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

    
    private void Sprint()
    {
        if (sprintAmount > 0)
        {            
            canSprint = true;
        }else
        {
            canSprint = false;
        }
        //Debug.Log(isSprinting);
        if (sprintInput.IsInProgress() && canSprint)
        {
            if (isMoving)
            {
                
                isSprinting = true;
                isCrouching = false;
            }else
            {

                isSprinting = false;
            }
        }else if (sprintInput.IsInProgress() && !canSprint)
        {
            isSprinting = false;
        }
        if (!sprintInput.IsInProgress())
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            sprintAmount -= sprintCost * Time.deltaTime;

            if(sprintAmount < 0) sprintAmount = 0f;

            sprintBar.fillAmount = sprintAmount / sprintMax;
            
            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());

            speed = sprintSpeed;

        }else
        {
            speed = walkSpeed;          
        }
        
    }
   
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(timeUntilRecharge);
    
        while (sprintAmount < sprintMax)
        {
            sprintAmount += chargeRate / 10f;
            if (sprintAmount > sprintMax)
            {
                sprintAmount = sprintMax;
            }
            sprintBar.fillAmount = sprintAmount/sprintMax;
            yield return new WaitForSeconds(0.1f);
        }
    }


    public void LoadData(GameData data)
    {
        this.transform.position = data.playerPosition;
    }
    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.transform.position;
    }
}
