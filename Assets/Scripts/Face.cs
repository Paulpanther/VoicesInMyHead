using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public AudioSource repeating;

    public void OnPutDown()
    {
        repeating.volume = 1;
    }

    public void OnPickUp()
    {
        repeating.volume = 0;
    }
}
