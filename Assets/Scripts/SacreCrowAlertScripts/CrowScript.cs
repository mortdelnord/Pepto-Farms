using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowScript : MonoBehaviour
{

    private bool inZone = false;
    private GameManager gameManager;
    private PlayerMovement playerMovement;
    public Transform alertPoint;
    
    public Animator crowAnimator;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {

            inZone = true;
            
            //alertPoint.position = collider.transform.position;

        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {

            inZone = false;
        }
    }

    private void Update()
    {
        if (inZone)
        {
            if (!playerMovement.isCrouching && playerMovement.isMoving)
            {
                
                crowAnimator.SetTrigger("Alert");
                alertPoint.position = playerMovement.transform.position;
                gameManager.ScareCrowAlert(alertPoint);
                inZone = false;
            }
        }
    }
}
