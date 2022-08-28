using UnityEngine;

public class GolemScript : UnitScript
{
    [Header("Data")]
    public BossData m_golemData;
    public GolemPhase m_phase;

    [Header("Conditions")]
    public bool m_isDead = false;
    public bool m_isActivated = false;
    public bool m_isAttacking = false;
    public bool m_isGrounded = false;
    public bool m_isRight = true;
    public bool m_canChaseTarget = false;
    public bool m_canTakeDamage = true;
    public float m_dstToPlayer;

    [Header("Components")]
    public Transform m_player;
    public Transform m_attackPoint;
    public Transform m_firePoint;
    public Transform m_lazerPoint;

    [Header("Ground control")]
    [SerializeField]
    private float m_groundDrag;
    [SerializeField]
    private float m_groundGravity;

    [Header("Air control")]
    [SerializeField]
    private float m_airDrag;
    [SerializeField]
    private float m_airGravity;

    public override void Start()
    {
        m_phase = GolemPhase.Phase0;
        GameEvents.StartBoss += Activate;
        base.Start();
    }

    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (m_isDead)
            return;
    }

    private void FixedUpdate()
    {
        AnimationUpdate();
        if (!m_isActivated)
        {
            return;
        }
        if (m_isDead)
            return;
        if (m_player != null)
            m_dstToPlayer = Vector2.Distance(transform.position, new Vector2(m_player.transform.position.x, transform.position.y));
        if (m_isGrounded)
        {
            ApplyGroundDrag();
        }
        else
        {
            ApplyAirDrag();
        }
        FindTarget();
        LookAtTarget();
    }

    private void ApplyAirDrag()
    {
        m_rigidbody2D.drag = m_airDrag;
        m_rigidbody2D.gravityScale = m_airGravity;
    }

    private void ApplyGroundDrag()
    {
        m_rigidbody2D.drag = m_groundDrag;
        m_rigidbody2D.gravityScale = m_groundGravity;
    }

    private void AnimationUpdate()
    {
        m_animator.SetBool("IsDead", m_isDead);
        m_animator.SetBool("IsGrounded", m_isGrounded);
        m_animator.SetBool("IsActivated", m_isActivated);
    }

    private void FindTarget()
    {
        if (m_player != null)
            return;
        try
        {
            m_player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch { }
    }

    private void LookAtTarget()
    {
        if (m_isAttacking)
            return;
        if (transform.position.x > m_player.position.x && m_isRight)
        {
            Flip();
        }
        else if (transform.position.x < m_player.position.x && !m_isRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        //Switch to where the obj is facing to
        m_isRight = !m_isRight;
        //Rotate target to the other direction
        transform.Rotate(0f, 180f, 0f);
    }

    public enum GolemPhase
    {
        Phase0,
        Phase1,
        Phase2
    }

    public void GolemDestroyed()
    {
        if(m_isDead)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameEvents.StartBoss -= Activate;
        GameEvents.OnGameWon();
    }

    private void Activate()
    {
        m_isActivated = true;
    }

    private void ChaseSet()
    {
        m_canChaseTarget = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.right) * m_golemData.AttackRange;
        Gizmos.DrawRay(transform.position, direction);
    }
}