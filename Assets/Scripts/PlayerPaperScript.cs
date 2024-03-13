using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPaperScript : MonoBehaviour
{
    [Header("Player Input")]
    public InputActionAsset playerInputs;
    private InputAction paperInput;

    [Header("Animator and Game Object Ref")]
    public GameObject handObject;

    public Animator handAnimator;

    private bool isLookingAtPaper = false;

    public float animTime;
    private float timer = 0f;
    private bool isAnimating = false;

    private void Start()
    {
        playerInputs.FindActionMap("Player").Enable();
        paperInput = playerInputs.FindActionMap("Player").FindAction("Paper");
    }

    private void Update()
    {
        if (paperInput.WasPerformedThisFrame())
        {
            isLookingAtPaper = !isLookingAtPaper;
            timer = 0f;
            ActivateHand();
        }
        if (isAnimating && !isLookingAtPaper)
        {
            timer += Time.deltaTime;
            if (timer >= animTime)
            {
                timer = 0f;
                isAnimating = false;
                //handObject.SetActive(false);
            }
        }

    }

    private void ActivateHand()
    {
        if (isLookingAtPaper)
        {
            timer = 0f;
            //handObject.SetActive(true);
            handAnimator.SetBool("IsLookingAtPaper", isLookingAtPaper);
        }else
        {
            timer = 0f;
            handAnimator.SetBool("IsLookingAtPaper", isLookingAtPaper);
            isAnimating = true;
        }
    }
}
