﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct DodgeSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        double elTime = SystemAPI.Time.ElapsedTime;
        foreach (var (anim, dodge, stat, tr, entity) in SystemAPI.Query<ActorModelAnimator, RefRW<Dodge>, RefRO<ActorMoveStat>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            if (dodge.ValueRO.isTrigger)
            {
                state.EntityManager.SetComponentEnabled<ControllEnable>(entity, false);
                anim.animator.SetTrigger("doDodge");
                dodge.ValueRW.isTrigger = false;
            }

            tr.ValueRW = tr.ValueRO.Translate(dodge.ValueRO.normal * stat.ValueRO.moveSpeed * deltaTime * 2.5f);
            tr.ValueRW.Rotation = Quaternion.Lerp(tr.ValueRO.Rotation, Quaternion.LookRotation(dodge.ValueRO.normal), deltaTime * stat.ValueRO.rotSpeed * 100);

            if (dodge.ValueRO.endTime <= elTime)
            {
                state.EntityManager.SetComponentEnabled<Dodge>(entity, false);
                state.EntityManager.SetComponentEnabled<ControllEnable>(entity, true);
            }            
        }
    }
}