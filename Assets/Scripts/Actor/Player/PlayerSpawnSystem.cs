using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public partial struct PlayerSpawnSystem : ISystem
{
    //void OnUpdate(ref SystemState state)
    //{
    //    foreach(var playerSpawn in SystemAPI.Query<RefRO<PlayerSpawnRes>>())
    //    {
    //        if (SystemAPI.HasSingleton<ResData>() == false)
    //            return;

    //        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
    //        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
    //        var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Actor_normal].prefab);
    //        ecb.AddComponent(entity, new PlayerTag());

    //        float3 spawnPos = new float3(0, 0.1f, 0);
    //        ecb.SetComponent(entity, new LocalTransform
    //        {
    //            Position = spawnPos,
    //            Scale = 1,
    //            Rotation = quaternion.identity
    //        });

    //        SetStatComponent(ref ecb, ref entity);

    //        SetModel(ref ecb, ref entity, ref spawnPos);

    //        // 기본 스킬 등록.
    //        ecb.AddComponent(entity, new ProjectileFire
    //        {
    //            spawnTime = 0,
    //            spawnDelay = 0.05f,
    //        });

    //        ecb.AddComponent(entity, new Dodge());
    //        ecb.SetComponentEnabled<Dodge>(entity, false);

    //        ecb.SetName(entity, "PlayerEntity");

    //        ecb.AddComponent(entity, new ControllEnable());

    //        state.Enabled = false;
    //    }
    //}

    //private void SetModel(ref EntityCommandBuffer ecb, ref Entity entity, ref float3 spawnPos)
    //{
    //    ////////////////////////////////////////////////////////////////////////////////////////////////
    //    // 모델 생성.
    //    GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate("Prefabs/Actor/PlayerModel");
    //    goModel.transform.position = spawnPos;
    //    ecb.AddComponent(entity, new ActorModelTransform { trasnform = goModel.transform });
    //    ecb.AddComponent(entity, new ActorModelAnimator { animator = goModel.GetComponentInChildren<Animator>() });
    //}

    //private void SetStatComponent(ref EntityCommandBuffer ecb, ref Entity entity)
    //{
    //    // 이동 스텟
    //    ecb.AddComponent(entity, new ActorMoveStat
    //    {
    //        moveSpeed = 10,
    //        rotSpeed = 20
    //    });

    //    // 체력.
    //    ecb.AddComponent(entity, new ActorHP
    //    {
    //        hp = 100
    //    });
    //}
}
