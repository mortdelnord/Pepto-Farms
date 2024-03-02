using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderInteraction : BaseInteraction
{
    
    [Range(0.1f, 3f)]
    public float ladderGetOnSpeed;

    [Range(0.1f, 100f)]
    public float ladderDis;
    private bool isInteracting = false;
    private Rigidbody playerRigidbody;
    public Transform climbPoint;
    public Transform topPoint;
    public Transform leavePoint;
    public Transform closestPoint;
    private float timer = 0f;
    private float timerMax = 1f;


    public override void Interact()
    {
        isInteracting = !isInteracting;
        playerRigidbody = playerObject.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            playerRigidbody.useGravity = false;
            playerRigidbody.isKinematic = true;
        }
        playerObject.GetComponent<PlayerMovement>().enabled = false;

        closestPoint = FindClosestPoint();
        
    }

    private void Update()
    {
       // Debug.DrawLine(topPoint.position, new Vector3(topPoint.position.x, topPoint.position.y + ladderDis, topPoint.position.z), Color.blue);
        if (isInteracting && playerRigidbody != null)
        {
            timer += Time.deltaTime;
            playerObject.transform.position = Vector3.Lerp(playerObject.transform.position, closestPoint.position, Time.deltaTime * ladderGetOnSpeed);
            //playerRigidbody.Move(climbPoint.position, playerObject.transform.rotation);
            if (Mathf.Approximately(Vector3.Distance(playerObject.transform.position, climbPoint.position), 0f) || timer >= timerMax)
            {
                timer = 0f;
                //Debug.Log("Is At Climb Point");
                isInteracting = false;
                playerRigidbody.isKinematic = false;
                playerObject.GetComponent<LadderScript>().isOnLadder = true;

            }
        }

        if (playerObject.GetComponent<LadderScript>().isOnLadder)
        {
           // Debug.Log(Vector3.Distance(playerObject.transform.position, leavePoint.position));
            if (Vector3.Distance(playerObject.transform.position, leavePoint.position) <= 0.8f)
            {
                playerObject.GetComponent<PlayerMovement>().enabled = true;
                playerRigidbody.useGravity = true;
                playerObject.GetComponent<LadderScript>().isOnLadder = false;
            }
        }
    }

    private Transform FindClosestPoint()
    {
        float topDis = Vector3.Distance(playerObject.transform.position, topPoint.position);
        float bottmDis = Vector3.Distance(playerObject.transform.position, climbPoint.position);
        if (bottmDis == Mathf.Min(topDis, bottmDis))
        {
            return climbPoint;
        }else
        {
            return topPoint;
        }
    }
}
