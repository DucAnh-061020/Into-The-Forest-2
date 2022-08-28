using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IUnitHp
{
    public float MaxHp { get; set; }
    public float Defense { get; set; }
    public float CurrentHp { get; set; }

    public void TakeDamage(float damage);

    public void SetStat(float maxHp, float defense);
}
