using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
    public float horizontalDrag = 0.3f;
    public float runMultiplier = 1.5f;
    public float fallMultiplier = 2.5f;
    public float releaseMultiplier = 2f;
    public float airControlMultiplier = 0.5f;

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
    public Transform[] groundChecks;

	private Animator anim;					
    private Rigidbody2D rb;
    private KnightController knight;

    private float crossHandTimer = 0f;
    private float defaultGravityScale;
    public float durationRun = 0;

    void Awake()
	{
		anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        knight = GetComponent<KnightController>();

        defaultGravityScale = rb.gravityScale;
    }

    void FixedUpdate ()
	{
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundChecks[0].position, 1 << LayerMask.NameToLayer("Ground"))
                    || Physics2D.Linecast(transform.position, groundChecks[1].position, 1 << LayerMask.NameToLayer("Ground"));
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");
        bool backwards = (h > 0 && !facingRight) || (h < 0 && facingRight);
        //Set animation paramaters
        anim.SetBool("Grounded", grounded);
        anim.SetFloat("VerSpeed", rb.velocity.y);
        anim.SetFloat("HorSpeed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("Speed", Mathf.Abs(h));

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

        //Dynamic gravity scale for satisfying jump
        if (grounded)
        {
            rb.gravityScale = defaultGravityScale;
        }
        if (rb.velocity.y > 0.3 && !Input.GetButton("Jump"))
            rb.gravityScale = defaultGravityScale * releaseMultiplier;
        if (rb.velocity.y < -0.3 && !grounded)
            rb.gravityScale = defaultGravityScale * fallMultiplier;

        //Run duration Calculation
        if(grounded && Input.GetAxis("Run") == 1 && h != 0)
        {
            durationRun += Time.fixedDeltaTime;
        } else if(Input.GetAxis("Run") == 0 || h == 0 || backwards)
        {
            durationRun = 0;
        }

        //Move the player
        if (h != 0)
        {
            if (grounded)
                //Ground movement
                rb.AddForce(new Vector2(h * moveForce * Time.fixedDeltaTime, 0));
            else if(rb.velocity.x < maxSpeed)
                //Air Movement, move dampened
                rb.AddForce(new Vector2(h * moveForce * airControlMultiplier * Time.fixedDeltaTime, 0));

            //Increase maxspeed when sprinting
            float runSpeed; 
            //Decrease maxspeed if walking backwards
            if (backwards && knight.active)
                runSpeed = maxSpeed * 0.5f;
            else
                runSpeed = durationRun > 1f ? maxSpeed * runMultiplier : maxSpeed;

            if (rb.velocity.x > runSpeed)
                rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            if (rb.velocity.x < runSpeed * -1)
                rb.velocity = new Vector2(runSpeed * -1, rb.velocity.y);
        }
        else if (grounded && rb.velocity.x != 0)
        {
            //Braking
            rb.AddForce(new Vector2(rb.velocity.x * -1 * horizontalDrag * Time.fixedDeltaTime, 0));
        }

        // Flip player
        if (backwards && !knight.active)
			Flip();

		// Jump action
		if((Input.GetButtonDown("Jump")) && grounded)
		{
            durationRun = 0;
			anim.SetTrigger("Jump");
			rb.velocity = new Vector2(rb.velocity.x, 0f) ;
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
