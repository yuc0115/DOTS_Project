using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;
using Unity.Mathematics;

public partial struct TestSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        //foreach(var (t, tr) in SystemAPI.Query<RefRW<TestEvent>, RefRO<LocalTransform>>())
        //{
        //    Physics.OverlapSphere(tr.ValueRO.Position, 0.5f);
        //}

        //foreach(var gem in SystemAPI.Query<RefRW<GemData>>())
        //{
        //    if (gem.ValueRO.spawnTime > SystemAPI.Time.ElapsedTime)
        //        continue;

        //    gem.ValueRW.spawnTime = SystemAPI.Time.ElapsedTime + gem.ValueRO.delay;

        //    var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        //    var entity = ecb.Instantiate(gem.ValueRO.entity);

        //    ecb.SetComponent(entity, new LocalTransform
        //    {
        //        Position = new float3(UnityEngine.Random.Range(-20.0f, 20.0f), 1, UnityEngine.Random.Range(-20.0f, 20.0f)),
        //        Rotation = Quaternion.identity,
        //        Scale = 1
        //    });

        //    ecb.AddComponent(entity, new GemTag());
        //}

        //foreach(var tr in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<GemTag>())
        //{
        //    tr.ValueRW = tr.ValueRO.RotateY(Time.deltaTime * 2.0f);
        //}
    }
}
