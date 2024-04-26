using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScareCrowAmbientNoises : MonoBehaviour
{
    public AudioSource ambientSource;
    public List<AudioClip> audioClips;
    public float timeBetweenNoiseMin;
    public float timeBetweenNoiseMax;
    private float timerMax;
    private float timer = 0f;
    private bool canPlayNoise = true;


    private void Start()
    {
       timerMax =  RandomTime();
    }
    private int RandomNoise()
    {
        if (audioClips != null)
        {
            return Random.Range(0, audioClips.Count - 1);

        }else
        {
            return 0;
        }
    }
    private float RandomTime()
    {
        return Random.Range(timeBetweenNoiseMin, timeBetweenNoiseMax);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timerMax)
        {
            timer = 0f;
            timerMax = RandomTime();
            if (canPlayNoise)
            {
                canPlayNoise = false;
                PlaySound();
            }
        }
    }
    private void PlaySound()
    {       
        ambientSource.PlayOneShot(audioClips[RandomNoise()]);
        canPlayNoise = true;
    }
}
