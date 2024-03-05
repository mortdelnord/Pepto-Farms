using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingScareCrow : ScareCrow
{
    
    [Range(0.0f,180.0f)]
    public float visionAngle;
    public bool InRange = false;
    
    [Range(.1f, 50f)]
    public float seeDis;

    [Range(0f, 30f)]
    public float huntTimerMax;

    //private float huntTimer;

    public float investigateTimerMax;
    private int investigateChecks = 0;
    public int invesigateCheckMax = 3;
    public float investigateRange;
    public float WanderRange;

    private bool isTurning = false;
    //public float spinSpeed = 10f;
    private Coroutine losePlayer;
    private Coroutine goToWander;

    private Vector3 lastPlayerPos;

    public enum State
    {
        Idle,
        Wander,
        Hunt,
        Investigate
    }

    public State state;




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
        

        Vector3 scareForward = transform.forward;

        Vector3 dirToPlayer = player.transform.position - transform.position;

        InRange = Physics.Raycast(transform.position, dirToPlayer, seeDis);
        Debug.DrawRay(transform.position, dirToPlayer, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.blue);

        float leftDot = Vector3.Dot(scareForward, dirToPlayer);

        float rightDot = leftDot/(Vector3.Magnitude(scareForward)*Vector3.Magnitude(dirToPlayer));

        float vision = Mathf.Cos(Mathf.Deg2Rad * (visionAngle/2.0f));

        if (rightDot > vision && NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, scareCrowNavAgent.areaMask) && InRange)
        {
            if (losePlayer != null) StopCoroutine(losePlayer);
            if (goToWander != null) StopCoroutine(goToWander);
            //scareCrowNavAgent.ResetPath();
            //Debug.Log("RessetPath before hunting");
            state = State.Hunt;
        }else
        {
            //Debug.Log("out of vision range in any way");
            if (state == State.Hunt)
            {
                Debug.Log("state is hunt");
                if (losePlayer == null)
                {
                    //scareCrowNavAgent.ResetPath();
                    Debug.Log("starting move to investigate");
                    losePlayer = StartCoroutine(MoveToInvestigate());
                }
            }
        }
    }

    private void Idle()
    {
        Debug.Log("Idle");
    }
    private void Wander()
    {
        
        if (!scareCrowNavAgent.hasPath && !isTurning)
        {
            Vector3 point;
            if (RandomNavmeshPoint(transform.position, WanderRange, out point))
            {
                NavMeshPath path = new NavMeshPath();
                if (scareCrowNavAgent.CalculatePath(point, path))
                {
                    scareCrowNavAgent.path = path;
                }
                //scareCrowNavAgent.SetDestination(point);
                Debug.Log(scareCrowNavAgent.path);
                Debug.Log(scareCrowNavAgent.pathStatus);
            }
        }else if (scareCrowNavAgent.hasPath && !isTurning)
        {
            if (scareCrowNavAgent.remainingDistance < 0.5f)
            {
                Debug.Log("Reset path while wandering");
                scareCrowNavAgent.ResetPath();
                //scareCrowNavAgent.path = null;
                
                //isTurning = true;
            }
        }
        // if (isTurning)
        // {
        //     Quaternion yRotation = Quaternion.Euler(transform.rotation.x, 180f, transform.rotation.z);
        //     transform.rotation = Quaternion.Lerp(transform.rotation, yRotation, Time.deltaTime * spinSpeed);
        //     if ( )
        // }
    }
    private void Hunt()
    {
        NavMeshPath path = new NavMeshPath();
        if (scareCrowNavAgent.CalculatePath(player.transform.position, path))
        {
            
            scareCrowNavAgent.path = path;
            lastPlayerPos = scareCrowNavAgent.destination;
        }
        //scareCrowNavAgent.SetDestination(player.transform.position);
        Debug.Log(scareCrowNavAgent.destination);
        Debug.Log(scareCrowNavAgent.pathStatus);
        if (Vector3.Distance(transform.position, player.transform.position) < 0.1f)
        {
            //Debug.Log(Vector3.Distance(transform.position, player.transform.position));
            KillPlayer();
            state = State.Idle;
        }

    }
    private void Investigate()
    {
        if (!scareCrowNavAgent.hasPath && investigateChecks < invesigateCheckMax)
        {
            Debug.Log("no path so finding point");
            Vector3 point;
            if (RandomNavmeshPoint(lastPlayerPos, investigateRange, out point))
            {
                Debug.Log(point);
                NavMeshPath path = new NavMeshPath();
                if (scareCrowNavAgent.CalculatePath(point, path))
                {
                    scareCrowNavAgent.path = path;
                }
                //scareCrowNavAgent.SetDestination(point);
                // bool cantSeePoint = Physics.Linecast(transform.position, point);
                // while (cantSeePoint)
                // {
                //     if (RandomNavmeshPoint(transform.position, investigateRange, out point))
                //     scareCrowNavAgent.SetDestination(point);
                // }
            }else Debug.Log("failed to find point");
        }else if (scareCrowNavAgent.hasPath && investigateChecks < invesigateCheckMax)
        {
            Debug.Log("Investigate there is a path");
            if (scareCrowNavAgent.remainingDistance < 0.5f)
            {
                Debug.Log(" Investigate Reset path");
                scareCrowNavAgent.ResetPath();
                //scareCrowNavAgent.path = null;
                Debug.Log(investigateChecks);
                investigateChecks ++;
            }
        }
        if (investigateChecks >= invesigateCheckMax)
        {
            Debug.Log("Investigation checks done");
            investigateChecks = 0;
            if (goToWander == null)
            {
                goToWander = StartCoroutine(MoveToWander());

            }
        }
        
    }

    private IEnumerator MoveToInvestigate()
    {
        losePlayer = null;
        Debug.Log("coroutine movetoinvestigate started");
        yield return new WaitForSeconds(huntTimerMax);
        Debug.Log("waited seconds");
        scareCrowNavAgent.ResetPath();
        
        state = State.Investigate;
    }
    private IEnumerator MoveToWander()
    {
        goToWander = null;
        Debug.Log("Move to wander started");
        yield return new WaitForSeconds(investigateTimerMax);
        Debug.Log("waited seconds");
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
