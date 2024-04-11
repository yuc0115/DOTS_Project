using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public partial struct SkillCollision : ISystem
{
    public ComponentLookup<ActorData_Hit> actorHitGroup;
    public ComponentLookup<SkillData_Damage> skillDamageGroup;

    void OnCreate(ref SystemState state)
    {
        actorHitGroup = state.GetComponentLookup<ActorData_Hit>();
        skillDamageGroup = state.GetComponentLookup<SkillData_Damage>(true);
    }

    void OnUpdate(ref SystemState state)
    {
        actorHitGroup.Update(ref state);
        skillDamageGroup.Update(ref state);

        var simulation = SystemAPI.GetSingletonRW<SimulationSingleton>();

        state.Dependency = new SkillCollisionJob()
        {
            actorHitGroup = actorHitGroup,
            skillDamageGroup = skillDamageGroup
        }.Schedule(simulation.ValueRW, state.Dependency);
    }
}


public struct SkillCollisionJob : ITriggerEventsJob
{
    public ComponentLookup<ActorData_Hit> actorHitGroup;
    [ReadOnly] public ComponentLookup<SkillData_Damage> skillDamageGroup;

    public void Execute(TriggerEvent triggerEvent)
    {
        var entityA = triggerEvent.EntityA;
        var entityB = triggerEvent.EntityB;

        bool ishitComponentA = actorHitGroup.HasComponent(entityA);
        bool ishitComponentB = actorHitGroup.HasComponent(entityB);

        if (ishitComponentA)
        {
            if (skillDamageGroup.HasComponent(entityB) == false)
            {
                Debug.LogErrorFormat("HasComponent null! : {0}", entityB);
                return;
            }
            var skillDamage = skillDamageGroup[entityB];

            var hit = actorHitGroup[entityA];
            if (hit.trigger == true)
            {
                Debug.LogError("충돌 중첩?");
            }
            hit.trigger = true;
            hit.damage = skillDamage.damage;
            hit.attacker = entityB;
            actorHitGroup[entityA] = hit;
        }

        if (ishitComponentB)
        {
            if (skillDamageGroup.HasComponent(entityA) == false)
            {
                Debug.LogErrorFormat("HasComponent null! : {0}", entityA);
                return;
            }
            var skillDamage = skillDamageGroup[entityA];

            var hit = actorHitGroup[entityB];
            if (hit.trigger == true)
            {
                Debug.LogError("충돌 중첩?");
            }
            hit.trigger = true;
            hit.damage = skillDamage.damage;
            hit.attacker = entityA;
            actorHitGroup[entityB] = hit;
        }
    }
}