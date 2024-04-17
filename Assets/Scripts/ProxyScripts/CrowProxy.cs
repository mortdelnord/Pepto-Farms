using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CrowProxy : MonoBehaviour
{
    public AudioSource source;
    public AudioClip crowAlertSound;
    public void CrowNoise()
    {
        source.PlayOneShot(crowAlertSound);
    }
}
