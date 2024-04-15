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

    public Animator flashlightAnimator;
    public float animTime;
    private float timer = 0f;
    private bool isAnimating = false;

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
            timer = 0f;
            ActivateFlashlight();
        }
        if (isAnimating && !isOn)
        {
            timer += Time.deltaTime;
            if (timer >= animTime)
            {
                timer = 0f;
                isAnimating = false;
            }
        }
    }

    private void ActivateFlashlight()
    {
        if (isOn)
        {
            timer = 0f;
            flashlightAnimator.SetBool("IsOn", isOn);

        }else
        {
            timer = 0f;
            flashlightAnimator.SetBool("IsOn", isOn);
            isAnimating = false;
        }
    }

}
