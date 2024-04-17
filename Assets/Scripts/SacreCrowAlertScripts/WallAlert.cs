using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAlert : MonoBehaviour
{
    private GameManager gameManager;
    AudioClip cornSlip;
    AudioSource source;
    private PlayerMovement playerMovement;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        source = gameObject.GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.ScareCrowAlert(playerMovement.transform);
            if (playerMovement.isMoving)
            {
                if (!source.isPlaying)
                {
                    source.Play();
                }
            }else
            {
                source.Stop();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source.Stop();
        }
    }
}
