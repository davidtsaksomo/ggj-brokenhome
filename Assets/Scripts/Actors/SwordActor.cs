using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordActor : MonoBehaviour
{
    [Tooltip("Reference to Sword Pivot Object")]
    public GameObject swordPivot;
    public SwordStates swordState;
    public SwordAttackPhase attackPhase;

    private Animator animator;

    [HideInInspector]
    public float timer = 0f;
    [HideInInspector]
    public float standbyTimer = 0f;
    [HideInInspector]
    public float somersaultTimer = 0f;

    public void Awake()
    {
        swordState = SwordStates.NonActive;
        animator =swordPivot.GetComponent<Animator>();
    }

    public void StateUpdate(float shieldAxis)
    {
        switch (swordState)
        {
            case SwordStates.NonActive:
                if (shieldAxis == 1)
                {
                    swordPivot.SetActive(true);
                    SetShieldBool(true);
                    SetSwordState(SwordStates.Shield);
                }
                break;
            case SwordStates.Shield:
                if (shieldAxis == 0)
                {
                    SetShieldBool(false);
                    swordPivot.SetActive(false);
                    SetSwordState(SwordStates.NonActive);
                }
                break;
            case SwordStates.Somersault:
                if (somersaultTimer <= 0f)
                {
                    SetSwordState(SwordStates.NonActive);
                    swordPivot.SetActive(false);
                }
                else
                {
                    somersaultTimer -= Time.deltaTime;
                }
                break;
            case SwordStates.Standby:
                if (shieldAxis == 1)
                {
                    swordPivot.SetActive(true);
                    SetShieldBool(true);
                    SetSwordState(SwordStates.Shield);

                }
                else if (standbyTimer <= 0)
                {
                    swordPivot.SetActive(false);
                    SetAttackPhase(SwordAttackPhase.Phase1);
                    SetSwordState(SwordStates.NonActive);

                }
                else
                {
                    standbyTimer -= Time.deltaTime;
                }
                break;
        }
        if (timer > 0)
            timer -= Time.deltaTime;
    }

    public SwordStates GetSwordState()
    {
        return swordState;
    }

    public void SetSwordState(SwordStates newState)
    {
        swordState = newState;
    }

    public SwordAttackPhase GetAttackPhase()
    {
        return attackPhase;
    }

    public void SetAttackPhase(SwordAttackPhase newPhase)
    {
        attackPhase = newPhase;
    }

    public void SetSomersaultTrigger()
    {
        animator.SetTrigger("somersault");
    }

    public void SetShieldBool(bool shield)
    {
        animator.SetBool("shield", shield);
    }

    public void SetAttackTrigger(int phase)
    {
        animator.SetTrigger("attack"+phase);
    }
}

