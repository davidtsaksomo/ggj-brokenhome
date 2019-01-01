using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.

	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
    public Transform[] groundChecks;

	private Animator anim;					// Reference to the player's animator component.
	private float crossHandTimer = 0f;
     
	void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast(transform.position, groundChecks[0].position, 1 << LayerMask.NameToLayer("Ground"))
                    || Physics2D.Linecast(transform.position, groundChecks[1].position, 1 << LayerMask.NameToLayer("Ground"));

        anim.SetBool("Grounded", grounded);
		anim.SetFloat ("VerSpeed", GetComponent<Rigidbody2D> ().velocity.y);
		anim.SetFloat ("HorSpeed", Mathf.Abs(GetComponent<Rigidbody2D> ().velocity.x));
		crossHandTimer += Time.deltaTime;
		if (crossHandTimer > 5f) {
			anim.SetTrigger ("Cross");
			crossHandTimer = 0;
		}
		if (Input.anyKey) {
			crossHandTimer = 0f;
		}

		// If the jump button is pressed and the player is grounded then the player should jump.
		if((Input.GetButtonDown("Jump")) && grounded)
			jump = true;
	}


	void FixedUpdate ()
	{
		// Cache the horizontal input.
		float h = Input.GetAxis("Horizontal");

		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat("Speed", Mathf.Abs(h));

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if(h !=0)
			// ... add a force to the player.
			GetComponent<Rigidbody2D>().velocity = new Vector2(h* maxSpeed,GetComponent<Rigidbody2D>().velocity.y);

		// If the input is moving the player right and the player is facing left...
		if(h > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if(h < 0 && facingRight)
			// ... flip the player.
			Flip();

		// If the player should jump...
		if(jump)
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger("Jump");
			//reset the y velocity

			GetComponent<Rigidbody2D> ().velocity = new Vector2(GetComponent<Rigidbody2D> ().velocity.x, 0f) ;

			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
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
