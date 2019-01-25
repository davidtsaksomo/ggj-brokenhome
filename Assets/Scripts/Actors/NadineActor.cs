using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadineActor : Actor
{

    public PositionStates positionState;
    public GazeStates gazeState;


    private float crossHandTimer = 0f;
    private Rigidbody2D rb;



    public override void Awake()
    {
        positionState = PositionStates.Grounded;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        // inherited variables
        spawnPosition = transform.position;
    }


    public void StateUpdate()
    {
        if (IsGrounded())
        {
            if (positionState == PositionStates.OnAir)
                positionState = PositionStates.Grounded;
        }
        else if (positionState == PositionStates.Grounded)
        {
            positionState = PositionStates.OnAir;
        }
    }

    public void AnimationUpdate(float horizontalAxisInput)
    {
        bool grounded = (positionState == PositionStates.Grounded);
        animator.SetBool("Grounded", grounded);
        animator.SetFloat("VerSpeed", rb.velocity.y);
        animator.SetFloat("HorSpeed", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("Speed", grounded ? Mathf.Abs(horizontalAxisInput) : 0);


        switch (positionState)
        {
            case PositionStates.Grounded:

                if (Input.anyKey)
                {
                    crossHandTimer = 0f;
                } else
                {
                    crossHandTimer += Time.fixedDeltaTime;
                    if (crossHandTimer > 5f)
                    {
                        animator.SetTrigger("Cross");
                        crossHandTimer = 0;
                    }
                }
                break;
        }
    }

    bool IsGrounded()
    {
        foreach (Transform gc in groundChecks)
        {
            if (Physics2D.Linecast(transform.position, gc.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                return true;
            }
        }
        return false;
    }

    public override void SetJumpTrigger()
    {
        animator.SetTrigger("Jump");
    }

    public override void SetCrouchBool(bool crouch)
    {
        animator.SetBool("Crouch", crouch);
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

}

