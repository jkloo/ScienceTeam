using UnityEngine;
using System.Collections;

public class CarrierController : MonoBehaviour {

    private AlienController alienController;
    public GameObject grabbedObject;

    // Use this for initialization
    void Start ()
    {
        alienController = GetComponent<AlienController>();
    }

    void TryGrabObject(GameObject grabObject)
    {
        if(grabObject == null)
        {
            return;
        }
        Debug.Log(grabObject);
        grabbedObject = grabObject;
        grabbedObject.GetComponent<CarryableController>().SetCarriedStatus(true);
    }

    public bool DropObject()
    {
        Debug.Log("Drop Called");
        if(grabbedObject == null)
        {
            return false;
        }
        grabbedObject.rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);
        grabbedObject.GetComponent<CarryableController>().SetCarriedStatus(false);
        grabbedObject = null;

        return grabbedObject != null;
    }

    public bool PickupObject()
    {
        Debug.Log("Pickup Called");
        if(grabbedObject == null)
        {
            TryGrabObject(FindCarryableObject(0.7f));
        }
        return grabbedObject != null;
    }

    GameObject FindCarryableObject(float range)
    {
        Vector2 pos = gameObject.transform.position;
        Vector2 target = pos + new Vector2 ((alienController.facingRight ? 1 : -1) * range, 0);
        RaycastHit2D[] raycastHits = Physics2D.LinecastAll(pos, target);
        foreach(RaycastHit2D hit in raycastHits)
        {
            if(hit.collider.gameObject.CompareTag("Carryable"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    // Update is called once per frame
    void Update ()
    {
        if(grabbedObject)
        {
            float newX = transform.position.x + (alienController.facingRight ? 1 : -1) * 0.9f;
            float newY = transform.position.y + 0.1f;
            Vector2 newPosition = new Vector2(newX, newY);
            grabbedObject.transform.position = newPosition;
        }
    }
}
