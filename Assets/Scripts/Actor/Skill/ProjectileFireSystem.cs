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
            if (SystemAPI.HasSingleton<ResData>() == false)
                continue;

            if (projectileFire.ValueRO.spawnTime > SystemAPI.Time.ElapsedTime)
                continue;

            var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();

            double elTime = SystemAPI.Time.ElapsedTime;
            projectileFire.ValueRW.spawnTime = elTime + projectileFire.ValueRO.spawnDelay;
            
            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Projectile_Normal].prefab);

            ecb.SetComponent(entity, new LocalTransform
            {
                Position = tr.ValueRO.Forward() + tr.ValueRO.Position + new float3(0, 1, 0),
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
