using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crouch : MonoBehaviour
{
    private Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
    }

    public void CrouchUpdate(float verticalInputAxis)
    {
        if (verticalInputAxis != -1 || actor.GetPositionState() != PositionStates.Grounded)
        {
            actor.SetCrouchBool(false);
        }

        switch (actor.GetPositionState())
        {
            case PositionStates.Grounded:
                if (verticalInputAxis == -1)
                {
                    actor.SetActionState(ActionStates.Crouching);
                    actor.SetMobilityState(MobilityStates.CannotMove);
                    actor.SetCrouchBool(true);
                }
                else if (actor.GetActionState() == ActionStates.Crouching)
                {
                    actor.SetActionState(ActionStates.Idle);
                    actor.SetMobilityState(MobilityStates.Idle);
                    actor.SetCrouchBool(false);
                }
                break;
        }
    }
}
