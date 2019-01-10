using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector_ : MonoBehaviour
{
    public float verticalDistance;
    public float raycastDistance;
    public Actor actor;

    private bool facingRight;

    public void Awake()
    {
        actor = GetComponent<Actor>();
    }

    public bool DetectLedge()
    {
        facingRight = actor.GetGazeState() == GazeStates.FacingRight;
        Vector2 point1 = transform.position;
        Vector2 point2 = new Vector2(point1.x, point1.y - verticalDistance);
        Vector2 dir = facingRight ? Vector2.right : Vector2.left;
        RaycastHit2D ray1 = Physics2D.Raycast(point1, dir, raycastDistance, 1 << LayerMask.NameToLayer("Ground"));
        RaycastHit2D ray2 = Physics2D.Raycast(point2, dir, raycastDistance, 1 << LayerMask.NameToLayer("Ground"));
        if (ray1.collider == null && ray2.collider != null)
            return true;
        return false;
    }
    
    public void ColliderSetActive(bool active)
    {
        GetComponent<BoxCollider2D>().enabled = active;
    }

    void OnDrawGizmos()
    {
        //Draw raycast
        Vector2 point1 = transform.position;
        Vector2 point2 = new Vector2(point1.x, point1.y - verticalDistance);
        Gizmos.color = Color.green;
        float distance = facingRight ? raycastDistance : raycastDistance * -1;
        Gizmos.DrawLine(point1, new Vector2(point1.x + distance, point1.y));
        Gizmos.DrawLine(point2, new Vector2(point2.x + distance, point2.y));

        //Draw collider
        Gizmos.color = Color.yellow;
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector2 center = facingRight ? new Vector2(transform.position.x + col.offset.x, transform.position.y + col.offset.y) 
                         : new Vector2(transform.position.x - col.offset.x, transform.position.y + col.offset.y);
        Gizmos.DrawWireCube(center, col.size);
    }
}
