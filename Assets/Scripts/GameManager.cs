using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public GameObject activeScarecrow;



    public void CrowAlert(Transform crowPoint)
    {
        Debug.Log(crowPoint, activeScarecrow);
    }







}
