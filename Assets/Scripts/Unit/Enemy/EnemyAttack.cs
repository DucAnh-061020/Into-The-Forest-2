using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private EnemyScripts enemyScripts;
    private float m_attackCounter;
    private GameObject pfAmmo;
    private bool m_damageActive = false;
    private Bullet m_bulletControl;
    private void Start()
    {
        enemyScripts = GetComponent<EnemyScripts>();
        switch (enemyScripts.EnemyData.Name)
        {
            case "Archer":
                pfAmmo = Instantiate(GameAssets.i.pfArcherArrow, enemyScripts.m_attackPoint.position, Quaternion.identity);
                m_bulletControl = pfAmmo.GetComponent<Bullet>();
                m_bulletControl.SetData(enemyScripts.EnemyData.ProjectTileTTL, enemyScripts.EnemyData.ProjectTileSpeed, enemyScripts.EnemyData.AttackDamage);
                pfAmmo.SetActive(false);
                break;
        }
    }

    private void Update()
    {
        if (enemyScripts.isDead)
            return;
    }

    private void FixedUpdate()
    {
        if (enemyScripts.isDead)
        {
            return;
        }
        m_attackCounter += Time.fixedDeltaTime;
        if (enemyScripts.m_currentState == EnemyScripts.EnemyState.Chase)
            return;

        if(CanAttack&& InAttackRange)
        {
            enemyScripts.m_animator.SetTrigger("Attack");
            AudioManager.instace.PlayClipByName("Shoot");
            m_attackCounter = 0;
        }
        DealDamage();
    }

    private bool InAttackRange => enemyScripts.dstToPlayer <= enemyScripts.EnemyData.AttackRange;

    private bool CanAttack => m_attackCounter >= enemyScripts.EnemyData.TimeBetweenAttack;

    private void Fire()
    {
        try
        {
            m_bulletControl.SetData(enemyScripts.EnemyData.ProjectTileTTL, enemyScripts.EnemyData.ProjectTileSpeed, enemyScripts.EnemyData.AttackDamage);
            m_bulletControl.SetOnFire(enemyScripts.m_attackPoint);
        }
        catch (Exception) { }
            
    }

    public void SetCanDamage() => m_damageActive = true;

    public void SetNonDamage() => m_damageActive = false;

    private void DealDamage()
    {
        if (!m_damageActive)
            return;
        Collider2D[] hitAttack = Physics2D.OverlapCircleAll(enemyScripts.m_attackPoint.position, enemyScripts.EnemyData.AttackDiameter, enemyScripts.m_targetLayer);

        //Damage enemy
        foreach (Collider2D target in hitAttack)
        {
            if (target != null && target.CompareTag("Player"))
            {
                target.GetComponent<IUnitHp>().TakeDamage(enemyScripts.EnemyData.AttackDamage);
            }
                    
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (enemyScripts.m_attackPoint != null)
            Gizmos.DrawWireSphere(enemyScripts.m_attackPoint.position, enemyScripts.EnemyData.AttackDiameter);
    }
}
