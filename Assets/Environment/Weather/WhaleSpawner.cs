using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSpawner : MonoBehaviour
{
    public float spawnHeight;
    public float areaSize;
    public float spawnRate;

    public GameObject whalePrefab;

    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;

        if (spawnTime >= 1.0f / spawnRate) {
            spawn();
            spawnTime -= (1.0f / spawnRate);
        }
    }

    void spawn() {
        Vector3 position = new Vector3(areaSize * (Random.value * 2 - 1), spawnHeight * (Random.value * 0.1f + 0.9f), -areaSize);
        float scale = Random.value * 0.4f + 0.8f;
        Object whale = Instantiate(whalePrefab, position, Quaternion.identity);
        ((GameObject)whale).transform.localScale = new Vector3(scale, scale, scale);
    }
}
