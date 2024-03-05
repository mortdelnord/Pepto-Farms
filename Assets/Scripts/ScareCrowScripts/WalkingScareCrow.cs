using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingScareCrow : ScareCrow
{
    
    // [Range(0.0f,180.0f)]
    // public float visionAngle;
    // public bool CanSee = false;
    
    // [Range(.1f, 50f)]
    // public float seeDis;

    // [Range(0f, 30f)]
    // public float huntTimerMax;

    // [Range(0f, 30f)]

    // public float investigateTimerMax;

    // [Range(0, 3)]
    // public int invesigateCheckMax = 3;
    // private int investigateChecks = 0;

    // [Range(0.1f, 20f)]
    // public float investigateRange;

    // [Range(10f, 100f)]
    // public float WanderRange;

    private Coroutine losePlayer = null;
    private Coroutine goToWander = null;
    public Animator scarecrowAnimator;

    //public Vector3 lastPlayerPos;

    // public enum State
    // {
    //     Idle,
    //     Wander,
    //     Hunt,
    //     Investigate
    // }

    // public State state;




    private void Update()
    {


        switch (state)
        {
            
            case State.Idle:
                Idle();
                break;
            case State.Wander:
                Wander();
                break;
            case State.Hunt:
                Hunt();
                break;
            case  State.Investigate:
                Investigate();
                break;
        }
        //Debug.Log(state);
        if (scarecrowAnimator.velocity.magnitude > 0)
        {
            scarecrowAnimator.SetBool("IsWalking", true);
        }else
        {
            scarecrowAnimator.SetBool("IsWalking", false);
        }

        Vector3 scareForward = transform.forward;

        Vector3 dirToPlayer = player.transform.position - transform.position;

        

        if (Physics.Raycast(transform.position, dirToPlayer, out RaycastHit hit, seeDis)) // If the player is in range and is not blocked by another object, then scarecrow can possibly see player
        {
            if (hit.transform.CompareTag("Player"))
            {
                CanSee = true;
            }else 
                CanSee = false;
        }

        Debug.DrawRay(transform.position, dirToPlayer, Color.red); // draw the ray cast and the forward position 
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        float leftDot = Vector3.Dot(scareForward, dirToPlayer);

        float rightDot = leftDot/(Vector3.Magnitude(scareForward)*Vector3.Magnitude(dirToPlayer));

        float vision = Mathf.Cos(Mathf.Deg2Rad * (visionAngle/2.0f));

        if (rightDot > vision && NavMesh.SamplePosition(player.transform.position, out NavMeshHit navHit, 0.1f, scareCrowNavAgent.areaMask) && CanSee) // if the player is within the field of view of the scarecrow and is on the scarecrows navmesh and can be seen 
        {
            //Debug.Log("Can see and is hunting");
            if (losePlayer != null) StopCoroutine(losePlayer); losePlayer = null; // stop coroutines of other states and sets those to null

            if (goToWander != null) StopCoroutine(goToWander); goToWander = null;
            
            state = State.Hunt; // go to state of hunting
        }else if (rightDot < vision || !NavMesh.SamplePosition(player.transform.position, out NavMeshHit playerHit, 0.1f, scareCrowNavAgent.areaMask) || !CanSee) // if the player is not a valid target in an way 
        {
            
            if (state == State.Hunt) // if the state is currently hunting
            {
                //Debug.Log("state is hunt");
                if (losePlayer == null) // if a coroutine isn't already going
                {
                    scareCrowNavAgent.ResetPath(); // reset current path 
                    //Debug.Log("starting move to investigate");
                    losePlayer = StartCoroutine(MoveToInvestigate()); // start coroutine
                }else Debug.Log("lose player is not null");
            }
        }
    }

    private void Idle()
    {
        return; // literally do nothing but sit 
    }
    private void Wander()
    {
        
        if (!scareCrowNavAgent.hasPath) // if the scarecrow doesn't have a path 
        {
            Vector3 point;
            if (RandomNavmeshPoint(transform.position, WanderRange, out point)) // get one centered around it at a larger radius 
            {
                NavMeshPath path = new NavMeshPath();
                if (scareCrowNavAgent.CalculatePath(point, path)) // make a new path
                {
                    scareCrowNavAgent.path = path;
                }                
                //Debug.Log(scareCrowNavAgent.path);
                //Debug.Log(scareCrowNavAgent.pathStatus);
            }
        }else if (scareCrowNavAgent.hasPath) // if scarecrow does have a path 
        {
            if (scareCrowNavAgent.remainingDistance < 0.5f) // if its almost there
            {
                Debug.Log("Reset path while wandering");
                scareCrowNavAgent.ResetPath(); // start the process again!               
            }
        }
        
    }
    private void Hunt()
    {
        NavMeshPath path = new NavMeshPath();
        if (scareCrowNavAgent.CalculatePath(player.transform.position, path)) // make a path toward player 
        {            
            scareCrowNavAgent.path = path;
            lastPlayerPos = scareCrowNavAgent.destination; // the last player location is that paths destination
        }
        
        //Debug.Log(scareCrowNavAgent.destination);
        //Debug.Log(scareCrowNavAgent.pathStatus);
        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f) // if your close, kill the player and go idle
        {
            //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
            KillPlayer();
            state = State.Idle;
        }

    }
    private void Investigate()
    {
        if (!scareCrowNavAgent.hasPath && investigateChecks < invesigateCheckMax) // if scarescrow doesn't have a path and hasn't investigated its max amount of points
        {
            //Debug.Log("no path so finding point");
            Vector3 point;
            if (RandomNavmeshPoint(lastPlayerPos, investigateRange, out point)) // Find a random point on the navMesh centered around the last known location of the player
            {
                //Debug.Log(point);
                NavMeshPath path = new NavMeshPath();
                if (scareCrowNavAgent.CalculatePath(point, path)) // make a new path towards that point 
                {
                    scareCrowNavAgent.path = path;
                }
                
            }else Debug.Log("failed to find point"); // code will loop if the point wasn't found but still investigating
        }else if (scareCrowNavAgent.hasPath && investigateChecks < invesigateCheckMax) // if we did find a point
        {
            //Debug.Log("Investigate there is a path");
            if (scareCrowNavAgent.remainingDistance < 0.5f) // If you've almost reached the point
            {
                //Debug.Log(" Investigate Reset path");
                scareCrowNavAgent.ResetPath(); // stop path 
                //Debug.Log(investigateChecks);
                investigateChecks ++; // congrats you've investigated a point, add it to the total
            }
        }
        if (investigateChecks >= invesigateCheckMax) // if your checks are maxxed out adn you haven't tried to wander yet 
        {
            Debug.Log("Investigation checks done");
            investigateChecks = 0;
            if (goToWander == null)
            {
                goToWander = StartCoroutine(MoveToWander()); // go wander the maze!

            }
        }
        
    }

    private IEnumerator MoveToInvestigate()
    {
        losePlayer = null;
        //Debug.Log("coroutine movetoinvestigate started");
        yield return new WaitForSeconds(huntTimerMax); // wait a little bit before investigating just in case screcrow and catch up 
        //Debug.Log("waited seconds");
        scareCrowNavAgent.ResetPath(); // reset the path 
        
        state = State.Investigate; // state goes to investigate
    }
    private IEnumerator MoveToWander()
    {
        goToWander = null;
        //Debug.Log("Move to wander started");
        yield return new WaitForSeconds(investigateTimerMax); // you know the gist by now
        //Debug.Log("waited seconds");
        scareCrowNavAgent.ResetPath();
        //scareCrowNavAgent.path = null;
        state = State.Wander;
    }

    
    private bool RandomNavmeshPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random. insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, scareCrowNavAgent.areaMask))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private void KillPlayer()
    {
        Debug.Log("KILL");
    }
}
