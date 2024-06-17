using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct SkillMoveSystem : ISystem
{
    private ComponentLookup<LocalTransform> _trLookup;

    void OnCreate(ref SystemState state)
    {
        _trLookup = state.GetComponentLookup<LocalTransform>();
    }

    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        foreach (var (item, tr) in SystemAPI.Query<RefRO<SkillData_MoveForward>, RefRW<LocalTransform>>())
        {
            tr.ValueRW = tr.ValueRO.Translate(item.ValueRO.direction * item.ValueRO.speed * deltaTime);
            quaternion targetRotation = Quaternion.LookRotation(item.ValueRO.direction, Vector3.up);
            tr.ValueRW.Rotation = targetRotation;
        }

        UpdateRandomBouncingSphere(ref state, deltaTime);

        UpdateRotationAround(ref state, deltaTime);
        UpdateGO(ref state);
    }

    private void UpdateRotationAround(ref SystemState state, float deltaTime)
    {
        _trLookup.Update(ref state);
        foreach (var (item, tr) in SystemAPI.Query<RefRO<SkillData_RotationAround>, RefRW<LocalTransform>>())
        {
            // 캐릭터를 중심으로 오브젝트를 회전시킵니다.
            //float3 center = float3.zero; // 캐릭터의 위치 (중심)
            //float angle = rotationComponent.rotationSpeed * deltaTime;
            //quaternion rotation = quaternion.AxisAngle(math.up(), angle);
            //float3 direction = translation.Value - center;
            //translation.Value = math.mul(rotation, direction) + center;

            LocalTransform casterTr = _trLookup[item.ValueRO.caster];
            float3 center = casterTr.Position; // 캐릭터의 위치 (중심)
            float angle = item.ValueRO.speed * deltaTime;
            quaternion rotation = quaternion.AxisAngle(math.up(), angle);
            float3 direction = tr.ValueRO.Position - center;
            tr.ValueRW.Position = math.mul(rotation, direction) + center;

            //tr.ValueRW = tr.ValueRO.Translate(item.ValueRO.direction * item.ValueRO.speed * deltaTime);
            //quaternion targetRotation = Quaternion.LookRotation(item.ValueRO.direction, Vector3.up);
            //tr.ValueRW.Rotation = targetRotation;
        }
    }

    [BurstDiscard]
    private void UpdateGO(ref SystemState state)
    { 
        foreach (var (goTr, localTr) in SystemAPI.Query<SkillData_ModelTransform, RefRO<LocalTransform>>().WithAll<SkillTag>())
        {
            goTr.trasnform.position = localTr.ValueRO.Position;
            goTr.trasnform.rotation = localTr.ValueRO.Rotation;
        }
    }
    [BurstCompile]
    private void UpdateRandomBouncingSphere(ref SystemState state, float deltaTime)
    {
        _trLookup.Update(ref state);
        foreach (var (item, tr, entity) in SystemAPI.Query<RefRW<SkillData_RandomBouncingSphere>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            tr.ValueRW = tr.ValueRO.Translate(item.ValueRO.direction * item.ValueRO.speed * deltaTime);
            quaternion targetRotation = Quaternion.LookRotation(item.ValueRO.direction, Vector3.up);
            tr.ValueRW.Rotation = targetRotation;

            LocalTransform casterTr = _trLookup[item.ValueRO.caster];
            LocalTransform skillTr = _trLookup[entity];
            float doundary = item.ValueRO.doundary;
            // 왼쪽.
            if (skillTr.Position.x < casterTr.Position.x - doundary && item.ValueRO.direction.x < 0)
            {
                item.ValueRW.direction = Vector3.Reflect(item.ValueRO.direction, Vector3.right);
            }

            // 오른쪽.
            if (skillTr.Position.x > casterTr.Position.x + doundary && item.ValueRO.direction.x > 0)
            {
                item.ValueRW.direction = Vector3.Reflect(item.ValueRO.direction, Vector3.left);
            }

            // 위.
            if (skillTr.Position.z > casterTr.Position.z + doundary && item.ValueRO.direction.z > 0)
            {
                item.ValueRW.direction = Vector3.Reflect(item.ValueRO.direction, Vector3.back);
            }

            // 아래.
            if (skillTr.Position.z < casterTr.Position.z - doundary && item.ValueRO.direction.z < 0)
            {
                item.ValueRW.direction = Vector3.Reflect(item.ValueRO.direction, Vector3.forward);
            }
        }
    }
}