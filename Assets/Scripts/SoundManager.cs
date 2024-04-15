using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
   //public AudioClip sound;
   private AudioSource source;
   public AudioSource loopSource;

   private void Start()
   {
    source = gameObject.GetComponent<AudioSource>();
   }

   public void PlaySound(AudioClip sound)
   {
    if (sound != null)
    {
        source.PlayOneShot(sound);
    }
   }
   public void LoopSounc(AudioClip sound)
   {
    loopSource.clip = sound;
    loopSource.Play();
   }
}
