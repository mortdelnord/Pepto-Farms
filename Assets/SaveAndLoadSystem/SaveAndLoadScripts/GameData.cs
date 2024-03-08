using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    
    public long lastUpdated;
    public int deathCount;

    public Vector3 playerPosition;

    public SerializableDictionary<string, bool> coinsCollected;



    //The values defined in this constructor will be the default values
    // the game starts with when there's no data to Load
    public GameData()
    {
        this.deathCount = 0;
        playerPosition = Vector3.zero;
        coinsCollected = new SerializableDictionary<string, bool>();
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
