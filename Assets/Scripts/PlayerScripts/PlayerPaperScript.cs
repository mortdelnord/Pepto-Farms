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
    [Header("Notes")]
    public GameObject noteNotif;
    public GameObject noteUI;

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
            ActivateHand(isLookingAtPaper);
        }
        if (isAnimating && !isLookingAtPaper)
        {
            timer += Time.deltaTime;
            if (timer >= animTime)
            {
                timer = 0f;
                isAnimating = false;
                if (noteUI.activeSelf || noteNotif.activeSelf)
                {
                    noteNotif.SetActive(false);
                    noteUI.SetActive(false);
                }
                //handObject.SetActive(false);
            }
        }

    }

    public void ActivateHand(bool isLooking)
    {
        isLookingAtPaper = isLooking;
        if (isLooking)
        {
            timer = 0f;
            //handObject.SetActive(true);
            handAnimator.SetBool("IsLookingAtPaper", isLooking);
        }else
        {
            timer = 0f;
            handAnimator.SetBool("IsLookingAtPaper", isLooking);
            isAnimating = true;
        }
    }
}
