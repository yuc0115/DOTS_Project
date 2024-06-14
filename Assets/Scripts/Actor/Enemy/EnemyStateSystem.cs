using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemyStateSystem : ISystem
{
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        foreach (var (actorState, target, rangeStat, tr, actorHP) in SystemAPI.Query<RefRW<ActorData_State>, RefRO<ActorData_Target>, RefRO<ActorData_AtkRangeStat>, RefRO<LocalTransform>, RefRO<ActorData_HP>>().WithAll<EnemyTag>())
        {
            if (actorHP.ValueRO.hp <= 0)
            {
                actorState.ValueRW.actorState = eActorState.Die;
                continue;
            }

            LocalTransform trPlayer = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.entity);
            float dist = Vector3.Magnitude(trPlayer.Position - tr.ValueRO.Position);

            if (dist > rangeStat.ValueRO.minAtkRange)
            {
                actorState.ValueRW.actorState = eActorState.Move;
            }
            else
            {
                actorState.ValueRW.actorState = eActorState.Idle;
            }
        }
    }
}
