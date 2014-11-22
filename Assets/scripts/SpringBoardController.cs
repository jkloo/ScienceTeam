using UnityEngine;
using System.Collections;

public class SpringBoardController : MonoBehaviour
{

    public float springForce = 750.0f;
    private Animator anim;
    private bool spring = false;
    private float timeBetweenSprings = 0.1f;
    private float timeSinceSpring = 0.0f;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(timeSinceSpring < timeBetweenSprings)
        {
            return;
        }
        Spring(other);
        if(spring)
        {
            timeSinceSpring = 0.0f;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(timeSinceSpring < timeBetweenSprings)
        {
            return;
        }
        Spring(other);
        if(spring)
        {
            timeSinceSpring = 0.0f;
        }
    }

    void Spring(Collider2D other)
    {
        Rigidbody2D body = other.gameObject.GetComponent<Rigidbody2D>();
        if(!other.isTrigger && (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Carryable")))
        {
            anim.SetTrigger("Spring");
            spring = true;
            body.velocity = new Vector2(body.velocity.x, 0.0f);
            body.AddForce(new Vector2(0, springForce * body.mass));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        spring = false;
    }

    void Update()
    {
        timeSinceSpring += Time.deltaTime;
    }

}
