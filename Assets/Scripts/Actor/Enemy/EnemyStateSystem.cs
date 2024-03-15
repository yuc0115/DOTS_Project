using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyStateSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var (actorState, target, rangeStat, tr, actorHP) in SystemAPI.Query<RefRW<ActorState>, RefRO<ActorTarget>, RefRO<ActorAtkRangeStat>, RefRO<LocalTransform>, RefRO<ActorHP>>().WithAll<EnemyTag>())
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
