using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingScareCrowProxy1 : MonoBehaviour
{
    public WalkingScareCrow scareCrow;
    public AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip runClip;
    public AudioClip jumpScareClip;

    public void StepEvent()
    {
        audioSource.PlayOneShot(walkClip);
    }

    public void JumpScareEvent()
    {
        audioSource.PlayOneShot(jumpScareClip);
    }
    public void DeathEvent()
    {
        Debug.Log("Aniamtion Killing");
        scareCrow.Killing();
    }
    public void RunEvent()
    {
        audioSource.PlayOneShot(runClip);
    }

}
