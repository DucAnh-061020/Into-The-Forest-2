using UnityEngine;

public class EnemyScripts : UnitScript
{
    [Header("Data")]
    public EnemyData EnemyData;
    [Header("Conditions")]
    public bool isDead = false;
    public bool isGrounded = false;
    public bool m_isRight = true;//face to the right
    public float dstToPlayer;
    [Header("Components")]
    public Transform Player;//get player
    public Transform m_attackPoint;
    public EnemyState m_currentState;

    public override void Start()
    {
        isDead = false;
        m_currentState = EnemyState.Idle;
        Player = null;
        base.Start();
    }

    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (isDead)
            return;

        if (Player != null)
        {
            LookAtTarget();
            if(dstToPlayer <= EnemyData.AttackRange)
            {
                m_currentState = EnemyState.Attack;
            }
            else
            {
                m_currentState = EnemyState.Chase;
            }
        }
        else FindTarget();

        if (isGrounded)
        {
            ApplyGroundDrag();
        }
        else
        {
            ApplyAirDrag();
        }
    }

    private void FixedUpdate()
    {
        if (Player != null)
            dstToPlayer = Vector2.Distance(transform.position, new Vector2(Player.transform.position.x, transform.position.y));
        AnimationUpdate();
    }

    private void ApplyAirDrag()
    {
        m_rigidbody2D.drag = 3;
        m_rigidbody2D.gravityScale = 10;
    }

    private void ApplyGroundDrag()
    {
        m_rigidbody2D.drag = 10;
        m_rigidbody2D.gravityScale = 1;
    }

    public enum EnemyState
    {
        Idle,
        Chase,
        Attack
    }

    private void FindTarget()
    {
        if (Player != null)
            return;
        try
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        catch { }
    }

    private void LookAtTarget()
    {
        if (transform.position.x > Player.position.x && m_isRight)
        {
            Flip();
        }
        else if (transform.position.x < Player.position.x && !m_isRight)
        {
            Flip();
        }
    }

    private void AnimationUpdate()
    {
        m_animator.SetFloat("Speed", Mathf.Abs(m_rigidbody2D.velocity.x));
        m_animator.SetBool("IsDead", isDead);
        m_animator.SetBool("IsGrounded", isGrounded);
    }

    private void Flip()
    {
        //Switch to where the obj is facing to
        m_isRight = !m_isRight;
        //Rotate target to the other direction
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 direction = transform.TransformDirection(Vector3.right) * EnemyData.AttackRange;
        Gizmos.DrawRay(transform.position, direction);
    }
}
