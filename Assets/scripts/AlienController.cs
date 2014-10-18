using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {

    public float maxSpeed = 10f;
    private bool facingRight = true;

    Animator anim;

    bool grounded = false;
    bool crouched = false;
    public Transform groundCheck;
    float groundRadius = 0.2f;
    public LayerMask whatIsGround;
    public float jumpForce = 700.0f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        grounded = Physics2D.OverlapCircle(groundCheck.position,
                                           groundRadius,
                                           whatIsGround);
        anim.SetBool("Ground", grounded);
        anim.SetBool("Crouch", crouched);
        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
        if(!crouched)
        {
            float move = Input.GetAxis("Horizontal");
            rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(move));            
            if (move > 0 && !facingRight)
            {
                Flip();
            }
            else if (move < 0 && facingRight)
            {
                Flip();
            }
        }

    }

    void Update()
    {
        if(grounded)
        {
            if(Input.GetButtonDown("Jump"))
            {
                anim.SetBool("Ground", false);
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
            }
            crouched = Input.GetButton("Crouch");
        }
        else
        {
            crouched= false;
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
