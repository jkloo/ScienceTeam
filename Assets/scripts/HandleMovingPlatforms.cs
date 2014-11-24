using UnityEngine;
using System.Collections;

public class HandleMovingPlatforms : MonoBehaviour {

    public GameObject standingOn;
    private Vector2 activeGlobalPlatformPoint;
    private CarrierController carrierController;

    void Start()
    {
        carrierController = GetComponent<CarrierController>();
    }

    void Update()
    {
        if (standingOn == null)
        {
            return;
        }
        Vector2 newGlobalPlatformPoint = standingOn.transform.position;
        Vector2 moveDistance = newGlobalPlatformPoint - activeGlobalPlatformPoint;
        if (moveDistance != Vector2.zero)
        {
            transform.Translate (moveDistance, Space.World);
        }

        activeGlobalPlatformPoint = newGlobalPlatformPoint;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        SetStandingOn(other);
    }

    void OnCollisionStay2D(Collision2D other)
    {
        SetStandingOn(other);
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(standingOn == other.gameObject)
        {
            standingOn = null;
        }
    }

    void SetStandingOn(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(standingOn == null || (standingOn.CompareTag("Carryable") && !other.gameObject.CompareTag("Carryable")))
            {
                if(carrierController != null && carrierController.grabbedObject == other.gameObject)
                {
                    return;
                }
                standingOn = other.gameObject;
                activeGlobalPlatformPoint = standingOn.transform.position;
            }
        }
    }
}
