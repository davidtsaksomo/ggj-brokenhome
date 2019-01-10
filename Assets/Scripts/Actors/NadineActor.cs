using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    TODO:
    - Urus state crouching
    - Urus walk dan animasinya
 */

public class NadineActor : Actor
{
    [HideInInspector]
    public bool facingRight;

    [HideInInspector]
    public PositionStates positionState;

    [HideInInspector]
    public GazeStates gazeState;

    [HideInInspector]
    public MobilityStates mobilityState;

    private float crossHandTimer = 0f;
    private Animator animator;
    private Rigidbody2D rb;

    public override void Awake()
    {
        positionState = PositionStates.Grounded;
        mobilityState = MobilityStates.Idle;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // inherited variables
        spawnPosition = transform.position;
        controller = GetComponent<Controller>();
    }
    public void FixedUpdate()
    {
        float horizontalAxisInput = Input.GetAxis("Horizontal");
        float verticalAxisInput = Input.GetAxis("Vertical");

        bool grounded = (positionState == PositionStates.Grounded);
        bool hold = Input.GetAxis("Hold") == 1;

        animator.SetBool("Grounded", grounded);
        animator.SetFloat("VerSpeed", rb.velocity.y);
        animator.SetFloat("HorSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Speed", !hold && grounded ? Mathf.Abs(horizontalAxisInput) : 0);

        switch (positionState)
        {
            case PositionStates.Grounded:
                crossHandTimer += Time.fixedDeltaTime;
                if (crossHandTimer > 5f)
                {
                    animator.SetTrigger("Cross");
                    crossHandTimer = 0;
                }
                if (Input.anyKey)
                {
                    crossHandTimer = 0f;
                }

                if ((Input.GetButtonDown("Jump")))
                {
                    animator.SetTrigger("Jump");
                } else if (Input.GetAxis("Vertical") == -1)
                {
                    animator.SetBool("Crouch", true);
                }
                break;
        }
    }

    public override PositionStates GetPositionState()
    {
        return positionState;
    }
    public override void SetPositionState(PositionStates newState)
    {
        positionState = newState;
    }

    public override GazeStates GetGazeState()
    {
        return gazeState;
    }
    public override void SetGazeState(GazeStates newState)
    {
        gazeState = newState;
    }

    public override MobilityStates GetMobilityState()
    {
        return mobilityState;
    }
    public override void SetMobilityState(MobilityStates newState)
    {
        mobilityState = newState;
    }
}
