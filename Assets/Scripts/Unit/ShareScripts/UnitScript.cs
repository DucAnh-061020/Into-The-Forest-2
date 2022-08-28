using UnityEngine;

public abstract class UnitScript : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D m_rigidbody2D;
    public Animator m_animator;
    public CapsuleCollider2D m_capCollider;
    public bool m_grounded;
    public bool m_isAlive = true;
    [Header("Layer Masks")]
    public LayerMask m_targetLayer;
    public LayerMask m_groundLayer;

    public virtual void Start()
    {
    }

    public virtual void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_capCollider = GetComponent<CapsuleCollider2D>();
    }
}
