using UnityEngine;

public class SwordController : MonoBehaviour
{
    [Tooltip("Reference to Sword Pivot Object")]
    public GameObject swordPivot;
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

    private SwordActor swordActor;
    private Actor actor;

    void Awake()
    {
        actor = GetComponent<Actor>();
        swordActor = GetComponent<SwordActor>();
    }

    void Update()
    {
        float shieldAxis = Input.GetAxis("Shield");
        bool knightButtonDown = Input.GetButtonDown("Knight");

        if (knightButtonDown)
        {
            if (actor.GetPositionState() == PositionStates.Grounded)
                Attack();
            else
                Somersault();
        }
        swordActor.StateUpdate(shieldAxis);

    }

    void LateUpdate()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float maxDegree = 60f;
        if (verticalAxis < 0)
        {
            if (actor.GetPositionState() == PositionStates.OnAir)
                maxDegree = 90f;
            else
                verticalAxis = 0;
        }
        float rotation = verticalAxis * maxDegree;
        if (actor.GetGazeState() == GazeStates.FacingLeft)
            rotation = rotation * -1;
        Rotate(rotation);
    }

    public void Attack()
    {
        if (swordActor.timer <= 0 && (swordActor.GetSwordState() == SwordStates.NonActive || swordActor.GetSwordState() == SwordStates.Standby))
        {
            swordActor.timer = betweenAttackTime;
            swordPivot.SetActive(true);

            switch (swordActor.GetAttackPhase())
            {
                case SwordAttackPhase.Phase1:
                    swordActor.SetAttackTrigger(1);

                    swordActor.SetAttackPhase(SwordAttackPhase.Phase2);
                    break;
                case SwordAttackPhase.Phase2:
                    swordActor.SetAttackTrigger(2);
                    swordActor.SetAttackPhase(SwordAttackPhase.Phase3);
                    break;
                case SwordAttackPhase.Phase3:
                    swordActor.SetAttackTrigger(3);
                    swordActor.SetAttackPhase(SwordAttackPhase.Phase1);
                    break;
            }

            Collider2D[] enemies = Physics2D.OverlapBoxAll(lance.transform.position, hitbox, 0f, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (Collider2D col in enemies)
            {
                col.GetComponent<Enemy>().TakeDamage(1);
                float force = actor.GetGazeState() == GazeStates.FacingRight ? attackForce : -1 * attackForce;
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
            }

            swordActor.standbyTimer = attackStandbyTime;
            swordActor.SetSwordState(SwordStates.Standby);

        }
    }

    public void Somersault()
    {
        if (swordActor.GetSwordState() != SwordStates.Somersault)
        {
            swordPivot.SetActive(true);
            swordActor.SetSomersaultTrigger();
            swordActor.SetSwordState(SwordStates.Somersault);
            swordActor.somersaultTimer = somersaultTime;
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
        if (swordActor.GetSwordState() != SwordStates.NonActive && swordActor.GetSwordState() != SwordStates.Somersault)
        {
            swordPivot.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rotation);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(lance.transform.position, hitbox);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, hitRadius);
    }
}
