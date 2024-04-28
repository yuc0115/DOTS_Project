using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Util
{

    public static int GetMaxExp(int level)
    {
        return 100 + level * 10;
    }

    public static void GamePause()
    {
        Time.timeScale = 0;
    }

    public static void GameReStart()
    {
        Time.timeScale = 1;
    }
}