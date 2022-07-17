using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class Chair : MonoBehaviour
{
    public float timeOverChair = 8;
    public UnityEvent sitOnChair;
    public TimePassedDetector timePassed;
    private Timer timer;
    public bool wasSitting = false;
    
    void Start()
    {
        timePassed = GetComponent<TimePassedDetector>();
        timer = new Timer(timeOverChair);
    }

    void Update()
    {
        timer.Update();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera") && !wasSitting)
        {
            timer.Start(() =>
            {
                wasSitting = true;
                timePassed.externInGoal = true;
                sitOnChair.Invoke();
            });
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            timer.Cancel();
        }
    }
}
