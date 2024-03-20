using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    private bool isOn = false;
    
    public GameObject flashlight;

    public InputActionAsset playerInputs;
    private InputAction flashlightInput;

    private void Start()
    {
        playerInputs.FindActionMap("Player").Enable();
        flashlightInput = playerInputs.FindActionMap("Player").FindAction("Flashlight");
    }
    private void Update()
    {
        if (flashlightInput.WasPerformedThisFrame())
        {
            isOn = !isOn;
            if (isOn)
            {
                flashlight.SetActive(true);
            }else
            {
                flashlight.SetActive(false);
            }
        }
    }

}
