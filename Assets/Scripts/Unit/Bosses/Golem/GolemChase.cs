using System;
using UnityEngine;

public class GolemChase : MonoBehaviour
{
    private GolemScript golemScript;

    private void Start()
    {
        golemScript = GetComponent<GolemScript>();
    }

    private void Update()
    {
        if (!golemScript.m_isActivated)
            return;
        CheckCollision();
        if(!golemScript.m_isDead)
            ChaseTarget();
    }

    public void ChaseTarget()
    {
        if (!golemScript.m_canChaseTarget)
            return;
        if (golemScript.m_isAttacking)
            return;
        if (golemScript.m_dstToPlayer <= golemScript.m_golemData.AttackRange)
            return;

        if (golemScript.m_isRight)
            golemScript.m_rigidbody2D.AddForce(Vector2.right * golemScript.m_golemData.MovementAcceleration, ForceMode2D.Impulse);
        else
            golemScript.m_rigidbody2D.AddForce(Vector2.left * golemScript.m_golemData.MovementAcceleration, ForceMode2D.Impulse);

        if (Mathf.Abs(golemScript.m_rigidbody2D.velocity.x) > golemScript.m_golemData.MaxSpeed)
            golemScript.m_rigidbody2D.velocity = new Vector2(Mathf.Sign(golemScript.m_rigidbody2D.velocity.x) * golemScript.m_golemData.MaxSpeed, golemScript.m_rigidbody2D.velocity.y);
    }

    private void CheckCollision()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(golemScript.m_capCollider.bounds.center - new Vector3(0, golemScript.m_capCollider.bounds.extents.y),
                                                    new Vector3(golemScript.m_capCollider.bounds.size.x, 0.1f),
                                                    0f,Vector2.down,0.1f,golemScript.m_groundLayer);
        if (raycastHit.collider != null)
        {
            golemScript.m_isGrounded= true;
            return;
        }
        golemScript.m_isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(golemScript.m_capCollider.bounds.center - new Vector3(0, golemScript.m_capCollider.bounds.extents.y),
                        new Vector3(golemScript.m_capCollider.bounds.size.x, 0.1f));
    }
}