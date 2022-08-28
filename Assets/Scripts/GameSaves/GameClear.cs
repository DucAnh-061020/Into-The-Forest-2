using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameClear
{
    public Guid Id;
    public float m_timeClear;
    public int m_levelClear;

    public GameClear(float timeClear, int levelClear)
    {
        Id = Guid.NewGuid();
        m_timeClear = timeClear;
        m_levelClear = levelClear;
    }
}