using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct DodgeSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        double elTime = SystemAPI.Time.ElapsedTime;
        foreach (var (animator, dodge, stat, tr, pv, entity) in SystemAPI.Query<ActorData_ModelAnimator, RefRW<Dodge>, RefRO<ActorData_MoveStat>, RefRW<LocalTransform>, RefRW<PhysicsVelocity>>().WithEntityAccess())
        {
            if (dodge.ValueRO.isTrigger)
            {
                state.EntityManager.SetComponentEnabled<ControllEnable>(entity, false);
                animator.animator.SetTrigger("doDodge");

                dodge.ValueRW.isTrigger = false;
            }

            pv.ValueRW.Linear = dodge.ValueRO.normal * stat.ValueRO.moveSpeed * 2.5f;
            tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(dodge.ValueRO.normal), deltaTime * stat.ValueRO.rotSpeed * 100);

            if (dodge.ValueRO.endTime <= elTime)
            {
                state.EntityManager.SetComponentEnabled<Dodge>(entity, false);
                state.EntityManager.SetComponentEnabled<ControllEnable>(entity, true);
            }
        }
    }
}