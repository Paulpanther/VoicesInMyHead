using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class AchievementEnable : MonoBehaviour
{
    public float time = 5;
    private Timer timer;
    
    void Start()
    {
        gameObject.SetActive(false);
        timer = new Timer(time);
    }

    // Update is called once per frame
    void Update()
    {
        timer.Update();
    }

    public void ShowAchievement()
    {
        gameObject.SetActive(true);
        timer.Start(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
