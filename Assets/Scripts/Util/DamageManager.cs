using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class DamageManager : Singleton<DamageManager>
{
    [SerializeField] private DamageNumber _damagePrefabs = null;

    public void SpawnDamage(float3 spawnPos, int damage)
    {
        DamageNumber prefab = _damagePrefabs;
        DamageNumber damageNumber = prefab.Spawn(spawnPos, damage);

        //damageNumber.enableFollowing = true;
        //damageNumber.followedTarget = enemy.GetPelvis();
        //Apply Demo Settings:
        //settings.Apply(newDamageNumber);
    }
}