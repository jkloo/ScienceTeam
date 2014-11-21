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

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Pressed", true);
            if(moveableObject)
            {
                moveableObject.On();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(switchType == SwitchType.HOLD)
            {
                moveableObject.Off();
            }
            anim.SetBool("Pressed", false);
        }
    }

}
