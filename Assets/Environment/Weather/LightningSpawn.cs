using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpawn : MonoBehaviour
{
    public float propability;
    public GameObject lightningPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float num = UnityEngine.Random.value;

        if (num <= propability) {
            Vector3 loc = Random.onUnitSphere * 10;
            loc.y = Mathf.Abs(loc.y);

            Instantiate(lightningPrefab, loc, Quaternion.identity);
        }

    }
}
