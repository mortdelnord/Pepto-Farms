using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendScareCrowProxy : MonoBehaviour
{
    public FriendScarecrow friendScarecrow;

    public void AnimationEvent()
    {
        friendScarecrow.doneInteractingEvent();
    }
}
