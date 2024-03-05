using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ScareCrow : MonoBehaviour
{
   public NavMeshAgent scareCrowNavAgent;
   public GameObject player;

   public Vector3 lastPlayerPos;

   [Range(0.0f,180.0f)]
    public float visionAngle;
    public bool CanSee = false;
    
    [Range(.1f, 50f)]
    public float seeDis;

    [Range(0f, 30f)]
    public float huntTimerMax;

    [Range(0f, 30f)]

    public float investigateTimerMax;

    [Range(0, 3)]
    public int invesigateCheckMax = 3;
    public int investigateChecks = 0;

    [Range(0.1f, 20f)]
    public float investigateRange;

    [Range(10f, 100f)]
    public float WanderRange;

   public enum State
   {
      Idle,
      Wander,
      Hunt,
      Investigate
   }

   public State state;

   private void Awake()
   {
    scareCrowNavAgent = gameObject.GetComponent<NavMeshAgent>();
    player = GameObject.Find("Player");
   }



}
