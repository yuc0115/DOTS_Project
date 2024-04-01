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
        foreach (var (timer, skillTrigger) in SystemAPI.Query<RefRW<AutoSkillData>, RefRW<SkillTrigger>>())
        {
            if (timer.ValueRO.fireTime > SystemAPI.Time.ElapsedTime)
                continue;

            timer.ValueRW.fireTime = SystemAPI.Time.ElapsedTime + timer.ValueRO.fireDelay;

            skillTrigger.ValueRW.id = UnityEngine.Random.Range(0, 10000);
            skillTrigger.ValueRW.isTrigger = true;
        }
    }
}