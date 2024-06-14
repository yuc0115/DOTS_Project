using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct EnemySystem : ISystem
{
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        // 배쉬 처리.
        //foreach (var (pv, push, entity) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<ActorData_Push>>().WithDisabled<ControllEnable>().WithAll<EnemyTag>().WithEntityAccess())
        foreach (var (tr, push, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<ActorData_Push>>().WithDisabled<ControllEnable>().WithAll<EnemyTag>().WithEntityAccess())
        {
            if (push.ValueRO.power <= 0.5f)
            {
                //tr.ValueRW.Linear = Vector3.zero;
                state.EntityManager.SetComponentEnabled<ControllEnable>(entity, true);
                continue;
            }
            push.ValueRW.power *= 0.8f;
            tr.ValueRW = tr.ValueRO.Translate(push.ValueRO.normal * push.ValueRO.power * deltaTime);
            //pv.ValueRW.Linear = push.ValueRO.normal * push.ValueRO.power;
        }
    }
}