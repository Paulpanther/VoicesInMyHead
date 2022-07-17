using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Timer = DefaultNamespace.Timer;

public class LootboxEat : MonoBehaviour
{
    public TimePassedDetector timePassedDetector;
    public UnityEvent doOnEat;
    public float sitTime;
    private Timer timer;
    public bool hasEaten = false;

    private void Start()
    {
        timer = new Timer(sitTime);
    }

    private void Update()
    {
        timer.Update();
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter" + other.tag);
        if (other.CompareTag("Obj"))
        {
            timer.Start(() =>
            {
                hasEaten = true;
                timePassedDetector.externInGoal = true;
                doOnEat.Invoke();
            });
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obj"))
        {
            timer.Cancel();
        }
    }
}
