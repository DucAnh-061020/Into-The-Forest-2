using System;
using UnityEngine;

public class GolemAttack : MonoBehaviour
{
    private GolemScript golemScript;
    private GameObject pfArm;
    private BossData m_golemData;
    private float AttackTimeCounter;
    private float SpecialAttackTimeCounter;
    private bool CanDealDamage = false;
    private GameObject pfLazer;
    private Bullet m_bulletControl;
    private GolemLazer m_lazerControl;

    private void Start()
    {
        golemScript = GetComponent<GolemScript>();
        m_golemData = golemScript.m_golemData;
        pfArm = Instantiate(GameAssets.i.pfGolemProjectile, golemScript.m_firePoint.position, Quaternion.identity);
        m_bulletControl = pfArm.GetComponent<Bullet>();
        pfArm.SetActive(false);
        pfLazer = Instantiate(GameAssets.i.pfGolemLazer, golemScript.m_lazerPoint.position, Quaternion.identity);
        m_lazerControl = pfLazer.GetComponent<GolemLazer>();
        pfLazer.SetActive(false);
    }

    private void FixedUpdate()
    {
        Attack();
    }

    private void Attack()
    {
        switch (golemScript.m_phase)
        {
            case GolemScript.GolemPhase.Phase0:
                AttackTimeCounter += Time.fixedDeltaTime;
                DealDamage();
                if (CanAttack && InMeleeRange)
                {
                    golemScript.m_animator.SetTrigger("Melee");
                    AttackTimeCounter = 0;
                    golemScript.m_isAttacking = true;
                }
                break;
            case GolemScript.GolemPhase.Phase1:
                AttackTimeCounter += Time.fixedDeltaTime;
                if (CanAttack)
                {
                    AttackTimeCounter = 0;
                    if (InMeleeRange)
                    {
                        DealDamage();
                        golemScript.m_animator.SetTrigger("Melee");
                        golemScript.m_isAttacking = true;
                        return;
                    }

                    if (InFireRange)
                    {
                        golemScript.m_animator.SetTrigger("FireArm");
                        AudioManager.instace.PlayClipByName("GolemShoot");
                        golemScript.m_isAttacking = true;
                        return;
                    }
                }
                break;
            case GolemScript.GolemPhase.Phase2:
                AttackTimeCounter += Time.fixedDeltaTime;
                SpecialAttackTimeCounter += Time.fixedDeltaTime;
                DealDamage();
                if (CanSpecialAttack)
                {
                    golemScript.m_animator.SetTrigger("FireLazer");
                    golemScript.m_rigidbody2D.velocity = Vector2.zero;
                    SpecialAttackTimeCounter = 0;
                    golemScript.m_isAttacking = true;
                    return;
                }
                if (!CanAttack)
                    break;
                AttackTimeCounter = 0;
                if (InMeleeRange)
                {
                    golemScript.m_isAttacking = true;
                    golemScript.m_animator.SetTrigger("Melee");
                    return;
                }

                if (InFireRange)
                {
                    golemScript.m_isAttacking = true;
                    golemScript.m_animator.SetTrigger("FireArm");
                    AudioManager.instace.PlayClipByName("GolemShoot");
                    return;
                }
                break;
        }
    }

    private bool CanAttack => AttackTimeCounter >= m_golemData.TimeBetweenAttack;

    private bool CanSpecialAttack => SpecialAttackTimeCounter >= m_golemData.SpecialAttackTime;

    private bool InMeleeRange => golemScript.m_dstToPlayer <= m_golemData.AttackRange;

    private bool InFireRange => golemScript.m_dstToPlayer <= m_golemData.SecondaryAttackRange;

    private void DealDamage()
    {
        if (!CanDealDamage)
            return;
        Collider2D[] hitAttack = Physics2D.OverlapCircleAll(golemScript.m_attackPoint.position, m_golemData.AttackDiameter, golemScript.m_targetLayer);
        foreach (Collider2D target in hitAttack)
        {
            if (target != null && target.CompareTag("Player"))
            {
                target.GetComponent<IUnitHp>().TakeDamage(m_golemData.AttackDamage);
                ResetDamage();
            }
        }
    }

    private void Fire()
    {
        m_bulletControl.SetData(m_golemData.ProjectTileTTL, m_golemData.ProjectTileSpeed, m_golemData.AttackDamage);
        m_bulletControl.SetOnFire(golemScript.m_firePoint);
    }

    private void FireLAzer()
    {
        m_lazerControl.SetData(m_golemData.AttackDamage * 1.5f);
        m_lazerControl.SetOnFire(golemScript.m_lazerPoint);
        AudioManager.instace.PlayClipByName("Lazer");
    }

    private void SetCanDamage() => CanDealDamage = true;

    private void ResetDamage() => CanDealDamage = false;

    public void EndAttack() => golemScript.m_isAttacking = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(golemScript.m_attackPoint.position, m_golemData.AttackDiameter);
    }
}