using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    
    public GameObject activeScarecrow;
    private GameObject[] arrayofScareCrows;

    

    private int deathCount = 0;

    public enum State
    {
        GameStart,
        OneStamp,
        TwoStamp,
        EndGame
    }

    public State state;

    private void Update()
    {

    }


    

    public void ScareCrowAlert(Transform crowPoint)
    {
        ActivateScareCrow();
        ScareCrow scareCrow = activeScarecrow.GetComponent<ScareCrow>();
        scareCrow.lastPlayerPos = crowPoint.position;
        scareCrow.state = ScareCrow.State.Investigate;

        Debug.Log(crowPoint, activeScarecrow);
    }


    public void ActivateScareCrow()
    {
        arrayofScareCrows = GameObject.FindGameObjectsWithTag("ScareCrow");
        Debug.Log(arrayofScareCrows);
        foreach (GameObject scarecrow in arrayofScareCrows)
        {
            if (scarecrow == activeScarecrow)
            {
                scarecrow.GetComponent<ScareCrow>().enabled = true;
                scarecrow.GetComponent<NavMeshAgent>().ResetPath();
            }else
            {
                scarecrow.GetComponent<ScareCrow>().enabled = false;
                scarecrow.GetComponent<NavMeshAgent>().ResetPath();
            }
        }
    }

    public void UpdateDeathCount()
    {
        deathCount ++;
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
        state = (State)data.gameState;
        activeScarecrow = data.activeScareCrow;
        ActivateScareCrow();
       // activeScarecrow.transform.position = data.scareScrowPos;

    }
    public void SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
        data.gameState = (int)state;
        data.activeScareCrow = this.activeScarecrow;
        //data.scareScrowPos = this.activeScarecrow.transform.position;
    }




}
