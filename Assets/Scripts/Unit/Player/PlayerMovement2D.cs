using System;
using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    [Header("Layer Masks")]
    [Header("Collisions Variables")]
    [SerializeField] private float m_groundRaycast = .1f;

    [Header("Movement Variables")]
    [SerializeField] private float m_groundLinearDrag;
    private float m_horizontalDirection;
    private float m_verticalDirection;
    private bool m_facingRight = true;
    private bool m_changingDirection => (playerScript.m_rigidbody2D.velocity.x > 0f && m_horizontalDirection < 0f) || (playerScript.m_rigidbody2D.velocity.x < 0f && m_horizontalDirection > 0f);

    [Header("Jump Variables")]
    [SerializeField] private float m_lowJumpGravity;
    [SerializeField] private float m_highJumpGravity;
    [SerializeField] private float m_coyotyTime = 0.2f;
    [SerializeField] private float m_airLinearDrag = 1f;
    [SerializeField] private float m_jumpBuffer = 0.1f;

    [Header("Components")]
    private float m_jumpBufferCounter;
    private float m_coyotyCounter;
    [SerializeField]
    private Player playerScript;
    [SerializeField]
    private PlayerData playerData;
    private void Start()
    {
    }

    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    private void Update()
    {
        CheckCollisions();
        FallMultiplier();
        CoyotyUpdate();

        if (playerScript.m_isAttacking)
        {
            StopMovement();
            return;
        }

        m_horizontalDirection = GetInput().x;
        m_verticalDirection = GetInput().y;

        JumpInput();
        CrouchInput();
        MoveCharacter();

        if(playerScript.m_grounded)
        {
            ApplyGroundLinearDrag();
            m_coyotyCounter = m_coyotyTime;
        }
        else
        {
            ApplyAirLignerDrag();
            m_coyotyCounter -= Time.deltaTime;
        }

        if (playerScript.m_canJump && m_jumpBufferCounter > 0f) Jump();
    }

    private void FixedUpdate()
    {
        UpdateAnimations();
    }

    private void CoyotyUpdate()
    {
        if (m_coyotyCounter > 0f)
        {
            playerScript.m_canJump = true;
            return;
        }
        playerScript.m_canJump = false;
        
    }

    private void CrouchInput()
    {
        if (!playerScript.m_grounded)
            return;
        if (Input.GetButtonDown("Crouch"))
        {
            playerScript.m_crouch = true;
            return;
        }
        if (Input.GetButtonUp("Crouch"))
        {
            playerScript.m_crouch = false;
            return;
        }
    }

    private void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            m_jumpBufferCounter = m_jumpBuffer;
            return;
        }
        m_jumpBufferCounter -= Time.deltaTime;
    }

    private void StopMovement()
    {
        playerScript.m_crouch = false;
        m_horizontalDirection = 0;
        if (playerScript.m_grounded)
        {
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLignerDrag();
        }
    }

    private void UpdateAnimations() {
        playerScript.m_animator.SetFloat("Speed", Mathf.Abs(m_horizontalDirection));
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        if (!playerScript.m_crouch)
        {
            playerScript.m_rigidbody2D.AddForce(new Vector2(m_horizontalDirection, 0f) * playerData.MovementAcceleration);
            if (Mathf.Abs(playerScript.m_rigidbody2D.velocity.x) > playerData.MaxSpeed)
                playerScript.m_rigidbody2D.velocity = new Vector2(Mathf.Sign(playerScript.m_rigidbody2D.velocity.x) * playerData.MaxSpeed, playerScript.m_rigidbody2D.velocity.y);
        }
        if((m_horizontalDirection < 0 && m_facingRight) || (m_horizontalDirection > 0 && !m_facingRight))
        {
            Flip();
        }
    }

    private void Jump()
    {
        ApplyAirLignerDrag();
        playerScript.m_rigidbody2D.velocity = new Vector2(playerScript.m_rigidbody2D.velocity.x, 0f);
        playerScript.m_rigidbody2D.AddForce(Vector2.up * playerData.JumpForce, ForceMode2D.Impulse);
        m_coyotyCounter = 0f;
        m_jumpBufferCounter = 0f;
        playerScript.m_animator.SetTrigger("Jump");
        AudioManager.instace.PlayClipByName("Jump");
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(m_horizontalDirection) < 0.01f || m_changingDirection)
        {
            playerScript.m_rigidbody2D.drag = m_groundLinearDrag;
        }
        else
        {
            playerScript.m_rigidbody2D.drag = m_airLinearDrag;
        }
    }

    private void ApplyAirLignerDrag()
    {
        playerScript.m_rigidbody2D.drag = m_airLinearDrag;
    }

    private void CheckCollisions()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(playerScript.m_capCollider.bounds.center - new Vector3(0, playerScript.m_capCollider.bounds.extents.y),
                                                    new Vector3(playerScript.m_capCollider.bounds.size.x,0.1f),
                                                    0f,
                                                    Vector2.down,
                                                    m_groundRaycast,
                                                    playerScript.m_groundLayer);
        if (raycastHit.collider != null)
        {
            playerScript.m_grounded = true;
            return;
        }
        playerScript.m_grounded = false;
    }

    private void FallMultiplier()
    {
        if (playerScript.m_rigidbody2D.velocity.y < 0)
            playerScript.m_rigidbody2D.gravityScale = m_highJumpGravity;
        else if (playerScript.m_rigidbody2D.velocity.y > 0 && !Input.GetButton("Jump"))
            playerScript.m_rigidbody2D.gravityScale = m_lowJumpGravity;
        else
            playerScript.m_rigidbody2D.gravityScale = 1;
    }

    private void Flip()
    {
        m_facingRight = !m_facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(playerScript.m_capCollider.bounds.center-new Vector3(0, playerScript.m_capCollider.bounds.extents.y), new Vector3(playerScript.m_capCollider.bounds.size.x,0.1f));
    }

}