using UnityEngine;
using UnityEngine.Events;

public class InHandDetector : MonoBehaviour
{
    public float handPickupDelay = 1;
    public float handPutdownDelay = 1;

    public UnityEvent leftHandPickup;
    public UnityEvent rightHandPickup;
    
    public UnityEvent leftHandPutdown;
    public UnityEvent rightHandPutdown;

    private bool possibleInLeftHand = false;
    private bool possibleInRightHand = false;
    
    private bool inLeftHand = false;
    private bool inRightHand = false;
    
    private bool possibleNotInLeftHand = false;
    private bool possibleNotInRightHand = false;

    private float leftHandCollisionTime;
    private float rightHandCollisionTime;
    
    private float leftHandExitTime;
    private float rightHandExitTime;
    
    void Start()
    {
    }

    void Update()
    {
        if (possibleInLeftHand && Time.time - leftHandCollisionTime > handPickupDelay)
        {
            possibleInLeftHand = false;
            inLeftHand = true;
            leftHandPickup.Invoke();
        }
        if (possibleInRightHand && Time.time - rightHandCollisionTime > handPickupDelay)
        {
            possibleInRightHand = false;
            inRightHand = true;
            rightHandPickup.Invoke();
        }
        
        if (possibleNotInLeftHand && Time.time - leftHandExitTime > handPutdownDelay)
        {
            possibleNotInLeftHand = false;
            inLeftHand = false;
            leftHandPutdown.Invoke();
        }
        if (possibleNotInRightHand && Time.time - rightHandExitTime > handPutdownDelay)
        {
            possibleNotInRightHand = false;
            inRightHand = false;
            rightHandPutdown.Invoke();
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("LeftHand"))
        {
            if (inLeftHand)
            {
                possibleNotInLeftHand = false;
            }
            else
            {
                possibleInLeftHand = true;
                leftHandCollisionTime = Time.time;
            }
        }
        if (obj.CompareTag("RightHand"))
        {
            if (inRightHand)
            {
                possibleNotInRightHand = false;
            }
            else
            {
                possibleInRightHand = true;
                rightHandCollisionTime = Time.time;
            }
        }
    }
    
    public void OnTriggerExit(Collider collision)
    {
        var obj = collision.gameObject;
        if (obj.CompareTag("LeftHand"))
        {
            if (inLeftHand)
            {
                possibleNotInLeftHand = true;
                leftHandExitTime = Time.time;
            }
            else
            {
                possibleInLeftHand = false;
            }
        }
        if (obj.CompareTag("RightHand"))
        {
            if (inRightHand)
            {
                possibleNotInRightHand = true;
                rightHandExitTime = Time.time;
            }
            else
            {
                possibleInRightHand = false;
            }
        }
    }
}
