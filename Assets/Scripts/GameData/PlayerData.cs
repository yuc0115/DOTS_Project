using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public int maxExp;
    public int curExp;

    public int level;

    public bool isLevelUp;

    public void SetLevel(int level)
    {
        this.level = level;
        maxExp = Util.GetMaxExp(this.level);
    }

    public void AddExp(int exp)
    {
        curExp += exp;

        if (maxExp <= curExp)
        {
            LevelUp();   
        }
    }

    private void LevelUp()
    {
        level++;
        curExp -= maxExp;
        maxExp = Util.GetMaxExp(level);
        isLevelUp = true;
    }
}
