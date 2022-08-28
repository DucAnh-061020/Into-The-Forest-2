using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class PlayerSave
{
    public int AttackLevel = 0;
    public int DefenseLevel = 0;
    public int HealthLevel = 0;
    public float[] position;
    public float currentHP;

    public PlayerSave(Player player)
    {
        AttackLevel = player.AttackLevel;
        DefenseLevel = player.DefenseLevel;
        HealthLevel = player.HealthLevel;
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        currentHP = player.currentHP;
    }
}