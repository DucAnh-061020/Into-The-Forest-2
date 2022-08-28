using UnityEngine;
public abstract class UnitStat : ScriptableObject
{
    [Header("Basic stats")]
    public string Name;
    public float MaxHp;
    public float Defense;
    public float MaxSpeed;
    public float AttackDamage;
    public float AttackDiameter;
    public float MovementAcceleration;
    public float ProjectTileSpeed;
    public float JumpForce;
    public float ProjectTileTTL;
}