using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    private UnityEngine.Object[] navlinkObjects;
    [ContextMenu ("Navmesh Link Components")]
    private void Generate()
    {
        navlinkObjects = FindObjectsOfType(typeof(NavMeshLinkResizer));
        foreach(var obj in navlinkObjects)
        {
            obj.GetComponent<NavMeshLinkResizer>().GenerateSize();
        }
    }
    public GameObject randomSoundObject;
    private float soundTimer = 0f;
    public float soundTimerMax = 2f;
    public Transform playerPos;
    public AudioSource gateSource;
    public GameObject activeScarecrow;
    public GameObject firstScarecrow;
    public GameObject ExitGate;
    private string activeScareCrowId;
    private GameObject[] arrayofScareCrows;

    public List<GameObject> stampsCollected;
    private int stampCollectedNum = 0;

    

    private int deathCount = 0;
    public bool isStarted = false;

    public enum State
    {
        GameStart = 4,
        ZeroStamp = 0,
        OneStamp = 1,
        TwoStamp = 2,
        EndGame = 3
    }

    public State state;

    private void Update()
    {
        if (state != State.GameStart)
        {
            Debug.Log(soundTimer);
            soundTimer += Time.deltaTime;
            if (soundTimer >= soundTimerMax)
            {
                Debug.Log("timer ended");
                soundTimerMax = UnityEngine.Random.Range(2f, 100f);
                soundTimer = 0f;
                Instantiate(randomSoundObject, RandomPointNearPlayer(), randomSoundObject.transform.rotation);
            }
        }
    }

    private void Start()
    {
        ActivateScareCrow();
    }

    public void UpdateGameState()
    {
        if (isStarted)
        {
            stampCollectedNum = 0;
            foreach (GameObject stamp in stampsCollected)
            {
                if (stamp.activeInHierarchy)
                {
                    stampCollectedNum ++;
                }
            }
            state = (State)stampCollectedNum;

        }else
        {
            state = State.GameStart;
        }
        GameState();
        

    }

    private void GameState()
    {
        if (state == State.GameStart)
        {
            Debug.Log("Game Has Started");
        }else if (state == State.ZeroStamp)
        {
            CloseGate();
            Debug.Log("The scarecrows are now active");
        }else if (state == State.OneStamp)
        {
            Debug.Log("One Stamp Collected");
        }else if (state == State.TwoStamp)
        {
            Debug.Log("Two Stamps Collected");
        }else if (state == State.EndGame)
        {
            Debug.Log("All Three STamps Collected");
            EndGame();
        }
        

    }

   
    private void EndGame()
    {

        ExitGate.SetActive(false);
        gateSource.Play();
    }
    private void CloseGate()
    {
        gateSource.Play();
        ExitGate.SetActive(true);
    }

    public void ScareCrowAlert(Transform crowPoint)
    {
        if (!isStarted)
        {
            isStarted = true;
            activeScarecrow = firstScarecrow;
            UpdateGameState();
        }
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
            ScareCrow scare = scarecrow.GetComponent<ScareCrow>();
            if (scare.id == activeScareCrowId)
            {
                activeScarecrow = scarecrow;
            }
        }
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

    public void KillPlayer()
    {
        Debug.Log("Game Manager killing player");
        PauseMenu.instance.isDead = true;
        PauseMenu.instance.isPaused = true;
        PauseMenu.instance.PauseGame();
        //DataPersistenceManager.instance.LoadGame();
    }

    public void LoadData(GameData data)
    {
        this.deathCount = data.deathCount;
        state = (State)data.gameState;
        stampCollectedNum = (int)state;
        activeScarecrow = data.activeScareCrow;
        activeScareCrowId = data.activeScareCrowId;
        ActivateScareCrow();
       // activeScarecrow.transform.position = data.scareScrowPos;

    }
    public void SaveData(GameData data)
    {
        data.deathCount = this.deathCount;
        data.gameState = (int)state;
        data.activeScareCrow = this.activeScarecrow;
        data.activeScareCrowId = activeScarecrow.GetComponent<ScareCrow>().id;
        
        //data.scareScrowPos = this.activeScarecrow.transform.position;
    }

    private Vector3 RandomPointNearPlayer()
    {
        Vector3 newPoint = playerPos.position + UnityEngine.Random.insideUnitSphere * 1;
        return newPoint;
    }


}
