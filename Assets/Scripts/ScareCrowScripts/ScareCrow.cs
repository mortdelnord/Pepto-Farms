using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ScareCrow : MonoBehaviour
{
   public NavMeshAgent scareCrowNavAgent;
   public GameObject player;

   private void Awake()
   {
    scareCrowNavAgent = gameObject.GetComponent<NavMeshAgent>();
    player = GameObject.Find("Player");
   }


}
