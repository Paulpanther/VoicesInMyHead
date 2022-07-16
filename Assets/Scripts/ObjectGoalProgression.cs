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

    public bool inGoal = false;
    
    private Vector3 startPos;

    public float lastRelDistanceCheckpoint;
    public float relDistance;
    public String lastEvent = "None";

    public TimePassedDetector timePassedDetector = null;
    
    private Timer checkpointTimer = new Timer(1);
    private Timer inGoalTimer = new Timer(1);
    private Timer startTimer = new Timer(1);

    private bool running = false;
    
    void Start()
    {
        timePassedDetector = GetComponent<TimePassedDetector>();
        startTimer.Start(() =>
        {
            startPos = transform.position;
            running = true;
        });
    }

    void Update()
    {
        startTimer.Update();
        if (!running) return;
        
        var wholeDistance = (goal.transform.position - startPos).magnitude;
        var currentDistance = (goal.transform.position - transform.position).magnitude;
        relDistance = currentDistance / wholeDistance;

        // Forwards
        if (lastRelDistanceCheckpoint - relDistance > 0.25)
        {
            var checkpoint = Mathf.Round(relDistance * 4) / 4;
            if (checkpoint < 1.10 && checkpoint > 0.10)
            {
                lastRelDistanceCheckpoint = checkpoint;
                
                var nextEvent = EventByDistance(checkpoint, true);
                if (nextEvent.GetPersistentEventCount() > 0)
                {
                    checkpointTimer.Start(() =>
                    {
                        lastEvent = "Forward at " + checkpoint;
                        nextEvent.Invoke();
                    });
                }
            }
        }
        // Backwards
        if (lastRelDistanceCheckpoint - relDistance < 0)
        {
            var checkpoint = Mathf.Round(relDistance * 4) / 4;
            if (checkpoint < 1.10 && checkpoint > 0.10)
            {
                lastRelDistanceCheckpoint = checkpoint + 0.25f;

                var nextEvent = EventByDistance(checkpoint, false);
                if (nextEvent.GetPersistentEventCount() > 0)
                {
                    checkpointTimer.Start(() =>
                    {
                        lastEvent = "Backwards at " + checkpoint;
                        nextEvent.Invoke();
                    });
                }
            }
        }
        
        inGoalTimer.Update();
        checkpointTimer.Update();
    }

    private UnityEvent EventByDistance(float distance, bool forward)
    {
        if (forward)
        {
            if (distance <= 0.25) return on25TowardsGoal;
            if (distance <= 0.5) return on50TowardsGoal;
            if (distance <= 0.75) return on75TowardsGoal;
            if (distance <= 1) return onStartTowardsGoal;
            throw new Exception("Elegoo");
        }
        else
        {
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
            inGoalTimer.Start(() =>
            {
                inGoal = true;
                if (timePassedDetector != null) timePassedDetector.externInGoal = true;
                onGoalEnter.Invoke();
            });
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other == goal)
        {
            inGoalTimer.Start(() =>
            {
                inGoal = false;
                if (timePassedDetector != null) timePassedDetector.externInGoal = false;
                onGoalExit.Invoke();
            });
        }
    }
}
