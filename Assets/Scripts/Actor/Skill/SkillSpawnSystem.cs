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
                SkillSpawn(ref state, item.skillID, item.tr, item.atkPower);
            }
        }
    }

    private void SkillSpawn(ref SystemState state, uint id, LocalTransform tr, int atkPower)
    {
        var resBuffer = SystemAPI.GetSingletonBuffer<ResData>();
        var ecb = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        //var ecb = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob, PlaybackPolicy.MultiPlayback);
        var entity = ecb.Instantiate(resBuffer[(int)eResDatas.Projectile_Normal].prefab);
        var skillTable = Table_Skill.instance.GetData(id);

        float3 spawnPos = SetSpawnTransform(in ecb, in entity, in skillTable, tr);

        SetMoveType(in ecb, in entity, in skillTable, tr);

        SetGOModel(in ecb, in entity, spawnPos, skillTable.resPath);

        // 스킬 데미지. 
        ecb.AddComponent(entity, new SkillData_Damage { damage = (int)(skillTable.damage * atkPower) });

        // 태그.
        ecb.AddComponent(entity, new SkillTag());

        // 히트된 애들 저장용.
        ecb.AddComponent(entity, new SkillData_Hit { hitDatas = new List<HitDataItem>() });
    }

    private GameObject SetGOModel(in EntityCommandBuffer ecb, in Entity entity, float3 spawnPos, string prefabPath)
    {
        GameObject goModel = ResourceManager.Instance.LoadObjectInstantiate(string.Format("{0}/{1}", ResourceManager.PathSkill, prefabPath));
        goModel.transform.position = spawnPos;

        ecb.AddComponent(entity, new SkillData_ModelTransform { trasnform = goModel.transform });

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

    private float3 SetSpawnTransform(in EntityCommandBuffer ecb, in Entity entity, in Table_SkillData tableData, LocalTransform actorTr)
    {
        LocalTransform tr;
        float3 spawnPos = float3.zero;
        switch (tableData.spawnPositionType)
        {
            case eSkillSpawnPositionType.Forward:
                spawnPos = actorTr.Position + new float3(0, 1, 0) + actorTr.Forward();
                tr = new LocalTransform
                {
                    Position = spawnPos,
                    Scale = 1,
                    Rotation = actorTr.Rotation
                }; 
                break;

            default:
                Debug.LogError("moveType 처리안됌 : " + tableData.spawnPositionType);
                return spawnPos;
        }

        ecb.SetComponent(entity, tr);

        return spawnPos;
    }
}