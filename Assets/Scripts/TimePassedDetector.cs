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
    public ObjectGoalProgression goalProgression;
    private Timer timer;
    
    void Start()
    {
        handDetector = GetComponent<InHandDetector>();
        goalProgression = GetComponent<ObjectGoalProgression>();
        
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
                if (goalProgression.inGoal)
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
