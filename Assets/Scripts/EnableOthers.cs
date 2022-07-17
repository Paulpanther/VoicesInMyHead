using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class EnableOthers : MonoBehaviour
{
    public GameObject obj;
    public float delay = 5;
    private Timer timer = new Timer(0);
    
    public void DoEnable()
    {
        timer = new Timer(delay);
        timer.Start(() =>
        {
            obj.SetActive(true);
        });
    }

    private void Update()
    {
        timer.Update();
    }
}
