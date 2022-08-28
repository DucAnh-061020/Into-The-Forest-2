using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stat", menuName ="Scriptable Object/Player Stat")]
public class PlayerData : UnitStat
{
    public float AttackLevel;
    public float DefenseLevel;
    public float HealthLevel;
}
