using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct GameInitialzeSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.HasSingleton<ResData>() == false)
            return;

        CreatePlayer(ref state, 1);

        state.Enabled = false;
    }

    private void CreatePlayer(ref SystemState state, uint actorID)
    {
        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Actor_normal].prefab);

        ecb.AddComponent(entity, new PlayerTag());

        float3 spawnPos = new float3(0, 0.1f, 0);
        ecb.SetComponent(entity, new LocalTransform
        {
            Position = spawnPos,
            Scale = 1,
            Rotation = quaternion.identity
        });

        ecb.AddComponent(entity, new IsActorInit { actorTableID = actorID });

        ecb.SetName(entity, "PlayerEntity");

        ecb.AddComponent(entity, new ControllEnable());

        ecb.AddComponent<SkillTrigger>(entity);

        // 회피 기능 추가.
        ecb.AddComponent(entity, new Dodge());
        ecb.SetComponentEnabled<Dodge>(entity, false);
    }
}