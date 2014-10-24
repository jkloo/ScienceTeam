using UnityEngine;
using System.Collections;

public class AlienController : MonoBehaviour {

    public float maxSpeed = 7.5f;

    public float jumpForce = 500.0f;

    public float glideFactor = 0.2f;

    public LayerMask whatIsGround;
    public Transform groundCheck;

    public Collider2D safeZone;
    public Transform respawnPosition;

    private Animator anim;

    private float hSpeed = 0.0f;
    private float vSpeed = 0.0f;
    private bool facingRight = true;

    private bool crouched = false;

    private bool grounded = false;
    private float groundRadius = 0.2f;

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

    void FixedUpdate()
    {
        if(respawn)
        {
            hSpeed = 0.0f;
            grounded = true;
            vSpeed = 0.0f;
        }
        else
        {
            grounded = Physics2D.OverlapCircle(groundCheck.position,
                                               groundRadius,
                                               whatIsGround);
            vSpeed = rigidbody2D.velocity.y;
            hSpeed = Input.GetAxis("Horizontal");
        }

        anim.SetFloat("Speed", Mathf.Abs(hSpeed));
        anim.SetBool("Ground", grounded);
        anim.SetBool("Crouch", crouched);
        anim.SetFloat("vSpeed", vSpeed);
        Flip(hSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        if(respawn) return;

        Move();
        if(grounded)
        {
            StopGlide();
            if(Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            crouched = Input.GetButton("Crouch");
        }
        else
        {
            if(Input.GetButtonDown("Glide"))
            {
                StartGlide();
            }
            else if(Input.GetButtonUp("Glide"))
            {
                StopGlide();
            }
            crouched = false;
        }
    }

    bool Flip(float move)
    {
        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
            return true;
        }
        return false;
    }

    void Move()
    {
        if(!crouched)
        {
            rigidbody2D.velocity = new Vector2(hSpeed * maxSpeed, rigidbody2D.velocity.y);
        }
    }

    void Jump()
    {
        canGlide = true;
        rigidbody2D.AddForce(new Vector2(0, jumpForce));
    }

    void StartGlide()
    {
        if(!glide && canGlide)
        {
            glide = true;
            canGlide = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0.1f * rigidbody2D.velocity.y);
            rigidbody2D.gravityScale *= glideFactor;
        }
    }

    void StopGlide()
    {
        if(glide)
        {
            rigidbody2D.gravityScale /= glideFactor;
            glide = false;
        }
    }

    void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    void Respawn()
    {
        StopGlide();
        StartCoroutine(RespawnWait());
    }

    IEnumerator RespawnWait()
    {
        respawn = true;
        rigidbody2D.isKinematic = true;

        anim.SetTrigger("Respawn");
        yield return new WaitForSeconds(respawnTime);

        rigidbody2D.isKinematic = false;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Checkpoint"))
        {
            respawnPosition = other.gameObject.transform;
        }
        else if(other.gameObject.CompareTag("Finish"))
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
}

