using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

public class PlayAreaCornerScript : MonoBehaviour
{
    public int cornerID;

    HmdQuad_t playAreaRect;

    // Start is called before the first frame update
    void Start()
    {
        var error = EVRInitError.None;
        // OpenVR.Init(ref error, EVRApplicationType.VRApplication_Background);
        OpenVR.Chaperone.ForceBoundsVisible(true);
        OpenVR.Chaperone.GetPlayAreaRect(ref playAreaRect);
        
        HmdVector3_t pos;
        switch (cornerID)
        {
            case 0: pos = playAreaRect.vCorners0; break;
            case 1: pos = playAreaRect.vCorners1; break;
            case 2: pos = playAreaRect.vCorners2; break;
            case 3: pos = playAreaRect.vCorners3; break;
            default: throw new System.ArgumentException("cornerID should be between 0 and 3 (both inclusive)");
        }

        transform.position = new Vector3(pos.v0, pos.v1, pos.v2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
