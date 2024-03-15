using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Physics;


public partial struct EnemyDieStateSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (actorState, entity) in SystemAPI.Query<RefRO<ActorState>>().WithAll<EnemyTag>().WithEntityAccess())
        {
            if (actorState.ValueRO.actorState != eActorState.Die)
                continue;

            ecb.DestroyEntity(entity);
        }

        // 모델 삭제 처리. 
        foreach (var (actorModel, entity) in SystemAPI.Query<ActorModelTransform>().WithNone<LocalToWorld>().WithEntityAccess())
        {
            if (actorModel.trasnform != null)
                GameObject.Destroy(actorModel.trasnform.gameObject);
            ecb.RemoveComponent<ActorModelTransform>(entity);
        }
    }
}
