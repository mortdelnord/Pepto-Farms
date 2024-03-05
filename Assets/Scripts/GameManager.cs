using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject activeScarecrow;



    public void ScareCrowAlert(Transform crowPoint)
    {
        ScareCrow scareCrow = activeScarecrow.GetComponent<ScareCrow>();
        scareCrow.lastPlayerPos = crowPoint.position;
        scareCrow.state = ScareCrow.State.Investigate;

        Debug.Log(crowPoint, activeScarecrow);
    }







}
