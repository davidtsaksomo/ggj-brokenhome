using UnityEngine;

public class KnightController : MonoBehaviour
{
    [Tooltip("Reference to Knight Pivot Object")]
    public GameObject knight;
    [Tooltip("Reference to Lance Object")]
    public GameObject lance;

    [Header("Ground Attack")]
    [Tooltip("Attack Hitbox Size")]
    public Vector2 hitbox;
    [Tooltip("Time before combo resets")]
    public float attackStandbyTime;
    [Tooltip("Minimum time between attack command")]
    public float betweenAttackTime;
    [Tooltip("Force applied to enemy when attack connects")]
    public float attackForce;

    [Header("Air Attack")]
    public float hitRadius;
    [Tooltip("Duration of doing somersault")]
    public float somersaultTime;
    [Tooltip("Force applied to enemy when attack connects")]
    public float airAttackForce;

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

            Collider2D[] enemies = Physics2D.OverlapBoxAll(lance.transform.position, hitbox, 0f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider2D col in enemies)
            {
                col.GetComponent<Enemy>().TakeDamage(1);
                float force = GetComponent<PlayerController>().facingRight ? attackForce : -1 * attackForce;
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
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
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, hitRadius, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider2D col in enemies)
            {
                col.GetComponent<Enemy>().TakeDamage(2);
                Vector3 normal = col.transform.position - transform.position;
                normal.Normalize();
                col.GetComponent<Rigidbody2D>().AddForce(airAttackForce * normal, ForceMode2D.Impulse);
            }
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
        return state == KnightState.Somersault;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(lance.transform.position, hitbox);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
