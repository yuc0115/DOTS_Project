using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial struct AutoSkillSystem : ISystem
{
    [BurstCompile]
    void OnUpdate(ref SystemState state)
    {
        foreach (var (skillData, skillTrigger) in SystemAPI.Query<RefRW<SkillData_AutoSkill>, RefRW<SkillData_Trigger>>())
        {
            if (skillData.ValueRO.fireTime > SystemAPI.Time.ElapsedTime)
                continue;

            skillData.ValueRW.fireTime = SystemAPI.Time.ElapsedTime + skillData.ValueRO.fireDelay;

            skillTrigger.ValueRW.id = skillData.ValueRO.skillID;
            skillTrigger.ValueRW.isTrigger = true;
        }
    }
}