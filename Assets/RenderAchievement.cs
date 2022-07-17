using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class RenderAchievement : MonoBehaviour
{
    private float duration = 50;
    private float startTime;
    private bool active = false;
    public Material invisibleMaterial;
    
    // Start is called before the first frame update
    void Start()
    {
        ShowAchievement();
    }

    private void ShowAchievement()
    {
      Texture2D texture = Resources.Load("Achievements/01") as Texture2D;
      Material material = new Material(Shader.Find("Diffuse"));
      material.mainTexture = texture;
      GetComponent<Renderer>().material = material;
      active = true;
      startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (active && Time.time - duration > startTime)
        {
            // stop
            GetComponent<Renderer>().material = invisibleMaterial;
            active = false;
        }
    }
}
