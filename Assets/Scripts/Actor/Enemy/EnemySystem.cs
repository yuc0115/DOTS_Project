using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemySystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        // 배쉬 처리.
        foreach (var (pv, push, entity) in SystemAPI.Query<RefRW<PhysicsVelocity>, RefRW<ActorData_Push>>().WithDisabled<ControllEnable>().WithAll<EnemyTag>().WithEntityAccess())
        {
            if (push.ValueRO.power <= 0.5f)
            {
                pv.ValueRW.Linear = Vector3.zero;
                state.EntityManager.SetComponentEnabled<ControllEnable>(entity, true);
                continue;
            }
            push.ValueRW.power *= 0.8f;
            pv.ValueRW.Linear = push.ValueRO.normal * push.ValueRO.power;
        }
    }
}