using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
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

        var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        CreatePlayer(in state, in ecb, 1);

        CreateTimer(in state, in ecb);

        CreateGemSpawner(in state, in ecb);

        CreateSkillSpawner(in state, in ecb);

        state.Enabled = false;
    }

    private void CreateGemSpawner(in SystemState state, in EntityCommandBuffer ecb)
    {
        var entity = ecb.CreateEntity();

        ecb.SetName(entity, "GemSpawner");
        ecb.AddComponent(entity, new GemSpawnData { datas = new Unity.Collections.NativeQueue<GemSpawnItem>(Allocator.Persistent) });
    }

    private void CreateSkillSpawner(in SystemState state, in EntityCommandBuffer ecb)
    {
        var entity = ecb.CreateEntity();

        ecb.SetName(entity, "SkillSpanwer");
        ecb.AddComponent(entity, new SkillData_Spawn { datas = new Unity.Collections.NativeQueue<SkillData_SpawnItem>(Allocator.Persistent) });
    }

    private void CreateTimer(in SystemState state, in EntityCommandBuffer ecb)
    {
        var entity = ecb.CreateEntity();

        ecb.SetName(entity, "Timer");
        ecb.AddComponent(entity, new GameTime { time = 0 });
    }

    private void CreatePlayer(in SystemState state, in EntityCommandBuffer ecb, uint actorID)
    {
        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
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

        // 회피 기능 추가.
        ecb.AddComponent(entity, new Dodge());
        ecb.SetComponentEnabled<Dodge>(entity, false);
    }
}