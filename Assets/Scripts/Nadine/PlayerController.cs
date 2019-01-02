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

	private Animator anim;					// Reference to the player's animator component.
	private float crossHandTimer = 0f;
    private Rigidbody2D rb;
    private float defaultGravityScale;
    public float durationRun = 0;

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        defaultGravityScale = rb.gravityScale;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void FixedUpdate ()
	{
        // The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
        grounded = Physics2D.Linecast(transform.position, groundChecks[0].position, 1 << LayerMask.NameToLayer("Ground"))
                    || Physics2D.Linecast(transform.position, groundChecks[1].position, 1 << LayerMask.NameToLayer("Ground"));
        // Cache the horizontal input.
        float h = Input.GetAxis("Horizontal");

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

        if (grounded)
        {
            rb.gravityScale = defaultGravityScale;
        }
        if (rb.velocity.y > 0.3 && !Input.GetButton("Jump"))
            rb.gravityScale = defaultGravityScale * releaseMultiplier;
        if (rb.velocity.y < -0.3 && !grounded)
            rb.gravityScale = defaultGravityScale * fallMultiplier;

        if(grounded && Input.GetAxis("Run") == 1 && h != 0)
        {
            durationRun += Time.fixedDeltaTime;
        } else if(Input.GetAxis("Run") == 0 || h == 0)
        {
            durationRun = 0;
        }

        // If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
        if (h != 0)
        {
            // ... add a force to the player.
            if (grounded)
                rb.AddForce(new Vector2(h * moveForce * Time.fixedDeltaTime, 0));
            else if(rb.velocity.x < maxSpeed)
                rb.AddForce(new Vector2(h * moveForce * airControlMultiplier * Time.fixedDeltaTime, 0));

            float runSpeed =   durationRun > 1f ? maxSpeed * runMultiplier : maxSpeed;
            if (rb.velocity.x > runSpeed)
                rb.velocity = new Vector2(runSpeed, rb.velocity.y);
            if (rb.velocity.x < runSpeed * -1)
                rb.velocity = new Vector2(runSpeed * -1, rb.velocity.y);
        }
        else if (grounded && rb.velocity.x != 0)
        {
            rb.AddForce(new Vector2(rb.velocity.x * -1 * horizontalDrag * Time.fixedDeltaTime, 0));
        }

        // If the input is moving the player right and the player is facing left...
        if (h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

		// If the player should jump...
		if((Input.GetButtonDown("Jump")) && grounded)
		{
            durationRun = 0;
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			//reset the y velocity
			rb.velocity = new Vector2(rb.velocity.x, 0f) ;
			// Add a vertical force to the player.
			rb.AddForce(new Vector2(0f, jumpForce));
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

}
