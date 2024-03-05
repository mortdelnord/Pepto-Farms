using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBoardCutOut : MonoBehaviour
{
    public GameObject cutOutObject;

    private bool isActive = false;
    public float timerMax;
    private float timer = 0f;

    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= timerMax)
            {
                isActive = false; 
                cutOutObject.SetActive(false); 
                timer = 0f;
            }

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collided");
        if (collider.CompareTag("Player"))
        {
            Debug.Log("Hit Player");
            cutOutObject.SetActive(true);
            isActive = true;
        }
    }
}
