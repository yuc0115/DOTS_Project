using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ActorInitializeSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach (var (actorInit, tr, entity) in SystemAPI.Query<RefRO<IsActorInit>, RefRO<LocalTransform>>().WithEntityAccess())
        {
            state.EntityManager.SetComponentEnabled<IsActorInit>(entity, false);

            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            Table_ActorData tableActorData = Table_Actors.instance.GetData(actorInit.ValueRO.actorTableID);

            ecb.AddComponent(entity, new ControllEnable());

            SetStatComponent(in ecb, in entity, in tableActorData);

            SetGOModel(in ecb, in entity, tr.ValueRO.Position, in tableActorData);

            SetSkillComponent(in ecb, in entity, in tableActorData);
        }
    }

    private void SetSkillComponent(in EntityCommandBuffer ecb, in Entity entity, in Table_ActorData tableActorData)
    {
        if (tableActorData.skills == null || tableActorData.skills.Length == 0)
            return;

        NativeList<SkillData_TriggerItem> datas = new NativeList<SkillData_TriggerItem>(Allocator.Persistent);
        //ecb.AddComponent(entity, new SkillData_Spawn { datas = new NativeList<SkillData_SpawnItem>(Allocator.Persistent) });
        foreach (uint id in tableActorData.skills)
        {
            Table_SkillData data = Table_Skill.instance.GetData(id);

            switch (data.spawnType)
            {
                case eSkillSpawnType.Time:
                    SkillData_TriggerItem item = new SkillData_TriggerItem();
                    item.id = id;
                    item.spawnDelay = data.spawnTypeValue;
                    item.isAnimation = !string.IsNullOrEmpty(data.animTriggerName);
                    datas.Add(item);
                    break;

                default:
                    Debug.LogErrorFormat("SkillSpawnType 처리 안됌 : {0}", data.spawnType);
                    break;
            }
        }

        ecb.AddComponent(entity, new SkillData_Trigger { datas = datas });
    }

    private void SetStatComponent(in EntityCommandBuffer ecb, in Entity entity, in Table_ActorData tableActorData)
    {
        // 이동 스텟
        ecb.AddComponent(entity, new ActorData_MoveStat
        {
            moveSpeed = tableActorData.moveSpeed,
            rotSpeed = tableActorData.rotSpeed
        });

        // 체력.
        ecb.AddComponent(entity, new ActorData_HP
        {
            hp = tableActorData.hp
        });

        // 기본 공격력.
        ecb.AddComponent(entity, new ActorData_AtkPower
        {
            atkPower = tableActorData.atkPower
        });

        // 사정거리.
        ecb.AddComponent(entity, new ActorData_AtkRangeStat
        {
            minAtkRange = tableActorData.minAtkRange,
            maxAtkRange = tableActorData.maxAtkRange
        });
    }

    /// <summary>
    /// GameObject 모델 연결.
    /// </summary>
    /// <param name="ecb"></param>
    /// <param name="entity"></param>
    /// <param name="spawnPos"></param>
    /// <param name="tableActorData"></param>
    /// <returns></returns>
    private void SetGOModel(in EntityCommandBuffer ecb, in Entity entity, float3 spawnPos, in Table_ActorData tableActorData)
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////
        // 모델 생성.
        GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate(ResourceManager.PathActor, tableActorData.resPath);
        goModel.transform.position = spawnPos;
        goModel.name = tableActorData.resPath;

        // componnet setting.
        ActorBaseMono mono = goModel.GetAddComponent<ActorBaseMono>();
        mono.SetEntity(entity, tableActorData.actorType, tableActorData.resPath);

        ecb.AddComponent(entity, new ActorData_ModelTransform { trasnform = goModel.transform });
        ecb.AddComponent(entity, new ActorData_ModelAnimator { animator = goModel.GetComponent<Animator>() });
    }
}