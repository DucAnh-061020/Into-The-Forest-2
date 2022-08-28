using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    [Header("Attack Positions")]
    [SerializeField] private Transform AttackPoint;
    [SerializeField] private Transform FirePoint;

    [Header("Player Attack Data")]
    [SerializeField] private PlayerData playerData;
    private float AttackDamage = 50;
    private float AttackDiameter;
    private bool m_damageActive = false;
    private float m_ResetTime = 3f;
    private float m_ResetCounter;

    private Player playerScript;
    private PlayerArrow m_arrowControl;
    
    private enum PlayerCombo
    {
        Melee0,
        Melee1,
        Melee2,
    }

    private PlayerCombo _currentCombo;

    // Start is called before the first frame update
    void Start()
    {
        _currentCombo = PlayerCombo.Melee0;
        AttackDamage = playerData.AttackDamage;
        AttackDiameter = playerData.AttackDiameter;
        GameEvents.UpgradeInitiated += Upgrade;
        if (SaveLoad.SaveExits("player"))
        {
            PlayerSave playerSave = SaveLoad.Load<PlayerSave>("player");
            playerData.AttackLevel = playerSave.AttackLevel;
            AttackDamage = playerData.AttackDamage + playerData.AttackDamage * .5f * playerData.AttackLevel;
        }
        else
        {
            playerData.AttackLevel = 0;
        }
    }

    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.m_crouch)
            return;
        if(!playerScript.m_grounded)
            return;
        if (playerScript.m_isAttacking)
            return;
        //melee
        if (Input.GetButtonDown("Fire1"))
        {
            Melee();
            return;
        }
        //fire arrow
        if (Input.GetButtonDown("Fire2"))
        {
            Bow();
            return;
        }
    }

    private void FixedUpdate()
    {
        DealDamage();
        if(_currentCombo != PlayerCombo.Melee0)
        {
            if (m_ResetCounter >= m_ResetTime)
                ResetCombo();
            m_ResetCounter += Time.fixedDeltaTime;
        }
    }

    private void Melee()
    {
        switch (_currentCombo)
        {
            case PlayerCombo.Melee0:
                playerScript.m_animator.SetTrigger("Melee0");
                playerScript.m_animator.SetTrigger("Shelth");
                _currentCombo++;
                break;
            case PlayerCombo.Melee1:
                playerScript.m_animator.SetTrigger("Melee1");
                _currentCombo++;
                break;
            case PlayerCombo.Melee2:
                playerScript.m_animator.SetTrigger("Melee2");
                _currentCombo = PlayerCombo.Melee0;
                break;
        }
        playerScript.m_isAttacking = true;
        m_ResetCounter = 0;
    }

    private void Bow()
    {
        playerScript.m_animator.SetTrigger("Fire0");
        playerScript.m_isAttacking = true;
        AudioManager.instace.PlayClipByName("Shoot");
    }

    private void Fire()
    {
        float finalDamage = Mathf.Floor(AttackDamage / 1.5f * 100) / 100;
        PlayerArrow.Create(FirePoint, finalDamage, playerData.ProjectTileSpeed, playerData.ProjectTileTTL);
    }

    private void ResetAttack()
    {
        playerScript.m_isAttacking = false;
        m_damageActive = false;
    }

    private void ResetCombo()
    {
        if(_currentCombo != PlayerCombo.Melee0)
            _currentCombo = PlayerCombo.Melee0;
        m_damageActive = false;
        m_ResetCounter = 0;
    }

    public void ApplyDamage()
    {
        m_damageActive = true;
    }

    private void DealDamage()
    {
        if (!m_damageActive)
            return;

        Collider2D[] hitAttack = Physics2D.OverlapCircleAll(AttackPoint.position, AttackDiameter, playerScript.m_targetLayer);
        foreach (Collider2D target in hitAttack)
        {
            if (target != null && !target.CompareTag("Player"))
            {
                target.GetComponent<IUnitHp>().TakeDamage(AttackDamage);
                m_damageActive = false;
            }
        }
    }

    private void Upgrade(UpgradeChoose upgrade)
    {
        if(upgrade == UpgradeChoose.Attack)
        {
            playerData.AttackLevel++;
            AttackDamage = playerData.AttackDamage + playerData.AttackDamage * .5f * playerData.AttackLevel;
        }
    }

    private void OnDestroy()
    {
        GameEvents.UpgradeInitiated -= Upgrade;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackDiameter);
    }
}
