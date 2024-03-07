using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingHedge : MonoBehaviour
{
    public Transform hedge;
    public Transform startPoint;
    public Transform endPoint;
    public float moveSpeed;

    private bool isMoving;
    private float timer = 0f;
    private float timerMax = 10f;



    private void Awake()
    {
        hedge.position = startPoint.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            hedge.position = Vector3.Lerp(hedge.position, endPoint.position, Time.deltaTime * moveSpeed);
            timer += Time.deltaTime;
            if (Mathf.Approximately(Vector3.Distance(hedge.position, endPoint.position), 0f) || timer >= timerMax)
            {
                isMoving = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            isMoving = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
