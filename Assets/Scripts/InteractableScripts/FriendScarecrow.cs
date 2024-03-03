using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendScarecrow : BaseInteraction
{

    public Animator scareCrowAnimator;
    public float animationTime;
    public GameObject ScavengerHuntStamp;



    public override void Interact()
    {
        
        Debug.Log("Animating");
        scareCrowAnimator.SetTrigger("Interact");
        Invoke(nameof(UpdateCanvas), animationTime);
    }



    private void UpdateCanvas()
    {
        Debug.Log("Done Animating");
        // ScavengerHuntStamp.SetActive(true);
        // ScavengerHuntStamp.transform.rotation = RandomRotation();
    }

    private Quaternion RandomRotation()
    {
        float zAngle = Random.Range(0f, 360f);
        Quaternion randRotate = Quaternion.Euler(0f, 0f, zAngle);
        return randRotate;
    }

}
