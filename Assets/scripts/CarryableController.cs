using UnityEngine;
using System.Collections;

public class CarryableController : MonoBehaviour {

    private Animator anim;
    private bool carried = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetCarriedStatus(bool status)
    {
        carried = status;
        anim.SetBool("Carried", status);
        rigidbody2D.isKinematic = status;
        collider2D.enabled = !status;
    }
}
