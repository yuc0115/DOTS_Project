using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct SkillMoveForwardSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (item, tr, entity) in SystemAPI.Query<RefRO<SkillData_MoveForward>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            tr.ValueRW = tr.ValueRO.Translate(item.ValueRO.direction * item.ValueRO.speed * deltaTime);
        }

        foreach (var (goTr, localTr) in SystemAPI.Query<SkillData_ModelTransform, RefRO<LocalTransform>>().WithAll<SkillTag>())
        {
            goTr.trasnform.position = localTr.ValueRO.Position;
            goTr.trasnform.rotation = localTr.ValueRO.Rotation;
        }
    }
}