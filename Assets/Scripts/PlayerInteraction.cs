using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Inputs")]
    public InputActionAsset playerInputs;
    private InputAction interactionInput;

    public LayerMask interactLayer;

    private bool canInteract = false;
    private RaycastHit hit;

    [Range(0.1f, 10.0f)]
    public float interactDis;


    private void Start()
    {
        playerInputs.FindActionMap("Player").Enable();
        interactionInput = playerInputs.FindActionMap("Player").FindAction("Interact");
    }



    private void Update()
    {
        canInteract = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDis, interactLayer);

        if (canInteract && interactionInput.WasPerformedThisFrame())
        {
            Debug.Log("Interacting");
            BaseInteraction objectInteract = hit.transform.gameObject.GetComponent<BaseInteraction>();
            objectInteract.Interact();
        }
    }


}
