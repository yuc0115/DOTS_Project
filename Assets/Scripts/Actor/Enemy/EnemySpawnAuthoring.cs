using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnAuthoring : MonoBehaviour
{
    public float spawnDelay = 0.1f;
    public float spawnTime;

    public GameObject normalMonster = null;

    public class Baker : Baker<EnemySpawnAuthoring>
    {
        public override void Bake(EnemySpawnAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemySpawnTime
            {
                spawnDelay = authoring.spawnDelay,
                spawnTime = authoring.spawnDelay,
            });

            AddComponent(entity, new EnemySpawnRes
            {
                entityEnemy = GetEntity(authoring.normalMonster, TransformUsageFlags.Dynamic)
            });
        }
    }
}

public struct EnemySpawnRes : IComponentData
{
    public Entity entityEnemy;
}

public struct EnemySpawnTime : IComponentData
{
    public float spawnDelay;
    public float spawnTime;
}