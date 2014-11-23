using UnityEngine;
using System.Collections;

public enum SwitchType
{
    TOGGLE = 0,
    HOLD
}

public class PushButtonController : MonoBehaviour
{
    public MoveableObjectController moveableObject;
    public Transform anchor;
    public SwitchType switchType = SwitchType.TOGGLE;

    private bool pressed = false;
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        SwitchActive(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if((other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Carryable")))
        {
            if(switchType == SwitchType.HOLD)
            {
                moveableObject.Off();
            }
            anim.SetBool("Pressed", false);
            pressed = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        SwitchActive(other);
    }

    void SwitchActive(Collider2D other)
    {
        if(pressed)
        {
            return;
        }
        if(!other.isTrigger && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Carryable")))
        {
            if(other.gameObject.CompareTag("Carryable"))
            {
                other.gameObject.transform.position = new Vector2(anchor.position.x, anchor.position.y + 0.4f);
                other.rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
            }
            anim.SetBool("Pressed", true);
            pressed = true;
            if(moveableObject)
            {
                moveableObject.On();
            }
        }
    }
}
