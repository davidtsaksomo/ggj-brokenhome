using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadineController : MonoBehaviour
{
    private Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
    }
    public void FixedUpdate()
    {
        float horizontalAxisInput = Input.GetAxis("Horizontal");
        float verticalAxisInput = Input.GetAxis("Vertical");
        bool hold = Input.GetAxis("Hold") == 1;
        bool jumpButtonDown = Input.GetButtonDown("Jump");
        bool jumpButton = Input.GetButton("Jump");
        float jumpAxis = Input.GetAxis("Jump");


        if (horizontalAxisInput < 0.1f && horizontalAxisInput > -0.1f)
        {
            horizontalAxisInput = 0;
        }
        GetComponent<NadineActor>().StateUpdate(hold);
        GetComponent<NadineActor>().AnimationUpdate(horizontalAxisInput, verticalAxisInput, hold);
        GetComponent<LedgeDetector_>().LedgeDetectorUpdate();
        GetComponent<Move>().MoveUpdate(horizontalAxisInput);
        GetComponent<Jump>().JumpUpdate(jumpButtonDown, jumpButton, jumpAxis, verticalAxisInput);

        bool backwards = (horizontalAxisInput > 0 && actor.GetGazeState() == GazeStates.FacingLeft || (horizontalAxisInput < 0 && actor.GetGazeState() == GazeStates.FacingRight));
        if(backwards)
            GetComponent<Flip>().FlipActor();

    }
}
