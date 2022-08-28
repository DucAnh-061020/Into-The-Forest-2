using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    private EnemyScripts enemyScripts;

    private void Start()
    {
        enemyScripts = GetComponent<EnemyScripts>();

    }

    private void Update()
    {
        CheckCollision();
        if (enemyScripts.m_currentState == EnemyScripts.EnemyState.Attack)
            return;
        ChaseTarget();
    }

    private void ChaseTarget()
    {
        if(enemyScripts.dstToPlayer < enemyScripts.EnemyData.AttackRange)
        {
            return;
        }
        if(enemyScripts.m_isRight)
            enemyScripts.m_rigidbody2D.AddForce(Vector2.right * enemyScripts.EnemyData.MovementAcceleration, ForceMode2D.Impulse);
        else
            enemyScripts.m_rigidbody2D.AddForce(Vector2.left * enemyScripts.EnemyData.MovementAcceleration, ForceMode2D.Impulse);
        if (Mathf.Abs(enemyScripts.m_rigidbody2D.velocity.x) > enemyScripts.EnemyData.MaxSpeed)
            enemyScripts.m_rigidbody2D.velocity = new Vector2(Mathf.Sign(enemyScripts.m_rigidbody2D.velocity.x) * enemyScripts.EnemyData.MaxSpeed, enemyScripts.m_rigidbody2D.velocity.y);
    }

    private void CheckCollision()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(enemyScripts.m_capCollider.bounds.center - new Vector3(0, enemyScripts.m_capCollider.bounds.extents.y),
                                                    new Vector3(enemyScripts.m_capCollider.bounds.size.x, 0.1f), 0f,
                                                    Vector2.down, 0.1f, enemyScripts.m_groundLayer);
        if(raycastHit.collider != null)
        {
            enemyScripts.isGrounded = true;
            return;
        }
        enemyScripts.isGrounded = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(enemyScripts.m_capCollider.bounds.center - new Vector3(0, enemyScripts.m_capCollider.bounds.extents.y), new Vector3(enemyScripts.m_capCollider.bounds.size.x, 0.1f));
    }
}