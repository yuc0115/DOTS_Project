using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct SKillFireSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var (SkillData, tr) in SystemAPI.Query<RefRW<SkillData_Trigger>, RefRO<LocalTransform>>())
        {
            if (SkillData.ValueRO.isFire == false)
                continue;

            SkillData.ValueRW.isFire = false;

            if (SystemAPI.HasSingleton<ResData>() == false)
                continue;

            var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Projectile_Normal].prefab);
            var skillTable = Table_Skill.instance.GetData(SkillData.ValueRO.id);

            ecb.SetComponent(entity, new LocalTransform
            {
                Position = tr.ValueRO.Forward() + tr.ValueRO.Position + new float3(0, 1, 0),
                Scale = 1,
                Rotation = tr.ValueRO.Rotation
            });

            // 스킬 데미지.
            ecb.AddComponent(entity, new SkillData_Damage
            {
                damage = skillTable.damage
            });

            ecb.AddComponent(entity, new Projectile
            {
                speed = 30,
                direction = tr.ValueRO.Forward()
            });

            double elTime = SystemAPI.Time.ElapsedTime;
            ecb.AddComponent(entity, new ProjectileDestroyTime
            {
                deleteTime = elTime + 1
            });

            ecb.AddComponent(entity, new SkillData_Hit
            {
                hitDatas = new List<HitDataItem>()
            });
            // 태그.
            ecb.AddComponent(entity, new SkillTag());
        }
    }
}