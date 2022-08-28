using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class LevelSave
{
    public bool isCleared;

    public LevelSave(Spawner level)
    {
        isCleared = level.gameObject.activeSelf;
    }
}