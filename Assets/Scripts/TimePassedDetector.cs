using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

public class TimePassedDetector : MonoBehaviour
{
    public float time;
    public UnityEvent doEveryTime;
    public UnityEvent doEveryTimeIfInHand;
    public UnityEvent doEveryTimeIfNotInHandInGoal;
    public UnityEvent doEveryTimeIfNotInHandNotInGoal;
    
    public InHandDetector handDetector;
    private Timer timer;
    public bool externInGoal = false;
    
    void Start()
    {
        handDetector = GetComponent<InHandDetector>();
        
        timer = new Timer(time);
        Action action = () => {};
        action = () =>
        {
            if (handDetector.inLeftHand || handDetector.inRightHand)
            {
                doEveryTimeIfInHand.Invoke();
            }
            if (!handDetector.inLeftHand && !handDetector.inRightHand)
            {
                if (externInGoal)
                {
                    doEveryTimeIfNotInHandInGoal.Invoke();
                }
                else
                {
                    doEveryTimeIfNotInHandNotInGoal.Invoke();
                }
            }
            doEveryTime.Invoke();
            timer.Start(action);
        };
        timer.Start(action);
    }

    void Update()
    {
        timer.Update();
    }
}
