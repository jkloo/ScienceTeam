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
    public SwitchType switchType = SwitchType.TOGGLE;
    private bool pressed = false;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(pressed)
        {
            return;
        }
        if(!other.isTrigger && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Carryable")))
        {
            anim.SetBool("Pressed", true);
            pressed = true;
            if(moveableObject)
            {
                moveableObject.On();
            }
        }
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
        if(pressed)
        {
            return;
        }
        if(!other.isTrigger && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Carryable")))
        {
            anim.SetBool("Pressed", true);
            pressed = true;
            if(moveableObject)
            {
                moveableObject.On();
            }
        }

    }
}
