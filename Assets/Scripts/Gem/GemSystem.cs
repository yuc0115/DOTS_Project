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
        foreach (var gems in SystemAPI.Query<RefRW<GemSpawnData>>())
        {
            if (SystemAPI.HasSingleton<ResData>() == false)
                continue;

            if (gems.ValueRO.datas.IsEmpty())
                continue;

            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            while(!gems.ValueRO.datas.IsEmpty())
            {
                var gem = gems.ValueRW.datas.Dequeue();
                var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
                var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Gem].prefab);

                ecb.SetComponent(entity, new LocalTransform
                {
                    Position = new float3(gem.spawnPos.x, gem.spawnPos.y + 1, gem.spawnPos.z),
                    Rotation = Quaternion.identity,
                    Scale = 1
                });
                ecb.AddComponent(entity, new GemData_Get
                {
                    speed = 20,
                    exp = 5
                });
                ecb.SetComponentEnabled<GemData_Get>(entity, false);

                ecb.AddComponent(entity, new GemTag());
            }
        }

        foreach (var tr in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<GemTag>())
        {
            tr.ValueRW = tr.ValueRO.RotateY(Time.deltaTime * 2.0f);
        }
    }
}
