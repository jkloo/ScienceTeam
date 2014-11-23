using UnityEngine;
using System.Collections;

public class CarryableController : MonoBehaviour {

    private Animator anim;
    private bool carried = false;
    private Vector2 startPosition;

    void Awake()
    {
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    public void SetCarriedStatus(bool status)
    {
        carried = status;
        anim.SetBool("Carried", status);
        rigidbody2D.isKinematic = status;
        collider2D.isTrigger = status;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("SafeZone"))
        {
            transform.position = startPosition;
            rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
        }
    }
}
