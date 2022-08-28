using UnityEngine;

public class PlayerHealth : MonoBehaviour, IUnitHp
{
    public float MaxHp { get; set; }
    public float Defense { get; set; }
    public float CurrentHp { get; set; }

    public bool m_canTakeDamage = true;
    public float m_imoTime = 3;
    public float m_imoCounter = 0;

    [SerializeField]
    private PlayerData playerData;
    private Player playerScript;

    public void SetStat(float maxHp, float defense)
    {
        CurrentHp = maxHp;
        MaxHp = maxHp;
        Defense = defense;
    }

    public void TakeDamage(float damage)
    {
        if (!playerScript.m_isAlive)
            return;
        if (!m_canTakeDamage)
            return;
        float damageCause = damage - Defense;
        float damageRecive = Mathf.Max(1, damageCause);
        CurrentHp -= damageRecive;
        playerScript.currentHP = CurrentHp;
        m_imoCounter = 0;
        DamagePopUp.Create(transform.position, damageRecive,Color.red);
        if (CurrentHp <= 0)
        {
            playerScript.m_isAlive = false;
        }
        GameEvents.OnUpdateHealth(CurrentHp);
        if (playerScript.m_isAttacking)
            return;
        playerScript.m_animator.SetTrigger("Hurt");
        AudioManager.instace.PlayClipByName("Hurt2");
    }

    #region Unity Methods
    private void Start()
    {
        m_imoCounter = m_imoTime;
        SetStat(playerData.MaxHp, playerData.Defense);
        if (SaveLoad.SaveExits("player"))
        {
            PlayerSave playerSave = SaveLoad.Load<PlayerSave>("player");
            CurrentHp = playerSave.currentHP;
            playerData.HealthLevel = playerSave.HealthLevel;
            playerData.DefenseLevel = playerSave.DefenseLevel;
            MaxHp = playerData.MaxHp + playerData.MaxHp * .5f * playerData.HealthLevel;
            Defense = playerData.Defense + playerData.Defense * .5f * playerData.DefenseLevel;
        }
        else
        {
            playerData.HealthLevel = 0;
            playerData.DefenseLevel = 0;
        }
        playerScript.currentHP = CurrentHp;
        GameEvents.UpgradeInitiated += Upgrade;
        GameEvents.OnSetHealth(MaxHp, CurrentHp);
    }

    private void Update()
    {
        if (m_imoCounter >= m_imoTime)
        {
            m_canTakeDamage = true;
        }
        else
        {
            m_canTakeDamage = false;
            m_imoCounter += Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {

    }

    private void Awake()
    {
        playerScript = GetComponent<Player>();
    }

    private void OnDestroy()
    {
        GameEvents.UpgradeInitiated -= Upgrade;
    }
    #endregion

    #region Event Handles
    private void Upgrade(UpgradeChoose upgrade)
    {
        switch (upgrade)
        {
            case UpgradeChoose.Defence:
                playerData.DefenseLevel++;
                Defense = playerData.Defense + playerData.Defense * .5f * playerData.DefenseLevel;
                break;
            case UpgradeChoose.Health:
                playerData.HealthLevel++;
                MaxHp = playerData.MaxHp + playerData.MaxHp * .5f * playerData.HealthLevel;
                GameEvents.OnSetHealth(MaxHp, CurrentHp);
                break;
        }
    }
    #endregion
}