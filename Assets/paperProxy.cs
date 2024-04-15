using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class paperProxy : MonoBehaviour
{
    public AudioSource paperSource;

    public void PaperFlap()
    {
        paperSource.Play();
    }

}
