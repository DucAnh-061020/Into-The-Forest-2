using System;
using UnityEngine;
using UnityEngine.SceneManagement;

//control player state machine and conditions
public class Player : UnitScript
{
    
    public bool m_crouch = false;
    public bool m_canJump = false;
    
    public bool m_isAttacking = false;
    public int m_extraJump = 0;
    public float currentHP;

    public int AttackLevel;
    public int DefenseLevel;
    public int HealthLevel;

    public override void Start()
    {
        if (SaveLoad.SaveExits("player"))
        {
            PlayerSave playerSave = SaveLoad.Load<PlayerSave>("player");
            currentHP = playerSave.currentHP;
            HealthLevel = playerSave.HealthLevel;
            AttackLevel = playerSave.AttackLevel;
            DefenseLevel = playerSave.DefenseLevel;
            Debug.Log("load save");
        }
        GameEvents.SaveInitiated += Save;
        GameEvents.UpgradeInitiated += Upgrade;
        base.Start();
    }

    private void OnPlayerDead()
    {
        GameEvents.OnGameOver();
    }

    public override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        AnimationUpdate();
    }

    private void OnDestroy()
    {
        GameEvents.SaveInitiated -= Save;
        GameEvents.UpgradeInitiated -= Upgrade;
    }

    private void AnimationUpdate()
    {
        m_animator.SetBool("IsDead", !m_isAlive);
        m_animator.SetBool("Crouch", m_crouch);
        m_animator.SetBool("OnGround", m_grounded);
        m_animator.SetFloat("FallSpeed", m_rigidbody2D.velocity.normalized.y);
    }

    #region Event Handles
    private void Save()
    {
        SaveLoad.Save(new PlayerSave(this), "player");
    }

    private void Upgrade(UpgradeChoose upgrade)
    {
        switch (upgrade)
        {
            case UpgradeChoose.Attack:
                AttackLevel++;
                break;
            case UpgradeChoose.Defence:
                DefenseLevel++;
                break;
            case UpgradeChoose.Health:
                HealthLevel++;
                break;
        }
        GameEvents.OnSaveInitiated();
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawRay(m_capCollider.bounds.center + new Vector3(m_capCollider.bounds.extents.x, 0), Vector2.down * (m_capCollider.bounds.extents.y + .1f));
        //Gizmos.DrawRay(m_capCollider.bounds.center - new Vector3(m_capCollider.bounds.extents.x, 0), Vector2.down * (m_capCollider.bounds.extents.y + .1f));
        //Gizmos.DrawRay(m_capCollider.bounds.center - new Vector3(m_capCollider.bounds.extents.x, m_capCollider.bounds.extents.y), Vector2.right * m_capCollider.bounds.extents.x);
    }
    #endregion
}
