using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Actor actor;
    private Rigidbody2D rb;

    [Tooltip("Force applied when moving horizontally")]
    public float moveForce = 50000f;
    [Tooltip("Max run speed")]
    public float maxSpeed = 6.2f;
    [Tooltip("Force applied to deaccelerate")]
    public float horizontalDrag = 15000f;
    [Tooltip("Max speed multiplier when sprinting")]
    [Range(1f, 5f)]
    public float runMultiplier = 1.5f;
    [Range(0f, 1f)]
    [Tooltip("Movement responsiveness while in air")]
    public float airControlMultiplier = 0.5f;

    public void Awake()
    {
        actor = GetComponent<Actor>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveUpdate(float horizontalAxisInput)
    {

        switch(actor.GetPositionState())
        {
            case PositionStates.Grounded:
                if (horizontalAxisInput != 0)
                {
                    rb.AddForce(new Vector2(horizontalAxisInput * moveForce * Time.fixedDeltaTime, 0));
                    LimitSpeed(maxSpeed);
                }
                else if (rb.velocity.x != 0)
                {
                    Deaccelerate();
                }
                break;
            case PositionStates.OnAir:
                if (horizontalAxisInput != 0)
                {
                    rb.AddForce(new Vector2(horizontalAxisInput * moveForce * airControlMultiplier * Time.fixedDeltaTime, 0));
                    LimitSpeed(maxSpeed);
                }
                break;
        }
    }

    void Deaccelerate()
    {
        rb.AddForce(new Vector2(rb.velocity.x * -1 * horizontalDrag * Time.fixedDeltaTime, 0));
    }

    void LimitSpeed(float movementSpeed)
    {
        if (rb.velocity.x > movementSpeed)
            rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
        if (rb.velocity.x < movementSpeed * -1)
            rb.velocity = new Vector2(movementSpeed * -1, rb.velocity.y);
    }
}
