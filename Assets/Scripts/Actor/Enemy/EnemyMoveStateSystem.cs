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

        foreach (var (tr, pv, target, moveStat, actorState) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PhysicsVelocity>, RefRO<ActorData_Target>, RefRO<ActorData_MoveStat>, RefRO<ActorData_State>>().WithAll<EnemyTag>().WithAll<ControllEnable>())
        {
            if (target.ValueRO.entity == Entity.Null)
                continue;

            if (actorState.ValueRO.actorState != eActorState.Move)
                continue;

            LocalTransform trTarget = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.entity);
            float3 vNormal = math.normalize(trTarget.Position - tr.ValueRO.Position);
            vNormal.y = 0;
            pv.ValueRW.Linear = vNormal * moveStat.ValueRO.moveSpeed;
            tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(vNormal), deltaTime * moveStat.ValueRO.rotSpeed);
        }

        foreach (var (goAnim, goTr, localTr) in SystemAPI.Query<ActorData_ModelAnimator, ActorData_ModelTransform, RefRO<LocalTransform>>().WithAll<EnemyTag>())
        {
            goTr.trasnform.position = localTr.ValueRO.Position;
            goTr.trasnform.rotation = localTr.ValueRO.Rotation;
        }
    }
}