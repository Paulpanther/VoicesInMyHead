using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThomasTeleporter : MonoBehaviour
{
    // TODO: Obtain camera orientation from player
    public GameObject player;
    public float minDistance;
    public float maxDistance;
    public float deadAngleDeg;
    float lastTeleportTime;

    // Start is called before the first frame update
    void Start()
    {
        lastTeleportTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Check if Thomas is not visible to player
        // TODO: Calculate safe angle where Thomas is allowed to spawn
        if (Time.time - lastTeleportTime < 5000) return;

        var radius = Random.Range(minDistance, maxDistance);
        var theta = Random.Range(0, 2 * Mathf.PI);

        var x = radius * cos(theta);
        var y = radius * sin(theta);
        transform.position = new Vector3(x, y, transform.position.z);
        transform.eulerAngles = new Vector3(0, 0, -(Mathf.Rad2Deg * theta));

        lastTeleportTime = Time.time;
    }
}
