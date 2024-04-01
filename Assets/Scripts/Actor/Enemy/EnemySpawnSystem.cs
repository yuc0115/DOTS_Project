using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemySpawnSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.HasSingleton<PlayerTag>() == false)
            return;

        foreach(var (spawnTime, spawnRes) in SystemAPI.Query<RefRW<EnemySpawnTime>, RefRO<EnemySpawnRes>>())
        {
            if (SystemAPI.HasSingleton<ResData>() == false)
                continue;

            if (spawnTime.ValueRO.spawnTime >= SystemAPI.Time.ElapsedTime)
                return;

            spawnTime.ValueRW.spawnTime += spawnTime.ValueRO.spawnDelay;

            var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
            var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Actor_normal].prefab);

            // 태그
            ecb.AddComponent(entity, new EnemyTag());

            SetPosComponent(ref ecb, ref entity);

            //// 타겟 설정.
            ecb.AddComponent(entity, new ActorTarget
            {
                entity = SystemAPI.GetSingletonEntity<PlayerTag>()
            });

            ecb.AddComponent(entity, new ActorState
            {
                actorState = eActorState.Idle
            });

            ecb.AddComponent(entity, new IsActorInit { actorTableID = 2 });
        }
    }

    private void SetPosComponent(ref EntityCommandBuffer ecb, ref Entity entity)
    {
        int r = UnityEngine.Random.Range(0, 4);
        float v = UnityEngine.Random.Range(0, 100) * 0.01f;
        float x, y;
        switch (r)
        {
            case 0: // left
                x = 0;
                y = v;
                break;

            case 1: // top
                x = v;
                y = 1;
                break;

            case 2: // right
                x = 1;
                y = v;
                break;

            default: // bottom
                x = v;
                y = 0;
                break;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        // spawnPos Ray
        UnityEngine.Ray ray = Camera.main.ViewportPointToRay(new Vector3(x, y, 0));
        RaycastInput input = new RaycastInput()
        {
            Start = ray.origin,
            Filter = CollisionFilter.Default,
            End = ray.GetPoint(Camera.main.farClipPlane)
        };

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        if (physicsWorld.CastRay(input, out var hit) == false)
        {
            Debug.LogWarning("return");
            return;
        }

        // 좌표 세팅.
        float3 spawnPos = new float3(hit.Position.x, hit.Position.y + 1f, hit.Position.z);
        ecb.SetComponent(entity, new LocalTransform
        {
            Position = spawnPos,
            Scale = 1,
            Rotation = Quaternion.identity
        });

        return;
    }
}