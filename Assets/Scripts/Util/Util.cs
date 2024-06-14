using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public static class Util
{
    public static int GetMaxExp(int level)
    {
        return 100 + level * 10;
    }

    public static T GetAddComponent<T>(this GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }

    public static void GamePause()
    {
        Time.timeScale = 0;
    }

    public static void GameReStart()
    {
        Time.timeScale = 1;
    }

    public static void FindClosestEntity<T>(
         DynamicBuffer<T> buffer,
         float3 currentPosition,
         ref ComponentLookup<LocalTransform> trLookup,
         System.Func<T, Entity> entitySelector,
         out Entity closestEnemy,
         out float3 enemyPos) where T : unmanaged, IBufferElementData
    {
        closestEnemy = Entity.Null;
        enemyPos = float3.zero;
        float closestDistance = float.MaxValue;

        foreach (var item in buffer)
        {
            Entity entity = entitySelector(item);
            if (entity != Entity.Null && trLookup.HasComponent(entity))
            {
                LocalTransform enemyTr = trLookup[entity];
                float distance = math.distance(currentPosition, enemyTr.Position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = entity;
                    enemyPos = enemyTr.Position;
                }
            }
        }
    }
}