using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AlienType {
    BLUE = 0,
    PINK,
    BEIGE,
    GREEN,
    YELLOW
}

public class AlienController : MonoBehaviour {

    public float maxSpeed = 7.5f;

    public float jumpForce = 500.0f;

    public float glideFactor = 0.2f;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public SpriteRenderer spriteRenderer;

    public Transform respawnPosition;

    public AlienType alienType;
    private Animator anim;

    private Color phaseColor = new Color(1f, 1f, 1f, 0.5f);
    private Color phaseableColor = new Color(1f, 1f, 1f, 0.9f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);

    private float hSpeed = 0.0f;
    private float vSpeed = 0.0f;
    private bool facingRight = true;

    private bool crouched = false;

    private bool grounded = false;
    private float groundRadius = 0.2f;

    private bool glide = false;
    private bool canGlide = false;

    private bool phase = false;
    private bool canPhase = true;

    private bool respawn = false;
    private float respawnTime = 0.5f;


    void Start()
    {
        anim = GetComponent<Animator>();
        alienType = AlienType.BLUE;
        MoveTo(respawnPosition.position);
        Spin();
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
        spriteRenderer.color = phase ? phaseColor : normalColor;
        Flip(hSpeed);

    }

    void Update()
    {
        if(respawn) return;

        Move();
        crouched = Input.GetButton("Crouch");

        if(grounded)
        {
            StopGlide();
        }

        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if(Input.GetButtonDown("Special"))
        {
            StartSpecial();
        }
        else if(Input.GetButtonUp("Special"))
        {
            StopSpecial();
        }

    }

    /*
    * Basic player movement functions
    */
    void Flip(float move)
    {
        if ((move > 0 && !facingRight) || (move < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
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
        if(grounded)
        {
            canGlide = true;
            rigidbody2D.AddForce(new Vector2(0, jumpForce));
			audio.pitch = Random.Range(0.9f, 1.0f);
			audio.Play();
        }
    }


    /*
    * AlienType based special abilities
    */
    void StartSpecial()
    {
        switch(alienType)
        {
            case AlienType.BLUE:
                StartPhase();
                break;
            case AlienType.PINK:
                StartGlide();
                break;
            case AlienType.BEIGE:
                break;
            case AlienType.GREEN:
                break;
            case AlienType.YELLOW:
                break;
            default:
                break;
        }
    }

    void StopSpecial()
    {
        switch(alienType)
        {
            case AlienType.BLUE:
                StopPhase();
                break;
            case AlienType.PINK:
                StopGlide();
                break;
            case AlienType.BEIGE:
                break;
            case AlienType.GREEN:
                break;
            case AlienType.YELLOW:
                break;
            default:
                break;
        }
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

    void StartPhase()
    {
        if(!phase && canPhase)
        {
            phase = true;
            canPhase = false;
            GameObject[] walls = GameObject.FindGameObjectsWithTag("Phaseable");
            foreach(GameObject wall in walls)
            {
				wall.collider2D.isTrigger = true;
                wall.GetComponent<SpriteRenderer>().color = phaseableColor;
            }
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    void StopPhase()
    {

        canPhase = true;
        phase = false;
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Phaseable");
        foreach(GameObject wall in walls)
        {
            if(!collider2D.bounds.Intersects(wall.collider2D.bounds))
            {
                wall.collider2D.isTrigger = false;
				wall.GetComponent<SpriteRenderer>().color = normalColor;
            }
            else
            {
                phase = true;
            }
        }
    }

    /*
    * Player object manipulation
    */
    void ChangeAlienType(AlienType newType)
    {
        alienType = newType;
        Spin();
    }

    void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    void Spin()
    {
        StopSpecial();
        StartCoroutine(SpinWait());
    }

    IEnumerator SpinWait()
    {
        respawn = true;
        rigidbody2D.isKinematic = true;

        anim.SetTrigger("Spin");
        yield return new WaitForSeconds(respawnTime);

        rigidbody2D.isKinematic = false;
        respawn = false;
    }

    void Respawn()
    {
        MoveTo(respawnPosition.position);
        Spin();
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

    /*
    * Collision/Trigger event handlers
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Checkpoint"))
        {
            respawnPosition = other.gameObject.transform;
        }
        else if(other.gameObject.CompareTag("Finish"))
        {
            Spin();
            NextLevelLoader nextLevelLoader = other.GetComponent<NextLevelLoader>();
            LoadNextLevel(nextLevelLoader.nextLevel);
        }
        else if(other.gameObject.CompareTag("DeathZone"))
        {
            Respawn();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("SafeZone"))
        {
            Respawn();
        }
        else if(other.gameObject.CompareTag("Phaseable"))
        {
            StopPhase();
        }
    }
}

