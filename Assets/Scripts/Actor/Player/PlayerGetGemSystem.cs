using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerGetGemSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.HasSingleton<PlayerTag>() == false)
            return;

        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerTag>();
        LocalTransform playerTr = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (gemTr, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<GemTag>().WithDisabled<GemData_Get>().WithEntityAccess())
        {
            float3 v = playerTr.Position - gemTr.ValueRO.Position;
            float dist = Vector3.Magnitude(v);

            if (dist >= 3)
                continue;

            ecb.SetComponentEnabled<GemData_Get>(entity, true);
        }

        foreach (var (gemTr, data, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<GemData_Get>>().WithAll<GemTag>().WithEntityAccess())
        {
            float3 v = playerTr.Position - gemTr.ValueRO.Position;
            float3 vNormal = Vector3.Normalize(v);

            gemTr.ValueRW = gemTr.ValueRO.Translate(vNormal * data.ValueRO.speed * SystemAPI.Time.DeltaTime);

            if (Vector3.Magnitude(v) <= 0.5f)
            {
                ecb.DestroyEntity(entity);
                GameManager.Instance.playerData.AddExp(data.ValueRO.exp);
            }
        }
    }
}