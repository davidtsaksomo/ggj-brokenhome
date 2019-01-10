using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Jump Movement")]
    [Tooltip("Force applied to do jumps")]
    public float jumpForce = 12200;

    [Tooltip("Gravity multiplier when falling")]
    public float fallMultiplier = 2.5f;

    [Tooltip("Gravity multiplier when releasing jump button")]
    public float releaseMultiplier = 2f;

    [Tooltip("Multiplier of horizontal control when on air")]
    [Range(0f, 1f)]
    public float airControlMultiplier = 0.5f;

    [Tooltip("Force aplied when jump while grabbing ledge")]
    public float ledgeUpForce;
    private bool rightLedge;

    [Space(10)]
    public Transform[] groundChecks;

    private Actor actor;
    private LedgeDetector_ ledgeDetector;
    private Rigidbody2D rb;
    private float defaultGravityScale;
    private bool facingRight;

    IEnumerator AfterLedge(float height)
    {
        float time = 0f;
        while(groundChecks[0].position.y <= height && time < 0.1f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        actor.SetPositionState(PositionStates.OnAir);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        actor = GetComponent<Actor>();
        facingRight = (actor.GetGazeState() == GazeStates.FacingRight);
        ledgeDetector = GetComponent<LedgeDetector_>();
        defaultGravityScale = rb.gravityScale;
    }

    void FixedUpdate()
    {
        facingRight = (actor.GetGazeState() == GazeStates.FacingRight);
        
        if (isGrounded())
        {
            rb.gravityScale = defaultGravityScale;
            actor.SetPositionState(PositionStates.Grounded);
        }

        if (ledgeDetector != null && actor.GetPositionState() == (PositionStates.OnAir) && rb.velocity.y < -0.3)
        {
            if (ledgeDetector.DetectLedge())
            {
                actor.SetPositionState(PositionStates.OnLedge);
                ledgeDetector.ColliderSetActive(true);
            }
        }

        switch (actor.GetPositionState())
        {
            case PositionStates.Grounded:
                if ((Input.GetButtonDown("Jump")))
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(new Vector2(0f, jumpForce));
                }
                actor.SetPositionState(PositionStates.OnAir);
                break;

            case PositionStates.OnAir:
                if (rb.velocity.y > 0.3 && !Input.GetButton("Jump"))
                {
                    rb.gravityScale = defaultGravityScale * releaseMultiplier;
                }
                else if (rb.velocity.y < -0.3)
                {
                    rb.gravityScale = defaultGravityScale * fallMultiplier;
                }
                actor.SetPositionState(PositionStates.OnAir);
                break;

            case PositionStates.OnLedge:
                if (Input.GetAxis("Vertical") == -1 && Input.GetAxis("Jump") == 1)
                {
                    actor.SetPositionState(PositionStates.OnAir);
                    ledgeDetector.ColliderSetActive(false);
                } else if (Input.GetButtonDown("Jump"))
                {
                    ledgeDetector.ColliderSetActive(false);
                    float delta = facingRight ? ledgeDetector.raycastDistance * -1.1f : ledgeDetector.raycastDistance * 1.1f;
                    transform.position = new Vector3(transform.position.x + delta, transform.position.y, transform.position.z);
                    rb.AddForce(new Vector2(0f, ledgeUpForce), ForceMode2D.Impulse);
                    rightLedge = facingRight;
                    actor.SetPositionState(PositionStates.WaitingLeap);
                    StartCoroutine(AfterLedge(ledgeDetector.transform.position.y));
                }
                actor.SetPositionState(PositionStates.OnAir);
                break;
        }
    }

    bool isGrounded()
    {
        foreach (Transform gc in groundChecks)
        {
            if (!Physics2D.Linecast(transform.position, gc.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                return false;
            }
        }
        return true;
    }
}
