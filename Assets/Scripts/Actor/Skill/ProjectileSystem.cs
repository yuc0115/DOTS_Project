using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ProjectileSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (projectile, tr) in SystemAPI.Query<RefRO<Projectile>, RefRW<LocalTransform>>())
        {
            tr.ValueRW = tr.ValueRO.Translate(projectile.ValueRO.direction * projectile.ValueRO.speed * deltaTime);
        }

        double elTime = SystemAPI.Time.ElapsedTime;
        foreach (var (destroyTime, entity) in SystemAPI.Query<RefRO<ProjectileDestroyTime>>().WithEntityAccess())
        {
            if (destroyTime.ValueRO.deleteTime > elTime)
                continue;

            
            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            ecb.DestroyEntity(entity);
        }
    }
}
