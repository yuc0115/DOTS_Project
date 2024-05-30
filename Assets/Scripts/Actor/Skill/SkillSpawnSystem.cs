using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct SkillSpawnSystem : ISystem
{
    void OnUpdate(ref SystemState state)
    {
        foreach(var spawnData in SystemAPI.Query<RefRO<SkillData_Spawn>>())
        {
            var spawnItems = spawnData.ValueRO.datas;
            while(spawnItems.Count > 0)
            {
                var item = spawnItems.Dequeue();
                SkillSpawn(ref state, item.attackerType, item.id, item.tr, item.atkPower);
            }
        }
    }

    private void SkillSpawn(ref SystemState state, eActorType attakerType, uint id, LocalTransform tr, int atkPower)
    {
        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Projectile_Normal].prefab);
        var skillTable = Table_Skill.instance.GetData(id);
        var prefabSize = state.EntityManager.GetComponentData<SkillData_PrefabSize>(resBuffer[(int)eResDatas.Projectile_Normal].prefab);

        double elTime = SystemAPI.Time.ElapsedTime;

        float3 spawnPos = float3.zero;
        quaternion spawnRotation = quaternion.identity;
        SetSpawnTransform(in ecb, in entity, in skillTable, tr, prefabSize.scale, out spawnPos, out spawnRotation);

        SetMoveType(in ecb, in entity, in skillTable, tr);

        SetGOModel(in ecb, in entity, prefabSize.scale, spawnPos, spawnRotation, skillTable.resPath);

        SetDestoryType(in ecb, in entity, in skillTable, elTime);

        ecb.AddComponent(entity, new SkillData_Attaker { actorType = attakerType });

        // 스킬 데미지. 
        ecb.AddComponent(entity, new SkillData_Damage { damage = (int)(skillTable.damage * atkPower) });

        // 태그.
        ecb.AddComponent(entity, new SkillTag());

        // 히트된 애들 저장용.
        ecb.AddComponent(entity, new SkillData_Hit { hitEffect =  skillTable.hitEffect, hitDatas = new List<HitDataItem>() });

        if (skillTable.pushPower >= 0)
            ecb.AddComponent(entity, new SkillData_PushPower { power = skillTable.pushPower });
    }

    private void SetDestoryType(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, double elTime)
    {
        switch (tableData.destoryType)
        {
            case eSkillDestoryType.Hit:
                ecb.AddComponent(entity, new SkillData_HitCount { count = tableData.destoryValue });
                break;

            default:
                Debug.LogError("타입 처리 안됌 : " + tableData.destoryType);
                break;
        }
        // 삭제 관련
        ecb.AddComponent(entity, new SkillData_DestoryTime { time = elTime + 1 });
    }

    private GameObject SetGOModel(in EntityCommandBuffer ecb, in Entity entity, float scale, float3 spawnPos, quaternion rotation, string prefabPath)
    {
        GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate(ResourceManager.PathSkill, prefabPath);
        Transform tr = goModel.transform;
        tr.position = spawnPos;
        tr.rotation = rotation;
        tr.localScale = Vector3.one * scale;

        ecb.AddComponent(entity, new SkillData_ModelTransform { trasnform = tr });

        return goModel;
    }

    private void SetMoveType(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, LocalTransform actorTr)
    {
        switch(tableData.moveType)
        {
            case eSkillMoveType.Forward:
                ecb.AddComponent(entity, new SkillData_MoveForward
                { 
                    direction = actorTr.Forward(),
                    speed = tableData.moveTypeValue
                });
                break;
        }
    }

    private void SetSpawnTransform(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, LocalTransform actorTr, float scale, out float3 spawnPos, out quaternion rotation)
    {
        spawnPos = float3.zero;
        rotation = quaternion.identity;
        LocalTransform tr;
        switch (tableData.spawnPositionType)
        {
            case eSkillSpawnPositionType.Forward:
                spawnPos = actorTr.Position + new float3(0, 1, 0) + actorTr.Forward();
                rotation = actorTr.Rotation;
                tr = new LocalTransform
                {
                    Position = spawnPos,
                    Scale = scale,
                    Rotation = rotation
                }; 
                break;

            default:
                Debug.LogError("moveType 처리안됌 : " + tableData.spawnPositionType);
                return;
        }

        ecb.SetComponent(entity, tr);
    }
}