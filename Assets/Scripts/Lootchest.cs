using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Lootchest : MonoBehaviour
{
    public Transform cap;
    public UnityEvent onOpen;

    private float target;
    private float animationStart = 0;
    private float animationDuration = 5;
    private bool isAnimated = false;

    // Start is called before the first frame update
    void Start()
    {
        target = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAnimated)
        {
            float delta = (Time.time - animationStart) / animationDuration;
            cap.localRotation = Quaternion.Lerp(Quaternion.Euler(cap.localEulerAngles.x, 0, 0), Quaternion.Euler(target, 0, 0), delta);
            if (delta > 1f) isAnimated = false;
        }
    }

    public void OnPlayerIsNear()
    {
        onOpen.Invoke();
        target = -140f;
        animationStart = Time.time;
        isAnimated = true;
    }

    public void OnPlayerLeave()
    {
        target = 0f;
        animationStart = Time.time;
        isAnimated = true;
    }
}
