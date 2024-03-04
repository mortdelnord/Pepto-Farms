using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkingScareCrow : ScareCrow
{
    
    [Range(0.0f,180.0f)]
    public float visionAngle;




    private void Update()
    {
        Vector3 scareForward = transform.forward;

        Vector3 dirToPlayer = player.transform.position - transform.position;

        float leftDot = Vector3.Dot(scareForward, dirToPlayer);

        float rightDot = leftDot/(Vector3.Magnitude(scareForward)*Vector3.Magnitude(dirToPlayer));

        float vision = Mathf.Cos(Mathf.Deg2Rad * (visionAngle/2.0f));

        if (rightDot > vision && NavMesh.SamplePosition(player.transform.position, out NavMeshHit hit, 0.1f, scareCrowNavAgent.areaMask))
        {
            scareCrowNavAgent.destination = hit.position;
        }
    }

}
