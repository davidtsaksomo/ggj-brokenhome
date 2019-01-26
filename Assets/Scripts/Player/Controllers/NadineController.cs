using UnityEngine;

public class NadineController : MonoBehaviour
{
    private Actor actor;
    public static bool cannotMove = false; 

    void Awake()
    {
        actor = GetComponent<Actor>();
    }
    public void FixedUpdate()
    {
        if (cannotMove)
            return;

        float horizontalAxisInput = Input.GetAxis("Horizontal");
        float verticalAxisInput = Input.GetAxis("Vertical");
        bool jumpButtonDown = Input.GetButtonDown("Jump");
        bool jumpButton = Input.GetButton("Jump");
        float jumpAxis = Input.GetAxis("Jump");

        if (horizontalAxisInput < 0.1f && horizontalAxisInput > -0.1f)
        {
            horizontalAxisInput = 0;
        }
        GetComponent<NadineActor>().StateUpdate();
        GetComponent<NadineActor>().AnimationUpdate(horizontalAxisInput);
        GetComponent<Move>().MoveUpdate(horizontalAxisInput);
        GetComponent<Jump>().JumpUpdate(jumpButtonDown, jumpButton, jumpAxis, verticalAxisInput);

        bool backwards = (horizontalAxisInput > 0 && actor.GetGazeState() == GazeStates.FacingLeft || (horizontalAxisInput < 0 && actor.GetGazeState() == GazeStates.FacingRight));
        if(backwards)
            GetComponent<Flip>().FlipActor();
    }   
}
