using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Mathematics;

public partial struct GemSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var gem in SystemAPI.Query<RefRW<GemSpawnData>>())
        {
            if (gem.ValueRO.isTrigger == false)
                continue;

            if (SystemAPI.HasSingleton<ResData>() == false)
                continue;

            gem.ValueRW.isTrigger = false;

            var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Gem].prefab);

            float3 pos = gem.ValueRO.spawnPos;
            ecb.SetComponent(entity, new LocalTransform
            {
                Position = new float3(pos.x, pos.y + 1, pos.z),
                Rotation = Quaternion.identity,
                Scale = 1
            });
            ecb.AddComponent(entity, new GemData_Get
            {
                speed = 20,
            });
            ecb.SetComponentEnabled<GemData_Get>(entity, false);

            ecb.AddComponent(entity, new GemTag());
        }

        foreach (var tr in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<GemTag>())
        {
            tr.ValueRW = tr.ValueRO.RotateY(Time.deltaTime * 2.0f);
        }
    }
}
