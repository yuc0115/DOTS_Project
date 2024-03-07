using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyMoveSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (tr, target, stat) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ActorTarget>, RefRO<ActorMoveStat>>().WithAll<EnemyTag>())
        {
            if (target.ValueRO.entity == Entity.Null)
                continue;

            LocalTransform trTarget = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.entity);
            float3 vNormal = math.normalize(trTarget.Position - tr.ValueRO.Position);
            tr.ValueRW = tr.ValueRO.Translate(vNormal * stat.ValueRO.moveSpeed * deltaTime);
            tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(vNormal), deltaTime * stat.ValueRO.rotSpeed);
        }

        foreach (var (goAnim, goTr, localTr) in SystemAPI.Query<ActorModelAnimator, ActorModelTransform, RefRO<LocalTransform>>().WithAll<EnemyTag>())
        {
            goTr.trasnform.position = localTr.ValueRO.Position;
            goTr.trasnform.rotation = localTr.ValueRO.Rotation;
        }
    }
}