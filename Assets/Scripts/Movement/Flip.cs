using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    private Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
    }
    public void FlipActor()
    {

        switch (actor.GetPositionState())
        {
            case PositionStates.Grounded:
            case PositionStates.OnAir:
                actor.SetGazeState(actor.GetGazeState() == GazeStates.FacingRight ? GazeStates.FacingLeft : GazeStates.FacingRight);
                Vector3 theScale = transform.localScale;
                theScale.x *= -1;
                transform.localScale = theScale;
                break;
        }
    }
}
