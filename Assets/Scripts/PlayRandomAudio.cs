using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PlayRandomAudio : MonoBehaviour
{
    public List<AudioSource> sources;

    public void Play()
    {
        var source = sources[new Random().Next(sources.Count)];
        source.Play();
    }
}
