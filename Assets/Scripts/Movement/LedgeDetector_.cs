using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector_ : MonoBehaviour
{
    public float verticalDistance;
    public float raycastDistance;
    public GameObject ledgeDetector;

    [SerializeField]
    private Actor actor;
    private Rigidbody2D rb;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void LedgeDetectorUpdate()
    {
        if (actor.GetPositionState() == PositionStates.OnAir && rb.velocity.y < -0.3)
        {
            if (DetectLedge(actor))
            {
                actor.SetPositionState(PositionStates.OnLedge);
                ColliderSetActive(true);
            }
        }
    }

    public bool DetectLedge(Actor actor)
    {
        Vector2 point1 = ledgeDetector.transform.position;
        Vector2 point2 = new Vector2(point1.x, point1.y - verticalDistance);
        Vector2 dir = actor.GetGazeState() == GazeStates.FacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D ray1 = Physics2D.Raycast(point1, dir, raycastDistance, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D ray2 = Physics2D.Raycast(point2, dir, raycastDistance, 1 << LayerMask.NameToLayer("Ground"));
        if (ray1.collider == null && ray2.collider != null)
            return true;
        return false;
    }

    public void ColliderSetActive(bool active)
    {
        ledgeDetector.GetComponent<BoxCollider2D>().enabled = active;
    }

    void OnDrawGizmos()
    {
        //Draw raycast
        Vector2 point1 = ledgeDetector.transform.position;
        Vector2 point2 = new Vector2(point1.x, point1.y - verticalDistance);
        Gizmos.color = Color.green;
        float distance = actor.GetGazeState() == GazeStates.FacingRight ? raycastDistance : raycastDistance * -1;
        Gizmos.DrawLine(point1, new Vector2(point1.x + distance, point1.y));
        Gizmos.DrawLine(point2, new Vector2(point2.x + distance, point2.y));

        //Draw collider
        Gizmos.color = Color.yellow;
        BoxCollider2D col = ledgeDetector.GetComponent<BoxCollider2D>();
        Vector2 center = actor.GetGazeState() == GazeStates.FacingRight ? new Vector2(ledgeDetector.transform.position.x + col.offset.x, ledgeDetector.transform.position.y + col.offset.y)
                         : new Vector2(ledgeDetector.transform.position.x - col.offset.x, ledgeDetector.transform.position.y + col.offset.y);
        Gizmos.DrawWireCube(center, col.size);
    }
}
