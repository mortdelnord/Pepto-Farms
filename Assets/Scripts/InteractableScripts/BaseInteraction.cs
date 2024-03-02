using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInteraction : MonoBehaviour
{
    public GameObject playerObject;


    private void Awake()
    {
        playerObject = GameObject.Find("Player");
    }
    public abstract void Interact();
}
