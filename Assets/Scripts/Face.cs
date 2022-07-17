using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void OnTriggerEnter(Collider other)
    {
        if (goalTime < Time.time && other.CompareTag("LeftHand") || other.CompareTag("RightHand"))
        {
            if (lastTeleportPos < teleportCount)
            {
                var nextPos = positions[lastTeleportPos++];
                currentGoalPos = nextPos.position;
                goalTime = Time.time + animationTime;
                startPos = transform.position;
            }
        }
    }

    private void Update()
    {
        if (goalTime > Time.time)
        {
            var wholeTime = goalTime - startTime;
            var current = goalTime - Time.time;
            var delta = current / wholeTime;
            transform.position = Vector3.Lerp(startPos, currentGoalPos, delta);
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
