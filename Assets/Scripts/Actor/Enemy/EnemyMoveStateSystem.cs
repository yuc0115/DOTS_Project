using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;


public partial struct EnemyMoveStateSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach(var (tr, target, moveStat, actorState) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<ActorTarget>, RefRO<ActorMoveStat>, RefRO<ActorState>>().WithAll<EnemyTag>())
        {
            if (target.ValueRO.entity == Entity.Null)
                continue;

            if (actorState.ValueRO.actorState != eActorState.Move)
                continue;

            LocalTransform trTarget = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.entity);
            float3 vNormal = math.normalize(trTarget.Position - tr.ValueRO.Position);
            vNormal.y = 0;
            tr.ValueRW = tr.ValueRO.Translate(vNormal * moveStat.ValueRO.moveSpeed * deltaTime);
            tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(vNormal), deltaTime * moveStat.ValueRO.rotSpeed);
        }
     
        foreach (var (goAnim, goTr, localTr) in SystemAPI.Query<ActorModelAnimator, ActorModelTransform, RefRO<LocalTransform>>().WithAll<EnemyTag>())
        {
            goTr.trasnform.position = localTr.ValueRO.Position;
            goTr.trasnform.rotation = localTr.ValueRO.Rotation;
        }
    }
}