using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Tooltip("Force applied to do jumps")]
    public float jumpForce = 12200;

    [Tooltip("Gravity multiplier when falling")]
    public float fallMultiplier = 2.5f;

    [Tooltip("Gravity multiplier when releasing jump button")]
    public float releaseMultiplier = 2f;

    [Tooltip("Force aplied when jump while grabbing ledge")]
    public float ledgeUpForce;

    private Actor actor;
    private Rigidbody2D rb;
    private float defaultGravityScale;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        actor = GetComponent<Actor>();
        defaultGravityScale = rb.gravityScale;
    }

    public void JumpUpdate(bool jumpButtonDown, bool jumpButton, float jumpAxis, float verticalAxisInput)
    {
        switch (actor.GetPositionState())
        {
            case PositionStates.Grounded:
                rb.gravityScale = defaultGravityScale;

                if (jumpButtonDown)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(new Vector2(0f, jumpForce));
                    actor.SetJumpTrigger();

                }

                break;

            case PositionStates.OnAir:
                if (rb.velocity.y > 0.3 && !jumpButton)
                {
                    rb.gravityScale = defaultGravityScale * releaseMultiplier;
                }
                else if (rb.velocity.y < -0.3)
                {
                    rb.gravityScale = defaultGravityScale * fallMultiplier;
                }
                actor.SetPositionState(PositionStates.OnAir);
                break;
            
        }
    }
}
