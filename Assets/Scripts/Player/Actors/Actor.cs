using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{

    public GameObject actorCollider;
    public Vector3 spawnPosition;
    public Transform[] groundChecks;
    protected Animator animator;

    public virtual void Awake()
    {
        spawnPosition = transform.position;
    }

    public virtual void Respawn()
    {
        transform.position = spawnPosition;
    }

    public virtual void SetJumpTrigger()
    { }

    public virtual void SetCrouchBool(bool crouch)
    { }

    public virtual PositionStates GetPositionState()
    {
        return PositionStates.Unknown;
    }
    public virtual void SetPositionState(PositionStates newState)
    { }

    public virtual GazeStates GetGazeState()
    {
        return GazeStates.Unknown;
    }
    public virtual void SetGazeState(GazeStates newState)
    { }
}