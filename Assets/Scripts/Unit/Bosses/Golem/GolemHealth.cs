using System;
using UnityEngine;

public class GolemHealth : MonoBehaviour, IUnitHp
{
    public float MaxHp { get; set; }
    public float Defense { get; set; }
    public float CurrentHp { get; set; }

    private GolemScript golemScript;
    private SimpleFlash flash;

    private void Start()
    {
        golemScript = GetComponent<GolemScript>();
        SetStat(golemScript.m_golemData.MaxHp, golemScript.m_golemData.Defense);
    }

    private void Awake()
    {
        flash = GetComponent<SimpleFlash>();
    }

    private void Update()
    {
        if (!golemScript.m_isActivated)
            return;
        switch (golemScript.m_phase)
        {
            case GolemScript.GolemPhase.Phase0:
                if (CurrentHp <= MaxHp / 2)
                {
                    golemScript.m_phase = GolemScript.GolemPhase.Phase1;
                    golemScript.m_animator.SetTrigger("Buff");
                    Defense *= 1.5f;
                }
                break;
            case GolemScript.GolemPhase.Phase1:
                if(CurrentHp <= MaxHp / 3)
                {
                    CurrentHp += MaxHp / 2;
                    golemScript.m_phase = GolemScript.GolemPhase.Phase2;
                    golemScript.m_animator.SetTrigger("Glow");
                    DamagePopUp.Create(transform.position, MaxHp / 2, Color.green);
                }
                break;
            case GolemScript.GolemPhase.Phase2:
                break;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!golemScript.m_isActivated)
            return;
        if (golemScript.m_isDead)
            return;
        float damageTake = damage - Defense;
        float damageRecive = Mathf.Max(1, damageTake);
        CurrentHp -= damageRecive;
        flash.Flash();
        DamagePopUp.Create(transform.position, damageRecive, Color.white);
        if (CurrentHp <= 0)
            golemScript.m_isDead = true;
    }

    public void SetStat(float maxHp, float defense)
    {
        MaxHp = maxHp;
        Defense = defense;
        CurrentHp = MaxHp;
    }
}