using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject activeScarecrow;



    public void ScareCrowAlert(Transform crowPoint)
    {
        Debug.Log(crowPoint, activeScarecrow);
    }







}
