using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ZoneEntrance : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject zoneScarecrow;



    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }



    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            gameManager.activeScarecrow = zoneScarecrow;
            gameManager.ActivateScareCrow();

        }
    }
}
