using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class ObjectGoalProgression : MonoBehaviour
{
    public Collider goal;

    public UnityEvent onGoalEnter;
    public UnityEvent onGoalExit;
    public UnityEvent onStartTowardsGoal;
    public UnityEvent on25TowardsGoal;
    public UnityEvent on50TowardsGoal;
    public UnityEvent on75TowardsGoal;
    
    public UnityEvent onStartAwayFromGoal;
    public UnityEvent on25AwayFromGoal;
    public UnityEvent on50AwayFromGoal;
    public UnityEvent on75AwayFromGoal;
    public UnityEvent tenSecondsIdle;
    
    private Vector3 startPos;

    private float lastRelDistanceCheckpoint;
    private Timer checkpointTimer = new Timer(1);
    private Timer startTimer = new Timer(1);

    private bool running = false;
    
    void Start()
    {
        startTimer.Start(() =>
        {
            startPos = transform.position;
            running = true;
            Debug.Log("Hey Hey");
        });
    }

    void Update()
    {
        startTimer.Update();
        if (!running) return;
        
        var wholeDistance = (goal.transform.position - startPos).magnitude;
        var currentDistance = (goal.transform.position - transform.position).magnitude;
        var relDistance = currentDistance / wholeDistance;

        // Forwards
        if (lastRelDistanceCheckpoint - relDistance > 0.25)
        {
            var checkpoint = Mathf.Round(relDistance * 4) / 4;
            if (checkpoint < 1.10 && checkpoint > 0.10)
            {
                lastRelDistanceCheckpoint = checkpoint;
                
                checkpointTimer.Start(() =>
                {
                    EventByDistance(checkpoint, true).Invoke();
                });
            }
        }
        // Backwards
        if (lastRelDistanceCheckpoint - relDistance < 0)
        {
            var checkpoint = Mathf.Round(relDistance * 4) / 4 + 0.25f;
            if (checkpoint < 1.10 && checkpoint > 0.10)
            {
                lastRelDistanceCheckpoint = checkpoint;

                checkpointTimer.Start(() =>
                {
                    EventByDistance(checkpoint, false).Invoke();
                });
            }
        }
        
        checkpointTimer.Update();
    }

    private UnityEvent EventByDistance(float distance, bool forward)
    {
        if (forward)
        {
            Debug.Log("Forward");
            if (distance <= 0.25) return on25TowardsGoal;
            if (distance <= 0.5) return on50TowardsGoal;
            if (distance <= 0.75) return on75TowardsGoal;
            if (distance <= 1) return onStartTowardsGoal;
            throw new Exception("Elegoo");
        }
        else
        {
            Debug.Log("Backwards");
            if (distance >= 1) return onStartAwayFromGoal;
            if (distance >= 0.75) return on75AwayFromGoal;
            if (distance >= 0.5) return on50AwayFromGoal;
            if (distance >= 0.25) return on25AwayFromGoal;
            throw new Exception("Elegoo");
        }
    }

    public void DebugPrintDistance()
    {
        Debug.Log(lastRelDistanceCheckpoint);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other == goal)
        {
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other == goal)
        {
            
        }
    }
}
