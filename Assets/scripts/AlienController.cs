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

public enum AlienState
{
    DISABLED = 0,
    INACTIVE,
    ACTIVE
}

public class AlienController : MonoBehaviour {

    public float maxSpeed = 7.5f;

    public float jumpForce = 500.0f;

    public float glideFactor = 0.2f;

    public LayerMask whatIsGround;
    public Transform groundCheck;
    public SpriteRenderer spriteRenderer;

    public AlienType alienType;
    private Animator anim;

    private Color phaseColor = new Color(1f, 1f, 1f, 0.5f);
    private Color normalColor = new Color(1f, 1f, 1f, 1f);

    public float hSpeed = 0.0f;
    private float vSpeed = 0.0f;
    private bool facingRight = true;

    private bool respawning = false;

    private bool crouched = false;

    private bool grounded = false;
    private float groundRadius = 0.2f;

    private bool glide = false;
    private bool canGlide = false;

    private bool phase = false;
    private bool canPhase = true;

    private bool gravInvert = false;

    private bool shrink = false;
    private float shrinkFactor = 0.5f;

    private bool spin = false;
    private float spinTime = 0.5f;

    private bool specialOn = false;

    private GameObject manager;
    private ItemManager itemManager;
    private LevelManager levelManager;


    public void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Start()
    {
        manager = GameObject.FindGameObjectWithTag("Manager");
        itemManager = manager.GetComponent<ItemManager>();
        levelManager = manager.GetComponent<LevelManager>();
    }

    void FixedUpdate()
    {
        if(spin)
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
            vSpeed = rigidbody2D.velocity.y * (gravInvert ? -1 : 1);
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
        if(spin) return;


        if(grounded)
        {
            StopGlide();
        }

#if !UNITY_ANDROID
        hSpeed = Input.GetAxis("Horizontal");
        crouched = Input.GetButton("Crouch");
        if(Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if(Input.GetButtonDown("Special"))
        {
            ToggleSpecial();
        }
        if(Input.GetButtonDown("SwitchBlue"))
        {
            ChangeAlienType(AlienType.BLUE);
        }
        else if(Input.GetButtonDown("SwitchGreen"))
        {
            ChangeAlienType(AlienType.GREEN);
        }
        else if(Input.GetButtonDown("SwitchPink"))
        {
            ChangeAlienType(AlienType.PINK);
        }
        else if(Input.GetButtonDown("SwitchBeige"))
        {
            ChangeAlienType(AlienType.BEIGE);
        }
        else if(Input.GetButtonDown("SwitchYellow"))
        {
            ChangeAlienType(AlienType.YELLOW);
        }
#endif
        Move();
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

    public void Jump()
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
    public void StartSpecial()
    {
        switch(alienType)
        {
            case AlienType.BLUE:
                StartGlide();
                break;
            case AlienType.PINK:
                StartInvertGravity();
                break;
            case AlienType.BEIGE:
                break;
            case AlienType.GREEN:
                StartPhase();
                break;
            case AlienType.YELLOW:
                StartShrink();
                break;
            default:
                break;
        }
    }

    public void StopSpecial()
    {
        specialOn = false;
        switch(alienType)
        {
            case AlienType.BLUE:
                StopGlide();
                break;
            case AlienType.PINK:
                StopInvertGravity();
                break;
            case AlienType.BEIGE:
                break;
            case AlienType.GREEN:
                StopPhase();
                break;
            case AlienType.YELLOW:
                StopShrink();
                break;
            default:
                break;
        }
    }

    public void ToggleSpecial()
    {
        if(specialOn)
        {
            StopSpecial();
        }
        else
        {
            StartSpecial();
        }
    }

    void StartGlide()
    {
        if(!glide && canGlide)
        {
            glide = true;
            canGlide = false;
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,
                                               0.1f * rigidbody2D.velocity.y);
            rigidbody2D.gravityScale *= glideFactor;
            specialOn = true;
        }
    }

    void StopGlide()
    {
        if(glide)
        {
            specialOn = false;
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
            spriteRenderer.color = phaseColor;
            levelManager.StartPhaseObjects();
            specialOn = true;
        }
    }

    void StopPhase()
    {
        specialOn = false;
        canPhase = true;
        phase = !levelManager.CanStopPhaseObjects();
        if(!phase)
        {
            levelManager.StopPhaseObjects();
            spriteRenderer.color = normalColor;
        }
    }

    void StartInvertGravity()
    {
        if(grounded)
        {
            gravInvert = true;
            rigidbody2D.gravityScale *= -1;
            jumpForce *= -1;
            Vector3 theScale = transform.localScale;
            theScale.y *= -1;
            transform.localScale = theScale;
            specialOn = true;
        }
    }

    void StopInvertGravity()
    {
        if(gravInvert && (grounded || respawning))
        {
            gravInvert = false;
            rigidbody2D.gravityScale *= -1;
            jumpForce *= -1;
            Vector3 theScale = transform.localScale;
            theScale.y *= -1;
            transform.localScale = theScale;
        }
    }

    void StartShrink()
    {
        shrink = true;
        Vector3 theScale = transform.localScale;
        theScale.x *= shrinkFactor;
        theScale.y *= shrinkFactor;
        transform.localScale = theScale;
        groundRadius *= shrinkFactor;
        specialOn = true;
    }

    void StopShrink()
    {
        if(shrink)
        {
            shrink = false;
            Vector3 theScale = transform.localScale;
            theScale.x /= shrinkFactor;
            theScale.y /= shrinkFactor;
            transform.localScale = theScale;
            groundRadius /= shrinkFactor;
        }
    }

    /*
    * Player object manipulation
    */
    public void ChangeAlienType(AlienType newType)
    {
        if(newType == alienType)
        {
            return;
        }
        if(spin)
        {
            return;
        }
        bool switched = levelManager.SetActiveAlienByType(newType);
        if(switched)
        {
            StopSpecial();
        }
    }

    void MoveTo(Vector3 position)
    {
        transform.position = position;
    }

    public void Spin()
    {
        StartCoroutine(SpinWait());
    }

    IEnumerator SpinWait()
    {
        spin = true;
        rigidbody2D.isKinematic = true;

        anim.SetTrigger("Spin");
        yield return new WaitForSeconds(spinTime);

        rigidbody2D.isKinematic = false;
        spin = false;
    }

    void Respawn()
    {
        respawning = true;
        levelManager.MoveAlienToRespawn(gameObject);
        StopSpecial();
        Spin();
        respawning = false;
    }

    /*
    * Collision/Trigger event handlers
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Checkpoint"))
        {
            levelManager.SetCheckpoint(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Finish"))
        {
            Spin();
            levelManager.LevelEnd();
        }
        else if(other.gameObject.CompareTag("DeathZone"))
        {
            Respawn();
        }
        else if(other.gameObject.CompareTag("Item"))
        {
            itemManager.PickUp(other.gameObject);
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
