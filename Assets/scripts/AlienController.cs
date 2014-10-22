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

