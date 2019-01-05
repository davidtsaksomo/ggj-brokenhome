using UnityEngine;

public class KnightController : MonoBehaviour
{
    public GameObject knight;


    public float attackStandbyTime;
    public float betweenAttackTime;
    public float somersaultTime;


    private float timer = 0f;
    private float standbyTimer = 0f;
    private float somersaultTimer = 0f;


    [HideInInspector]
    public KnightState state = KnightState.NonActive;
    private AttackPhase phase = AttackPhase.Phase1;
    private Animator anim;

    public enum KnightState
    {
        NonActive,
        Shield,
        Somersault,
        Standby
    }
    private enum AttackPhase
    {
        Phase1,
        Phase2,
        Phase3
    }

    void Start()
    {
        anim = knight.GetComponent<Animator>();
    }

    void Update()
    {
        switch (state)
        {
            case KnightState.NonActive:
                if (Input.GetAxis("Shield") == 1)
                {
                    knight.SetActive(true);
                    anim.SetBool("shield", true);
                    state = KnightState.Shield;
                }
                break;
            case KnightState.Shield:
                if (Input.GetAxis("Shield") == 0)
                {
                    anim.SetBool("shield", false);
                    knight.SetActive(false);
                    state = KnightState.NonActive;

                }
                break;
            case KnightState.Somersault:
                if (somersaultTimer <= 0f)
                {
                    state = KnightState.NonActive;
                    knight.SetActive(false);
                } else
                {
                    somersaultTimer -= Time.deltaTime;
                }
                break;
            case KnightState.Standby:
                if (Input.GetAxis("Shield") == 1)
                {
                    knight.SetActive(true);
                    anim.SetBool("shield", true);
                    state = KnightState.Shield;
                }
                else if(standbyTimer <= 0)
                {
                    knight.SetActive(false);
                    phase = AttackPhase.Phase1;
                    state = KnightState.NonActive;
                } else
                {
                    standbyTimer -= Time.deltaTime;
                }
                break;
        }
        if(timer > 0)
            timer -= Time.deltaTime;

    }

    public void Attack()
    {
        if(timer <= 0 && (state == KnightState.NonActive || state == KnightState.Standby))
        {
            timer = betweenAttackTime;
            knight.SetActive(true);
            switch (phase)
            {
                case AttackPhase.Phase1:
                    anim.SetTrigger("attack1");
                    phase = AttackPhase.Phase2;
                    break;
                case AttackPhase.Phase2:
                    anim.SetTrigger("attack2");
                    phase = AttackPhase.Phase3;
                    break;
                case AttackPhase.Phase3:
                    anim.SetTrigger("attack3");
                    phase = AttackPhase.Phase1;
                    break;
            }
            standbyTimer = attackStandbyTime;
            state = KnightState.Standby;
        }
    }

    public void Somersault()
    {
        if(state != KnightState.Somersault)
        {
            knight.SetActive(true);
            anim.SetTrigger("somersault");
            state = KnightState.Somersault;
            somersaultTimer = somersaultTime;
        }

    }

    public void Rotate(float rotation)
    {
        if(state != KnightState.NonActive && state != KnightState.Somersault)
        {
            knight.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }

    public bool IsHold()
    {
        return state != KnightState.NonActive;
    }
}
