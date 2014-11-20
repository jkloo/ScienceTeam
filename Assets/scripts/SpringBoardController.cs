using UnityEngine;
using System.Collections;

public class SpringBoardController : MonoBehaviour
{

    public float springForce = 750.0f;
    private Animator anim;
    private bool spring = false;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Spring");
        anim.SetTrigger("Spring");
        Rigidbody2D body = other.gameObject.GetComponent<Rigidbody2D>();
        if(body && !spring)
        {
            spring = true;
            body.velocity = new Vector2(body.velocity.x, 0.0f);
            body.AddForce(new Vector2(0, springForce));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        spring = false;
    }

}
