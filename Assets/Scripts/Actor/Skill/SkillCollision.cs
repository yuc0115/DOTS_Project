using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;

public partial struct SkillCollision : ISystem
{
    public ComponentLookup<ActorHP> actorHPGroup11;
    

    void OnCreate(ref SystemState state)
    {
        actorHPGroup11 = state.GetComponentLookup<ActorHP>();
    }

    void OnUpdate(ref SystemState state)
    {
        actorHPGroup11.Update(ref state);
        var simulation = SystemAPI.GetSingletonRW<SimulationSingleton>();
        
        state.Dependency = new SkillTEstHitJob()
        {
            actorHPGroup = actorHPGroup11
        }.Schedule(simulation.ValueRW, state.Dependency);
    }
}


public struct SkillTEstHitJob : ITriggerEventsJob
{
    public ComponentLookup<ActorHP> actorHPGroup;
    
    public void Execute(TriggerEvent triggerEvent)
    {
        var entityA = triggerEvent.EntityA;
        var entityB = triggerEvent.EntityB;

        bool isHPComponentA = actorHPGroup.HasComponent(entityA);
        bool isHPComponentB = actorHPGroup.HasComponent(entityB);

        if (isHPComponentA)
        {
            var actorHP = actorHPGroup[entityA];
            actorHP.hp = 0;
            actorHPGroup[entityA] = actorHP;
        }

        if (isHPComponentB)
        {
            var actorHP = actorHPGroup[entityB];
            actorHP.hp = 0;
            actorHPGroup[entityB] = actorHP;
        }
    }
}