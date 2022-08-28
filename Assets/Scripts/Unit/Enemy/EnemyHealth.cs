using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IUnitHp
{
    private EnemyScripts enemyScripts;
    private SimpleFlash flash;
    public float MaxHp { get; set; }
    public float Defense { get; set; }
    public float CurrentHp { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        enemyScripts = GetComponent<EnemyScripts>();
        SetStat(enemyScripts.EnemyData.MaxHp,enemyScripts.EnemyData.Defense);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void Awake()
    {
        flash = GetComponent<SimpleFlash>();
    }

    private void OnDestroy()
    {
        
    }

    public void TakeDamage(float damage)
    {
        if (enemyScripts.isDead)
            return;
        float damageTake = damage - Defense;
        float damageRecive = Mathf.Max(1, damageTake);
        CurrentHp -= damageRecive;
        DamagePopUp.Create(transform.position, damageRecive,Color.white);
        flash.Flash();
        if (CurrentHp <= 0)
        {
            enemyScripts.isDead = true;
        }
        AudioManager.instace.PlayClipByName("Hurt");
    }

    public void SetStat(float maxHp, float defense)
    {
        MaxHp = maxHp;
        CurrentHp = maxHp;
        Defense = defense;
    }

    private void DestroyOnDead()
    {
        GameEvents.OnEnemyDestroyed(GetInstanceID());
        return;
    }
}
