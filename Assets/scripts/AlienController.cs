using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {

    public float maxSpeed = 10f;
    public float jumpForce = 700.0f;
    public LayerMask whatIsGround;
    public Transform groundCheck;
    public Collider2D safeZone;
    public Transform respawnPosition;

    private Animator anim;
    private float groundRadius = 0.2f;
    private bool facingRight = true;
    private bool crouched = false;
    private bool grounded = false;
    private bool respawn = false;
    private float respawnTime = 0.5f;
    private float vSpeed = 0.0f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        Respawn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(respawn)
        {
            grounded = true;
            vSpeed = 0.0f;
        }
        else
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position,
                                               groundRadius,
                                               whatIsGround);
            vSpeed = rigidbody2D.velocity.y;
        }
        anim.SetBool("Ground", grounded);
        anim.SetBool("Crouch", crouched);
        anim.SetFloat("vSpeed", vSpeed);
        if(!crouched && !respawn)
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
        if(grounded && !respawn)
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Checkpoint"))
        {
            respawnPosition = other.gameObject.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other == safeZone)
        {
            Respawn();
        }
    }

    void Respawn()
    {
        StartCoroutine(RespawnWait());
    }

    IEnumerator RespawnWait()
    {
        transform.position = respawnPosition.position;
        rigidbody2D.velocity = Vector2.zero;

        float gravity = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0.0f;
        respawn = true;
        anim.SetTrigger("Respawn");
        yield return new WaitForSeconds(respawnTime);
        rigidbody2D.gravityScale = gravity;
        respawn = false;
    }
}
