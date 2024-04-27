using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource interactSource;
    [Header("Inputs")]
    public InputActionAsset playerInputs;
    private InputAction interactionInput;

    public LayerMask interactLayer;
    public GameObject InteractUI;

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
        canInteract = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDis, interactLayer); // Bool is only true if raycast hits an interactable
        if (canInteract) InteractUI.SetActive(true);
        else InteractUI.SetActive(false);

        if (canInteract && interactionInput.WasPerformedThisFrame())
        {
            Debug.Log("Interacting");
            BaseInteraction objectInteract = hit.transform.gameObject.GetComponent<BaseInteraction>();
            objectInteract.Interact(); // activate the unique interaction of the interactable
            interactSource.Play();
        }
    }


}
