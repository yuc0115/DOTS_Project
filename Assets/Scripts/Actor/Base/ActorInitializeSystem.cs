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

            SetStatComponent(in ecb, in entity, in tableActorData);

            GameObject actorModel = SetModel(in ecb, in entity, tr.ValueRO.Position, in tableActorData);

            SetSkillComponent(in ecb, in entity, in tableActorData);
            // 모노 붙임.
            ActorBaseMono actor = actorModel.AddComponent<ActorBaseMono>();
            actor.SetEntity(entity);
        }
    }

    private void SetSkillComponent(in EntityCommandBuffer ecb, in Entity entity, in Table_ActorData tableActorData)
    {
        foreach(var pair in tableActorData.skills)
        {
            Table_SkillData data = Table_Skill.instance.GetData(pair);

            switch (data.skillType)
            {
                case eSkillType.AutoSkill:
                    SetAutoSkill(in ecb, in entity, in data);
                    break;

                default:
                    Debug.LogErrorFormat("처리 안됌 : {0}", data.skillType);
                    break;
            }
        }

        void SetAutoSkill(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData data)
        {
            // 기본 스킬 등록.
            ecb.AddComponent(entity, new AutoSkillData
            {
                skillID = data.id,
                fireDelay = data.fireDelay,
            });
        }
    }

    private void SetStatComponent(in EntityCommandBuffer ecb, in Entity entity, in Table_ActorData tableActorData)
    {
        // 이동 스텟
        ecb.AddComponent(entity, new ActorMoveStat
        {
            moveSpeed = tableActorData.moveSpeed,
            rotSpeed = tableActorData.rotSpeed
        });

        // 체력.
        ecb.AddComponent(entity, new ActorHP
        {
            hp = tableActorData.hp
        });

        // 사정거리.
        ecb.AddComponent(entity, new ActorAtkRangeStat
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
    private GameObject SetModel(in EntityCommandBuffer ecb, in Entity entity, float3 spawnPos, in Table_ActorData tableActorData)
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////
        // 모델 생성.
        GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate(string.Format("{0}/{1}", ResourceManager.Instance.GetPathActorRes(),  tableActorData.resPath));
        goModel.transform.position = spawnPos;

        ecb.AddComponent(entity, new ActorModelTransform { trasnform = goModel.transform });
        ecb.AddComponent(entity, new ActorModelAnimator { animator = goModel.GetComponent<Animator>() });

        return goModel;
    }
}