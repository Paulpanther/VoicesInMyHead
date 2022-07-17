using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    
    private AudioSource thunder;
    private Light lightSource;

    // Start is called before the first frame update
    void Start()
    {
        thunder = gameObject.GetComponent<AudioSource>();
        lightSource = gameObject.GetComponent<Light>();

        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        if (!thunder.isPlaying) GameObject.Destroy(gameObject);
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.1f);

        lightSource.color = Color.black;
    }
}
