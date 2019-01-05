using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Ground Movement")]
    [Tooltip("Force applied when moving horizontally")]
    public float moveForce = 365f;
    [Tooltip("Max run speed")]
    public float maxSpeed = 5f;
    [Tooltip("Force applied to deaccelerate")]
    public float horizontalDrag = 0.3f;
    [Tooltip("Max speed multiplier when sprinting")]
    [Range(1f, 5f)]
    public float runMultiplier = 1.5f;
    [Space(10)]
    [Header("Jump Movement")]
    [Tooltip("Force applied to do jumps")]
    public float jumpForce = 1000f;
    [Tooltip("Gravity multiplier when falling")]
    public float fallMultiplier = 2.5f;
    [Tooltip("Gravity multiplier when releasing jump button")]
    public float releaseMultiplier = 2f;
    [Tooltip("Multiplier of horizontal control when on air")]
    [Range(0f, 1f)]
    public float airControlMultiplier = 0.5f;

    [HideInInspector]
    public bool facingRight = true;         // For determining which way the player is currently facing.
	private bool grounded = false;			// Whether or not the player is grounded.
    [Tooltip("Target of groundcheck raycast")]
    public Transform[] groundChecks;

    [Header("Ledge Movement")]
    public LedgeDetector ledgeDetector;
    public float ledgeUpForce;
    private bool rightLedge;

    [Space(10)]
    public GameObject nadineCollider;

    private Animator anim;					
    private Rigidbody2D rb;
    private KnightController knight;

    private float crossHandTimer = 0f;
    private float defaultGravityScale;
    private float durationRun = 0;

    private Vector3 respawnPoint;

    [HideInInspector]
    public enum PlayerState {
        Grounded,
        OnAir,
        Crouching,
        OnLedge,
        WaitingLeap
    }
    private PlayerState state;

    void Awake()
	{
		anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        knight = GetComponent<KnightController>();

        defaultGravityScale = rb.gravityScale;
        respawnPoint = transform.position;
        state = PlayerState.Grounded;
    }

    void FixedUpdate ()
	{
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");
        if(h < 0.1f && h > -0.1f)
        {
            h = 0;
        }
        bool backwards = (h > 0 && !facingRight) || (h < 0 && facingRight);
        bool hold = Input.GetAxis("Hold") == 1;
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundChecks[0].position, 1 << LayerMask.NameToLayer("Ground"))
                    || Physics2D.Linecast(transform.position, groundChecks[1].position, 1 << LayerMask.NameToLayer("Ground"));
        
        //Set animation paramaters
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("VerSpeed", rb.velocity.y);
        anim.SetFloat("HorSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Speed", !hold && grounded ? Mathf.Abs(h) : 0);

        if (Input.GetAxis("Vertical") != -1)
            anim.SetBool("Crouch", false);

        if (grounded)
        {
            rb.gravityScale = defaultGravityScale;
            if (state == PlayerState.OnAir || state == PlayerState.WaitingLeap)
                state = PlayerState.Grounded;
        }
        else if (state == PlayerState.Grounded || state == PlayerState.Crouching)
        {
            state = PlayerState.OnAir;
        }

        //If encounter a ledge grab it
        if (state == PlayerState.OnAir && rb.velocity.y < -0.3)
        {
            if (ledgeDetector.DetectLedge(this))
            {
                state = PlayerState.OnLedge;
                ledgeDetector.ColliderSetActive(true);
            }
        }

        switch (state)
        {
            case PlayerState.Grounded:
                crossHandTimer += Time.fixedDeltaTime;
                if (crossHandTimer > 5f)
                {
                    anim.SetTrigger("Cross");
                    crossHandTimer = 0;
                }
                if (Input.anyKey)
                {
                    crossHandTimer = 0f;
                }


                //Run duration Calculation
                if (Input.GetAxis("Run") == 1 && h != 0)
                {
                    durationRun += Time.fixedDeltaTime;
                }
                else if (Input.GetAxis("Run") == 0 || h == 0 || backwards)
                {
                    durationRun = 0;
                }

                //Move the player
                if (h != 0 && !hold)
                {
                    rb.AddForce(new Vector2(h * moveForce * Time.fixedDeltaTime, 0));
                    float runSpeed;
                    //Decrease maxspeed if walking backwards, Increase while Sprinting
                    if (backwards && knight.IsHold())
                        runSpeed = maxSpeed * 0.5f;
                    else
                        runSpeed = durationRun > 1f ? maxSpeed * runMultiplier : maxSpeed;

                    LimitSpeed(runSpeed);
                }
                else if (rb.velocity.x != 0)
                {
                    Deaccelerate();
                }

                // Jump action
                if ((Input.GetButtonDown("Jump")))
                {
                    durationRun = 0;
                    anim.SetTrigger("Jump");
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(new Vector2(0f, jumpForce));
                }
                else if (Input.GetAxis("Vertical") == -1)
                {
                    state = PlayerState.Crouching;
                    anim.SetBool("Crouch", true);
                }

                // Flip player
                if (backwards && !knight.IsHold())
                    Flip();

                break;
            case PlayerState.OnAir:
                //Dynamic gravity scale for satisfying jump
                if (rb.velocity.y > 0.3 && !Input.GetButton("Jump"))
                {
                    rb.gravityScale = defaultGravityScale * releaseMultiplier;
                }
                else if (rb.velocity.y < -0.3)
                {
                    rb.gravityScale = defaultGravityScale * fallMultiplier;
                }

                //Move the player
                if (h != 0)
                {
                    rb.AddForce(new Vector2(h * moveForce * airControlMultiplier * Time.fixedDeltaTime, 0));

                    float runSpeed;
                    //Decrease maxspeed if walking backwards
                    if (backwards && knight.IsHold())
                        runSpeed = maxSpeed * 0.5f;
                    else
                        runSpeed = maxSpeed;
                    LimitSpeed(runSpeed);
                }

                // Flip player
                if (backwards && !knight.IsHold())
                    Flip();

                break;
            case PlayerState.Crouching:
                if (Input.GetAxis("Vertical") != -1)
                {
                    state = PlayerState.Grounded;
                    anim.SetBool("Crouch", false);
                }
                if (rb.velocity.x != 0)
                {
                    Deaccelerate();
                }
                // Flip player
                if (backwards && !knight.IsHold())
                    Flip();
                break;
            case PlayerState.OnLedge:
                if (Input.GetAxis("Vertical") == -1 && Input.GetAxis("Jump") == 1)
                {
                    state = PlayerState.OnAir;
                    ledgeDetector.ColliderSetActive(false);
                } else if (Input.GetButtonDown("Jump"))
                {
                    ledgeDetector.ColliderSetActive(false);
                    float delta = facingRight ? ledgeDetector.raycastDistance * -1.1f : ledgeDetector.raycastDistance * 1.1f;
                    transform.position = new Vector3(transform.position.x + delta, transform.position.y, transform.position.z);
                    rb.AddForce(new Vector2(0f, ledgeUpForce), ForceMode2D.Impulse);
                    rightLedge = facingRight;
                    state = PlayerState.WaitingLeap;
                    StartCoroutine(AfterLedge(ledgeDetector.transform.position.y));
                }
                break;
            case PlayerState.WaitingLeap:

                if ((h < 0 && rightLedge) || (h > 0 && !rightLedge))
                {
                    rb.AddForce(new Vector2(h * moveForce * airControlMultiplier * Time.fixedDeltaTime, 0));
                    float runSpeed;
                    //Decrease maxspeed if walking backwards
                    if (backwards && knight.IsHold())
                        runSpeed = maxSpeed * 0.5f;
                    else
                        runSpeed = maxSpeed;
                    LimitSpeed(runSpeed);
                }

                break;
        }

        if (Input.GetButtonDown("Knight"))
        {
            if(state == PlayerState.Crouching || state == PlayerState.Grounded)
                knight.Attack();
            else
                knight.Somersault();
        }
    }


    void LateUpdate()
    {
        RotateKnight();
    }

    IEnumerator AfterLedge(float height)
    {
        float time = 0f;
        while(groundChecks[0].position.y <= height && time < 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        state = PlayerState.OnAir;
    }

    void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    void LimitSpeed(float runSpeed)
    {
        if (rb.velocity.x > runSpeed)
            rb.velocity = new Vector2(runSpeed, rb.velocity.y);
        if (rb.velocity.x < runSpeed * -1)
            rb.velocity = new Vector2(runSpeed * -1, rb.velocity.y);
    }



    void RotateKnight()
    {
        //Rotating the knight
        float verticalAxis = Input.GetAxis("Vertical");
        float maxDegree = 30f;
        if (verticalAxis < 0)
        {
            if (state == PlayerState.OnAir)
                maxDegree = 90f;
            else
                verticalAxis = 0;
        }
        float rotation = verticalAxis * maxDegree;
        if (!facingRight)
            rotation = rotation * -1;
        knight.Rotate(rotation);
    }

    void Deaccelerate()
    {
        rb.AddForce(new Vector2(rb.velocity.x * -1 * horizontalDrag * Time.fixedDeltaTime, 0));
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
    }

    void OnDrawGizmos()
    {
        //Draw raycast
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, groundChecks[0].position);
        Gizmos.DrawLine(transform.position, groundChecks[1].position);

    }
}
