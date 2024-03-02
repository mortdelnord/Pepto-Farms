using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderScript : MonoBehaviour
{
    public InputActionAsset playerInputs;
    
    private InputAction movementInput;
    private PlayerMovement playerMovement;
    private Rigidbody playerRb;

    public bool isOnLadder = false;
    public float ladderSpeed;

    public LayerMask groundMask;
    private bool isOnGround = false;

    private void Start()
    {
        isOnLadder = false;
        playerInputs.FindActionMap("Player").Enable();
        movementInput = playerInputs.FindActionMap("Player").FindAction("Movement");
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerRb = gameObject.GetComponent<Rigidbody>();
    }

    // private void OnTriggerEnter(Collider collider)
    // {
    //     if (collider.CompareTag("Ladder"))
    //     {
    //         playerMovement.enabled = false;
    //         playerRb.useGravity = false;
    //         isOnLadder = true;
    //     }
    // }
    // private void OnTriggerExit(Collider collider)
    // {
    //     if (collider.CompareTag("Ladder"))
    //     {
    //         Debug.Log("LeaveLadder");
    //         playerMovement.enabled = true;
    //         playerRb.useGravity = true;
    //         isOnLadder = false;
    //     }
    // }

    private void Update()
    {
        if (isOnLadder)
        {
            Vector2 axis = movementInput.ReadValue<Vector2>();

            isOnGround = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);

            if (axis.y > 0)
            {
                playerRb.AddForce(Vector3.up * ladderSpeed, ForceMode.Force);
            }else if (axis.y < 0)
            {
                playerRb.AddForce(Vector3.down * ladderSpeed, ForceMode.Force);
                if (isOnGround)
                {
                    Debug.Log("Leave Ladder from being on ground");
                    isOnLadder = false;
                    playerMovement.enabled = true;
                }
            }
        }
    }


}
