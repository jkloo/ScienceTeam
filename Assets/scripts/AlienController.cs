using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {

    public float maxSpeed = 7.5f;

    public float jumpForce = 500.0f;

    public float glideFactor = 0.01f;
    public float glideDuration = 0.5f;

    public LayerMask whatIsGround;
    public Transform groundCheck;

    public Collider2D safeZone;
    public Transform respawnPosition;

    private Animator anim;

    private float vSpeed = 0.0f;
    private bool facingRight = true;

    private bool crouched = false;

    private float groundRadius = 0.2f;
    private bool grounded = false;

    private bool glide = false;
    private bool canGlide = false;

    private bool respawn = false;
    private float respawnTime = 0.5f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        MoveTo(respawnPosition.position);
        Respawn();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(respawn)
        {
            grounded = true;
            vSpeed = 0.0f;
            canGlide = false;
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
            HandleFlip(move);
        }

    }

    void Update()
    {
        if(respawn) return;

        if(grounded)
        {
            if(glide)
            {
                glide = false;
                rigidbody2D.gravityScale /= glideFactor;
            }
            if(Input.GetButtonDown("Jump"))
            {
                canGlide = true;
                anim.SetBool("Ground", false);
                rigidbody2D.AddForce(new Vector2(0, jumpForce));
            }
            crouched = Input.GetButton("Crouch");
        }
        else
        {
            if(Input.GetButtonDown("Glide") && !glide && canGlide)
            {
                glide = true;
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.1f * rigidbody2D.velocity.y);
                rigidbody2D.gravityScale *= glideFactor;
                canGlide = false;
            }
            else if(Input.GetButtonUp("Glide") && glide)
            {
                rigidbody2D.gravityScale /= glideFactor;
                glide = false;
            }
            crouched = false;
        }
    }

    void HandleFlip(float move)
    {
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
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
        else if(other.gameObject.CompareTag("Finish") && !respawn)
        {
            Respawn();
            NextLevelLoader nextLevelLoader = other.GetComponent<NextLevelLoader>();
            LoadNextLevel(nextLevelLoader.nextLevel);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other == safeZone)
        {
            MoveTo(respawnPosition.position);
            Respawn();
        }
    }

    void Respawn()
    {
        if(glide)
        {
            rigidbody2D.gravityScale /= glideFactor;
        }
        StartCoroutine(RespawnWait());
    }

    void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    IEnumerator RespawnWait()
    {
        respawn = true;
        float gravity = rigidbody2D.gravityScale;
        rigidbody2D.gravityScale = 0.0f;
        rigidbody2D.velocity = Vector2.zero;

        anim.SetTrigger("Respawn");
        yield return new WaitForSeconds(respawnTime);

        rigidbody2D.gravityScale = gravity;
        respawn = false;
    }

    void LoadNextLevel(string nextLevel)
    {
        StartCoroutine(LoadNextLevelWait(nextLevel));
    }

    IEnumerator LoadNextLevelWait(string nextLevel)
    {
        yield return new WaitForSeconds(respawnTime);
        Application.LoadLevel(nextLevel);
    }
}

