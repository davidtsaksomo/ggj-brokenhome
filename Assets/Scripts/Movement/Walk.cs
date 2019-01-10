using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MonoBehaviour
{
    private Actor actor;
    private Rigidbody2D rb;
    private bool facingRight;
    private bool cannotMove;

    public void Awake()
    {
        actor = GetComponent<Actor>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        float horizontalAxisInput = actor.controller.horizontalAxisInput;
        if(horizontalAxisInput < 0.1f && horizontalAxisInput > -0.1f)
        {
            actor.SetMobilityState(MobilityStates.Idle);
        }
        facingRight = (actor.GetGazeState() == GazeStates.FacingRight);
        bool backwards = (horizontalAxisInput > 0 && facingRight || (horizontalAxisInput < 0 && facingRight)); // di NadineActor?
        bool cannotMove = (actor.GetMobilityState() == MobilityStates.CannotMove);

        switch(actor.GetPositionState())
        {
            case PositionStates.Grounded:
                if (horizontalAxisInput != 0 && !cannotMove)
                {
                    rb.AddForce(new Vector2(horizontalAxisInput * actor.controller.moveForce * Time.fixedDeltaTime, 0));
                    actor.SetMobilityState(MobilityStates.Moving);
                    LimitSpeed(actor.controller.maxSpeed);
                }
                else if (rb.velocity.x != 0)
                {
                    Deaccelerate();
                }
                break;
            case PositionStates.OnAir:
                if (horizontalAxisInput != 0)
                {
                    rb.AddForce(new Vector2(horizontalAxisInput * actor.controller.moveForce * actor.controller.airControlMultiplier * Time.fixedDeltaTime, 0));
                    actor.SetMobilityState(MobilityStates.Moving);
                    LimitSpeed(actor.controller.maxSpeed);
                }
                break;
            case PositionStates.WaitingLeap:
                bool rightLedge = (actor.GetGazeState() == GazeStates.FacingRight);
                if ((horizontalAxisInput < 0 && rightLedge) || (horizontalAxisInput > 0 && !rightLedge))
                {
                    rb.AddForce(new Vector2(horizontalAxisInput * actor.controller.moveForce * actor.controller.airControlMultiplier * Time.fixedDeltaTime, 0));
                    actor.SetMobilityState(MobilityStates.Moving);
                    LimitSpeed(actor.controller.maxSpeed);
                }
                break;
            case PositionStates.OnLedge:
                break;
        }
    }

    void Deaccelerate()
    {
        rb.AddForce(new Vector2(rb.velocity.x * -1 * actor.controller.horizontalDrag * Time.fixedDeltaTime, 0));
    }

    void LimitSpeed(float movementSpeed)
    {
        if (rb.velocity.x > movementSpeed)
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        if (rb.velocity.x < movementSpeed * -1)
            rb.velocity = new Vector2(movementSpeed * -1, rb.velocity.y);
    }
}
