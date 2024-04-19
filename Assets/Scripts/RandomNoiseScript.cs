using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoiseScript : MonoBehaviour
{
    public List<AudioClip> soundSlips;
    public AudioSource source;

    private float timer = 0f;
    private float timerMax = 5f;
    public void PlayNoise()
    {
        int index = Random.Range(0, soundSlips.Count-1);
        source.PlayOneShot(soundSlips[index]);
    }


    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > timerMax)
        {
            Destroy(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 5f);
    }
}
