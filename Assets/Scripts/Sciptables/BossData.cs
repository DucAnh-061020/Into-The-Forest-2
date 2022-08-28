using UnityEngine;

[CreateAssetMenu(fileName = "New Boss", menuName = "Scriptable Object/Boss")]
public class BossData : EnemyData
{
    [Header("Details")]
    public float SpecialAttackTime;
    public float SecondaryAttackRange;
}
