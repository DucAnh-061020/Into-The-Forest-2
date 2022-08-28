using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Scriptable Object/Enemy")]
public class EnemyData : UnitStat
{
    [Header("Specific Stats")]
    public float AttackRange;
    public float TimeBetweenAttack;
    public EnemyType Type;
}

public enum EnemyType
{
    Melee,
    Range,
    Boss
}