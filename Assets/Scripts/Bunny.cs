using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Bunny : MonoBehaviour
{
    public UnityEvent onNotUpEnough;
    public UnityEvent onPutDown;
    public UnityEvent onUpEnough;

    public float goalHeight = 1.5f;

    public float height;
    public TimePassedDetector timePassed;

    public void Start()
    {
        timePassed = GetComponent<TimePassedDetector>();
    }

    public void HandOff()
    {
        height = transform.position.y;
        
        if (height < 0.2)
        {
            onPutDown.Invoke();
        } else if (height < goalHeight)
        {
            onNotUpEnough.Invoke();
        }
        else
        {
            timePassed.externInGoal = true;
            onUpEnough.Invoke();
        }
    }
}
