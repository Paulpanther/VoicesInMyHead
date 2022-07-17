using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningScript : MonoBehaviour
{
    public GameObject gameLogo;
    public GameObject gameDevLogo;

    // Start is called before the first frame update
    void Start()
    {
        
        gameLogo.GetComponent<MeshRenderer>().enabled = false;
        gameDevLogo.GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    IEnumerator waiter()
    {   yield return new WaitForSeconds(3);
        gameDevLogo.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(0.5f);
        gameDevLogo.GetComponent<MeshRenderer>().enabled = true;

        yield return new WaitForSeconds(4);
        gameLogo.GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(0.5f);
        gameLogo.GetComponent<MeshRenderer>().enabled = true;

        
        yield return new WaitForSeconds(10);
        gameDevLogo.GetComponent<MeshRenderer>().enabled = false;
        gameLogo.GetComponent<MeshRenderer>().enabled = false;

    }
}
