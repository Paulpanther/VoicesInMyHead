using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Face : MonoBehaviour
{
    public AudioSource repeating;
    public List<Transform> positions;
    public int lastTeleportPos = 0;
    public int teleportCount = 4;
    public Vector3 currentGoalPos;
    public Vector3 startPos;
    public float goalTime;
    public float startTime;
    public float animationTime = 1;
    public Transform parent;

    public void Start()
    {
        parent.GetComponent<SteamVR_TrackedObject>().enabled = false;
        parent.transform.position = positions[0].position;
        lastTeleportPos++;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (goalTime < Time.time && other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            if (lastTeleportPos < teleportCount)
            {
                var nextPos = positions[lastTeleportPos++];
                currentGoalPos = nextPos.position;
                goalTime = Time.time + animationTime;
                startTime = Time.time;
                startPos = parent.transform.position;
            }
            else
            {
                parent.GetComponent<SteamVR_TrackedObject>().enabled = true;
            }
        }
    }

    private void Update()
    {
        if (goalTime > Time.time)
        {
            var wholeTime = goalTime - startTime;
            var current = Time.time - startTime;
            var delta = current / wholeTime;
            parent.transform.position = Vector3.Lerp(startPos, currentGoalPos, delta);
        }
    }

    public void OnPutDown()
    {
        repeating.volume = 1;
    }

    public void OnPickUp()
    {
        repeating.volume = 0;
    }
}
