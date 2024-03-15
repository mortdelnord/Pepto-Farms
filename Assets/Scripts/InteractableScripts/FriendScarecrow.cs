using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FriendScarecrow : BaseInteraction, IDataPersistence
{

    [SerializeField] private string id;

    [ContextMenu("Generate guid for id")]
    
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    public Animator scareCrowAnimator;
    public float animationTime;
    public GameObject ScavengerHuntStamp;
    

    private bool isCollected = false;

    private void Awake()
    {
        UpdateCanvas();
    }



    public override void Interact()
    {
        if (!isCollected)
        {
            isCollected = true;
            Debug.Log("Animating");
            scareCrowAnimator.SetTrigger("Interact");
            Invoke(nameof(UpdateCanvas), animationTime);

        }else
        {
            Debug.Log("has been collected");
            isCollected = false;
        }
    }



    private void UpdateCanvas()
    {
        if (isCollected)
        {
            Debug.Log("Done Animating");
            ScavengerHuntStamp.SetActive(true);
            ScavengerHuntStamp.transform.rotation = RandomRotation();

        }
    }

    private Quaternion RandomRotation()
    {
        float zAngle = Random.Range(0f, 360f);
        Quaternion randRotate = Quaternion.Euler(0f, 0f, zAngle);
        return randRotate;
    }

    public void LoadData(GameData data)
    {   
        data.coinsCollected.TryGetValue(id, out isCollected);
        UpdateCanvas();
    }

    public void SaveData(GameData data)
    {
        if (data.coinsCollected.ContainsKey(id))
        {
            data.coinsCollected.Remove(id);
        }
        data.coinsCollected.Add(id, isCollected);
    }

}