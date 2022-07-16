using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Timer
    {
        private Action action;
        private float startTime;
        private bool running = false;
        private float delay;
        
        public Timer(float delay)
        {
            this.delay = delay;
        }

        public void Update()
        {
            if (running && Time.time - startTime > delay)
            {
                running = false;
                action.Invoke();
            }
        }

        public void Start(Action action)
        {
            this.action = action;
            startTime = Time.time;
            running = true;
        }

        public void Cancel()
        {
            running = false;
        }
    }
}