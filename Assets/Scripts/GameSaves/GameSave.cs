using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSave
{
    public float timeLast;
    public int lastScene;
    public int currentLevel;
    public GameSave(float time,int scene,int level)
    {
        timeLast = time;
        lastScene = scene;
        currentLevel = level;
    }
}
