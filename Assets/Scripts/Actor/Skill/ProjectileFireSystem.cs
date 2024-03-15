using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ProjectileFireSystem : ISystem
{
    void OnUpdate(ref SystemState state) 
    {
        foreach(var (projectileFire, tr) in SystemAPI.Query<RefRW<ProjectileFire>, RefRO<LocalTransform>>())
        {
            if (projectileFire.ValueRO.spawnTime > SystemAPI.Time.ElapsedTime)
                continue;

            double elTime = SystemAPI.Time.ElapsedTime;
            projectileFire.ValueRW.spawnTime = elTime + projectileFire.ValueRO.spawnDelay;
            
            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entity = ecb.Instantiate(projectileFire.ValueRO.resEntity);

            ecb.SetComponent(entity, new LocalTransform
            {
                Position = tr.ValueRO.Forward() + tr.ValueRO.Position,
                Scale = 1,
                Rotation = Quaternion.identity
            });

            ecb.AddComponent(entity, new Projectile
            {
                speed = 30,
                direction = tr.ValueRO.Forward()
            });

            ecb.AddComponent(entity, new ProjectileDestroyTime
            {
                deleteTime = elTime + 1
            });

            // еб╠в.
            ecb.AddComponent(entity, new SkillTag());
        }
    }
}
