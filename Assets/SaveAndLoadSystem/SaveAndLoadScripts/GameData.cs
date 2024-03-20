using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    
    public long lastUpdated;
    public int deathCount;

    public Vector3 playerPosition;

    public GameObject activeScareCrow;
    public string activeScareCrowId;
    public Vector3 walkScareScrowPos;
    public Vector3 jumpScareCrowPos;
    public Vector3 crawlScareCrowPos;

    public int walkscareCrowState;
    public int jumpScareCrowState;
    public int crawlScareCrowState;
    public int gameState;

    public SerializableDictionary<string, bool> coinsCollected;



    //The values defined in this constructor will be the default values
    // the game starts with when there's no data to Load
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        coinsCollected = new SerializableDictionary<string, bool>();
        activeScareCrow = null;
        activeScareCrowId = null;
        walkScareScrowPos = new Vector3(34.59f, 0f, 7.09f);
        jumpScareCrowPos = Vector3.zero;
        crawlScareCrowPos = Vector3.zero;
        gameState = 4;
        walkscareCrowState = 0;
        jumpScareCrowState = 0;
        crawlScareCrowState = 0;
    }

    public int GetPercentageComplete()
    {
        int totalCollected = 0;

        foreach(bool collected in coinsCollected.Values)
        {
            if (collected)
            {
                totalCollected ++;
            }
        }

        //ensure we dont divide by 0 when calculating percentage
        int percentageCompleted = -1;
        if (coinsCollected.Count != 0)
        {
            percentageCompleted = (totalCollected * 100 / coinsCollected.Count);
        }
        return percentageCompleted;
    }


}
