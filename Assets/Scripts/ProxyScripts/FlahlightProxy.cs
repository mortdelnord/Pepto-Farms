using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlahlightProxy : MonoBehaviour
{
    public GameObject flashLight;
    public AudioSource source;
    public AudioClip flashLightClick;
    

    public void LightSwitch()
    {
        flashLight.SetActive(!flashLight.activeSelf);
        source.PlayOneShot(flashLightClick);
    }
}


